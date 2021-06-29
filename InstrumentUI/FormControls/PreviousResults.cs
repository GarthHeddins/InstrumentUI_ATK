using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.Controls;
using InstrumentUI_ATK.Common;
using DataService = InstrumentUI_ATK.DataService;
using System.Drawing.Printing;
using System.Diagnostics;

namespace InstrumentUI_ATK.FormControls
{
    public partial class PreviousResults : UserControl, IFormControl
    {
        private Image _tabActiveImage = InstrumentUI_ATK.Properties.Resources.previousresults_tab_active;
        private Image _tabInActiveImage = InstrumentUI_ATK.Properties.Resources.previousresults_tab_inactive;
        private ResultListItem _currentResultItem = null;
        private string _currentAnalysisID = null;

        public DataService.EnumHelpCode HelpCode { get { return DataService.EnumHelpCode.INSTRUMENT_PREVIOUS_RESULT; } }

        public PreviousResults()
        {
            Trace.WriteLine("Enter PreviousResults() constructor."); 
            InitializeComponent();

            //display sample ids of all the results in the left pane
            LoadAllResults(true);
            Trace.WriteLine("Exit PreviousResults() constructor."); 
        }

        #region Event handlers

        /// <summary>
        /// Control Load event. Localize all labels and texts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousResults_Load(object sender, EventArgs e)
        {
            LocalizeResource();
            flpResults.Focus();
        }

        /// <summary>
        /// Display result description of the selected Sample Id/Analysis ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resultListItem_OnItemClicked(object sender, EventArgs e)
        {
            if (sender is ResultListItem)
            {
                LoadResult((ResultListItem)sender);
            }
        }

        /// <summary>
        /// Sample ID tab is clicked. Load all Sample IDs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSampleId_Click(object sender, EventArgs e)
        {
            btnSampleId.BackgroundImage = _tabActiveImage;
            btnAnalysisId.BackgroundImage = _tabInActiveImage;

            // reset the current result item
            _currentResultItem = null;

            LoadAllResults(true);
        }

        /// <summary>
        /// Analysis ID tab is clicked. Load all the analysis IDs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnalysisId_Click(object sender, EventArgs e)
        {
            btnAnalysisId.BackgroundImage = _tabActiveImage;
            btnSampleId.BackgroundImage = _tabInActiveImage;

            // reset the current result item
            _currentResultItem = null;

            LoadAllResults(false);
        }

        /// <summary>
        /// Email button click, launch the default email client with address specified
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
                {
                    emails = adminPreference.Email;
                }

