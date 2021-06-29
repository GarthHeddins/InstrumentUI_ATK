using System;
using System.Configuration;
using System.Windows.Forms;
using InstrumentUI_ATK.DataService;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess;
using System.IO;
using System.Threading;

namespace InstrumentUI_ATK.FormControls
{
    public partial class SetUp : UserControl, IFormControl
    {
        
        private const string DEFAULT_REPORT = "DefaultReport";
        private const string COLON = ":";
        private string username = string.Empty;

        private AdminPreference adminPreference
        { get; set; }

        public EnumHelpCode HelpCode { get { return EnumHelpCode.INSTRUMENT_ADMIN_SETUP; } }

        public SetUp()
        {
            InitializeComponent();
        }

        public void HideMessage()
        {
            lblMessage.Visible = false;
        }

        private void SetUp_Load(object sender, EventArgs e)
        {
            LocalizeResource();
            PopulateAdminSetUp();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string logoPath = string.Empty;
                bool algorithmVersion = false;
                string reportAddress1 = string.Empty;
                string reportAddress2 = string.Empty;
                string mailTo = string.Empty;
                string defaultReport = "Single Analysis PDF";
                string reportDirectory = string.Empty;

                if (!txtLogo.Text.IsNullOrWhiteSpace())
                    logoPath = txtLogo.Text.Trim();

                if (chkAddAlgoVersion.Checked)
                    algorithmVersion = true;

                if (!txtReportAddress1.Text.IsNullOrWhiteSpace())
                    reportAddress1 = txtReportAddress1.Text.Trim();

                if (!txtReportAddress2.Text.IsNullOrWhiteSpace())
                    reportAddress2 = txtReportAddress2.Text.Trim();

                if (!txtMailTo.Text.IsNullOrWhiteSpace())
                    mailTo = txtMailTo.Text.Trim();

                if (!txtReportDirectory.Text.IsNullOrWhiteSpace())
                    reportDirectory = txtReportDirectory.Text.Trim();

                defaultReport = cbDefaultReport.SelectedItem.ToString();

                // save admin preferences on the server
                SavePreferenceOnServer(logoPath, algorithmVersion, reportAddress1, reportAddress2);

                // save admin preferences on the client(local db cache)
                SaveAdminPreference(logoPath, algorithmVersion, reportAddress1, reportAddress2, mailTo, 
                                        defaultReport, reportDirectory);

                lblMessage.Visible = true; 
                lblMessage.Focus();
            }
            catch (Exception ex)
            {
                Helper.LogError("btnOk_Click", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        private void SavePreferenceOnServer(string logoPath, bool algorithmVersion, string reportAddress1,
                                            string reportAddress2)
        {
            DataService.InstrumentServiceClient serviceClient = null;
            bool isServiceClosed = false;

            try
            {
                CustomUserReportHeader reportHeaderInfo = new CustomUserReportHeader();
                reportHeaderInfo.Address1 = reportAddress1;
                reportHeaderInfo.Address2 = reportAddress2;
                reportHeaderInfo.IsAlgorithamVersion = algorithmVersion;
                reportHeaderInfo.UserId = Helper.CurrentUser.Id;

                if (string.IsNullOrEmpty(logoPath.Trim()))
                    reportHeaderInfo.CompanyLogo = null;
                else
                    reportHeaderInfo.CompanyLogo = File.ReadAllBytes(logoPath);


                serviceClient = Helper.GetServiceInstance();

                serviceClient.UpdateUserReportHeader(reportHeaderInfo);

                serviceClient.Close();
                isServiceClosed = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Abort();

                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }
        }

        private void SaveAdminPreference(string logoPath, bool algorithmVersion, string reportAddress1, 
                                            string reportAddress2, string mailTo, string defaultReport, 
                                            string reportDirecotry)
        {
            if (null == adminPreference)
            {
                DAL.InsertAdminPreference(logoPath, algorithmVersion, reportAddress1, reportAddress2, 
                                            mailTo, defaultReport, reportDirecotry);
                adminPreference = new AdminPreference();
            }
            else
                DAL.UpdateAdminPreference(logoPath, algorithmVersion, reportAddress1, reportAddress2, 
                                            mailTo, defaultReport, reportDirecotry);

            adminPreference.LogoFilePath = logoPath;
            adminPreference.AlgorithamVersion = algorithmVersion;
            adminPreference.AddressOnReportLine1 = reportAddress1;
            adminPreference.AddressOnReportLine2 = reportAddress2;
            adminPreference.Email = mailTo;
            adminPreference.DefaultReport = defaultReport;
            adminPreference.ReportDirectory = reportDirecotry;
        }

        private void PopulateAdminSetUp()
        {
            cbDefaultReport.Items.Clear();
            cbDefaultReport.Items.Add("CofA PDF");
            cbDefaultReport.Items.Add("CofA Word");
            cbDefaultReport.Items.Add("Single Analysis PDF");
            cbDefaultReport.Items.Add("Single Analysis Word");
            cbDefaultReport.SelectedIndex = 2;

            adminPreference = DAL.GetAdminPreference();
            if (null != adminPreference)
            {
                txtLogo.Text = adminPreference.LogoFilePath;
                chkAddAlgoVersion.Checked = adminPreference.AlgorithamVersion ?? false;
                txtReportAddress1.Text = adminPreference.AddressOnReportLine1;
                txtReportAddress2.Text = adminPreference.AddressOnReportLine2;
                txtMailTo.Text = adminPreference.Email;
                txtReportDirectory.Text = adminPreference.ReportDirectory;

                foreach (object item in cbDefaultReport.Items)
                {
                    if (item.ToString() == adminPreference.DefaultReport)
                    {
                        cbDefaultReport.SelectedItem = item;                        
                    }
                }                              
            }

            //string defaultReportDirectory = ConfigurationManager.AppSettings[Helper.DEFAULT_REPORT_DIRECTORY];
            //if (!defaultReportDirectory.IsNullOrWhiteSpace())
            //{
            //    cbDefaultReportDir.Items.Clear();
            //    cbDefaultReportDir.Items.Add(defaultReportDirectory);
            //    cbDefaultReportDir.SelectedIndex = 0;
            //}

            //string defaultReport = ConfigurationManager.AppSettings[DEFAULT_REPORT];

            //if (!defaultReport.IsNullOrWhiteSpace())
            //{
                
            //}
        }

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

            lblCustomizeTitle.Text = ResourceHelper.Customize;
            lblAddLogo.Text = ResourceHelper.Add_Logo + COLON;
            chkAddAlgoVersion.Text = ResourceHelper.Add_Algo_Version;
            lblDefaultReportDirTitle.Text = ResourceHelper.Default_Report_Dir;
            lblDefaultReportTitle.Text = ResourceHelper.Default_Report;
            //lblReportAddressTitle.Text = ResourceHelper.Report_Address;
            lblEmailOptionsTitle.Text = ResourceHelper.Email_Options;
            lblMailTo.Text = ResourceHelper.Mail_To + COLON;
            btnBrowse.Text = ResourceHelper.Browse;
            //btnCancel.Text = ResourceHelper.Cancel;
            btnOk.Text = ResourceHelper.Save;
            lblMessage.Text = ResourceHelper.Changes_Saved;
            lblAddress1.Text = ResourceHelper.Address_1 + COLON;
            lblAddress2.Text = ResourceHelper.Address_2 + COLON;
            btnReportDirectory.Text = ResourceHelper.Browse;
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                
                this.HideMessage();
                DialogResult result = openFileDialogForLogo.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtLogo.Text = openFileDialogForLogo.FileName;
                }
            }
            catch (Exception ex)
            {
                Helper.LogError("btnBrowse_Click", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }


        private void control_Click(object sender, EventArgs e)
        {
            this.HideMessage();
        }

        private void btnReportDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                this.HideMessage();
                
                DialogResult result = folderBrowserDialogForReport.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtReportDirectory.Text = folderBrowserDialogForReport.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                Helper.LogError("btnReportDirectory_Click", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        /// <summary>
        /// Adjust the size/location of lables according to the Spanish text.
        /// </summary>
        private void AdjustSpanish()
        {
            this.lblLine1.Location = new System.Drawing.Point(116, 31);
            this.lblLine1.Size = new System.Drawing.Size(279, 1);

            this.lblLine2.Location = new System.Drawing.Point(277, 184);
            this.lblLine2.Size = new System.Drawing.Size(120, 1);

            this.lblLine3.Location = new System.Drawing.Point(197, 258);
            this.lblLine3.Size = new System.Drawing.Size(200, 1);

            this.lblLine4.Location = new System.Drawing.Point(250, 331);
            this.lblLine4.Size = new System.Drawing.Size(147, 1);
        }

        /// <summary>
        /// Adjust the size/location of lables back to the English text.
        /// </summary>
        private void AdjustEnglish()
        {
            this.lblLine1.Location = new System.Drawing.Point(99, 31);
            this.lblLine1.Size = new System.Drawing.Size(296, 1);

            this.lblLine2.Location = new System.Drawing.Point(196, 184);
            this.lblLine2.Size = new System.Drawing.Size(201, 1);

            this.lblLine3.Location = new System.Drawing.Point(130, 258);
            this.lblLine3.Size = new System.Drawing.Size(267, 1);

            this.lblLine4.Location = new System.Drawing.Point(122, 331);
            this.lblLine4.Size = new System.Drawing.Size(275, 1);
        }
    }
}
