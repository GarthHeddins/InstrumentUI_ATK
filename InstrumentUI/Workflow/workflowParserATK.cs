using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using CommonShared;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess;
using System.Configuration;
using MicroLabData;
using System.IO.Compression;
using System.Collections.Generic;
using InstrumentUI_ATK.DataService;


namespace InstrumentUI_ATK.Workflow
{
    /// <summary>
    /// This class uses the Agilent Toolkit.
    /// </summary>
    /// 
    internal class WorkflowParserATK : WorkflowParser
    {
        private int varResult;

    #region Members

        //private bool _isBkgExpired = false;
        private string _specFileName = string.Empty;        
        private byte[] _spectralFile = null;
        private string _xmlData = string.Empty;
        private bool _isChecking = true;        

        private readonly AgilentToolkit _agilentToolkit;
        private string _spcFileName;

        #endregion

        public WorkflowParserATK()
        {            
            _agilentToolkit = new AgilentToolkit();
        }

        #region Public Methods

        public override void ExecuteWorkflow()
        {
            Trace.WriteLine("Entering WorkflowParserATK.ExecuteWorkflow() function.");

            DateTime startTime = DateTime.Now;
            Trace.WriteLine("Workflow Start Time - " + startTime.Minute + ":" + startTime.Second + ":" +
                            startTime.Millisecond);

            while (_fileReader.Read())
            {
                if (_fileReader.IsStartElement("prescanprocessing"))
                {
                    XmlReader inner = _fileReader.ReadSubtree();
                    PreScanProcessing(inner);
                    inner.Close();
                    continue;
                }

                if (_fileReader.IsStartElement("scanbackground"))
                {
                    XmlReader inner = _fileReader.ReadSubtree();
                    ScanBackground(inner, false);
                    inner.Close();
                    continue;
                }

                if (_fileReader.IsStartElement("scansample"))
                {
                    if (!IsWorkflowCancelled)
                    {
                        XmlReader inner = _fileReader.ReadSubtree();
                        ScanSample(inner);
                        inner.Close();
                    }
                    else
                    {
                        break;
                    }
                    continue;
                }

                if (_fileReader.IsStartElement("preresultsprocessing"))
                {
                    if (!IsWorkflowCancelled)
                    {
                        DoStartPreResultsProcessing();
                        XmlReader inner = _fileReader.ReadSubtree();
                        PreResultsProcessing(inner);
                        inner.Close();
                    }
                    else
                    {
                        break;
                    }
                    continue;
                }
            }

            DateTime stopTime = DateTime.Now;
            Trace.WriteLine("Workflow Stop Time - " + stopTime.Minute + ":" + stopTime.Second + ":" +
                            stopTime.Millisecond);
            Trace.WriteLine("Total Time - " + (stopTime - startTime).Minutes + ":" + (stopTime - startTime).Seconds +
                            ":" + (stopTime - startTime).Milliseconds);

            Trace.WriteLine("Leaving WorkflowParserATK.ExecuteWorkflow() function.");
        }

        public override void ExecuteWorkflow(InstrumentPhase instrumentPhase)
        {
            Trace.WriteLine("Entering WorkflowParserATK.ExecuteWorkflow(" + instrumentPhase + ") function.");

            DateTime startTime = DateTime.Now;
            Trace.WriteLine("Workflow Start Time - " + startTime.Minute + ":" + startTime.Second + ":" +
                            startTime.Millisecond);

            if (instrumentPhase.Equals(InstrumentPhase.All))
            {
                ExecuteWorkflow();
            }
            else
            {
                while (_fileReader.Read())
                {
                    if (_fileReader.IsStartElement(instrumentPhase.GetStringValue()) ||
                        _fileReader.IsStartElement(InstrumentPhase.ScanBackground.GetStringValue()))
                    {
                        XmlReader inner = _fileReader.ReadSubtree();
                        switch (instrumentPhase)
                        {
                            case InstrumentPhase.InstrumentInit:
                                InstrumentInit(inner);
                                //instrumentPhase = InstrumentPhase.ScanBackground;  EOS Change
                                continue;
                            case InstrumentPhase.ScanBackground:
                                ScanBackground(inner, true);
                                continue;
                        }
                    }
                }
            }

            DateTime stopTime = DateTime.Now;
            Trace.WriteLine("Workflow Stop Time - " + stopTime.Minute + ":" + stopTime.Second + ":" +
                            stopTime.Millisecond);
            Trace.WriteLine("Total Time - " + (stopTime - startTime).Minutes + ":" + (stopTime - startTime).Seconds +
                            ":" + (stopTime - startTime).Milliseconds);
            Trace.WriteLine("Leaving WorkflowParserATK.ExecuteWorkflow(" + instrumentPhase + ") function.");
        }


        public override void ExecuteWorkflow(InstrumentPhase instrumentPhase, string fileName)
        {
            ExecuteWorkflow(instrumentPhase);
        }

        #endregion

        #region Private Methods

