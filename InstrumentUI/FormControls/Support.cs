using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataService;

namespace InstrumentUI_ATK.FormControls
{
    public partial class Support : UserControl, IFormControl
    {
        //private ManualBackground _manualBackground;
        private const string CONTACT_EMAIL = "ContactEmail";
        private const string COLON = ":";

        public EnumHelpCode HelpCode { get { return EnumHelpCode.INSTRUMENT_ADMIN_SUPPORT; } }

        public Support()
        {
            InitializeComponent();

        }


        private void Support_Load(object sender, EventArgs e)
        {
            try
            {
                LocalizeResource();
                PopulateAdminSupport();
            }
            catch (Exception ex)
            {
                Helper.LogError("Support_Load", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Helper.DisplayError("To be implemented."); //TODO: To be implemented
        }

        private void PopulateAdminSupport()
        {
            // get all XPM file from the specified folder
            string[] files = Directory.GetFiles(Helper.FOLDER_PATH_FILES, "*.XPM");

            // sort alphabetically with file names
            Array.Sort<string>(files, delegate(string path1, string path2) { return path1.CompareTo(path2); });
            
            lblContactNo.Text = ConfigurationManager.AppSettings[Helper.CONTACT_NUMBER];
            lblContactEmail.Text = ConfigurationManager.AppSettings[CONTACT_EMAIL];
            Version objVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = objVersion.ToString();
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

            lblVersionTitle.Text = ResourceHelper.Version;
            lblVersionText.Text = ResourceHelper.Version_Text;
            lblDiagnosticsTitle.Text = ResourceHelper.Instrument_Diagnostics;
            btnOk.Text = ResourceHelper.Save;
            lblMessage.Text = ResourceHelper.Changes_Saved;
            lblContactInfoTitle.Text = ResourceHelper.Contact_Information;
            lblClick.Text = ResourceHelper.Click;
            lnkHere.Text = ResourceHelper.Here.ToLower();
            lblDiagnosticsText2.Text = ResourceHelper.Diagnostics_Text_2;
        }


        private void lnkDiagnostics_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (Helper.CurrentSpectrometer.Name.Trim().ToLower() == "alpha")
                    Process.Start("IExplore.exe", "http://10.10.0.1/home.htm");
                else
                    Process.Start("IExplore.exe", "http://10.10.0.1");
            }
            catch (Exception ex)
            {
                Helper.LogError("lnkDiagnostics_LinkClicked", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
        }

        /// <summary>
        /// Adjust the size/location of lables according to the Spanish text.
        /// </summary>
        private void AdjustSpanish()
        {
            this.lblVersion.Location = new System.Drawing.Point(179, 113);

            this.lnkHere.Location = new System.Drawing.Point(114, 177);
            this.lblDiagnosticsText2.Location = new System.Drawing.Point(142, 177);

            this.lblLine1.Location = new System.Drawing.Point(194, 39);
            this.lblLine1.Size = new System.Drawing.Size(202, 1);

            this.lblLine4.Location = new System.Drawing.Point(219, 160);
            this.lblLine4.Size = new System.Drawing.Size(177, 1);
        }

        /// <summary>
        /// Adjust the size/location of lables back to the English text.
        /// </summary>
        private void AdjustEnglish()
        {
            this.lblVersion.Location = new System.Drawing.Point(168, 113);

            this.lnkHere.Location = new System.Drawing.Point(71, 177);
            this.lblDiagnosticsText2.Location = new System.Drawing.Point(102, 177);

            this.lblLine1.Location = new System.Drawing.Point(165, 39);
            this.lblLine1.Size = new System.Drawing.Size(231, 1);

            this.lblLine4.Location = new System.Drawing.Point(190, 160);
            this.lblLine4.Size = new System.Drawing.Size(206, 1);
        }

        private void btnRepairDB_Click(object sender, EventArgs e)
        {
                Helper.RepairDB(RepairDBMessage.ShowResuts);
        }
    }
}
