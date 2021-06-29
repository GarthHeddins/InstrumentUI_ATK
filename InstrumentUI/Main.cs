using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.FormControls;
using InstrumentUI_ATK.ModalForm;
using InstrumentUI_ATK.Properties;
using InstrumentUI_ATK.RibbonTabs;
using InstrumentUI_ATK.Workflow;

namespace InstrumentUI_ATK
{
    public partial class Main : Form
    {
        #region Properties

        //SD: Constant string that represents the appsettings key name within
        //the config file.  Allows us to determine at runtime if we should
        //start up the workflow or not. 
        private const string RUN_WORFLOW = "RunWorkflow";
        private readonly Image _tabActiveImage = Resources.ribbon_tab_active;
        private readonly Image _tabInActiveImage = Resources.ribbon_tabrow_tabinactive;
        private Preferences ctrlPreferences;
        private int MultiScanRunCount;        
                

        /// <summary>
        /// Analyze Tab
        /// </summary>
        public RibbonTabAnalyze TabAnalyze { get; set; }


        /// <summary>
        /// Current Tab
        /// </summary>
        public IRibbonTab CurrentTab { get; set; }


        /// <summary>
        /// It contains currently open control from Analyze tab(like Routine, Check)
        /// </summary>
        public Analyze FormCurrentAnalyze { get; set; }


        /// <summary>
        /// Reference to the Routine form
        /// </summary>
        public Routine FormRoutine 
        { 
            get { return _formRoutine; } 
            set { _formRoutine = value; FormCurrentAnalyze = value; } 
        }
        private Routine _formRoutine;


        /// <summary>
        /// Reference to the Check form
        /// </summary>
        public Check FormCheck 
        { 
            get { return _formCheck; } 
            set { _formCheck = value; FormCurrentAnalyze = value; } 
        }
        private Check _formCheck;


        /// <summary>
        /// Reference to the Schedule form
        /// </summary>
        public ScheduleForm FormSchedule { get; set; }

        #endregion


        public Main()
        {
            
            try
            {
                InitializeComponent();

                Rectangle screen = Screen.PrimaryScreen.WorkingArea;
                int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
                int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
                this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
                this.Size = new Size(w, h);
                
                string culture;
                //set the culture
                try
                {
                    //System.Diagnostics.Trace.WriteLine("Main()..Constructor");
                    DAL.CheckSpeedMode();
                    CheckAppConfig();
                    culture = DAL.GetCulture();
                }
                catch (Exception ex1)
                {
                    Helper.LogError("Main.cs", "", ex1, false);
                    culture = "en-US";
                }

                try
                {
                    if (culture == "English (US)") culture = "en-US";
                        ResourceHelper.Culture = new CultureInfo(culture, false);
                }
                catch (Exception ex2)
                {
                    Helper.LogError("Main.cs", "", ex2, false);
                }

                Helper.CurrentOwner = this;

                // hide the submit data tab, this will be a future enhancement.
                btnRibbonHeaderSubmitData.Visible = false;
            }
            catch (Exception ex)
            {
                Helper.DisplayError("The following Error occurred in the Main() constructor: " + ex.Message);
                Helper.LogError("Main.cs", "", ex, false);
            }
            
        }


        private void CheckAppConfig()
        {
            try
            {
                Trace.WriteLine("Check App.Config settings.");

                string resultHeaderPurgeLimit = ConfigurationManager.AppSettings["ResultHeaderPurgeLimit"];
                if (resultHeaderPurgeLimit.IsNullOrWhiteSpace())
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings.Add("ResultHeaderPurgeLimit", "500");
                    config.AppSettings.Settings.Add("ResultHeaderReturnLimit", "250");
                    config.AppSettings.Settings.Add("ThirdPartyStartArgs", "-i -n");
                    config.AppSettings.Settings.Add("SupportPath", "C:\\InstrumentUI_ATK Setup Files\\TeamViewerQS_en.exe");
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    Helper.SupportPath = ConfigurationManager.AppSettings["SupportPath"];
                }

                string averageScan = ConfigurationManager.AppSettings["AverageScans"];
                if (averageScan.IsNullOrWhiteSpace())
                {
                    Trace.WriteLine("Adding AverageScans keys.");
                    Configuration configS = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configS.AppSettings.Settings.Add("AllowAveraging", "false");
                    configS.AppSettings.Settings.Add("AverageScans", "false");
                    configS.AppSettings.Settings.Add("AverageScanScans", "3");
                    configS.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }

                string averageScanCleanCheck = ConfigurationManager.AppSettings["AverageScanCleanCheck"];
                if (averageScanCleanCheck.IsNullOrWhiteSpace())
                {
                    Configuration configS = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configS.AppSettings.Settings.Add("AverageScanCleanCheck", "true");
                    configS.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception ex)
            {
                Helper.LogError("CheckAppConfig()", "", ex, true);
            }
        }


