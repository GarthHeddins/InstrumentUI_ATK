using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.Common;
using System.IO;

namespace InstrumentUI_ATK.FormControls
{
    public partial class Result : UserControl, IFormControl
    {
        public event EventHandler OnNextSample;
        public event EventHandler OnRepeatSample;
        public event EventHandler OnResultsDeleted;

        private int _requestId;
        private ResultHeader _resultHeader;
        private List<ResultIdentifier> _resultIdentifiers;
        private List<ResultDetail> _resultDetails;
        private AdminPreference _adminPreference;

        public DataService.EnumHelpCode HelpCode { get { return DataService.EnumHelpCode.INSTRUMENT_RESULT; } }

        public Result()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the result control based on the specified result id
        /// </summary>
        /// <param name="requestID"></param>
        public Result(int requestID) : this()
        {
            try
            {
                this._requestId = requestID;

                // get the result header and display them on the screen
                _resultHeader = DAL.GetResultHeader(requestID.ToString());
                if (_resultHeader != null)
                {
                    lblAnalysisIdVal.Text = _resultHeader.AnalysisId;
                    Helper.AnalysisID = _resultHeader.AnalysisId;
                    lblSampleIdVal.Text = _resultHeader.SampleId;

                    lblMaterialTypeVal.Text = _resultHeader.MaterialDesc;
                    lblCategoryVal.Text = _resultHeader.CategoryDesc;
                    lblSubCategoryVal.Text = _resultHeader.SCDesc;
                    lblPresentationVal.Text = _resultHeader.PresDesc;
                }
                
                // get the results identifiers and display them on the screen
                _resultIdentifiers = DAL.GetResultIdentifiers(requestID.ToString());
                SetSampleIdentifiers(_resultIdentifiers);

                // get the trait result values and display them on the screen
                _resultDetails = DAL.GetResultDetails(requestID.ToString());

                InstrumentUI_ATK.Controls.ResultBoard resultBoard = new InstrumentUI_ATK.Controls.ResultBoard(3, 116, 100, 105);
                resultBoard.AlternateRowColor = Color.FromArgb(208, 213, 217);
                resultBoard.TextFont = new Font("Arial", 10, FontStyle.Regular);
                resultBoard.TextColor = Color.FromArgb(60, 60, 60);
                resultBoard.BorderColor = Color.FromArgb(60, 60, 60);

                foreach (var item in _resultDetails)
                {
                    resultBoard.AddRow(item.ModelGroupName, item.DisplayText, item.BusRuleText);
                }

                pnlTraitResults.Controls.Add(resultBoard);

                // Play alert sound here
                Helper.MakeAlertSound();
                
                // if Auto Print is on then, then prints the results
                _adminPreference = DAL.GetAdminPreference();
                if (_adminPreference != null && _adminPreference.AutoPrint && _resultHeader != null)
                    Helper.PrintResults(_resultHeader.AnalysisId);

                btnDeleteResults.Selected = true;
            }
            catch (Exception ex)
            {
                ResetResults(); // clear items from the results form
                Helper.LogError("Result.Result", "requestID=" + requestID, ex, false);
                Helper.DisplayError(ResourceHelper.Error_10701); // unable to load the results
            }
        }

        #region Control Events

        /// <summary>
        /// Result Control Load event. Localize all resources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Result_Load(object sender, EventArgs e)
        {
            LocalizeResource();
        }

        /// <summary>
        /// Click event of Next Sample button. Raise the OnNextSample event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextSample_Click(object sender, EventArgs e)
        {
            if (this.OnNextSample != null)
                this.OnNextSample(this, new EventArgs());
        }

        /// <summary>
        /// Click event of Next Repeat button. Raise the OnRepeatSample event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRepeatSample_Click(object sender, EventArgs e)
        {
            if (this.OnRepeatSample != null)
                this.OnRepeatSample(this, new EventArgs());
        }

