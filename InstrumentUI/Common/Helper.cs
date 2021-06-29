using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Forms;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.DataAccess.Model;
using InstrumentUI_ATK.DataService;
using InstrumentUI_ATK.ModalForm;
using InstrumentUI_ATK.Workflow;
using SampleIdentifier = InstrumentUI_ATK.DataAccess.SampleIdentifier;
using Trait = InstrumentUI_ATK.DataAccess.Trait;
using User = InstrumentUI_ATK.DataService.User;
using System.IO;
using System.Text;

namespace InstrumentUI_ATK.Common
{
    public class Helper
    {
        #region Properties

        public static bool AverageSampleCleanCheck = Convert.ToBoolean(ConfigurationManager.AppSettings["AverageScanCleanCheck"]);
        public static System.Xml.XmlReader CleanCheckXml;              

        public static readonly string PATH_QTA = "C:\\QTA\\InstrumentUI_ATK\\"; //Application.StartupPath; //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Application.ProductName + "\\";
        public static string SupportPath = PATH_QTA + "TeamViewerQS_en.exe";
        public static readonly string CONNECTION_STRING = "Data Source=" + Application.StartupPath + "\\DataAccess\\InstrumentUIDB.sdf;Password=admin123";
        public static readonly string FOLDER_PATH_FILES = "C:\\QTA\\InstrumentUI_ATK\\Files\\";
        public static readonly string FILE_PATH_SPECTRAL = "C:\\QTA\\TEST\\TEST.000"; //FOLDER_PATH_FILES + "S1234567890.0";
        public static readonly string FILE_PATH_ANTARIS = "C:\\QTA\\TEST\\TEST.txt";
        public static readonly string FILE_PATH_ZEISS = "C:\\QTA\\TEST\\TEST.txt";
        public static readonly string ANTARIS_FILE_TYPE = "Antaris";        
        public static readonly string MACRO_FOLDER_PATH = "C:\\qta\\macro\\";
        public static readonly string UPDATE_DOWNLOAD_FILE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\InstrumentUI_ATK.msi";

        public const string COLON = " :";
        public const string SAMPLE_CLASS_ROUTINE = "Routine";
        public const string SAMPLE_CLASS_IDENTIFY = "Identify";
        public const string SAMPLE_CLASS_SCHEDULED = "Scheduled";
        public const string SAMPLE_CLASS_AUTOSAMPLER = "AutoSampler";
        public const string SAMPLE_CLASS_CHECK = "Check";
        public const string SAMPLE_CLASS_VALIDATION = "Validation";
        public const string SAMPLE_CLASS_FEASIBILITY = "Feasibility";
        public const string SAMPLE_CLASS_TEST = "Test";
        public const string SAMPLE_CLASS_ROUNDROBIN = "RoundRobin";
        public const string SAMPLE_CLASS_DEMO = "Demo";

        public const string CULTURE_NAME_ENGLISH = "en-US";
        public const string CULTURE_NAME_SPANISH = "es-ES";

        public const string ERROR_INSTRUMENT_UI = "Instrument UI";
        public const string ROLE_INSTRUMENT_USER = "instrument_user";
        public const string MODEL_STAGE_ROUTINE = "commercialized Q2";

        public const string ERROR_INVALID_LOCAL_DATA = "Data in the local cache is not valid.";
        public const string ERROR_SCAN_TIME_OUT = "A time out error occurred while analyzing sample.";
        public const string ERROR_RESULT_LOCAL_INSERT = "An error occurred while inserting results in the local db cache.";

        public const string CONTACT_NUMBER = "ContactNumber";
        public const string DEFAULT_REPORT_DIRECTORY = "DefaultReportDirectory";
        public const string DEFAULT_LANGUAGE = "DefaultLanguage";
        public const string INFO_URL = @"";
        
        public const string REPORT_CofA_PDF = "CofA PDF";
        public const string REPORT_CofA_WORD = "CofA Word";
        public const string REPORT_SINGLE_ANALYSIS_PDF = "Single Analysis PDF";
        public const string REPORT_SINGLE_ANALYSIS_WORD = "Single Analysis Word";
        public const string REPORT_DEFAUT_CofA_PDF = "CofA PDF";
        public const string CLEANBKGFILE = "Cleanbkg.000";
        public const string CLEANBKGFILEPATH = "C:\\QTA\\macro\\";

        private const string USER_NAME = "UserName";
        private const string InstrumentService_NS = "";
        public const string QTA_URL = "http://qta.com/";
        private const string SPECTROMETER_TYPE = "SpectrometerType";
        private const string LOCATION = "Location";

        public static DataService.RDCriteriaWrapper RdWrapper;
        public static decimal recordCount = 3;
        public static int averageTerm = 3;        
        public static double[] y1;
        public static double[] avgY;
        public static double[] x1;
        public static double[] avgX;
        public static Sample currentSample;