        protected void DeleteFile(string fileName)
        {
            Trace.WriteLine("Entering WorkflowParserATK.DeleteFile(" + fileName + ") function.");

            Trace.WriteLine("Deleting " + fileName + " file.");

            File.Delete(fileName);

            Trace.WriteLine("Leaving WorkflowParserATK.DeleteFile(" + fileName + ") function.");
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        #endregion

        protected void DiagnosticCheck(XmlReader inner)
        {
            Trace.WriteLine("Entering DiagnosticCheck method");
            string message = string.Empty;
            string parameters = string.Empty;
            string faultmessage = string.Empty;
            try
            {
                Trace.WriteLine("Entering try block in DiagnosticCheck method");
                while (inner.Read())
                {
                    if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                    {
                        message = inner.ReadString();
                        continue;
                    }
                    if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "parameters")
                    {
                        parameters = inner.ReadString();
                        continue;
                    }
                    if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                    {
                        faultmessage = inner.ReadString();
                        continue;
                    }
                }

                var sbResult = new StringBuilder(1023);

                if (!String.IsNullOrEmpty(message))
                    OnRaiseMessageEvent(new MessageEventArgs(message));

                Trace.WriteLine("Invoking DiagnosticCheck Direct Command: message = " + message + ", parameters= " +
                                parameters);
                var success = _agilentToolkit.DiagnosticCheck(parameters, sbResult);

                Trace.WriteLine("Command Ran Successfully: " + success);
                Trace.WriteLine("Result Value: " + sbResult);

                if (!success || sbResult.ToString().Contains("fail"))
                {                    
                    _WorkflowCancelledMessage = ResourceHelper.DCFMessage +
                                                sbResult.ToString().Substring(sbResult.ToString().IndexOf(",") + 1);
                    CancelWorkflow("-1", _WorkflowCancelledMessage);
                    Trace.WriteLine("DiagnosticCheck() !success || fail:  after CancelWorkflow().");
                    Trace.WriteLine("DiagnosticCheck() !success || fail:  after OnRaiseCancelWorkflowEvent().");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failure: WorkflowParserATK.DiagnosticCheck(): " + ex.Message.ToString());
                Helper.LogError("DiagnosticCheck", "", ex, false);
                CancelWorkflow("-1", _WorkflowCancelledMessage);
            }
            finally
            {
                Trace.WriteLine("Entering finally block in DiagnosticCheck method");
            }
            Trace.WriteLine("Exiting DiagnosticCheck method");
        }

        protected void CleanUp(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.CleanUp() function.");

            string message = string.Empty;
            string parameters = string.Empty;
            string faultmessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "parameters")
                {
                    parameters = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }

            if (!String.IsNullOrEmpty(message))
                OnRaiseMessageEvent(new MessageEventArgs(message));

            Trace.WriteLine("Deleting all files ...");

            try
            {
                if (Directory.Exists(parameters))
                {
                    Trace.WriteLine(parameters + " directory exists.");
                    foreach (string file in Directory.GetFiles(parameters, "*.*"))
                    {
                        Trace.WriteLine("Setting " + file + " file attribute to Normal.");
                        File.SetAttributes(file, FileAttributes.Normal);
                        Trace.WriteLine("Deleting " + file + " file.");
                        File.Delete(file);
                    }

                    /*Trace.WriteLine("Invoking CleanUp Direct Command");
                    var sbResult = new StringBuilder(1023);
                    var success = AgilentTalk.CleanUp(parameters, sbResult);
                    //if (!success || sbResult.ToString().Contains("fail"))
                    if (!success)
                    {
                        _isWorkflowCancelled = true;
                        _WorkflowCancelledMessage = ResourceHelper.CUFMessage;
                        Trace.WriteLine("CleanUp() !success || fail:  after CancelWorkflow().");
                        Trace.WriteLine("CleanUp() !success || fail:  after OnRaiseCancelWorkflowEvent().");
                    }
                    Trace.WriteLine("Command Ran Successfully: " + success);*/
                }
                else
                    Trace.WriteLine("No files or directory found.");
            }
            catch (DirectoryNotFoundException de)
            {
                Helper.LogError("WorkflowParser.CleanUp", string.Empty, de, false);
                Trace.WriteLine("Directory Not Found.\n\nException: " + de.Message);
            }
            catch (Exception e)
            {
                Helper.LogError("WorkflowParser.CleanUp", string.Empty, e, false);
                Trace.WriteLine("General Exception: " + e.Message);
            }
            try
            {
                if (File.Exists(parameters + "TEST.txt"))
                {
                    Helper.LogError("CleanUp", "", "Unable to delete spectral file", "");
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("", ResourceHelper.SFD_Message));
                }
            }
            catch (Exception e)
            {
                Helper.LogError("WorkflowParser.CleanUp", string.Empty, e, false);
                Trace.WriteLine("General Exception: " + e.Message);
            }

            Trace.WriteLine("Leaving WorkflowParserATK.CleanUp() function.");
        }

        protected void BackgroundCheck(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.BackgroundCheck() function.");

            string message = string.Empty;            

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
            }

            int retVal = (DateTime.Now - Helper.LastBKG).CompareTo(new TimeSpan(0, 0, Helper.CurrentUser.BackgroundTTL));
            //-1    The difference is shorter than the BackgroundTTL value.
            // 0    The difference is equal to the BackgroundTTL value.
            // 1    The difference is longer than the BackgroundTTL value -or- BackgroundTTL value is null.