        /// <summary>
        /// Click event of Delete Sample. Delete the specified sample result then take user to analyze any new sample
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteResults_Click(object sender, EventArgs e)
        {
            if (btnDeleteResults.Selected)
            {
                DataService.InstrumentServiceClient serviceClient = new DataService.InstrumentServiceClient();
                bool isServiceClosed = false;

                try
                {
                    bool isSuccess = false;

                    using (InstrumentUIDataContext dataContext = new InstrumentUIDataContext(Helper.CONNECTION_STRING))
                    {
                        using (var transaction = new System.Transactions.TransactionScope())
                        {
                            // delete result header
                            var resultHeader = dataContext.ResultHeaders.Where(r => r.RequestId == _requestId.ToString());
                            dataContext.ResultHeaders.DeleteAllOnSubmit(resultHeader);

                            // delete result detail
                            var resultDetail = dataContext.ResultDetails.Where(r => r.RequestId == _requestId.ToString());
                            dataContext.ResultDetails.DeleteAllOnSubmit(resultDetail);

                            // delete result identifiers
                            var resultIdentifiers = dataContext.ResultIdentifiers.Where(r => r.RequestId == _requestId.ToString());
                            dataContext.ResultIdentifiers.DeleteAllOnSubmit(resultIdentifiers);

                            // submit the changes 
                            dataContext.SubmitChanges();

                            serviceClient = Helper.GetServiceInstance();

                            // get the result status
                            int result = serviceClient.DeleteResult(_requestId.ToString());

                            serviceClient.Close();
                            isServiceClosed = true;

                            if (result > 0)
                            {
                                // complete the trasaction.
                                transaction.Complete();
                                isSuccess = true;
                            }
                        }
                    }

                    if (isSuccess)
                    {
                        Helper.DisplayMessage(ResourceHelper.Delete_Results, ResourceHelper.Delete_Results_Success, false);

                        if (this.OnResultsDeleted != null)
                            this.OnResultsDeleted(this, new EventArgs());
                    }
                    else
                    {
                        Helper.DisplayError(ResourceHelper.Error);
                    }
                }
                catch (Exception ex)
                {
                    Helper.LogError("Result.btnDeleteResults_Click", "_requestId=" + _requestId, ex, true);
                }
                finally
                {
                    if (!isServiceClosed)
                        serviceClient.Abort();

                    if (Helper.ContextScope != null)
                        Helper.ContextScope.Dispose();
                }
            }
        }