                System.Diagnostics.Process.Start("mailto:" + emails);
            }
            catch (Exception ex)
            {
                Helper.LogError("PreviousResults.picMail_Click", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// Create PDF report of the selected Item(Certificate of Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picCofAPDF_Click(object sender, EventArgs e)
        {
            if (_currentAnalysisID != null)
            {
                Helper.GenerateReport(ReportName.CertificateOfAnalysis, _currentAnalysisID,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.PDF, true);
            }
        }

        /// <summary>
        /// Create Word report of the selected item(Certificate of Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picCofAWord_Click(object sender, EventArgs e)
        {
            if (_currentAnalysisID != null)
            {
                Helper.GenerateReport(ReportName.CertificateOfAnalysis, _currentAnalysisID,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.WORD, true);
            }
        }

        /// <summary>
        /// Create PDF report of the selected Item(Single Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picPDF_Click(object sender, EventArgs e)
        {
            if (_currentAnalysisID != null)
            {
                Helper.GenerateReport(ReportName.SingleAnalysis, _currentAnalysisID,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.PDF, true);
            }
        }

        /// <summary>
        /// Create Word report of the selected item(Single Analysis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picWord_Click(object sender, EventArgs e)
        {
            if (_currentAnalysisID != null)
            {
                Helper.GenerateReport(ReportName.SingleAnalysis, _currentAnalysisID,
                                        InstrumentUI_ATK.DataService.EnumRequestIdType.ANALYSIS_ID,
                                        InstrumentUI_ATK.DataService.EnumReportFormat.WORD, true);
            }
        }

        /// <summary>
        /// Prints the result details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picPrint_Click(object sender, EventArgs e)
        {
            if (_currentAnalysisID != null)
                Helper.PrintResults(_currentAnalysisID);
        }

        ///// <summary>
        ///// Print screen
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
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
        /// Display the list of sample IDs OR Analysis IDs based on the isDisplaySampleId
        /// </summary>
        /// <param name="isDisplaySampleId">if true then display list of Sample IDs otherwse display Analysis ID list</param>
        private void LoadAllResults(bool isDisplaySampleId)
        {
            Trace.WriteLine("Enter LoadAllResults()"); 
            ResultListItem resultListItem;
            int serialNumber = 1; // start serial number from 1
            try
            {
                Trace.WriteLine("LoadAllResults() - Before: DAL.GetAllResultHeader()."); 
                var results = DAL.GetAllResultHeader();
                Trace.WriteLine("LoadAllResults() - After:  DAL.GetAllResultHeader()."); 
                flpResults.Controls.Clear();

                ResultListItem lastResultItem = null;
                ResultListItem firstResultItem = null;

                // create control to display sample ID or Analysis ID and then add them to the left pane
                foreach (var item in results)
                {
                    if (isDisplaySampleId)
                        resultListItem = new ResultListItem(serialNumber.ToString(), item.SampleId);
                    else
                        resultListItem = new ResultListItem(serialNumber.ToString(), item.AnalysisId);

                    resultListItem.Id = item.RequestId;
                    resultListItem.SelectedColor = Color.FromArgb(204, 204, 204);
                    resultListItem.OnItemClicked += new EventHandler(resultListItem_OnItemClicked);

                    // set the previous resultItem of current item
                    resultListItem.LastResultItem = lastResultItem;

                    // set the next result item of LastItem to current item(current item will be the nextItem for the lastItem)
                    if (lastResultItem != null)
                        lastResultItem.NextResultItem = resultListItem;

                    // now change the last resultitem to the current item form the next loop
                    lastResultItem = resultListItem;

                    // set the first result item
                    if (firstResultItem == null)
                        firstResultItem = resultListItem;

                    flpResults.Controls.Add(resultListItem);

                    serialNumber++;
                }

                // get the first result(latest) selected
                if (firstResultItem != null)
                    LoadResult(firstResultItem);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception: LoadAllResults()"); 
                Helper.LogError("LoadAllResults()", string.Empty, ex, true);
            }
            Trace.WriteLine("Exit LoadAllResults()"); 
        }

        /// <summary>
        /// Load the details of result in the right pane based on the specified request id
        /// </summary>
        /// <param name="requestId"></param>
        private void LoadSelectedResult(string requestId)
        {
            // get the result header and display them on the screen
            ResultHeader resultHeader = DAL.GetResultHeader(requestId);
            if (resultHeader != null)
            {
                _currentAnalysisID = resultHeader.AnalysisId;
                lblAnalysisIdVal.Text = resultHeader.AnalysisId;

                lblMaterialTypeVal.Text = resultHeader.MaterialDesc;
                lblCategoryVal.Text = resultHeader.CategoryDesc;
                lblSubCategoryVal.Text = resultHeader.SCDesc;
                lblPresentationVal.Text = resultHeader.PresDesc;
                lblDateVal.Text = resultHeader.LocalTimeStamp.ToShortDateString();
                lblSampleTimeVal.Text = resultHeader.LocalTimeStamp.ToShortTimeString() + " Local Time";
            }
            else
            {
                _currentAnalysisID = resultHeader.AnalysisId;
            }

            // get the results identifiers and display them on the screen
            var resultIdentifiers = DAL.GetResultIdentifiers(requestId);
            SetSampleIdentifiers(resultIdentifiers);

            // get the trait result values and display them on the screen
            var resultDetails = DAL.GetResultDetails(requestId);

            InstrumentUI_ATK.Controls.ResultBoard resultBoard = new InstrumentUI_ATK.Controls.ResultBoard(3, 190, 123, 130);
            resultBoard.AlternateRowColor = Color.FromArgb(208, 213, 217);
            resultBoard.TextFont = new Font("Arial", 10, FontStyle.Regular);
            resultBoard.TextColor = Color.FromArgb(60, 60, 60);
            resultBoard.BorderColor = Color.FromArgb(60, 60, 60);

            foreach (var item in resultDetails)
            {
                resultBoard.AddRow(item.ModelGroupName, item.DisplayText, item.BusRuleText);
            }

            pnlTraitResults.Controls.Clear();
            pnlTraitResults.Controls.Add(resultBoard);
        }

        /// <summary>
        /// Display Sample Identifiers of the selected result
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

        private void LoadResult(ResultListItem resultListItem)
        {
            // make the clicked sample Id/Analysis ID selected in the left pane
            foreach (var control in flpResults.Controls.OfType<ResultListItem>())
            {
                ((ResultListItem)control).IsSelected = false;
            }

            // load all the details of selected results in the right pane
            resultListItem.IsSelected = true;
            _currentResultItem = resultListItem;
            LoadSelectedResult(resultListItem.Id);
        }

        ///// <summary>
        ///// Print the document
        ///// </summary>
        //private void PrintResults()
        //{
        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        //    printDocument.Print();
        //}

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            lblAnalysisId.Text = ResourceHelper.Analysis_ID + " :";
            lblResults.Text = ResourceHelper.Analytical_Results;

            lblSelectMaterial.Text = ResourceHelper.Select_Material;
            lblIdentifySample.Text = ResourceHelper.Identify_Sample;

            btnAnalysisId.Text = ResourceHelper.Analysis_ID;
            btnSampleId.Text = ResourceHelper.Sample_ID;

            lblMaterialType.Text = ResourceHelper.Material_Type;
            lblCategory.Text = ResourceHelper.Category;
            lblSubCategory.Text = ResourceHelper.SubCategory;
            lblPresentation.Text = ResourceHelper.Presentation;
            lblSampleDate.Text = ResourceHelper.SampleDate;           
        }

        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_currentResultItem != null)
            {
                if (keyData == Keys.Up && _currentResultItem.LastResultItem != null)
                {
                    LoadResult(_currentResultItem.LastResultItem);
                }
                else if (keyData == Keys.Down && _currentResultItem.NextResultItem != null)
                {
                    LoadResult(_currentResultItem.NextResultItem);
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
       
    }
}