        private static Dictionary<int, IEnumerable<SampleIdentifier>> _sampleIdentifiers;
        private static Dictionary<int, string> _materialCodes;
        public static User CurrentUser { get; set; }
        public static DataAccess.SpectrometerType CurrentSpectrometer { get; set; }
        public static List<Trait> CurrentTraits { get; set; }
        public static List<Trait> SelectedTraits { get; set; }
        public static List<DataAccess.SampleClass> CurrentSampleClasses { get; set; }
        public static Main CurrentOwner { get; set; }
        public static Sample CurrentSample { get; set; }
        public static OperationContextScope ContextScope { get; set; }
        public static FormControls.IFormControl CurrentForm { get; set; }
        public static string SpectralFileData { get; set; }
        public static bool SQLError { get; set; }
        public static DateTime LastBKG = DateTime.Now.AddDays(-1);
        public static string SpectromterType { get; set; }
        public static int MultiScanRunCount { get; set; }
        public static string AnalyzeType { get; set; }
        public static bool StartUpBkgScan { get; set; }
        public static string SampleID { get; set; }
        public static string AnalysisID { get; set; }
        public static string szMaterial { get; set; }
        public static string szTrait { get; set; }
        public static bool CancelAverage;       
        public static int AverageCurrentSample {get; set;}
        public static bool DisplayCancelMessage = true; // Used because wf_Parser_RaiseCancelWorkflowEvent sets isCancelled = true;
        public static int CleanCheckFailCount { get; set; }
        public static int CleanCheckLimit { get; set; }
        public static bool CleanCheckFail;
        public static bool NewCleanBkg;
        public static bool IsAverageScanCleanCheckOn;
        public static bool IsAveragingOn { get; set; }        
        public static double ChartOffset { get; set; }        
        public static int traitId { get; set; }

        
        public static WorkflowParser Parser;

        public static bool SpeedModeDualScan
        {
            get
            {
                if (_speedModeDualScan.HasValue)
                {
                    return _speedModeDualScan.Value;
                }
                else
                {
                    var adminPreference = DAL.GetAdminPreference();
                    if (adminPreference != null)
                        _speedModeDualScan = adminPreference.SpeedModeDualScan;
                    else
                        _speedModeDualScan = false;

                    return _speedModeDualScan.Value;
                }
            }
            set
            {
                _speedModeDualScan = value;
            }
        }
        private static bool? _speedModeDualScan;

        
        public static bool SpeedMode
        {
            get
            {
                if (_speedMode.HasValue)
                {
                    return _speedMode.Value;
                }
                else
                {
                    var adminPreference = DAL.GetAdminPreference();
                    if (adminPreference != null)
                        _speedMode = adminPreference.SpeedMode;
                    else
                        _speedMode = false;

                    return _speedMode.Value;
                }
            }
            set
            {
                _speedMode = value;
            }
        }
        private static bool? _speedMode;


        public static bool IsAutoSampleId
        {
            get
            {
                if (_isAutoSampleId.HasValue)
                {
                    return _isAutoSampleId.Value;
                }
                else
                {
                    var adminPreference = DAL.GetAdminPreference();
                    if (adminPreference != null)
                        _isAutoSampleId = adminPreference.AutoSampleId;
                    else
                        _isAutoSampleId = false;

                    return _isAutoSampleId.Value;
                }
            }
            set
            {
                _isAutoSampleId = value;
            }
        }
        private static bool? _isAutoSampleId;

        private static bool _isBeepOn = true;


        /// <summary>
        /// The Queue containing the scan waiting to be run
        /// </summary>
        public static ScheduleQueue ScanQueue { get; set; }

        #endregion


        /// <summary>
        /// Create a new instance of the Helper class
        /// </summary>
        static Helper()
        {
            IsAveragingOn = false;            
            ScanQueue = new ScheduleQueue();
            StartUpBkgScan = true;
        }


        /// <summary>
        /// Create a service object and return it in open state.
        /// </summary>
        /// <returns></returns>
        public static InstrumentServiceClient GetServiceInstance()
        {
            // create service object
            var serviceClient = new InstrumentServiceClient();

            // open connection if not open
            if (serviceClient.State != CommunicationState.Opened || 
                serviceClient.State != CommunicationState.Opening)
            {
                try
                {
                    serviceClient.Open();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Exception: " + ex);
                    DisplayError("Login Failed");
                    Application.Exit();
                }
            }

            if (OperationContext.Current == null)
            {
                ContextScope = new OperationContextScope(serviceClient.InnerChannel);
            }

            if (CurrentUser != null)
            {
                MessageHeader customHeader = MessageHeader.CreateHeader(USER_NAME, InstrumentService_NS, CurrentUser.UserName);
                OperationContext.Current.OutgoingMessageHeaders.Add(customHeader);

                if (CurrentUser.UserAccessibleSpectrometerType != null)
                {
                    customHeader = MessageHeader.CreateHeader(SPECTROMETER_TYPE, InstrumentService_NS, CurrentUser.UserAccessibleSpectrometerType.SpectrometerTypeName);
                    OperationContext.Current.OutgoingMessageHeaders.Add(customHeader);
                }

                if (CurrentUser.Location != null)
                {
                    customHeader = MessageHeader.CreateHeader(LOCATION, InstrumentService_NS, CurrentUser.Location.LocationName);
                    OperationContext.Current.OutgoingMessageHeaders.Add(customHeader);
                }
            }

            return serviceClient;
        }