        /// <summary>
        /// Create word report (Certificate of Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picCofAWord_Click(object sender, EventArgs e)
        {
            if (_resultHeader != null)
            {
                this.Cursor = Cursors.WaitCursor;
                Helper.GenerateReport(ReportName.CertificateOfAnalysis, _resultHeader.AnalysisId,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.WORD, true);
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Create PDF report (Certificate of Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picCofAPDF_Click(object sender, EventArgs e)
        {
            if (_resultHeader != null)
            {
                this.Cursor = Cursors.WaitCursor;
                Helper.GenerateReport(ReportName.CertificateOfAnalysis, _resultHeader.AnalysisId,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.PDF, true);
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Create PDF report(Single Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picPDF_Click(object sender, EventArgs e)
        {
            if (_resultHeader != null)
            {
                this.Cursor = Cursors.WaitCursor;
                Helper.GenerateReport(ReportName.SingleAnalysis, _resultHeader.AnalysisId,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.PDF, true);
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Create Word report (Single Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picWord_Click(object sender, EventArgs e)
        {
            if (_resultHeader != null)
            {
                this.Cursor = Cursors.WaitCursor;
                Helper.GenerateReport(ReportName.SingleAnalysis, _resultHeader.AnalysisId,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.WORD, true);
                this.Cursor = Cursors.Arrow;
            }
        }

        private byte[] GetByteArray(string spectralFileData)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, spectralFileData);
            ms.Seek(0, 0);
            return ms.ToArray();
        }

        /// <summary>
        /// Print the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picPrint_Click(object sender, EventArgs e)
        {
            //Helper.PrintFile("C:\\Testdoc1.pdf", "\\rsi-nod-printvm\\NOD_C1_BF");
            this.Cursor = Cursors.WaitCursor;
            if (_resultHeader != null)
                Helper.PrintResults(_resultHeader.AnalysisId);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Open up the default mail client with email addresses specified in admin preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picMail_Click(object sender, EventArgs e)
        {
            try
            {
                var adminPreference = DAL.GetAdminPreference();
                string emails = "";

                if (adminPreference != null)
                    emails = adminPreference.Email;

                System.Diagnostics.Process.Start("mailto:" + emails);
            }
            catch (Exception ex)
            {
                Helper.LogError("picMail_Click", string.Empty, ex, true);
            }
        }

        //private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    int x = SystemInformation.WorkingArea.X;
        //    int y = SystemInformation.WorkingArea.Y;
        //    int width = this.Width;
        //    int height = this.Height;

        //    Rectangle bounds = new Rectangle(x, y, width, height);

        //    Bitmap img = new Bitmap(width, height);

        //    this.DrawToBitmap(img, bounds);

        //    e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //    e.Graphics.DrawImage(img, 50, 50, 637, 363); //scale down image to (637, 363)
        //}

        #endregion

        #region Class Level Private Functions

        /// <summary>
        /// Display Sample Identifiers on the screen
        /// </summary>
        /// <param name="resultIdentifiers"></param>
        private void SetSampleIdentifiers(List<ResultIdentifier> resultIdentifiers)
        {
            if (resultIdentifiers.Count() > 6) // seven sample identifiers
            {
                lblIdentify7.Text = resultIdentifiers[6].Description;
                lblIdentify7Val.Text = resultIdentifiers[6].AttribValue;
            }
            else
            {
                lblIdentify7.Text = string.Empty;
                lblIdentify7Val.Text = string.Empty;
            }

            if (resultIdentifiers.Count() > 5) // six sample identifiers
            {
                lblIdentify6.Text = resultIdentifiers[5].Description;
                lblIdentify6Val.Text = resultIdentifiers[5].AttribValue;
            }
            else
            {
                lblIdentify6.Text = string.Empty;
                lblIdentify6Val.Text = string.Empty;
            }

            if (resultIdentifiers.Count() > 4) // five sample identifiers
            {
                lblIdentify5.Text = resultIdentifiers[4].Description;
                lblIdentify5Val.Text = resultIdentifiers[4].AttribValue;
            }
            else
            {
                lblIdentify5.Text = string.Empty;
                lblIdentify5Val.Text = string.Empty;
            }

            if (resultIdentifiers.Count() > 3) // four sample identifiers
            {
                lblIdentify4.Text = resultIdentifiers[3].Description;
                lblIdentify4Val.Text = resultIdentifiers[3].AttribValue;
            }
            else
            {
                lblIdentify4.Text = string.Empty;
                lblIdentify4Val.Text = string.Empty;
            }

            if (resultIdentifiers.Count() > 2) // three sample identifiers
            {
                lblIdentify3.Text = resultIdentifiers[2].Description;
                lblIdentify3Val.Text = resultIdentifiers[2].AttribValue;
            }
            else
            {
                lblIdentify3.Text = string.Empty;
                lblIdentify3Val.Text = string.Empty;
            }

            if (resultIdentifiers.Count() > 1) // two sample identifiers
            {
                lblIdentify2.Text = resultIdentifiers[1].Description;
                lblIdentify2Val.Text = resultIdentifiers[1].AttribValue;
            }
            else
            {
                lblIdentify2.Text = string.Empty;
                lblIdentify2Val.Text = string.Empty;
            }

            if (resultIdentifiers.Count() > 0) // one sample identifier
            {
                lblIdentify1.Text = resultIdentifiers[0].Description;
                lblIdentify1Val.Text = resultIdentifiers[0].AttribValue;
            }
            else
            {
                lblIdentify1.Text = string.Empty;
                lblIdentify1Val.Text = string.Empty;
            }
        }

        //private void PrintResults()
        //{
        //    //PrintDocument printDocument = new PrintDocument();
        //    //printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        //    //printDocument.Print();

        //    if (_resultHeader != null)
        //    {
        //        var preferences = DAL.GetAdminPreference();

        //        ReportName reportName = ReportName.CertificateOfAnalysis;
        //        InstrumentUI_ATK.DataService.EnumReportFormat reportFormat = InstrumentUI_ATK.DataService.EnumReportFormat.PDF;

        //        if (preferences != null && string.IsNullOrEmpty(preferences.DefaultReport))
        //        {
        //            string defaultReport = preferences.DefaultReport;

        //            if (defaultReport == Helper.REPORT_CofA_PDF)
        //            {
        //                reportName = ReportName.CertificateOfAnalysis;
        //                reportFormat = InstrumentUI_ATK.DataService.EnumReportFormat.PDF;
        //            }
        //            else if (defaultReport == Helper.REPORT_CofA_WORD)
        //            {
        //                reportName = ReportName.CertificateOfAnalysis;
        //                reportFormat = InstrumentUI_ATK.DataService.EnumReportFormat.WORD;
        //            }
        //            else if (defaultReport == Helper.REPORT_SINGLE_ANALYSIS_PDF)
        //            {
        //                reportName = ReportName.SingleAnalysis;
        //                reportFormat = InstrumentUI_ATK.DataService.EnumReportFormat.PDF;
        //            }
        //            else if (defaultReport == Helper.REPORT_SINGLE_ANALYSIS_WORD)
        //            {
        //                reportName = ReportName.SingleAnalysis;
        //                reportFormat = InstrumentUI_ATK.DataService.EnumReportFormat.WORD;
        //            }
        //        }

        //        string filePath = Helper.GenerateReport(reportName, _resultHeader.AnalysisId,
        //                                                    InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
        //                                                    reportFormat, true);

        //        if (string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        //        {
        //            Helper.PrintFile(filePath, "");
        //        }
        //    }
        //}

        private void ResetResults()
        {
            btnDeleteResults.Selected = false;

            lblMaterialTypeVal.Text = string.Empty;
            lblCategoryVal.Text = string.Empty;
            lblSubCategoryVal.Text = string.Empty;
            lblPresentationVal.Text = string.Empty;

            lblIdentify1.Text = string.Empty;
            lblIdentify1Val.Text = string.Empty;

            lblIdentify2.Text = string.Empty;
            lblIdentify2Val.Text = string.Empty;

            lblIdentify3.Text = string.Empty;
            lblIdentify3Val.Text = string.Empty;

            lblIdentify4.Text = string.Empty;
            lblIdentify4Val.Text = string.Empty;

            lblIdentify5.Text = string.Empty;
            lblIdentify5Val.Text = string.Empty;

            lblIdentify6.Text = string.Empty;
            lblIdentify6Val.Text = string.Empty;

            lblIdentify7.Text = string.Empty;
            lblIdentify7Val.Text = string.Empty;

            lblAnalysisIdVal.Text = string.Empty;

            picCofAPDF.Enabled = false;            
            //picMail.Enabled = false;
            picPDF.Enabled = false;
            picPrint.Enabled = false;
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            if (ResourceHelper.Culture.Name.ToLower() == Helper.CULTURE_NAME_ENGLISH.ToLower())
            {
                AdjustEnglish();
            }
            else if (ResourceHelper.Culture.Name.ToLower() == Helper.CULTURE_NAME_SPANISH.ToLower())
            {
                AdjustSpanish();
            }

            lblAnalysisId.Text = ResourceHelper.Analysis_ID + " :";
            lblHeading.Text = ResourceHelper.Result_For + " :";

            lblMaterialHead.Text = ResourceHelper.Select_Material;
            lblIdentifyHead.Text = ResourceHelper.Identify_Sample;

            btnDeleteResults.Text = ResourceHelper.Delete_Results;
            btnNextSample.Text = ResourceHelper.Next_Sample;
            btnRepeatSample.Text = ResourceHelper.Repeat_Sample;

            lblMaterialType.Text = ResourceHelper.Material_Type;
            lblCategory.Text = ResourceHelper.Category;
            lblSubCategory.Text = ResourceHelper.SubCategory;
            lblPresentation.Text = ResourceHelper.Presentation;
        }

        /// <summary>
        /// Adjust the size/location of lables according to the Spanish text.
        /// </summary>
        private void AdjustSpanish()
        {
            this.btnDeleteResults.Size = new System.Drawing.Size(118, 86);

            this.btnRepeatSample.Location = new System.Drawing.Point(776, 421);
            this.btnRepeatSample.Size = new System.Drawing.Size(110, 86);

            this.btnNextSample.Location = new System.Drawing.Point(893, 421);
            this.btnNextSample.Size = new System.Drawing.Size(118, 86);

            this.lblSampleIdVal.Location = new System.Drawing.Point(189, 11);
        }

        /// <summary>
        /// Adjust the size/location of lables back to the English text.
        /// </summary>
        private void AdjustEnglish()
        {
            this.btnDeleteResults.Size = new System.Drawing.Size(97, 86);

            this.btnRepeatSample.Location = new System.Drawing.Point(792, 421);
            this.btnRepeatSample.Size = new System.Drawing.Size(101, 86);

            this.btnNextSample.Location = new System.Drawing.Point(924, 421);
            this.btnNextSample.Size = new System.Drawing.Size(87, 86);

            this.lblSampleIdVal.Location = new System.Drawing.Point(141, 11);
        }

        #endregion


    }
}