            Trace.WriteLine("BackgroundTTL: " + Helper.CurrentUser.BackgroundTTL);
            Trace.WriteLine("Now: " + DateTime.Now);
            Trace.WriteLine("Last Background Taken: " + Helper.LastBKG.ToString());
            Trace.WriteLine("Comparison Result: " + retVal);

            if (retVal > 0)
            {
                Trace.WriteLine("Background is expired");
                //_isBkgExpired = true;
            }            

            Trace.WriteLine("Leaving WorkflowParserATK.BackgroundCheck() function.");
        }

        protected void ScanNewBackground(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.ScanNewBackground() function.");

            string message = string.Empty;
            string parameters = string.Empty;
            string faultmessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "parameters")
                {
                    parameters = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }
            
            // Check if Agilent Device is ready
            var sbResult = new StringBuilder(1023);
            Trace.WriteLine("Invoking DiagnosticCheck Direct Command: message = " + message + ", parameters= " +
                            parameters);

            bool success = _agilentToolkit.DiagnosticCheck(parameters, sbResult);

            // EOS change
            //if(bool.Parse(ConfigurationManager.AppSettings["NewCleanBkg"]))
               // ScanNewCleanBackground();

            // valResult = 1 is successful, valResult = 0 is failure
            int valResult = 0;
            try
            {
                valResult = _agilentToolkit.SetComputeParams(PHASEPOINTS.PP_512, PHASETYPE.PT_MERTZ, APODTYPE.APOD_TRIANGULAR, APODTYPE.APOD_HAPPGENZEL, ZFFTYPE.ZFF_NONE, OFFSETCORRECTTYPE.OT_NONE);
                if (valResult != 1)
                {
                    CancelWorkflow("-1", message);
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "An error occurred on SetComputeParams in ScanNewBackground"));
                }
            }
            catch (Exception e)
            {
                Helper.LogError("WorkflowParserATK.ScanNewBackground()", "", e, false);
                CancelWorkflow("-1", message);
            }

            Trace.WriteLine("Waiting ...");
            Thread.Sleep(1000);
            Trace.WriteLine("scan new background ...");

            bool tumbIIR = true;

            if (Helper.CurrentSpectrometer.Name.Equals("Agilent 5500t"))
            {
                tumbIIR = _agilentToolkit.CheckTumbIIR();
            }

            if (tumbIIR == false)
            {
                CancelWorkflow();
                OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "Check TumbIIR position"));
                return;
            }

            try
            {
                int numScans = Convert.ToInt32(ConfigurationManager.AppSettings["agil_backgroundScans"]);
                int res = Convert.ToInt32(ConfigurationManager.AppSettings["agil_res"]);

                valResult = _agilentToolkit.StartSingleBeam(numScans, 600, 4000, res, true, false);
                if (valResult <= 0)
                {
                    CancelWorkflow("-1", message);
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "An error occurred on StartSingleBeam in ScanNewBackground"));
                }
            }
            catch (Exception e)
            {
                Helper.LogError("WorkflowParserATK.ScanNewBackground()", "", e, false);
                CancelWorkflow("-1", message);
                OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "An error occurred on StartSingleBeam in ScanNewBackground"));
            }

            Trace.WriteLine("Exiting WorkflowParserATK.ScanNewBackground() function.");
        }
        
        private Result GetAverage(Result result)
        {

            DataService.InstrumentServiceClient serviceClient = null;            
            DataService.ResultDetail rDetail = new DataService.ResultDetail();
            Helper.RdWrapper = new RDCriteriaWrapper();
            RDDataWrapper rdData = new RDDataWrapper();
            Helper.RdWrapper.Locations = new List<LocationCriteria>();
            LocationCriteria rdLocaction = new LocationCriteria();
            int dCount = 0;
            int traitCount = 0;
            bool isServiceClosed = false;
            try
            {
                serviceClient = Helper.GetServiceInstance();
                rdLocaction.LocationId = Helper.CurrentUser.Location.LocationId;
                rdLocaction.CompanyId = Helper.CurrentUser.Company.CompanyId;
                Helper.RdWrapper.Locations.Add(rdLocaction);
                Helper.RdWrapper.SelectedCategory = Helper.currentSample.CategoryId;
                Helper.RdWrapper.SelectedMaterial = Helper.currentSample.MaterialId;
                Helper.RdWrapper.SelectedSampleClass = Helper.currentSample.SampleTypeName;                
                int numberOfScans = int.Parse(System.Configuration.ConfigurationManager.AppSettings["AverageScanScans"]);

                rdData = serviceClient.GetTrendData(Helper.RdWrapper, Convert.ToInt16(numberOfScans));
                   
                serviceClient.Close();                                       
                double avgVal = 0.0;
                //List<decimal> res = new List<decimal>();
     
                var rrDetails = (from r in rdData.RDDetailDataList
                                 where !r.DisplayText.Contains("Outlier") || !r.DisplayText.Contains('>') || !r.DisplayText.Contains('<')
                                        select r).ToList();

                traitCount = rrDetails.Count;
                foreach (var detail in result.Details)
                {
                    var v = (from s in rrDetails                             
                             select s);

                    List<RDDetailData> test = rrDetails.Where(i => i.Trait == detail.TraitName).ToList();
                        
                    var res = (from t in test
                               select Convert.ToDouble(t.DisplayText)).ToList();

                    avgVal = res.Average();
                    Trace.WriteLine("Average value of " + rrDetails[dCount].Trait + " = " + avgVal);
                    result.Details[dCount].DisplayText = Math.Round(avgVal, 4).ToString();
                    dCount++;
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Close();

                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }        

            return result;
        }

        private void CreatePayload(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.CreatePayload() function.");

            string message = string.Empty;
            string faultmessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }

            Trace.WriteLine("Invoking Create Payload Command");

            try
            {
                // create XML payload based on the current sample scan
                string path = _spcFileName;
                try
                {
                    _spectralFile = File.ReadAllBytes("C:\\qta\\test\\test.0");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Failed to read Spectral file.");
                    Trace.WriteLine(ex.Message);

                    Helper.LogError("WorkflowParser.CreatePayload", string.Empty, ex, false);

                    // Cancel Workflow because of exception
                    CancelWorkflow("-1", message);
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", faultmessage));
                }

                if (_spectralFile != null)
                {
                    string currentDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
                    string OmFileCreateDateTime = "null";

                    // get the file creation date, assuming that there will be only one file(background file with .0 extension) at a time
                    if (File.Exists(path))
                        OmFileCreateDateTime = File.GetCreationTime(path).ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");


                    XNamespace ns = "x-schema:qta_sample_schema.xdr";

                    // get the modelgroup count
                    int modelGroupCount = Helper.CurrentSample.Traits.Count();

                    // create the list of modelgroups
                    var modelGroupAttributes = new XElement[modelGroupCount];
                    int cnt = 0;
                    foreach (var modelGroup in Helper.CurrentSample.Traits)
                    {
                        modelGroupAttributes[cnt] = new XElement(ns + "sample_model_group",
                                                                 new XAttribute("id", modelGroup.Id));
                        cnt++;
                    }

                    // get the sample identifier count
                    int ipAttribCount = Helper.CurrentSample.SampleIdentifiers.Count;

                    // create the list of sample identifiers
                    var ipAttribAttributes = new XElement[ipAttribCount];

                    for (cnt = 0; cnt < ipAttribCount; cnt++)
                    {
                        ipAttribAttributes[cnt] = new XElement(ns + "ip_attrib",
                                                               new XAttribute("attrib_id",
                                                                              Helper.CurrentSample.SampleIdentifiers[cnt
                                                                                  ].Id),
                                                               new XAttribute("field_name",
                                                                              Helper.CurrentSample.SampleIdentifiers[cnt
                                                                                  ].Name),
                                                               new XAttribute("data_type_enum",
                                                                              Helper.CurrentSample.SampleIdentifiers[cnt
                                                                                  ].Value));
                    }

                    var requestElement = new XElement(ns + "qta_sample",
                                                      new XElement(ns + "sample_data",
                                                                   new XAttribute("file_type", "Agilent"),
                                                                   new XElement(ns + "location",
                                                                                new XAttribute("id",
                                                                                               Helper.CurrentUser.
                                                                                                   Location.LocationId.
                                                                                                   ToString())),
                                                                   new XElement(ns + "hardware",
                                                                                new XAttribute("id",
                                                                                               Helper.CurrentUser.
                                                                                                   UserAccessibleSpectrometerType
                                                                                                   .SpectrometerTypeId),
                                                                                new XAttribute("serial_number",
                                                                                               Helper.CurrentUser.
                                                                                                   UserAccessibleSpectrometerType
                                                                                                   .SerialNumber),
                                                                                new XAttribute("bkgfile_createdate",
                                                                                               OmFileCreateDateTime)),
                                                                   new XElement(ns + "company",
                                                                                new XAttribute("id",
                                                                                               Helper.CurrentUser.
                                                                                                   Company.CompanyId)),
                                                                   new XElement(ns + "user",
                                                                                new XAttribute("id",
                                                                                               Helper.CurrentUser.Id)),
                                                                   new XElement(ns + "sample_option",
                                                                                new XAttribute("name",
                                                                                               Helper.CurrentSample.
                                                                                                   SampleTypeName)),
                                                                   new XElement(ns + "sample_model_groups",
                                                                                modelGroupAttributes)),
                                                      new XElement(ns + "local_datetime",
                                                                   new XAttribute("value", currentDateTime)),
                                                      new XElement(ns + "ip_tracking",
                                                                   new XElement(ns + "table_name", ipAttribAttributes)));

                    var xDeclaration = new XDeclaration("1.0", "utf-16", "yes");
                    var xDoc = new XDocument(xDeclaration, requestElement);

                    using (var sw = new StringWriter())
                    {
                        xDoc.Save(sw, SaveOptions.None);
                        xDoc.Save("C:\\QTA\\TEST\\payload.xml", SaveOptions.None);
                        _xmlData = sw.ToString();
                    }
                }
            }
            finally
            {
                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }

            Trace.WriteLine("Leaving WorkflowParserATK.CreatePayload() function.");
        }

        private void SendPayload(XmlReader inner)
        {
            //TODO: remove this when Eric is ready to start sending data to back end
            //return;
            Trace.WriteLine("Entering WorkflowParserATK.SendPayload() function.");

            string message = string.Empty;
            string faultmessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }

            DataService.InstrumentServiceClient serviceClient = new DataService.InstrumentServiceClient();
            bool isServiceClosed = false;
            try
            {
                serviceClient = Helper.GetServiceInstance();

                // submit the data for analysis and get the request id
                //
                Trace.WriteLine("Invoking InstrumentServiceClient.SendDataForAnalysis()");
                string requestId = serviceClient.SendDataForAnalysis(_xmlData, _spectralFile);
                Trace.WriteLine("Send Payload Command Complete");

                serviceClient.Close();
                isServiceClosed = true;

                if (String.IsNullOrEmpty(requestId)) // some error ocurred, display message and log the error
                {
                    Trace.WriteLine(
                        "Error: Workflow Cancelled InstrumentServiceClient.SendDataForAnalysis() Returned null/empty value.");

                    CancelWorkflow("-1", message);
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", faultmessage));

                    _isChecking = false;
                }
                else
                {
                    // set the resuest id as current request id and start the timer
                    _currentRequestId = requestId;

                    Trace.WriteLine("Current Request ID: " + _currentRequestId);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Current Request ID: {0} {1}", _currentRequestId, ex.Message));
                CancelWorkflow("-1", ex.ToString());
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Abort();

                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }

            Trace.WriteLine("Leaving WorkflowParserATK.SendPayload() function.");
        }

        protected void CheckForResults(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.CheckForResults() function.");

            string message = string.Empty;
            int waittime = 0;
            int waititeration = 0;
            string faultmessage = string.Empty;
            //Added the following for logging purposes: DAO 2/16/2012
            string userName = Helper.CurrentUser.UserName;
            string locationName = Helper.CurrentUser.Location.ToString();

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "waittime")
                {
                    waittime = Convert.ToInt32(inner.ReadString());
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "waititeration")
                {
                    waititeration = Convert.ToInt32(inner.ReadString());
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }

            while (waititeration > 0 && _isChecking)
            {
                Trace.WriteLine("Waiting ...");
                Thread.Sleep(waittime);

                Trace.WriteLine("Checking for Results ...");
                DataService.Result newResult = new Result();

                DataService.InstrumentServiceClient serviceClient = new DataService.InstrumentServiceClient();
                bool isServiceClosed = false;
                bool isSuccess = false;

                try
                {
                    serviceClient = Helper.GetServiceInstance();

                    // get the result status
                    DataService.EnumResultStatus resultStatus = serviceClient.GetResultStatus(_currentRequestId);

                    if (resultStatus == DataService.EnumResultStatus.SUCCESS)
                        // stop checking and insert the result in the local db cache
                    {
                        _isChecking = false;
                        DataService.Result result = serviceClient.GetResult(_currentRequestId);
                        bool outlier = false;
                        List<HistoryDetail> hDetails = new List<HistoryDetail>();
                        
                        foreach (var r in result.Details)
                        {
                            if (r.DisplayText == "Outlier" && Helper.IsAveragingOn)
                            {
                                outlier = true;
                            }
                        }

                        if (Helper.IsAveragingOn && !outlier)
                        {
                            newResult = GetAverage(result);

                            foreach (var r in newResult.Details)
                            {
                                HistoryDetail hD = new HistoryDetail();

                                hD.ModelGroupName = r.TraitName;
                                hD.DisplayText = r.DisplayText;
                                hD.ResultValue = Convert.ToDecimal(r.DisplayText);
                                hDetails.Add(hD);
                            }
                            bool success = serviceClient.UpdateHistoryDetails(Convert.ToInt32(result.RequestId), hDetails);
                            Trace.WriteLine("History Update " + success.ToString());
                            // insert results and record sample identifier in the recordedSampleIdentifier table
                            isSuccess = DAL.InsertResults(newResult, Helper.CurrentSample.MaterialId);
                        }
                        else
                        {
                            // insert results and record sample identifier in the recordedSampleIdentifier table
                            isSuccess = DAL.InsertResults(result, Helper.CurrentSample.MaterialId);
                        }

                        if (!isSuccess)
                        {
                            //Cancel Workflow because of exception
                            CancelWorkflow("-1", message);
                            OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1",Helper.ERROR_RESULT_LOCAL_INSERT));
                        }
                    }
                    else if (resultStatus == DataService.EnumResultStatus.FAILURE)
                        // log the error and display error message
                    {
                        //stop checking
                        _isChecking = false;

                        // Get the error message and then log the error
                        DataService.Error error = serviceClient.GetResultError(_currentRequestId);
                        serviceClient.LogError(error.ErrorMessage, string.Empty, error.ErrorCode.ToString(),
                                               "CheckForResults", "_currentRequestId=" + _currentRequestId);

                        //Cancel Workflow because of exception
                        CancelWorkflow("-1", message);
                        OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", faultmessage));
                    }

                    serviceClient.Close();
                    isServiceClosed = true;
                }
                catch (Exception ex)
                {
                    //stop checking
                    _isChecking = false;
                    Helper.LogError("Workflow.CheckForResults", "_currentRequestId=" + _currentRequestId, ex, false);
                    CancelWorkflow("-1", ex.ToString());
                }
                finally
                {
                    if (!isServiceClosed)
                        serviceClient.Abort();

                    if (Helper.ContextScope != null)
                        Helper.ContextScope.Dispose();

                    waititeration--;

                    if (waititeration == 0)
                        // Time Out if still no results after roughly (waititeration * waittime) milliseconds
                    {
                        _isChecking = false;
                        Helper.LogError("WorkflowParserATK.CheckForResults", "",
                                        ResourceHelper.Error_10901 + " : Timeout-Request Number: " + _currentRequestId +
                                        " User: " + userName + " Location: " + locationName, string.Empty, "10901");

                        //Cancel Workflow because of exception
                        CancelWorkflow("-1", message);
                        OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", Helper.ERROR_SCAN_TIME_OUT));
                    }
                }
            }

            Trace.WriteLine("Leaving  .CheckForResults() function.");
        }

        private void InstrumentInit(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.InstrumentInit() function.");

            Trace.WriteLine(
                "Thread Name: " + Thread.CurrentThread.Name +
                ", ThreadID: " + Thread.CurrentThread.ManagedThreadId +
                ", Background Thread: " + Thread.CurrentThread.IsBackground);

            while (inner.Read())
            {
                if (!_isWorkflowCancelled)
                {
                    if (inner.IsStartElement("diagnosticcheck"))
                    {
                        XmlReader diag = inner.ReadSubtree();
                        DiagnosticCheck(diag);
                        diag.Close();
                        continue;
                    }
                    if (inner.IsStartElement("getserialnumber"))
                    {
                        XmlReader diag = inner.ReadSubtree();
                        GetSerialNumber(diag);
                        diag.Close();
                        continue;
                    }
                }
            }
            Trace.WriteLine("Leaving WorkflowParserATK.InstrumentInit() function.");
            return;
        }

        private void GetSerialNumber(XmlReader inner)
        {
            Trace.WriteLine("Entering GetSerialNumber.SendPayload() function.");

            string message = string.Empty;
            string faultmessage = string.Empty;
            var returnVal = 0;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }

            try
            {
                
                Trace.WriteLine("Entering try block in GetSerialNumber.SendPayload() function.");
                //TODO: GetVersionEx is returning 1.  The docs say 0 is success.  Nothing is mentioned about 1.
                returnVal = _agilentToolkit.GetVersionEx(new MLVersion());
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Entering catch block in GetSerialNumber.SendPayload() function.");
                Helper.LogError("GetSerialNumber", "", ex, false);
                CancelWorkflow("-1", ex.ToString());
            }
            switch (returnVal)
            {
                case 0:
                    break;
                case -1:
                    CancelWorkflow("-1", message);
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "Instrument Not Connected"));
                    break;
                default:
                    CancelWorkflow("-1", message);
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "Unknown error in GetVersionEx"));
                    break;
            }

            Trace.WriteLine("Exiting GetSerialNumber.SendPayload() function.");

        }

        private void PreScanProcessing(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.PreScanProcessing() function.");

            //Added by SD on 05/27
            Trace.WriteLine(
                "Thread Name: " + Thread.CurrentThread.Name +
                ", ThreadID: " + Thread.CurrentThread.ManagedThreadId +
                ", Background Thread: " + Thread.CurrentThread.IsBackground);

            while (inner.Read())
            {
                if (!_isWorkflowCancelled)
                {
                    if (inner.IsStartElement("cleanup"))
                    {
                        XmlReader cleanup = inner.ReadSubtree();
                        CleanUp(cleanup);
                        cleanup.Close();
                        continue;
                    }
                }
                else
                    break;
            }
            Trace.WriteLine("Leaving WorkflowParserATK.PreScanProcessing() function.");
        }

        private void ScanBackground(XmlReader inner, bool BkgCheckOnly)
        {
            Trace.WriteLine("Entering WorkflowParserATK.ScanBackground() function.");

            //Added by SD on 05/27
            Trace.WriteLine(
                "Thread Name: " + Thread.CurrentThread.Name +
                ", ThreadID: " + Thread.CurrentThread.ManagedThreadId +
                ", Background Thread: " + Thread.CurrentThread.IsBackground);

            OnRaiseMessageEvent(new MessageEventArgs(ResourceHelper.WFScanBackgroundMessage));

            while (inner.Read())
            {
                if (inner.IsStartElement("dobackgroundcheck"))
                {
                    XmlReader backgnd = inner.ReadSubtree();
                    BackgroundCheck(backgnd);
                    backgnd.Close();
                    continue;
                }

                if (inner.IsStartElement("scannewbackground") && BkgCheckOnly == false)
                {
                    XmlReader newbkg = inner.ReadSubtree();
                    ScanNewBackground(newbkg);
                    newbkg.Close();
                    continue;
                }
                if (inner.IsStartElement("cleancheck"))
                {
                    XmlReader clnchk = inner.ReadSubtree();
                    CleanCheck(clnchk);
                    clnchk.Close();
                    continue;
                }
            }

            Trace.WriteLine("Leaving WorkflowParserATK.ScanBackground() function.");
        }


        protected void SamplePlacementMessage(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.SamplePlacementMessage() function.");

            string message = string.Empty;
            string uimessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "uimessage")
                {
                    uimessage = inner.ReadString();
                    continue;
                }
            }

            if (!String.IsNullOrEmpty(message))
                OnRaiseUIMessageEvent(new UIMessageEventArgs(uimessage));

            Trace.WriteLine("Leaving WorkflowParserATK.SamplePlacementMessage() function.");
        }

        //Code from WorkflowParserATK

        protected void CleanCheck(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.CleanCheck() function.");

            string message = string.Empty;
            string peak1 = string.Empty;
            string peak2 = string.Empty;
            string highLimit = string.Empty;
            string lowLimit = string.Empty;
            string faultmessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "peak1")
                {
                    peak1 = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "peak2")
                {
                    peak2 = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "highLimit")
                {
                    highLimit = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "lowLimit")
                {
                    lowLimit = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }
            

            //if (!String.IsNullOrEmpty(message))
            //    OnRaiseMessageEvent(new MessageEventArgs(message));

            var sbResult = new StringBuilder(1023);

            //parameters = parameters.Replace("_CLEANPARAMVALUE_", Helper.CurrentUser.CleanParam.ToString());
            //parameters = parameters.Replace("_XPMFILEPARAMVALUE_", _files.FirstOrDefault() ?? "");

            Trace.WriteLine("Invoking CleanCheck Command");

            var success = _agilentToolkit.CleanCheck(peak1, peak2,highLimit,lowLimit, sbResult);
            //sbResult.Append(success ? "pass" : "fail");

            Trace.WriteLine("Command Ran Successfully: " + success);
            Trace.WriteLine("Result Value: " + sbResult);

            if (sbResult.ToString().Contains("Dirty Reading"))
            {
                string errorCode = FormatErrorCode(sbResult);
                Helper.LogError("Clean Check", "", ResourceHelper.Error_10607 + " : " + errorCode, string.Empty, "10607");
                CancelWorkflow("-1", message);
                OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", faultmessage, errorCode));
            }
            else if (success == false)
            {                
                CancelWorkflow();
                //OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", faultmessage));
            }


            Trace.WriteLine("Leaving WorkflowParserATK.CleanCheck() function.");
        }


        private static string FormatErrorCode(StringBuilder sbResult)
        {
            return sbResult.ToString();
        }

        private void ScanSample(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.ScanSample() function.");
            bool Average = Helper.IsAveragingOn;
            OnRaiseMessageEvent(new MessageEventArgs(ResourceHelper.WFScanSampleMessage));

            while (inner.Read())
            {
               
                if (inner.IsStartElement("sampleplacementmessage"))
                {
                    XmlReader spmsg = inner.ReadSubtree();
                    SamplePlacementMessage(spmsg);
                    spmsg.Close();
                    continue;
                }
                if (inner.IsStartElement("samplescan"))
                {
                    XmlReader sampscan = inner.ReadSubtree();
                    SampleScan(sampscan);
                    sampscan.Close();
                    continue;
                }
                if (inner.IsStartElement("savescan"))
                {
                    XmlReader savscan = inner.ReadSubtree();
                    SaveScan(savscan);
                    savscan.Close();
                    continue;
                }
            }

            Trace.WriteLine("Leaving WorkflowParserATK.ScanSample() function.");
        }

        protected void SampleScan(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.SampleScan() function.");

            string message = string.Empty;
            string parameters = string.Empty;
            string faultmessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "parameters")
                {
                    parameters = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }
            
            RunSample(parameters, message, faultmessage);           

            Trace.WriteLine("Leaving WorkflowParserATK.SampleScan() function.");
        }

        protected void RunSample(string parameters, string message, string faultmessage)
        {
            var sbResult = new StringBuilder(1023);
            // Check if Agilent Device is ready
            _spcFileName = parameters;

            bool tumbIIR = true;

            if (Helper.CurrentSpectrometer.Name.Equals("Agilent 5500t"))
            {
                tumbIIR = _agilentToolkit.CheckTumbIIR();
            }

            if (tumbIIR == false)
            {
                Thread.Sleep(1000);
                tumbIIR = _agilentToolkit.CheckTumbIIR();
                if (tumbIIR == false)
                {
                    CancelWorkflow();
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "Check TumbIIR position"));
                    return;
                }
            }

            // valResult = 1 is successful, valResult = 0 is failure
            int valResult = 0;
            try
            {
                valResult = _agilentToolkit.SetComputeParams(PHASEPOINTS.PP_512, PHASETYPE.PT_MERTZ, APODTYPE.APOD_TRIANGULAR, APODTYPE.APOD_HAPPGENZEL, ZFFTYPE.ZFF_NONE, OFFSETCORRECTTYPE.OT_NONE);
                if (valResult != 1)
                {
                    CancelWorkflow("-1", message);
                    OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "An error occurred on SetComputeParams in ScanSample"));
                    return;
                }
            }
            catch (Exception e)
            {
                Helper.LogError("WorkflowParserATK.ScanSample()", "", e, false);
                CancelWorkflow("-1", message);
                OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", "An error occurred on SetComputeParams in ScanSample"));
                return;
            }

            // Check when spectra is ready            
            Trace.WriteLine("Waiting ...");
            Trace.WriteLine("CheckProgressStruct ...");

            int numScans = Convert.ToInt32(ConfigurationManager.AppSettings["agil_numScans"]);
            int res = Convert.ToInt32(ConfigurationManager.AppSettings["agil_res"]);
            
            // Delay to allow unit signal to settle            
            Thread.Sleep(5000);

            varResult = _agilentToolkit.StartSpectrum(numScans, 650, 4000, res, DATAXTYPE.XT_WN, DATAYTYPE.YT_Abs, true);
            int currentUnits = 0;
            int totalUnits = 0;
            FTIR_STATE progress = FTIR_STATE.FTIR_Collecting;
            do
            {
                progress = _agilentToolkit.CheckProgress(ref currentUnits, ref totalUnits);
            } while (progress == FTIR_STATE.FTIR_Collecting);

            if (varResult < 0)
            {
                var startSpectrumResult = "";
                switch (varResult)
                {
                    case -3:
                        startSpectrumResult = "No background data is present";
                        break;
                    case -4:
                        startSpectrumResult = "Incompatible background data found";
                        break;
                    case -1:
                        startSpectrumResult = "Scan cannot be started";
                        break;
                }
                CancelWorkflow("-1", message);
                OnRaiseCancelWorkflowEvent(new CancelWorkflowEventArgs("-1", faultmessage, string.Format("Bad return values from StartSpectrum: {0}", startSpectrumResult)));
            }

            // Check when spectra is ready            
            Trace.WriteLine("Waiting ...");
            Thread.Sleep(100);
            Trace.WriteLine("CheckProgressStruct ...");

            // Spectra size
            double[] array1 = new double[2000];
            double actualFrom = 650;
            double actualTo = 4000;
            int actualRes = 8;

            varResult = _agilentToolkit.GetSpectrum(null, 2000, ref actualFrom, ref actualTo, ref actualRes);

            // Check when spectra is ready            
            Trace.WriteLine("Waiting ...");
            Thread.Sleep(100);
            Trace.WriteLine("CheckProgressStruct ...");

            double[] d = new double[varResult];
            varResult = _agilentToolkit.GetSpectrum(d, varResult, ref actualFrom, ref actualTo, ref actualRes);
            double[] waveNum = new double[varResult];
            double waveInc = (actualTo - actualFrom) / (varResult - 1);
            int cnt = 0;
            waveNum[0] = actualFrom;   //EOS changed
            for (cnt = 1; cnt < varResult; cnt++)
            {
                waveNum[cnt] = waveNum[cnt - 1] + waveInc;
            }

            cnt = 0;
            string[] createText = new string[varResult];
            byte[] createByte;
            string testString = string.Empty;

            for (cnt = 0; cnt < varResult; cnt++)
            {
                createText[cnt] = (Math.Round(waveNum[cnt], 6)).ToString() + " " + (Math.Round(d[cnt], 6)).ToString();
                createByte = GetBytes(createText[cnt]);
                testString += createText[cnt] + System.Environment.NewLine;
            }

            if (!Directory.Exists(@"c:\qta\test"))
            {
                Directory.CreateDirectory(@"c:\qta\test");
            }
            File.WriteAllLines(_spcFileName, createText);
            var bytes = System.Text.Encoding.UTF8.GetBytes(testString);

            File.WriteAllBytes(@"c:\qta\test\test.0", bytes);
            File.WriteAllText(@"c:\qta\test\test.txt", System.Text.Encoding.UTF8.GetString(bytes));
        }

        protected void SaveScan(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.SaveScan() function.");

            string message = string.Empty;
            string parameters = string.Empty;
            string faultmessage = string.Empty;

            while (inner.Read())
            {
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "message")
                {
                    message = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "parameters")
                {
                    parameters = inner.ReadString();
                    continue;
                }
                if (inner.MoveToContent() == XmlNodeType.Element && inner.Name == "faultmessage")
                {
                    faultmessage = inner.ReadString();
                    continue;
                }
            }

            /*Trace.WriteLine("Invoking SaveScan Command: parameters= " + parameters);

            var success = AgilentTalk.SaveScan(parameters);

            Trace.WriteLine("Command Ran Successfully: " + success);
            Trace.WriteLine("Leaving WorkflowParserATK.SaveScan() function.");*/
        }


        private void PreResultsProcessing(XmlReader inner)
        {
            Trace.WriteLine("Entering WorkflowParserATK.PreResultsProcessing() function.");

            //Added by SD on 05/27
            Trace.WriteLine(
                "Thread Name: " + Thread.CurrentThread.Name +
                ", ThreadID: " + Thread.CurrentThread.ManagedThreadId +
                ", Background Thread: " + Thread.CurrentThread.IsBackground);

            OnRaiseMessageEvent(new MessageEventArgs(ResourceHelper.WFPreResultsMessage));

            while (inner.Read())
            {
                if (inner.IsStartElement("createpayload"))
                {
                    XmlReader crepld = inner.ReadSubtree();
                    CreatePayload(crepld);
                    crepld.Close();
                    continue;
                }
                if (inner.IsStartElement("sendpayload"))
                {
                    XmlReader senpld = inner.ReadSubtree();
                    SendPayload(senpld);
                    senpld.Close();
                    continue;
                }
                if (inner.IsStartElement("checkforresults"))
                {
                    XmlReader chkres = inner.ReadSubtree();
                    CheckForResults(chkres);
                    chkres.Close();
                    continue;
                }
            }

            Trace.WriteLine("Leaving WorkflowParserATK.PreResultsProcessing() function.");
        }
    }
}