        /// <summary>
        /// Retreive the list of Sample Identifiers for the specified Material ID
        /// </summary>
        /// <param name="materialId">Material ID</param>
        /// <returns>List of Sample Identifiers</returns>
        public static IEnumerable<SampleIdentifier> GetSampleIdentifiers(int materialId)
        {
            Trace.WriteLine("Enter Helper.GetSampleIdentifiers()");

            if (_sampleIdentifiers == null)
                _sampleIdentifiers = new Dictionary<int, IEnumerable<SampleIdentifier>>();

            if (!_sampleIdentifiers.ContainsKey(materialId))
            {
                var sampleIdentifiers = DAL.GetSampleIdentifiersByMaterial(materialId);
                _sampleIdentifiers.Add(materialId, sampleIdentifiers);
            }

            Trace.WriteLine("Exit Helper.GetSampleIdentifiers()");

            return _sampleIdentifiers[materialId];
        }


        /// <summary>
        /// Retreive the Material Code for the specified Material ID
        /// </summary>
        /// <param name="materialId">Material ID</param>
        /// <returns>Material Code</returns>
        public static string GetMaterialCode(int materialId)
        {
            if (_materialCodes == null)
                _materialCodes = new Dictionary<int, string>();

            if (!_materialCodes.ContainsKey(materialId))
            {
                var materialCode = DAL.GetMaterialCode(materialId);

                _materialCodes.Add(materialId, materialCode);
            }

            return _materialCodes[materialId];
        }


        /// <summary>
        /// Display a Message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="showAlertIcon"></param>
        public static void DisplayMessage(string title, string message, bool showAlertIcon)
        {
            DisplayMessage(title, message, showAlertIcon, false);
        }


        /// <summary>
        /// Display a Message with the Cancel option
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="showAlertIcon"></param>
        /// <param name="useAlternateBackground"></param>
        /// <param name="cancelButton"></param>
        /// <param name="cancelAction"></param>
        public static void DisplayMessageWithCancel(string title, string message, bool showAlertIcon, bool useAlternateBackground, bool cancelButton, Action cancelAction)
        {
            try
            {
                MakeAlarmSound();

                var messageDialog = new MessageDialog(title, message, showAlertIcon, true, cancelAction);

                if (useAlternateBackground)
                    messageDialog.UseAlternateBackground();

                if (CurrentOwner == null)
                    messageDialog.ShowDialog();
                else
                    ShowDialog(CurrentOwner, messageDialog);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error caught in Helper.DisplayMessage() error: " + ex.Message, "");
                LogError("", "", "Error caught in Helper.DisplayMessage() error: " + ex.Message, "");
            }
        }


        /// <summary>
        /// Display message
        /// </summary>
        public static void DisplayMessage(string title, string message, bool showAlertIcon, bool useAlternateBackground)
        {
            try
            {
                MakeAlarmSound();

                var messageDialog = new MessageDialog(title, message, showAlertIcon);

                if (useAlternateBackground)
                    messageDialog.UseAlternateBackground();

                if (CurrentOwner == null)
                    messageDialog.ShowDialog();
                else
                    ShowDialog(CurrentOwner,messageDialog);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error caught in Helper.DisplayMessage() error: " + ex.Message, "");
                LogError("", "", "Error caught in Helper.DisplayMessage() error: " + ex.Message, "");
            }
        }


        /// <summary>
        /// Checks if the current thread is the one that main was started on and calls a threadsafe instance of ShowDialog
        /// </summary>
        /// <param name="main">Main</param>
        /// <param name="dialog">MessageDialog to display</param>
        private static void ShowDialog(Main main, MessageDialog dialog)
        {
            if (main.InvokeRequired)
                main.Invoke(new ShowDialogDelegate(InternalShowDialog), new object[] { main, dialog });
            else
                InternalShowDialog(main, dialog);
        }


        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <param name="main">Main</param>
        /// <param name="dialog">MessageDialog to display</param>
        private static void InternalShowDialog(Main main, MessageDialog dialog)
        {
            dialog.ShowDialog(main);
        }


        /// <summary>
        /// Display error message
        /// </summary>
        public static void DisplayError(string title, string errorMessage)
        {
            DisplayMessage(title, errorMessage, true, false);
        }


        /// <summary>
        /// Display error message
        /// </summary>
        public static void DisplayError(string title, string errorMessage, bool useAlternateBackground)
        {
            MakeAlertSound();
            DisplayMessage(title, errorMessage, true, useAlternateBackground);
        }


        /// <summary>
        /// Display generic error message
        /// </summary>
        public static void DisplayError(string errorMessage)
        {
            DisplayError(string.Empty, errorMessage, false);
        }


