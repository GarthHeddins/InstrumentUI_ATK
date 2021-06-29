using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.DataService;

namespace InstrumentUI_ATK.FormControls
{
    public partial class Preferences : UserControl, IFormControl
    {
        private class Language
        {
            public string Name
            { get; set; }

            public string Value
            { get; set; }
        }

        private const string DISPLAY_MEMBER = "Name";
        private const string VALUE_MEMBER = "Value";
        private const string USERNAME = "Username";
        private const string LANGUAGE_SECTION = "userPreferenceGroup/language";
        public event EventHandler OnChangePasswordLinkClicked;

        private AdminPreference adminPreference
        { get; set; }

        public EnumHelpCode HelpCode { get { return EnumHelpCode.INSTRUMENT_ADMIN_PREFERENCE; } }

        public Preferences()
        {
            InitializeComponent();
        }

        public void HideMessage()
        {
            lblMessage.Visible = false;
        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            try
            {
                LocalizeResource();

                try
                {
                    NameValueCollection languageSection = (NameValueCollection)ConfigurationManager.GetSection(LANGUAGE_SECTION);
                    List<Language> lstLanguage = new List<Language>();
                    foreach (string sKey in languageSection.Keys)
                    {
                        if (!languageSection[sKey].IsNullOrWhiteSpace())
                        {
                            Language language = new Language();
                            language.Name = sKey.Trim();
                            language.Value = languageSection[sKey].Trim();
                            lstLanguage.Add(language);
                        }
                    }
                    cbLanguage.DisplayMember = DISPLAY_MEMBER;
                    cbLanguage.ValueMember = VALUE_MEMBER;
                    cbLanguage.DataSource = lstLanguage;
                }
                catch (ConfigurationErrorsException ex)
                {
                    Helper.LogError("Preferences_Load", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
                }
                catch (InvalidCastException ex)
                {
                    Helper.LogError("Preferences_Load", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
                }

                PopulateAdminPreference();
            }
            catch (Exception ex)
            {
                Helper.LogError("Preferences_Load", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string language = string.Empty;
                string culture = string.Empty;
                bool autoPrint = false;
                bool autoSampleId = false;
                bool soundOn = false;
                bool SpeedMode = false;
                bool SpeedModeDualScan = false;

                if (null != cbLanguage.SelectedItem)
                {
                    culture = Convert.ToString(cbLanguage.SelectedValue);
                    if(!culture.IsNullOrWhiteSpace())
                        language = cbLanguage.Text;
                }

                if (chkAutoPrint.Checked)
                    autoPrint = true;

                if (chkAutoSampleId.Checked)
                    autoSampleId = true;

                if (chkSoundOn.Checked)
                    soundOn = true;

                if (chkSpeedMode.Checked)
                    SpeedMode = true;

                if (chkSpeedModeDualScan.Checked)
                    SpeedModeDualScan = true;

                SaveAdminPreference(culture, culture, autoPrint, autoSampleId, soundOn, SpeedMode, SpeedModeDualScan);
                Helper.SpeedMode = SpeedMode;
                Helper.SpeedModeDualScan = SpeedModeDualScan;

                Helper.CurrentOwner.LocalizeResource();
                
                if (Helper.CurrentOwner.FormRoutine != null)
                    Helper.CurrentOwner.FormRoutine.LocalizeResource();

                if (Helper.CurrentOwner.FormCheck != null)
                    Helper.CurrentOwner.FormCheck.LocalizeResource();

                if (Helper.CurrentOwner.CurrentTab != null)
                    Helper.CurrentOwner.CurrentTab.LocalizeResource();
            }
            catch (Exception ex)
            {
                Helper.LogError("btnOk_Click", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        private void PopulateAdminPreference()
        {
            adminPreference = DAL.GetAdminPreference();
            if (null != adminPreference)
            {
                cbLanguage.SelectedValue = adminPreference.Language;
                //if (null != cbLanguage.SelectedItem)
                //    SetCulture(cbLanguage.SelectedValue.ToString());
                chkAutoPrint.Checked = adminPreference.AutoPrint;
                chkAutoSampleId.Checked = adminPreference.AutoSampleId;
                chkSoundOn.Checked = adminPreference.SoundsOn;
                chkSpeedMode.Checked = adminPreference.SpeedMode;
                if (adminPreference.SpeedMode == false)
                    chkSpeedModeDualScan.Enabled = false;
                else
                    chkSpeedModeDualScan.Checked = adminPreference.SpeedModeDualScan;
            }
            else
                chkSpeedModeDualScan.Enabled = false;

            string username = ConfigurationManager.AppSettings[USERNAME];
            if (!username.IsNullOrWhiteSpace())
                chkRememberUsername.Checked = true;
        }

        private void SaveAdminPreference(string language, string culture, bool autoPrint, bool autoSampleId, bool soundOn, bool SpeedMode, bool SpeedModeDualScan)
        {
            if (null == adminPreference)
            {
                DAL.InsertAdminPreference(language, autoPrint, autoSampleId, soundOn, SpeedMode, SpeedModeDualScan);
                adminPreference = new AdminPreference();
            }
            else
                DAL.UpdateAdminPreference(language, autoPrint, autoSampleId, soundOn, SpeedMode, SpeedModeDualScan);

            adminPreference.Language = language;
            adminPreference.AutoPrint = autoPrint;
            adminPreference.AutoSampleId = autoSampleId;
            adminPreference.SoundsOn = soundOn;
            adminPreference.SpeedMode = SpeedMode;
            adminPreference.SpeedModeDualScan = SpeedModeDualScan;

            Helper.IsAutoSampleId = autoSampleId;

            try
            {
                Configuration appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                appSettings.AppSettings.Settings.Remove(USERNAME);
                if (chkRememberUsername.Checked)
                    appSettings.AppSettings.Settings.Add(USERNAME, Helper.CurrentUser.UserName);
                appSettings.Save();
                ConfigurationManager.RefreshSection("appSettings");
                lblMessage.Visible = true;

                SetCulture(culture);
            }
            catch (ConfigurationErrorsException cee)
            {
                Helper.DisplayError(cee.Message);//TODO: Message from resource file
            }
        }

        private void SetCulture(string culture)
        {
            if (!culture.IsNullOrWhiteSpace())
            {
                ResourceHelper.Culture = new CultureInfo(culture, false);
                LocalizeResource();
            }
            else
                Helper.DisplayError(ResourceHelper.Culture_Error);
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

            lblLanguageTitle.Text = ResourceHelper.Language;
            lblOptionsTitle.Text = ResourceHelper.Options;
            chkSoundOn.Text = ResourceHelper.Sound_On;
            chkAutoSampleId.Text = ResourceHelper.Auto_Sample_Id;
            chkAutoPrint.Text = ResourceHelper.Auto_Print;
            lblChangePassTitle.Text = ResourceHelper.Change_Password;
            lblClick.Text = ResourceHelper.Click;
            lnkHere.Text = ResourceHelper.Here;
            lblChangePassText.Text = ResourceHelper.To_Change_Password;
            btnOk.Text = ResourceHelper.Save;
            lblMessage.Text = ResourceHelper.Changes_Saved;
            chkRememberUsername.Text = ResourceHelper.Remember_Username;
        }

        private void lnkHere_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (null != this.OnChangePasswordLinkClicked)
                    OnChangePasswordLinkClicked(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Helper.LogError("lnkHere_LinkClicked", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        private void control_Click(object sender, EventArgs e)
        {
            this.HideMessage();
        }

        /// <summary>
        /// Adjust the size/location of lables according to the Spanish text.
        /// </summary>
        private void AdjustSpanish()
        {
            this.lnkHere.Location = new System.Drawing.Point(116, 231);
            this.lblChangePassText.Location = new System.Drawing.Point(145, 231);

            this.lblLineTop.Location = new System.Drawing.Point(74, 33);
            this.lblLineTop.Size = new System.Drawing.Size(321, 1);

            this.lblLineMiddle.Location = new System.Drawing.Point(97, 116);
            this.lblLineMiddle.Size = new System.Drawing.Size(298, 1);

            this.lblLineBottom.Location = new System.Drawing.Point(167, 216);
            this.lblLineBottom.Size = new System.Drawing.Size(228, 1);
        }

        /// <summary>
        /// Adjust the size/location of lables back to the English text.
        /// </summary>
        private void AdjustEnglish()
        {
            this.lnkHere.Location = new System.Drawing.Point(72, 246);
            this.lblChangePassText.Location = new System.Drawing.Point(101, 246);

            this.lblLineTop.Location = new System.Drawing.Point(97, 33);
            this.lblLineTop.Size = new System.Drawing.Size(298, 1);

            this.lblLineMiddle.Location = new System.Drawing.Point(85, 116);
            this.lblLineMiddle.Size = new System.Drawing.Size(310, 1);

            this.lblLineBottom.Location = new System.Drawing.Point(165, 231);
            this.lblLineBottom.Size = new System.Drawing.Size(240, 1);
        }

        private void chkSpeedMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSpeedMode.Checked)
            {
                chkSpeedModeDualScan.Enabled = true;
            }
            else
            {
                chkSpeedModeDualScan.Checked = false;
                chkSpeedModeDualScan.Enabled = false;
            }

        }
    }
}