        /// <summary>
        /// Main Form load event. Localize all texts on the control.
        /// </summary>
        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                Helper.CurrentForm = ctrlLogin;
                LocalizeResource();
            }
            catch (Exception ex)
            {
                Helper.DisplayError("The following Error occurred in the Main.Main_Load() : " + ex.Message);
                Helper.LogError("Main.cs", "", ex, false);
            }
        }


        /// <summary>
        /// Override WndProc to listen any custom notification
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethod.WM_MAKE_ACTIVE)
            {
                MakeActive();
            }
            base.WndProc(ref m);
        }


        #region Functions to Show/Hide different form controls

        /// <summary>
        /// Display Console Start up screen
        /// </summary>
        private void DisplayConsoleStartUp()
        {
            ClearBody();
            var consoleStartUp = new ConsoleStartUp {Dock = DockStyle.Fill};
            pnlBody.Controls.Add(consoleStartUp);

            consoleStartUp.OnConsoleStartUpCompleted += consoleStartUp_OnConsoleStartUpCompleted;
            consoleStartUp.StartUp(Helper.CurrentUser.Id);
        }


        /// <summary>
        /// Display Workflow dialog
        /// </summary>
        private void DisplayWorkflow(bool isInitializePhase)
        {
            //SD: This is the UI wrapper class that encapsulates the actual
            //WorkflowParser class.  WorkflowParser is run on a separate
            //background worker thread.  As per requirements, since both
            //classes, Workflow & the WorkflowParser class, can cancel one
            //one another - each class monitors the other's property to 
            //raise the custom events & prevent a background worker's racing
            //condition and exit gracefully.
            var workflowForm = new WorkflowForm();
            int x = ((Width - workflowForm.Width) / 2) + Location.X;
            int y = ((pnlBody.Height - workflowForm.Height) / 2) + pnlBody.Location.Y;
            workflowForm.Location = new Point(x, y);
            workflowForm.IsInitPhase = isInitializePhase;

            if (isInitializePhase)
                workflowForm.OnInitPhaseCompleted += workflowForm_OnInitPhaseCompleted;
            else
                workflowForm.OnAllPhasesCompleted += workflowForm_OnAllPhasesCompleted;
            
            workflowForm.OnWfError += workflowForm_OnWfError;
            workflowForm.StartUp();

            if (!isInitializePhase)
                workflowForm.ShowDialog(this);
        }
        

        void workflowForm_OnWfError(object sender, FaultMessageEventArgs e)
        {
            ((WorkflowForm)sender).Dispose();
            picLogout_Click(null, null, () => Helper.DisplayError("XML File Error", e.StepFaultMessage, Application.Restart));
        }


        /// <summary>
        /// Display Tabs and Icon according to thier role
        /// </summary>
        private void DisplayMain()
        {
            ClearBody();
            pnlHeader.Visible = true;
            pnlMiddle.Visible = true;

            SelectTabAnalyze();

            if (TabAnalyze != null)
                TabAnalyze.DisplayTabIcon(Helper.CurrentSampleClasses);
        }


        /// <summary>
        /// Display Login Form
        /// </summary>
        private void DisplayLogin()
        {
            pnlBody.Controls.Clear();
            pnlHeader.Visible = false;

            ctrlLogin.Anchor = AnchorStyles.None;
            ctrlLogin.Location = GetControlLocation(ctrlLogin);

            pnlBody.Controls.Add(ctrlLogin);

            toolStripStatuslbl.Visible = false;
            lblDemo.Visible = false;

            Helper.CurrentForm = ctrlLogin;
        }


        /// <summary>
        /// Display Routine Form
        /// </summary>
        private void DisplayRoutine()
        {
            Helper.AnalyzeType = "Routine";
            ClearBody();

            if (TabAnalyze.HasSampleClasses)
            {
                if (FormRoutine == null)
                {
                    FormRoutine = new Routine(Helper.CurrentTraits);
                    FormRoutine.OnAnalyzeFormValidationCompleted += analyze_OnAnalyzeFormValidationCompleted;
                    FormRoutine.Dock = DockStyle.Fill;

                    // get the last selected values from the database regarding the Analyze tab
                    CacheSample sample = DAL.GetCachedSample(Helper.SAMPLE_CLASS_ROUTINE);

                    //set the last selected values On Routine
                    FormRoutine.Init(sample);
                }
                else
                {
                    FormCurrentAnalyze = FormRoutine;
                    FormRoutine.ResetSampleIdentifiers();
                }

                pnlBody.Controls.Add(FormRoutine);

                //if (isRepeatSample || isNextSample)
                //    this.FormRoutine.RepeatSample(isNextSample);

                Helper.CurrentForm = FormRoutine;
                FormRoutine.SetFocus();
            }
            else
            {
                // Sample class dosen't exist
                Helper.DisplayError(ResourceHelper.Error_10403); 
            }
        }


        /// <summary>
        /// Display Check Form
        /// </summary>
        private void DisplayCheck()
        {
            Helper.AnalyzeType = "Check";
            ClearBody();

            if (TabAnalyze.HasSampleClasses)
            {
                if (FormCheck == null)
                {
                    FormCheck = new Check(Helper.CurrentTraits) {Dock = DockStyle.Fill};
                    FormCheck.OnAnalyzeFormValidationCompleted += analyze_OnAnalyzeFormValidationCompleted;

                    // get the last selected values from the database regarding the Analyze tab
                    CacheSample sample = DAL.GetCachedSample(Helper.SAMPLE_CLASS_CHECK);

                    //set the last selected values On Check
                    FormCheck.Init(sample);
                }
                else
                {
                    FormCurrentAnalyze = FormCheck;
                    FormCheck.ResetSampleIdentifiers();
                }

                pnlBody.Controls.Add(FormCheck);
                Helper.CurrentForm = FormCheck;
                FormCheck.SetFocus();
            }
            else
            {
                // Sample class dosen't exist
                Helper.DisplayError(ResourceHelper.Error_10403);
            }
        }


        /// <summary>
        /// EventHandler Wrapper for calling DisplaySchedule
        /// </summary>
        private void DisplaySchedule(object sender, EventArgs e)
        {
            DisplaySchedule();
        }


        /// <summary>
        /// Display the Schedule form
        /// </summary>
        private void DisplaySchedule()
        {
            ClearBody();

            if (FormSchedule == null)
            {
                FormSchedule = new ScheduleForm(Helper.CurrentTraits) {Dock = DockStyle.Fill};
                FormSchedule.OnManageQueueClicked += FormSchedule_OnManageQueueClick;
                FormSchedule.OnScheduleSummaryClicked += FormSchedule_OnScheduleSummaryClick;
            }

            pnlBody.Controls.Add(FormSchedule);
        }


        /// <summary>
        /// Display the Manage Queue form
        /// </summary>
        private void DisplayManageQueueForm()
        {
            ClearBody();
            var form = new ScheduleQueueForm {Dock = DockStyle.Fill};
            form.OnBackClicked += DisplaySchedule;
            pnlBody.Controls.Add(form);
        }


        /// <summary>
        /// Display the Schedule Summary form
        /// </summary>
        private void DisplayScheduleSummaryForm()
        {
            ClearBody();
            var form = new ScheduleSummaryForm {Dock = DockStyle.Fill};
            form.OnBackClicked += DisplaySchedule;
            pnlBody.Controls.Add(form);
        }


        private void DisplayAnalyze(bool isRepeatSample, bool isNextSample)
        {
            if (FormCurrentAnalyze is Routine)
                DisplayRoutine();
            else if (FormCurrentAnalyze is Check)
                DisplayCheck();

            if (isRepeatSample || isNextSample)
                FormCurrentAnalyze.RepeatSample(isNextSample);
        }


        /// <summary>
        /// Display Preferences Form
        /// </summary>
        private void DisplayPreference()
        {
            ClearBody();
            if (null == ctrlPreferences)
            {
                ctrlPreferences = new Preferences();
                ctrlPreferences.OnChangePasswordLinkClicked += 
                    ctrlPreferences_OnChangePasswordLinkClicked;
            }

            // set Y location same as setup and X location based on the Preference form
            SetUp ctrlSetup = new SetUp();
            Point pointSetup = GetControlLocation(ctrlSetup);
            Point pointPreference = GetControlLocation(ctrlPreferences);

            ctrlPreferences.Location = new Point(pointPreference.X, pointSetup.Y);

            ctrlPreferences.Anchor = AnchorStyles.None;
            pnlBody.Controls.Add(ctrlPreferences);

            Helper.CurrentForm = ctrlPreferences;
        }

        private void DisplayChangePassword()
        {
            ClearBody();
            ChangePassword ctrlChangePassword = new ChangePassword();
            ctrlChangePassword.OnChangePasswordCancelClicked += ctrlChangePassword_OnChangePasswordCancelClicked;
            pnlBody.Controls.Add(ctrlChangePassword);
            Helper.CurrentForm = ctrlChangePassword;
        }


        private void DisplaySupport()
        {
            ClearBody();
            Support ctrlSupport = new Support();
            ctrlSupport.Anchor = AnchorStyles.None;

            // set Y location same as setup and X location based on the support form
            SetUp ctrlSetup = new SetUp();
            Point pointSetup = GetControlLocation(ctrlSetup);
            Point pointSupport = GetControlLocation(ctrlSetup);

            ctrlSupport.Location = new Point(pointSupport.X, pointSetup.Y);
            pnlBody.Controls.Add(ctrlSupport);
            Helper.CurrentForm = ctrlSupport;
        }


        private void DisplaySetup()
        {
            ClearBody();
            SetUp ctrlSetup = new SetUp {Anchor = AnchorStyles.None};
            ctrlSetup.Location = GetControlLocation(ctrlSetup);
            pnlBody.Controls.Add(ctrlSetup);
            Helper.CurrentForm = ctrlSetup;
        }


        /// <summary>
        /// Display QTA Alerts
        /// </summary>
        private void DisplayAlerts(bool autoPopup)
        {
            pnlHeader.Visible = true;
            Alerts alerts = new Alerts();

            if (!autoPopup || alerts.HasAlerts)
                alerts.ShowDialog(this);

            // if no alert then don't display the toolstrip
            toolStripStatuslbl.Visible = alerts.HasAlerts;
        }


        private void DisplayResults(string requestId)
        {
            ClearBody();

            Result results = new Result(Convert.ToInt32(requestId));
            results.OnNextSample += results_OnNextSample;
            results.OnRepeatSample += results_OnRepeatSample;
            results.OnResultsDeleted += results_OnResultsDeleted;
            
            results.Anchor = AnchorStyles.None;
            results.Location = GetControlLocation(results);
            pnlBody.Controls.Add(results);
            Helper.CurrentForm = results;
        }


		private void DisplayPreviousResults()
        {
            ClearBody();
		    PreviousResults previousResults = new PreviousResults {Anchor = AnchorStyles.None};

		    // set Y location same as setup and X location based on the support form
            Result ctrlResult = new Result();
            Point pointResult = GetControlLocation(ctrlResult);
            Point pointpreviousResults = GetControlLocation(previousResults);

            previousResults.Location = new Point(pointpreviousResults.X, pointResult.Y);
            pnlBody.Controls.Add(previousResults);

            iconPreviousResults.Selected = true;
            Helper.CurrentForm = previousResults;
        }


        /// <summary>
        /// Clear the panel which contains the FormControls
        /// </summary>
        private void ClearBody()
        {
            pnlBody.Controls.Clear();
            AcceptButton = null;
            iconPreviousResults.Selected = false;
        }

        #endregion

        #region Form Control Events

        private void ctrlLogin_OnUserAuthenticated(object sender, EventArgs e)
        {
            //if demo user then switch on the demo mode
            if (Helper.CurrentUser.IsDemo())
                lblDemo.Visible = true;
            else
                lblDemo.Visible = false;

            //Hide the middle icon panel
            pnlMiddle.Visible = false;

            //Move dark blue area higher to cover phone chat and help icons
            pnlBody.Location = new Point(pnlBody.Location.X, pnlMiddle.Location.Y);

            // increase the height of body panel
            pnlBody.Height = (statusStripMain.Location.Y - pnlBody.Location.Y) - 3; // 3px space in between

            //Update time for succesful login
            DataService.InstrumentServiceClient serviceClient = Helper.GetServiceInstance(); ;
            serviceClient.UpdateLoginTime(Helper.CurrentUser.Id);

            DisplayConsoleStartUp();
        }


        private void consoleStartUp_OnConsoleStartUpCompleted(object sender, EventArgs e)
        {
            Helper.CurrentTraits = ((ConsoleStartUp)sender).Traits;
            Helper.CurrentSampleClasses = ((ConsoleStartUp)sender).SampleClasses;
            Helper.CurrentSpectrometer = ((ConsoleStartUp)sender).SpectType;

            //SD: Since the Initialization Phase is required to run on console
            //startup completion, I have slightly modified the section below
            //to support the required functionality.

            //SD: Check the RunWorkflow Configuration setting and if true,
            //display and start initialization phase of the Workflow
            if (ConfigurationManager.AppSettings[RUN_WORFLOW].Equals("true") && !Helper.CurrentUser.IsDemo())
                DisplayWorkflow(true);
            else
            {
                //Move dark blue area lower to display phone chat and help icons
                pnlBody.Location = new Point(pnlBody.Location.X, pnlMiddle.Location.Y + pnlMiddle.Size.Height + 3); // 3px space in between

                //Display the middle icon panel
                pnlMiddle.Visible = true;

                // decrease the height of body panel
                pnlBody.Height = (statusStripMain.Location.Y - pnlBody.Location.Y) - 3; // 3px space in between

                DisplayMain();

                toolStripStatuslbl.Visible = true;
                DisplayRoutine();
                DisplayAlerts(true);
            }
        }


        /// <summary>
        /// Handles the WorkflowForm's OnInitPhaseCompleted event
        /// </summary>
        private void workflowForm_OnInitPhaseCompleted(object sender, EventArgs e)
        {
            //SD: Event Handler for Workflow's OnInitPhaseCompleted that drives
            //the appropriate Instrument UI behavior - display analyze + alerts.
            if (((WorkflowForm)sender).IsCancelled)
            {
                Trace.WriteLine("Initialize Phase of Workflow Cancelled ...");
                ((WorkflowForm)sender).Dispose();
                picLogout_Click(null, null);
            }
            else
            {
                //Move dark blue area lower to display phone chat and help icons
                pnlBody.Location = new Point(pnlBody.Location.X, pnlMiddle.Location.Y + pnlMiddle.Size.Height + 3); // 3px space in between

                //Display the middle icon panel
                pnlMiddle.Visible = true;

                // decrease the height of body panel
                pnlBody.Height = (statusStripMain.Location.Y - pnlBody.Location.Y) - 3; // 3px space in between

                ((WorkflowForm)sender).Dispose();
                DisplayMain();

                toolStripStatuslbl.Visible = true;

                if (Helper.CurrentSampleClasses.Any(i => i.Name == Helper.SAMPLE_CLASS_SCHEDULED))
                    DisplaySchedule();
                else
                    DisplayRoutine();

                DisplayAlerts(true);
                pnlMiddle.Visible = true;

                if (((WorkflowForm)sender).WorkflowParser.IsWorkflowCancelled)
                {
                    Helper.DisplayError(((WorkflowForm)sender).WorkflowParser.WorkflowCancelledMessageTitle, ((WorkflowForm)sender).WorkflowParser.WorkflowCancelledMessage, () => Application.Restart());
                }
            }
        }


        /// <summary>
        /// Handles the WorkflowForm's OnAllPhasesCompleted event
        /// </summary>
        private void workflowForm_OnAllPhasesCompleted(object sender, EventArgs e)
        {
            //SD: Event Handler for Workflow's OnAllPhasesCompleted that drives
            //the appropriate Instrument UI behavior - 
            bool SpeedMode = Helper.SpeedMode;
            bool SpeedModeDualScan = Helper.SpeedModeDualScan;

            if (((WorkflowForm)sender).IsCancelled)
            {
                MultiScanRunCount = 0; 
                ((WorkflowForm)sender).Dispose();
                FormCurrentAnalyze.SetAnalyzeButtonEnabled(true);
            }
            else
            {
                string requestId = ((WorkflowForm)sender).WorkflowParser.CurrentRequestId;
                ((WorkflowForm)sender).Dispose();

                if ((!SpeedMode && !SpeedModeDualScan) || FormCurrentAnalyze.GetContainerControl().ToString() == "InstrumentUI_ATK.FormControls.Check")
                {
                    DisplayResults(requestId);
                }
                else if (SpeedMode && !SpeedModeDualScan)
                {
                    DisplayAnalyze(true, true);
                }
                else 
                {
                    if (SpeedModeDualScan && MultiScanRunCount == 0)
                    {
                        MultiScanRunCount++;
                        //this.FormCurrentAnalyze.btnInsertPrevious_Click(this, e);
                        FormCurrentAnalyze.btnAnalyze_Click(this, e);
                    }
                    else if (SpeedModeDualScan && SpeedMode)
                    {
                        MultiScanRunCount = 0;
                        DisplayAnalyze(true, true);
                        FormCurrentAnalyze.SetAnalyzeButtonEnabled(false);
                    }
                    else if (SpeedModeDualScan)
                    {
                        MultiScanRunCount = 0;
                        DisplayResults(requestId);
                    }
                }
            }
        }

        
        void analyze_OnAnalyzeFormValidationCompleted(object sender, EventArgs e)
        {
            Helper.SelectedTraits = ((Analyze)sender).SelectedTraits;
            
            //TODO: Comment to see the result screen
            //**************************************************************************
            //SD: Check the RunWorkflow Configuration setting and if true,
            //display and start remaining phases of the Workflow
            if (ConfigurationManager.AppSettings[RUN_WORFLOW].Equals("true"))
                DisplayWorkflow(false);

            //**************************************************************************

            // TODO: Remove for release.(Test code to see the result screen)
            //**************************************************************************
            //DataService.InstrumentServiceClient serviceClient = Helper.GetServiceInstance(); ;
            //DataService.Result result = serviceClient.GetResult("102");

            //// insert results and record sample identifier in the recordedSampleIdentifier table
            //bool isSuccess = DAL.InsertResults(result, Helper.CurrentSample.MaterialId);

            //DisplayResults("102");
            //**************************************************************************
        }


        private void results_OnRepeatSample(object sender, EventArgs e)
        {
            DisplayAnalyze(true, false);
        }


        private void results_OnNextSample(object sender, EventArgs e)
        {
            DisplayAnalyze(false, true);
        }


        private void results_OnResultsDeleted(object sender, EventArgs e)
        {
            DisplayAnalyze(false, true);
        }

        #endregion

        #region Ribbon Tab Selected Events

        /// <summary>
        /// Analyze tab click
        /// </summary>
        private void btnRibbonHeaderAnalyze_Click(object sender, EventArgs e)
        {
            SelectTabAnalyze();
        }


        /// <summary>
        /// Reports tab click
        /// </summary>
        private void btnRibbonHeaderReports_Click(object sender, EventArgs e)
        {
            SelectTabReports();
        }


        /// <summary>
        /// Submit Data tab click
        /// </summary>
        private void btnRibbonHeaderSubmitData_Click(object sender, EventArgs e)
        {
            SelectTabSubmitData();
        }


        private void btnRibbonHeaderAdmin_Click(object sender, EventArgs e)
        {
            SelectTabAdmin();
        }

        #endregion

        #region Functions to Enable/Disable different Ribbon tabs

        /// <summary>
        /// Select the Analyze tab display related icons
        /// </summary>
        private void SelectTabAnalyze()
        {
            if (TabAnalyze == null)
            {
                TabAnalyze = new RibbonTabAnalyze();
                TabAnalyze.OnRoutineClick += TabAnalyze_OnRoutineClick;
                TabAnalyze.OnCheckClick += TabAnalyze_OnCheckClick;
                TabAnalyze.OnScheduledClick += TabAnalyze_OnScheduledClick;
            }

            InitTab(TabAnalyze);

            pnlRibbonTab.Controls.Add(TabAnalyze);

            btnRibbonHeaderAnalyze.BackgroundImage = _tabActiveImage;
        }


        /// <summary>
        /// Select the Reports tab display related icons
        /// </summary>
        private void SelectTabReports()
        {
            RibbonTabReports tabReports = new RibbonTabReports();
            //tabReports.OnCofAClick += new EventHandler(tabReports_OnCofAClick);
            //tabReports.OnSingleAnalysisClick += new EventHandler(tabReports_OnSingleAnalysisClick);
            tabReports.OnWebReportClick += tabReports_OnTrendGraphClick;

            InitTab(tabReports);

            pnlRibbonTab.Controls.Add(tabReports);

            btnRibbonHeaderReports.BackgroundImage = _tabActiveImage;
        }


        private void tabReports_OnTrendGraphClick(object sender, EventArgs e)
        {
            ClearBody();
        }


        /// <summary>
        /// Select the Submit Data tab display related icons
        /// </summary>
        private void SelectTabSubmitData()
        {
            RibbonTabSubmitData tabSubmitdata = new RibbonTabSubmitData();

            InitTab(tabSubmitdata);

            pnlRibbonTab.Controls.Add(tabSubmitdata);

            btnRibbonHeaderSubmitData.BackgroundImage = _tabActiveImage;
        }


        /// <summary>
        /// Select the Admin tab display related icons
        /// </summary>
        private void SelectTabAdmin()
        {
            RibbonTabAdmin tabAdmin = new RibbonTabAdmin();
            tabAdmin.OnPreferencesClick += tabAdmin_OnPreferencesClick;
            tabAdmin.OnSupportClick += tabAdmin_OnSupportClick;
            tabAdmin.OnSetupClick += tabAdmin_OnSetupClick;
            tabAdmin.OnBodyClick += tabAdmin_OnBodyClick;

            InitTab(tabAdmin);

            pnlRibbonTab.Controls.Add(tabAdmin);

            btnRibbonHeaderAdmin.BackgroundImage = _tabActiveImage;
        }


        void tabAdmin_OnBodyClick(object sender, EventArgs e)
        {
            HideMessage();
        }


        /// <summary>
        /// It Does all the common task required to load a tab.
        /// </summary>
        /// <param name="tab"></param>
        private void InitTab(IRibbonTab tab)
        {
            CurrentTab = tab;

            ClearBody();
            DeSelectAllTabs();
            pnlRibbonTab.Controls.Clear();

            tab.LocalizeResource();

            if (CurrentTab.DefaultButton != null)
                CurrentTab.DefaultButton.PerformClick();
        }


        /// <summary>
        /// Clear all tab selections
        /// </summary>
        private void DeSelectAllTabs()
        {
            btnRibbonHeaderAnalyze.BackgroundImage = _tabInActiveImage;
            btnRibbonHeaderReports.BackgroundImage = _tabInActiveImage;
            btnRibbonHeaderSubmitData.BackgroundImage = _tabInActiveImage;
            btnRibbonHeaderAdmin.BackgroundImage = _tabInActiveImage;
        }

        #endregion

        /// <summary>
        /// Logout click event
        /// </summary>
        private void picLogout_Click(object sender, EventArgs e)
        {
            RememberAnalyze();
            FormRoutine = null;
            FormCheck = null;

            // display login form
            DisplayLogin();

            // reset login control values and initialize its values if requied(remember me functionality)
            ctrlLogin.ResetValues();
            ctrlLogin.InitValues();

            //reset memory values
            Helper.ClearValues();
            GC.Collect();
            GC.WaitForPendingFinalizers();                                        
            Application.Exit();
        }


        private void picLogout_Click(object sender, EventArgs e, Action action)
        {
            action.Invoke();
            RememberAnalyze();
            FormRoutine = null;
            FormCheck = null;

            // display login form
            DisplayLogin();

            // reset login control values and initialize its values if requied(remember me functionality)
            ctrlLogin.ResetValues();
            ctrlLogin.InitValues();

            //reset memory values
            Helper.ClearValues();
            GC.Collect();
            GC.WaitForPendingFinalizers();                                        
            Application.Exit();
        }


        /// <summary>
        /// Status toolstrip clicked, display all alerts including popup
        /// </summary>
        private void toolStripStatuslbl_Click(object sender, EventArgs e)
        {
            HideMessage();
            DisplayAlerts(false);
        }


        /// <summary>
        /// Routine button on Analyze tab clicked, display Routine control
        /// </summary>
        private void TabAnalyze_OnRoutineClick(object sender, EventArgs e)
        {
            DisplayRoutine();
        }


        /// <summary>
        /// Check Sample is selected, display the Check control
        /// </summary>
        private void TabAnalyze_OnCheckClick(object sender, EventArgs e)
        {
            DisplayCheck();
        }


        /// <summary>
        /// Scheduled button is clicked, display Schedule form
        /// </summary>
        private void TabAnalyze_OnScheduledClick(object sender, EventArgs e)
        {
            DisplaySchedule();
        }


        /// <summary>
        /// Schedule Form's Manage Queue button is clicked, display Manage Queue form
        /// </summary>
        private void FormSchedule_OnManageQueueClick(object sender, EventArgs e)
        {
            DisplayManageQueueForm();
        }


        /// <summary>
        /// Schedule Form's Schedule Summary button is clicked, display Summary Queue form
        /// </summary>
        private void FormSchedule_OnScheduleSummaryClick(object sender, EventArgs e)
        {
            DisplayScheduleSummaryForm();
        }


        private void tabAdmin_OnPreferencesClick(object sender, EventArgs e)
        {
            if (null != ctrlPreferences)
            {
                ctrlPreferences.Dispose();
                ctrlPreferences = null;
            }
            DisplayPreference();
        }


        private void ctrlPreferences_OnChangePasswordLinkClicked(object sender, EventArgs e)
        {
            DisplayChangePassword();
        }


        private void ctrlChangePassword_OnChangePasswordCancelClicked(object sender, EventArgs e)
        {
            DisplayPreference();
        }


        private void tabAdmin_OnSupportClick(object sender, EventArgs e)
        {
            DisplaySupport();
        }


        private void tabAdmin_OnSetupClick(object sender, EventArgs e)
        {
            DisplaySetup();
        }


        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            btnRibbonHeaderAnalyze.Text = ResourceHelper.Analyze;
            btnRibbonHeaderReports.Text = ResourceHelper.Reports;
            btnRibbonHeaderSubmitData.Text = ResourceHelper.Submit_Data;
            btnRibbonHeaderAdmin.Text = ResourceHelper.Admin;

            lblPreviousResults.Text = ResourceHelper.Previous_Results;
            btnChat.Text = ResourceHelper.Chat;

            toolStripStatuslbl.Text = ResourceHelper.You_Have_Alerts;

            Text = ResourceHelper.Quality_Trait_Analysis;
            lblDemo.Text = ResourceHelper.Demo;
        }

        #region Class Level Private Functions

        /// <summary>
        /// It store all information on the Routine and Check from(provided by user) into the database.
        /// </summary>
        private void RememberAnalyze()
        {
            using (var dataContext = new InstrumentUIDataContext(Helper.CONNECTION_STRING))
            {
                using (var transaction = new TransactionScope())
                {
                    if (FormRoutine != null)
                        InsertCacheSample(dataContext, FormRoutine, Helper.SAMPLE_CLASS_ROUTINE);

                    if (FormCheck != null)
                        InsertCacheSample(dataContext, FormCheck, Helper.SAMPLE_CLASS_CHECK);

                    dataContext.SubmitChanges();

                    transaction.Complete();
                }
            }
        }


        /// <summary>
        /// Insert user selections in the local db cache
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="currentSample"></param>
        /// <param name="sampleType"></param>
        private void InsertCacheSample(InstrumentUIDataContext dataContext, Analyze currentSample, string sampleType)
        {
            // Clear tables before recording any data
            dataContext.ExecuteCommand("DELETE CacheSample WHERE SampleType='" + sampleType + "'");

            var sample = new CacheSample
                             {
                                 MaterialId = currentSample.MaterialId,
                                 CategoryId = currentSample.CategoryId,
                                 SubCategoryId = currentSample.SubCategoryId,
                                 PresentationId = currentSample.PresentationId,
                                 Traits = String.Join(",", currentSample.SelectedTraits.Select(t => t.Id.ToString()).ToArray()),
                                 SampleType = sampleType
                             };

            dataContext.CacheSamples.InsertOnSubmit(sample);
        }

        #endregion

        private void iconPreviousResults_Click(object sender, EventArgs e)
        {
            
            iconPreviousResults.Selected = true;
            if (CurrentTab != null)
                CurrentTab.DeSelectButtons();

            DisplayPreviousResults();
        }


        private void picPhone_Click(object sender, EventArgs e)
        {
            HideMessage();
            Phone phone = new Phone();
            phone.ShowDialog(this);
        }


        /// <summary>
        /// Chat button click. It opens a url specified for chat
        /// </summary>
        private void btnChat_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                Process.Start(Helper.SupportPath);
            }
            catch (Exception ex)
            {
                Helper.LogError("btnChat_Click", string.Empty, ex, true);
            }
        }


        private void picHelp_Click(object sender, EventArgs e)
        {
            DataService.InstrumentServiceClient serviceClient = null;
            bool isServiceClosed = false;

            try
            {
                HideMessage();
                serviceClient = Helper.GetServiceInstance();

                // Get all the help url
                string helpURL = "https://secure.qta.com/qtahelp"; //serviceClient.GetHelpUrl(Helper.CurrentForm.HelpCode);

                serviceClient.Close();
                isServiceClosed = true;

                if (!string.IsNullOrEmpty(helpURL))
                    Process.Start("IExplore.exe", helpURL);
            }
            catch (Exception ex)
            {
                Helper.LogError("picHelp_Click", string.Empty, ex, true);
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Abort();

                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }
        }


        /// <summary>
        /// This event will open url www.qta.com in browser at click of logo
        /// </summary>
        private void picHeader1_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                Process.Start("IExplore.exe", Helper.QTA_URL);
            }
            catch (Exception ex)
            {
                Helper.LogError("picHeader1_Click", string.Empty, ex, true);
            }
        }


        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            RememberAnalyze();

            if (FormSchedule != null)
                FormSchedule.Dispose();
        }


        /// <summary>
        /// Bring the form in the front
        /// </summary>
        private void MakeActive()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            // get our current "TopMost" value 
            bool top = TopMost;
            // make our form jump to the top of everything
            TopMost = true;
            // set it back to whatever it was
            TopMost = top;
        }


        private Point GetControlLocation(UserControl ctrl)
        {
            int x = (pnlBody.Width / 2) - (ctrl.Width / 2);
            int y = (pnlBody.Height / 2) - (ctrl.Height / 2);

            return new Point(x, y);
        }


        private void form_Click(object sender, EventArgs e)
        {
            HideMessage();
        }


        private void HideMessage()
        {
            if (Helper.CurrentForm is Preferences)
            {
                ((Preferences)Helper.CurrentForm).HideMessage();
            }
            else if (Helper.CurrentForm is SetUp)
            {
                ((SetUp)Helper.CurrentForm).HideMessage();
            }
        }


        private void picInfo_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                Process.Start("IExplore.exe", Helper.INFO_URL);
            }
            catch (Exception ex)
            {
                Helper.LogError("btnChat_Click", string.Empty, ex, true);
            }
        }
    }
}
    