        /// <summary>
        /// Display generic error message
        /// </summary>
        public static void DisplayError(string errorMessage, bool useAlternateBackground)
        {
            DisplayError(string.Empty, errorMessage, useAlternateBackground);
        }


        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodArgument"></param>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        /// <param name="errorNumber"></param>
        public static void LogError(string methodName, string methodArgument, string message, string stackTrace, string errorNumber)
        {
            try
            {
                InstrumentServiceClient serviceClient = null;
                bool isServiceClosed = false;
                try
                {
                    serviceClient = GetServiceInstance();
                    serviceClient.LogError(message, stackTrace, errorNumber, methodName, methodArgument);
                    serviceClient.Close();
                    isServiceClosed = true;
                }
                finally
                {
                    if (ContextScope != null)
                        ContextScope.Dispose();

                    if (!isServiceClosed)
                        serviceClient.Abort();
                }
            }
            catch (Exception)
            {
                DisplayError(ResourceHelper.Error_10005);
            }
        }


        /// <summary>
        /// It logs any error occurred in the application and returns a generic message.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodArgument"></param>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        public static void LogError(string methodName, string methodArgument, string message, string stackTrace)
        {
            try
            {
                InstrumentServiceClient serviceClient = null;
                bool isServiceClosed = false;
                try
                {
                    serviceClient = GetServiceInstance();
                    serviceClient.LogError(message, stackTrace, string.Empty,
                                            methodName, methodArgument);
                    serviceClient.Close();
                    isServiceClosed = true;
                }
                finally
                {
                    if (ContextScope != null)
                        ContextScope.Dispose();

                    if (!isServiceClosed)
                        serviceClient.Abort();
                }
            }
            catch (Exception)
            {
                DisplayError(ResourceHelper.Error_10005);
            }
        }


        /// <summary>
        /// Log the any exception and display error if the displayError is true otherwise don't display any message.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodArgument"></param>
        /// <param name="ex"></param>
        /// <param name="displayError"></param>
        public static void LogError(string methodName, string methodArgument, Exception ex, bool displayError)
        {
            //displayError
            if (ex is EndpointNotFoundException) // not connected with internet so don't log exception
            {
                DisplayError(ResourceHelper.Error_10100);
            }
            else
            {
                // Log exception which are generation by this application(FaultException are comming 
                // from Service and they are already logged)
                if (ex.GetType() != typeof(FaultException))
                {
                    if (ex is SqlCeException)
                    {
                        if (ex.Message.Contains("corrupt"))
                            LogError(methodName, methodArgument, "SqlCeException: " + ex.Message, ex.StackTrace);
                        else
                            LogError(methodName, methodArgument, "SqlCeException: The database file may be corrupted. Run the repair utility to check the database file. " + ex.Message, ex.StackTrace);
                    }
                    else
                        LogError(methodName, methodArgument, ex.Message, ex.StackTrace);
                }

                if (ex is SqlCeException && ((SqlCeException)ex).NativeError == 25046) // error code for missing database file
                    DisplayError(ResourceHelper.Error_10003);

                else if (ex is SqlCeException)
                    RepairDB(RepairDBMessage.HideAll);

                else if (displayError)
                    DisplayError(ResourceHelper.Error);
            }
        }


        /// <summary>
        /// Repaire Database
        /// </summary>
        /// <param name="DisplayErrormsg"></param>
        public static void RepairDB(RepairDBMessage DisplayErrormsg)
        {
            try
            {
                using (var sqlCeEngine = new SqlCeEngine(CONNECTION_STRING))
                {
                    sqlCeEngine.Repair(CONNECTION_STRING, RepairOption.RecoverAllPossibleRows);
                    if (sqlCeEngine.Verify())
                    {
                        sqlCeEngine.Shrink();
                        sqlCeEngine.Dispose();
                        if (DisplayErrormsg == RepairDBMessage.ShowResuts)
                            DisplayMessage("Database Repaired.", "The Database has been successfully repaired.", false);
                    }
                    else
                    {
                        sqlCeEngine.Repair(CONNECTION_STRING, RepairOption.DeleteCorruptedRows);
                        if (sqlCeEngine.Verify())
                        {
                            sqlCeEngine.Shrink();
                            sqlCeEngine.Dispose();
                            if (DisplayErrormsg == RepairDBMessage.ShowResuts)
                                DisplayMessage("Database Repaired.", "The Database has been successfully repaired.", false);
                        }
                        else
                            if (DisplayErrormsg == RepairDBMessage.ShowResuts)
                                DisplayMessage("Database Repair Failed.", "Unable to repair Database.", true);
                    }
                    if (DisplayErrormsg == RepairDBMessage.ShowError)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Repair DB Error" + ex.ToString());
            }
        }


        /// <summary>
        /// Clear Sample Identifiers and Material Codes
        /// </summary>
        public static void ClearValues()
        {
            if (_sampleIdentifiers != null)
            {
                _sampleIdentifiers.Clear();
                _sampleIdentifiers = null;
            }

            if (_materialCodes != null)
            {
                _materialCodes.Clear();
                _materialCodes = null;
            }

        }


        /// <summary>
        /// Make the computer play a sound
        /// </summary>
        private static void MakeSound(string soundFile)
        {
            if (!_isBeepOn) return;

            try
            {
                DataAccess.AdminPreference adminPreference = DAL.GetAdminPreference();
                if (null != adminPreference)
                {
                    if (adminPreference.SoundsOn)
                    {
                        var player = new SoundPlayer {SoundLocation = soundFile};
                        player.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                _isBeepOn = false; // to stop it from reocurring because LogError itself calls MakeSound method
                LogError("Helper.MakeSound", string.Empty, ex, false);
            }
        }


        /// <summary>
        /// Make the computer play the Alert sound
        /// </summary>
        public static void MakeAlertSound()
        {
            MakeSound(Environment.CurrentDirectory + "/common/soundFiles/ElectronicChime.wav");
        }


        /// <summary>
        /// Make the computer play the Alarm sound
        /// </summary>
        public static void MakeAlarmSound()
        {
            MakeSound(Environment.CurrentDirectory + "/common/soundFiles/ComputerError.wav");
        }


        /// <summary>
        /// Generate Default report
        /// </summary>
        public static string GenerateReport(ReportName reportName, string analysisID, EnumRequestIdType requestType, EnumReportFormat reportFormat, bool isOpen)
        {
            try
            {
                string defaultReportDirectory = ConfigurationManager.AppSettings[DEFAULT_REPORT_DIRECTORY];

                if (!System.IO.Directory.Exists(defaultReportDirectory))
                    System.IO.Directory.CreateDirectory(defaultReportDirectory);

                string reportFilePath = defaultReportDirectory.TrimEnd('\\') + "\\" + analysisID + "_" + DateTime.Now.ToString("MMddyyyyHHmmss");

                if (reportFormat == EnumReportFormat.WORD)
                {
                    reportFilePath += ".doc";
                }
                else if (reportFormat == EnumReportFormat.PDF)
                {
                    reportFilePath += ".pdf";
                }

                Uri url = null;

                if (reportName == ReportName.CertificateOfAnalysis)
                {
                    url = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportURL"] + "Download/CofA?analysisID=" + analysisID);
                }
                else if (reportName == ReportName.SingleAnalysis)
                {
                    url = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportURL"] + "Download/SingleAnalysis?analysisID=" + analysisID);
                }

                HttpWebRequest request = null;
                CookieContainer cookieJar = new CookieContainer();

                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieJar;
                request.Method = "GET";
                HttpStatusCode responseStatus;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    responseStatus = response.StatusCode;
                    url = request.Address;
                }

                if (responseStatus == HttpStatusCode.OK)
                {
                    UriBuilder urlBuilder = new UriBuilder(url);

                    request = (HttpWebRequest)WebRequest.Create(urlBuilder.ToString());
                    request.Referer = url.ToString();
                    request.CookieContainer = cookieJar;
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        using (StreamWriter requestWriter = new StreamWriter(requestStream, Encoding.ASCII))
                        {
                            string postData = "UserName=" + CurrentUser.UserName + "&Password=" + CurrentUser.Password + "&submit=Send";
                            requestWriter.Write(postData);
                        }

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                using (Stream output = File.OpenWrite(reportFilePath))
                                {
                                    responseStream.CopyTo(output);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Client was unable to connect!");
                }

                if (isOpen)
                {
                    Process.Start(reportFilePath);
                }

                return reportFilePath;
            }
            catch (Exception ex)
            {
                LogError("Helper.GenerateReport", "name=" + reportName.ToString() + ", requestId=" + analysisID
                                    + ", requestType=" + requestType.ToString()
                                    + ", reportFormat=" + reportFormat.ToString(), ex, true);
            }

            return null;
        }

        /// <summary>
        /// It returns the instance of workflow based on the user is demo user or normal user.
        /// </summary>
        /// <returns></returns>
        public static WorkflowParser GetWorkflowInstance()
        {

            // Check the Workflow file's SpectTypeId to see which parser to use
            Parser = new WorkflowParser();

            if (Parser.SpectTypeId.Contains("AGILENT"))
                Parser = new WorkflowParserATK();
            else if (Parser.SpectTypeId.Contains("MRT-TA"))
                Parser = new WorkflowParserATK();
            return Parser;
        }


        /// <summary>
        /// Preint the default report based on the specified analysis id
        /// </summary>
        /// <param name="analysisId"></param>
        public static void PrintResults(string analysisId)
        {
            if (!string.IsNullOrEmpty(analysisId))
            {
                var preferences = DAL.GetAdminPreference();

                ReportName reportName = ReportName.CertificateOfAnalysis;
                EnumReportFormat reportFormat = EnumReportFormat.PDF;

                if (preferences != null && !string.IsNullOrEmpty(preferences.DefaultReport))
                {
                    string defaultReport = preferences.DefaultReport;

                    if (defaultReport == REPORT_CofA_PDF)
                    {
                        reportName = ReportName.CertificateOfAnalysis;
                        reportFormat = EnumReportFormat.PDF;
                    }
                    else if (defaultReport == REPORT_CofA_WORD)
                    {
                        reportName = ReportName.CertificateOfAnalysis;
                        reportFormat = EnumReportFormat.WORD;
                    }
                    else if (defaultReport == REPORT_SINGLE_ANALYSIS_PDF)
                    {
                        reportName = ReportName.SingleAnalysis;
                        reportFormat = EnumReportFormat.PDF;
                    }
                    else if (defaultReport == REPORT_SINGLE_ANALYSIS_WORD)
                    {
                        reportName = ReportName.SingleAnalysis;
                        reportFormat = EnumReportFormat.WORD;
                    }
                }

                string filePath = GenerateReport(reportName, analysisId, EnumRequestIdType.ANALYSIS_ID, reportFormat, false);

                if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                {
                    PrintFile(filePath, "");
                }
            }
        }


        /// <summary>
        /// Sends the file to the printer choosed.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="printerPath"></param>
        public static bool PrintFile(string fileName, string printerPath)
        {
            // Check if the incomming strings are null or empty.
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            //Instantiate the object of ProcessStartInfo.
            var objProcess = new Process();
            try
            {
                //Print the file.
                objProcess.StartInfo.FileName = fileName;
                objProcess.StartInfo.Verb = "Print";
                objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                objProcess.StartInfo.UseShellExecute = true;
                objProcess.Start();

                // Return true for success.
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception and return false as failure.
                LogError("Helper.PrintFile", "fileName=" + fileName, ex, true);
                return false;
            }
            finally
            { 
                // Close the process.
                objProcess.Close();
            }
        }


        /// <summary>
        /// Display Error
        /// </summary>
        /// <param name="title"></param>
        /// <param name="errorMessage"></param>
        /// <param name="closeAction"></param>
        public static void DisplayError(string title, string errorMessage, Action closeAction)
        {
            MakeAlarmSound();

            var messageDialog = new MessageDialog(title, errorMessage, true, closeAction);

            if (CurrentOwner == null)
                messageDialog.ShowDialog();
            else
                messageDialog.ShowDialog(CurrentOwner);
            
            Parser.CancelWorkflow();
        }


        /// <summary>
        /// This will check for any updated version of the product, If exists then it downloads the installer file(.msi).
        /// </summary>
        public static void CheckApplicationUpdate()
        {
            InstrumentServiceClient serviceClient = null;
            bool isServiceClosed = false;

            try
            {
                serviceClient = GetServiceInstance();

                // get the current product version info
                UpdateInfo updateInfo = serviceClient.GetUpdateInfo();

                serviceClient.Close();
                isServiceClosed = true;

                if (!string.IsNullOrEmpty(updateInfo.FileUrl))
                {
                    var currentVersion = new Version(Application.ProductVersion);
                    var newVersion = new Version(updateInfo.Version);

                    if (currentVersion.CompareTo(newVersion) < 0)
                    {
                        var webClient = new WebClient();
                        webClient.DownloadFileCompleted += updates_DownloadFileCompleted;
                        webClient.DownloadFileAsync(new Uri(updateInfo.FileUrl), UPDATE_DOWNLOAD_FILE_PATH);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Helper.CheckApplicationUpdate", string.Empty, ex, false);
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Abort();

                if (ContextScope != null)
                    ContextScope.Dispose();
            }
        }


        /// <summary>
        /// It starts the installer file then shut down the application.
        /// </summary>
        static void updates_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Instantiate the object of ProcessStartInfo.
            var objProcess = new Process();
            try
            {
                MessageBox.Show(ResourceHelper.Product_Update_Message, ResourceHelper.Product_Update);

                // start the msi installer
                objProcess.StartInfo.FileName = UPDATE_DOWNLOAD_FILE_PATH;
                objProcess.Start();

                // exit from the application to install the updated version
                Application.Exit();
            }
            catch (Exception ex)
            {
                // Log the exception
                LogError("Helper.updates_DownloadFileCompleted", string.Empty, ex, false);
            }
            finally
            {
                // Close the process.
                objProcess.Close();
            }
        }


        private static readonly object _enqueueLock = new object();

        /// <summary>
        /// Adds the Job to the ScanQueue to be executed
        /// </summary>
        /// <param name="scanJob">ScheduleItem</param>
        public static void AddScanJobToScanQueue(ScheduleItem scanJob)
        {
            Trace.WriteLine("Enter Helper.AddScanJobToScanQueue()");

            lock (_enqueueLock)
            {
                var queueItem = new ScheduleQueueItem
                                    {
                                        Location = scanJob.LocationNumber,
                                        ScheduledTime = DateTime.Now,
                                        Sample = CreateSampleFromScheduleItem(scanJob)
                                    };

                ScanQueue.Enqueue(queueItem);

                Trace.WriteLine("A new scan job for sample location " + scanJob.LocationNumber + " has been added to the scan queue.");
            }

            Trace.WriteLine("Exit Helper.AddScanJobToScanQueue()");
        }


        /// <summary>
        /// Create a Sample from a ScheduleItem
        /// </summary>
        /// <param name="job">Schedule Item</param>
        /// <returns>Sample</returns>
        public static Sample CreateSampleFromScheduleItem(ScheduleItem job)
        {
            Trace.WriteLine("Enter Helper.CreateSampleFromScheduleItem()");

            var sample = new Sample { MaterialId = job.MaterialId };

            // Retrieve the Traits which contain the information for the Sample
            var port = string.Format("Port {0}", job.LocationNumber);
            var traits = CurrentTraits.Where(t => t.MaterialId == job.MaterialId &&
                                                  t.PresentationName == port)
                                      .ToList();

            if (traits.Count() > 0)
            {
                var data = traits[0];

                // Create Sample class which will hold all values specified by user
                sample.SampleTypeName = SAMPLE_CLASS_SCHEDULED;

                sample.MaterialName = data.MaterialName;

                sample.CategoryId = data.CategoryId.Value;
                sample.CategoryName = data.CategoryName;

                sample.SubCategoryId = data.SubcategoryId.Value;
                sample.SubCategoryName = data.SubcategoryName;

                sample.PresentationId = data.PresentationId.Value;
                sample.PresentationName = data.PresentationName;

                sample.SampleIdentifiers = new List<SampleIdentifier>();
                sample.Traits = new List<IdNamePair>();

                // Add the Sample Identifiers
                var sampleIdentifiers = GetSampleIdentifiers(job.MaterialId)
                                              .OrderBy(si => si.DisplayOrder)
                                              .ToList();

                // Sample Identifier 1 = Sample ID
                if (sampleIdentifiers.Count > 0)
                {
                    var si = sampleIdentifiers[0];
                    si.Value = GenerateSampleID(job.LocationNumber, job.Material);
                    sample.SampleIdentifiers.Add(si);
                }

                // Sample Identifier 2 = Batch # (In theory, depends on database)
                if (sampleIdentifiers.Count > 1)
                {
                    var si = sampleIdentifiers[1];
                    si.Value = job.UserField1;
                    sample.SampleIdentifiers.Add(si);
                }

                // Sample Identifier 3 = Comments (In theory, depends on database)
                if (sampleIdentifiers.Count > 2)
                {
                    var si = sampleIdentifiers[2];
                    si.Value = job.UserField2;
                    sample.SampleIdentifiers.Add(si);
                }


                // Add the traits
                foreach (var trait in traits)
                    sample.Traits.Add(new IdNamePair { Id = trait.Id, Name = trait.Name });
            }

            Trace.WriteLine("Exit Helper.CreateSampleFromScheduleItem()");

            return sample;
        }


        /// <summary>
        /// Generates a Sample ID for the Sample
        /// </summary>
        /// <returns>Sample ID</returns>
        private static string GenerateSampleID(short locationNumber, string materialCode)
        {
            return string.Format("{0}{1}_{2}", locationNumber, materialCode, DateTime.Now.ToString("yyMMddHHmm"));
        }


        /// <summary>
        /// This function replaces the workflow message text from the relevent resource file
        /// </summary>
        public static string GetLocalizedResourceMessage(string key)
        {
            string value;
            string dcfMessageCode = string.Empty;

            if (key.IndexOf(",") > 0)
            {
                if (key.Substring(0, key.IndexOf(",")) == "DCFMessage")
                {
                    dcfMessageCode = key.Substring(key.IndexOf(",") + 2);
                    key = key.Substring(0, key.IndexOf(","));
                }
            }

            switch (key)
            {
                case "AverageCleanCheckPrompt":
                    value = ResourceHelper.AverageCleanCheckPrompt;
                    break;
                case "AveragePrompt":
                    value = ResourceHelper.AveragePrompt;
                    break;
                case "Start_OPUS_Message":
                    value = ResourceHelper.Start_OPUS_Message;
                    break;
                case "BPUIMessage":
                    value = ResourceHelper.BPUIMessage;
                    break;
                case "CTIMessage":
                    value = ResourceHelper.CTIMessage;
                    break;
                case "CTIFMessage":
                    value = ResourceHelper.CTIFMessage;
                    break;
                case "GSNMessage":
                    value = ResourceHelper.GSNMessage;
                    break;
                case "GSNFMessage":
                    value = ResourceHelper.GSNFMessage;
                    break;
                case "DCMessage":
                    value = ResourceHelper.DCMessage;
                    break;
                case "DCFMessage":
                    value = ResourceHelper.DCFMessage + "\r\n \r\n Error: " + 10305 + "  Result: " + dcfMessageCode;
                    break;
                case "WUCMessage":
                    value = ResourceHelper.WUCMessage;
                    break;
                case "WUCFMessage":
                    value = ResourceHelper.WUCFMessage;
                    break;
                case "CUMessage":
                    value = ResourceHelper.CUMessage;
                    break;
                case "CUFMessage":
                    value = ResourceHelper.CUFMessage;
                    break;
                case "BGCMessage":
                    value = ResourceHelper.BGCMessage;
                    break;
                case "CLMessage":
                    value = ResourceHelper.CLMessage;
                    break;
                case "OLMessage":
                    value = ResourceHelper.OLMessage;
                    break;
                case "RLOnMessage":
                    value = ResourceHelper.RLOnMessage;
                    break;
                case "RLOffMessage":
                    value = ResourceHelper.RLOffMessage;
                    break;
                case "BLOffMessage":
                    value = ResourceHelper.BLOffMessage;
                    break;
                case "BLOnMessage":
                    value = ResourceHelper.BLOnMessage;
                    break;
                case "SNBMessage":
                    value = ResourceHelper.SNBMessage;
                    break;
                case "SBMessage":
                    value = ResourceHelper.SBMessage;
                    break;
                case "LBMessage":
                    value = ResourceHelper.LBMessage;
                    break;
                case "SPMessage":
                    value = ResourceHelper.SPMessage;
                    break;
                case "SPUIMessage":
                    switch (CurrentSpectrometer.Name.ToUpper())
                    {
                        case "MPAVBTK":
                            value = ResourceHelper.SPMPAVUIMessage;
                            break;
                        case "MIR":
                        case "MIRBTK":
                            value = ResourceHelper.MIRSPUIMessage;
                            break;
                        case "SG":
                        case "SGBTK":
                            value = ResourceHelper.SGSPUIMessage;
                            break;
                        case "ALPHA":
                        case "ALPHABTK":
                            value = ResourceHelper.ALPHASPUIMessage;
                            break;
                        case "VECTOR":
                        case "VECTORBTK":
                            value = ResourceHelper.VECTORSPUIMessage;
                            break;
                        case "MPA":
                        case "MPABTK":
                            value = ResourceHelper.MPASPUIMessage;
                            break;
                        default:
                            value = ResourceHelper.SPUIMessage;
                            break;
                    }
                    break;
                case "SMessage":
                    value = ResourceHelper.SMessage;
                    break;
                case "SFMessage":
                    value = ResourceHelper.SFMessage;
                    break;
                case "GSDMessage":
                    value = ResourceHelper.GSDMessage;
                    break;
                case "GSDFMessage":
                    value = ResourceHelper.GSDFMessage;
                    break;
                case "SSMessage":
                    value = ResourceHelper.SSMessage;
                    break;
                case "SSFMessage":
                    value = ResourceHelper.SSFMessage;
                    break;
                case "USMessage":
                    value = ResourceHelper.USMessage;
                    break;
                case "USFMessage":
                    value = ResourceHelper.USFMessage;
                    break;
                case "SCMessage":
                    value = ResourceHelper.SCMessage;
                    break;
                case "STCMessage":
                    value = ResourceHelper.STCMessage;
                    break;
                case "CXPMessage":
                    value = ResourceHelper.CXPMessage;
                    break;
                case "CXPFMessage":
                    value = ResourceHelper.CXPFMessage;
                    break;
                case "SXPMessage":
                    value = ResourceHelper.SXPMessage;
                    break;
                case "SXPFMessage":
                    value = ResourceHelper.SXPFMessage;
                    break;
                case "CFRMessage":
                    value = ResourceHelper.CFRMessage;
                    break;
                case "CCMessage":
                    value = ResourceHelper.CCMessage;
                    break;
                case "CCFMessage":
                    value = ResourceHelper.CCFMessage;
                    break;
                case "HCMessage":
                    value = ResourceHelper.HCMessage;
                    break;
                case "CleanCheckBackgroundRequired":
                    value = ResourceHelper.CleanCheckBackgroundRequired;
                    break;
                case "CleanCheckShortMsg":
                    value = ResourceHelper.CleanCheckShortMsg;
                    break;
                case "AMessage":
                    value = ResourceHelper.AMessage;
                    break;
                case "AFMessage":
                    value = ResourceHelper.AFMessage;
                    break;
                default:
                    value = key;
                    break;
            }
            return value;
        }


        /// <summary>
        /// Calculates the Interval time (min.secs) based on the number of scans per hour
        /// </summary>
        /// <param name="scansPerHour"># of scans per hour</param>
        /// <returns>Interval Time</returns>
        public static decimal CalculateInterval(short scansPerHour)
        {
            return scansPerHour > 0 ? 60 / (decimal)scansPerHour : 0;
        }


        /// <summary>
        /// Calculates the number of seconds based on a decimal (min.%)
        /// </summary>
        /// <param name="minutes">Minutes as represented by a decimal</param>
        /// <returns>Seconds</returns>
        public static short CalculateSecondsFromFraction(decimal minutes)
        {
            var fraction = minutes - (short)minutes;
            var second = fraction == 0 ? (short)0 : short.Parse((59 * fraction).ToString("00"));
            return second;
        }
    }


    public delegate void ShowDialogDelegate(Main main, MessageDialog dialog);


    /// <summary>
    /// Report Names
    /// </summary>
    public enum ReportName
    {
        CertificateOfAnalysis,
        SingleAnalysis
    }


    /// <summary>
    /// Repair Database Messages
    /// </summary>
    public enum RepairDBMessage
    {
        ShowResuts,
        ShowError,
        HideAll
    }
}