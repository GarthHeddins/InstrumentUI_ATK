using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.DataAccess.Model;
using InstrumentUI_ATK.Workflow;
using Quartz;
using Quartz.Impl;

namespace InstrumentUI_ATK.FormControls
{
    public partial class ScheduleForm : UserControl
    {
        #region Properties

        /// <summary>
        /// Scheduler (used to move scheduled scans to the scan queue)
        /// </summary>
        private IScheduler _scheduler;


        /// <summary>
        /// Lock for assuring thread safety when running scans
        /// </summary>
        private readonly object _queueLock = new object();


        /// <summary>
        /// Whether a scan is running
        /// </summary>
        private bool _scanIsRunning;


        /// <summary>
        /// List of Traits
        /// </summary>
        public List<Trait> AllTraits { get; set; }


        /// <summary>
        /// Gets or Sets the Maximum Number of Scans allowed
        /// </summary>
        private short MaxScans
        {
            get
            {
                if (_maxScans == -1)
                {
                    var strMaxScans = ConfigurationManager.AppSettings["MaxScans"];
                    if (string.IsNullOrEmpty(strMaxScans))
                        throw new ArgumentNullException("MaxScans", "Argument must be set in the appSettings section of the application's configuration file.");


                    short maxScans;
                    if (short.TryParse(strMaxScans, out maxScans))
                    {
                        _maxScans = maxScans;
                    }
                    else
                    {
                        throw new ArgumentException("MaxScans", "Argument set in the appSettings section of the application's configuration file had an invalid value.  Argument must be an Integer.");
                    }
                }
                return _maxScans;
            }
        }
        private short _maxScans = -1;


        /// <summary>
        /// Gets the number of scheduled scans
        /// </summary>
        private int ScanCount
        {
            get
            {
                var activeControlName = ActiveControl == null ? "" : ActiveControl.Name;
                var scanCount = 0;
                for (short rowNumber = 1; rowNumber <= 6; rowNumber++)
                {
                    var currentScanControlName = string.Format("Schedule{0}Scans", rowNumber);
                    var currentUserField1ControlName = string.Format("Schedule{0}UserField1", rowNumber);
                    var currentUserField2ControlName = string.Format("Schedule{0}UserField2", rowNumber);
                    var currentMaterialControlName = string.Format("Schedule{0}Material", rowNumber);

                    var scansControl = GetScansPerHourControl(rowNumber);
                    var activeControl = GetActiveControl(rowNumber);

                    if (activeControl.Checked 
                                    || (activeControlName == currentScanControlName) 
                                    || (activeControlName == currentUserField1ControlName) 
                                    || (activeControlName == currentUserField2ControlName)
                                    || (activeControlName == currentMaterialControlName))
                        scanCount += (int)scansControl.Value;
                }
                return scanCount;
            }
        }


        /// <summary>
        /// Gets the number of scheduled scans for Active lines
        /// </summary>
        private int ActiveScanCount
        {
            get
            {
                var scanCount = 0;
                for (short i = 1; i <= 6; i++)
                {
                    var scansControl = GetScansPerHourControl(i);
                    var activeControl = GetActiveControl(i);

                    if (activeControl.Checked)
                        scanCount += (int)scansControl.Value;
                }
                return scanCount;
            }
        }


        protected Color HighlightForeColor = Color.Red;


        /// <summary>
        /// Data Access Layer for the Schedule
        /// </summary>
        private ScheduleRepository _repository;


        /// <summary>
        /// BackgroundWorker object which runs the scheduled scans
        /// </summary>
        private BackgroundWorker ScanRunner;


        /// <summary>
        /// WorkflowParser object which handles the Scan
        /// </summary>
        private WorkflowParser Scan;


        /// <summary>
        /// The format string for messages added to the Scan History
        /// </summary>
        private const string _scanHistoryMessageFormat = "Sample Location {0}, {1}, {2}";

        #endregion

        #region Event Handlers

        public event EventHandler OnManageQueueClicked;
        public event EventHandler OnScheduleSummaryClicked;


        /// <summary>
        /// Handle the OnCheckedChanged event for the Active checkbox
        /// </summary>
        private void ActiveCheckedChanged(object sender, EventArgs e)
        {
            SuspendLayout();

            var cb = (CheckBox)sender;
            var locationNumber = GetLocationNumberFromControlName(cb);
            string message;

            if ((cb.Checked == false) || CanScheduleBeSaved(locationNumber, out message))
                SaveSchedule(locationNumber);
            else
            {
                cb.CheckedChanged -= ActiveCheckedChanged;
                cb.Checked = false;
                cb.CheckedChanged += ActiveCheckedChanged;

                // Display a popup message
                Helper.DisplayError(message);
            }

            ResumeLayout();
        }


        /// <summary>
        /// Handle the OnSelectedValueChanged event for the Material combobox
        /// </summary>
        private void MaterialValueChanged(object sender, EventArgs e)
        {
            SuspendLayout();

            var control = (ComboBox)sender;
            var locationNumber = GetLocationNumberFromControlName(control);
            var scansControl = GetScansPerHourControl(locationNumber);

            UpdateScansMaxValue(control, scansControl);

            SaveSchedule(locationNumber);
            UpdateScans(locationNumber);

            ResumeLayout();
        }


        /// <summary>
        /// Handle the OnValueChanged event for the Scans per Hour number picker
        /// </summary>
        private void ScansPerHourValueChanged(object sender, EventArgs e)
        {
            SuspendLayout();

            var control = (NumericUpDown)sender;
            var locationNumber = GetLocationNumberFromControlName(control);

            SaveSchedule(locationNumber);
            UpdateScans(locationNumber);

            ResumeLayout();
        }


        /// <summary>
        /// Handles teh Manage Queue button's OnClick event
        /// </summary>
        private void btnQueue_Click(object sender, EventArgs e)
        {
            if (OnManageQueueClicked != null)
                OnManageQueueClicked(this, new EventArgs());
        }


        /// <summary>
        /// Handles the View Summary button's OnClick event
        /// </summary>
        private void btnSummary_Click(object sender, EventArgs e)
        {
            if (OnScheduleSummaryClicked != null)
                OnScheduleSummaryClicked(this, new EventArgs());
        }

        #endregion


        /// <summary>
        /// Create a new instance of the Schedule form
        /// </summary>
        /// <param name="allTraits"></param>
        public ScheduleForm(List<Trait> allTraits)
        {
            _repository = new ScheduleRepository();

            InitializeComponent();

            AllTraits = allTraits;

            InitializeScanQueue();
            InitializeUI();
            InitializeScheduler();
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) 
                    components.Dispose();

                // Stop the Scheduler
                if (_scheduler.IsStarted)
                    _scheduler.Shutdown();

                // Make sure that no more Scans get started
                Helper.ScanQueue.Clear();

                if (ScanRunner.IsBusy)
                    ScanRunner.CancelAsync();
            }
            base.Dispose(disposing);
        }

        #region UI Methods

        /// <summary>
        /// Initialize the Control with default data
        /// </summary>
        public void InitializeUI()
        {
            SuspendLayout();

            LocalizeResource();

            var scheduleList = _repository.GetSchedule(Helper.CurrentUser.UserName);
            scheduleList = DeactivateScheduleItems(scheduleList);

            foreach (var item in scheduleList)
            {
                bool isActive = item.MaterialId > 0 && item.ScansPerHour > 0 && item.IsActive;

                SetupMaterial(item.LocationNumber, item.MaterialId, isActive);
                SetupScans(item.LocationNumber, item.ScansPerHour);
                SetupUserFields(item.LocationNumber, item.MaterialId, item.UserField1, item.UserField2);
                SetupActivate(item.LocationNumber, isActive);

                LockSchedule(item.LocationNumber, isActive);
            }

            UpdateScanCount();
            
            SetEventHandlers();

            ResumeLayout();
        }


        /// <summary>
        /// Marks all of the items in a Schedule as inactive and then saves the Schedule
        /// </summary>
        /// <param name="scheduleList">List of ScheduleItems</param>
        private List<ScheduleItem> DeactivateScheduleItems(List<ScheduleItem> scheduleList)
        {
            foreach(var item in scheduleList)
                item.IsActive = false;

            _repository.SaveSchedule(scheduleList);

            return scheduleList;
        }


        /// <summary>
        /// Set the Event Handlers for the Form controls
        /// </summary>
        private void SetEventHandlers()
        {
            Schedule1Active.CheckedChanged += ActiveCheckedChanged;
            Schedule2Active.CheckedChanged += ActiveCheckedChanged;
            Schedule3Active.CheckedChanged += ActiveCheckedChanged;
            Schedule4Active.CheckedChanged += ActiveCheckedChanged;
            Schedule5Active.CheckedChanged += ActiveCheckedChanged;
            Schedule6Active.CheckedChanged += ActiveCheckedChanged;

            Schedule1Active.GotFocus += ControlGotFocus;
            Schedule2Active.GotFocus += ControlGotFocus;
            Schedule3Active.GotFocus += ControlGotFocus;
            Schedule4Active.GotFocus += ControlGotFocus;
            Schedule5Active.GotFocus += ControlGotFocus;
            Schedule6Active.GotFocus += ControlGotFocus;

            Schedule1Material.SelectedValueChanged += MaterialValueChanged;
            Schedule2Material.SelectedValueChanged += MaterialValueChanged;
            Schedule3Material.SelectedValueChanged += MaterialValueChanged;
            Schedule4Material.SelectedValueChanged += MaterialValueChanged;
            Schedule5Material.SelectedValueChanged += MaterialValueChanged;
            Schedule6Material.SelectedValueChanged += MaterialValueChanged;

            Schedule1Material.GotFocus += ControlGotFocus;
            Schedule2Material.GotFocus += ControlGotFocus;
            Schedule3Material.GotFocus += ControlGotFocus;
            Schedule4Material.GotFocus += ControlGotFocus;
            Schedule5Material.GotFocus += ControlGotFocus;
            Schedule6Material.GotFocus += ControlGotFocus;

            Schedule1Scans.ValueChanged += ScansPerHourValueChanged;
            Schedule2Scans.ValueChanged += ScansPerHourValueChanged;
            Schedule3Scans.ValueChanged += ScansPerHourValueChanged;
            Schedule4Scans.ValueChanged += ScansPerHourValueChanged;
            Schedule5Scans.ValueChanged += ScansPerHourValueChanged;
            Schedule6Scans.ValueChanged += ScansPerHourValueChanged;

            Schedule1Scans.GotFocus += ControlGotFocus;
            Schedule2Scans.GotFocus += ControlGotFocus;
            Schedule3Scans.GotFocus += ControlGotFocus;
            Schedule4Scans.GotFocus += ControlGotFocus;
            Schedule5Scans.GotFocus += ControlGotFocus;
            Schedule6Scans.GotFocus += ControlGotFocus;

            Schedule1UserField1.TextChanged += UserFieldTextChanged;
            Schedule2UserField1.TextChanged += UserFieldTextChanged;
            Schedule3UserField1.TextChanged += UserFieldTextChanged;
            Schedule4UserField1.TextChanged += UserFieldTextChanged;
            Schedule5UserField1.TextChanged += UserFieldTextChanged;
            Schedule6UserField1.TextChanged += UserFieldTextChanged;

            Schedule1UserField1.GotFocus += ControlGotFocus;
            Schedule2UserField1.GotFocus += ControlGotFocus;
            Schedule3UserField1.GotFocus += ControlGotFocus;
            Schedule4UserField1.GotFocus += ControlGotFocus;
            Schedule5UserField1.GotFocus += ControlGotFocus;
            Schedule6UserField1.GotFocus += ControlGotFocus;

            Schedule1UserField2.TextChanged += UserFieldTextChanged;
            Schedule2UserField2.TextChanged += UserFieldTextChanged;
            Schedule3UserField2.TextChanged += UserFieldTextChanged;
            Schedule4UserField2.TextChanged += UserFieldTextChanged;
            Schedule5UserField2.TextChanged += UserFieldTextChanged;
            Schedule6UserField2.TextChanged += UserFieldTextChanged;

            Schedule1UserField2.GotFocus += ControlGotFocus;
            Schedule2UserField2.GotFocus += ControlGotFocus;
            Schedule3UserField2.GotFocus += ControlGotFocus;
            Schedule4UserField2.GotFocus += ControlGotFocus;
            Schedule5UserField2.GotFocus += ControlGotFocus;
            Schedule6UserField2.GotFocus += ControlGotFocus;

            ScheduledScans.GotFocus += ControlGotFocus;
            AvailableScans.GotFocus += ControlGotFocus;

            // Hide the Manage Queue button
            var showManageQueueButton = ConfigurationManager.AppSettings["ShowManageQueueButton"];
            if (!string.IsNullOrEmpty(showManageQueueButton) && showManageQueueButton.ToLower() == "false")
                btnQueue.Visible = false;

            // Hide the Schedule Summary button
            var showScheduleSummaryButton = ConfigurationManager.AppSettings["ShowScheduleSummaryButton"];
            if (!string.IsNullOrEmpty(showScheduleSummaryButton) && showScheduleSummaryButton.ToLower() == "false")
                btnSummary.Visible = false;
        }


        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            if (ResourceHelper.Culture.Name.ToLower() == Helper.CULTURE_NAME_ENGLISH.ToLower())
            {
                //AdjustEnglish();
            }
            else if (ResourceHelper.Culture.Name.ToLower() == Helper.CULTURE_NAME_SPANISH.ToLower())
            {
                //AdjustSpanish();
            }

            lblControlTitle.Text = ResourceHelper.Schedule;
            lblColumnActive.Text = ResourceHelper.Active;
            lblColumnLocation.Text = ResourceHelper.SampleLocation;
            lblColumnMaterial.Text = ResourceHelper.Material;
            lblColumnUserField1.Text = ResourceHelper.UserField1;
            lblColumnUserField2.Text = ResourceHelper.UserField2;
            lblColumnScansPerHour.Text = ResourceHelper.ScansPerHour;
            lblColumnIntervals.Text = ResourceHelper.SampleInterval;
            lblScheduledScans.Text = ResourceHelper.Scheduled;
            lblAvailableScans.Text = ResourceHelper.Available;
            btnSummary.Text = ResourceHelper.ViewSummary;
            btnQueue.Text = ResourceHelper.ManageQueue;
        }


        /// <summary>
        /// Updates the Scan Counts
        /// </summary>
        void ControlGotFocus(object sender, EventArgs e)
        {
            UpdateScanCount();
        }


        /// <summary>
        /// Save changes when the User Field's Text value has changed
        /// </summary>
        void UserFieldTextChanged(object sender, EventArgs e)
        {
            var control = (Control) sender;
            var locationNumber = GetLocationNumberFromControlName(control);

            SaveSchedule(locationNumber);
        }


        /// <summary>
        /// Fill material type dropdown with materials
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <param name="defaultValue">Default Material</param>
        /// <param name="isActive">Whether the Schedule is Active or not</param>
        protected void SetupMaterial(short locationNumber, int defaultValue, bool isActive)
        {
            var materials = GetMaterialList(locationNumber);
            var materialControl = GetMaterialControl(locationNumber);
            materialControl.DataSource = materials;
            materialControl.DisplayMember = "Name";
            materialControl.ValueMember = "Id";
            materialControl.SelectedValue = defaultValue;

            if (locationNumber == 1)
            {
                if (materials.Count > 1)
                    SetUserFieldHeadings(materials[1].Id.Value);
            }
        }


        /// <summary>
        /// Retrieve the list of Materials 
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>List of Materials</returns>
        private List<IdNamePair> GetMaterialList(short locationNumber)
        {
            var port = string.Format("PORT {0}", locationNumber);
            var materials = AllTraits.Where(m => m.MaterialId > 0 && 
                                                 m.ModelGroupStageName.ToUpper() == "INLINE" &&
                                                 m.PresentationName.ToUpper() == port)
                                     .Select(t => new IdNamePair { Id = t.MaterialId, Name = t.MaterialName })
                                     .Distinct((m1, m2) => m1.Id == m2.Id)
                                     .OrderBy(m => m.Name);

            IEnumerable<IdNamePair> materialsWithSelect = new[] { new IdNamePair { Id = 0, Name = ResourceHelper.Select } };

            materialsWithSelect = materialsWithSelect.Concat(materials);

            return materialsWithSelect.ToList();
        }


        /// <summary>
        /// Set the values for the Scans per Hour
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <param name="defaultValue">Default Scans per Hour</param>
        private void SetupScans(short locationNumber, short defaultValue)
        {
            var materialControl = GetMaterialControl(locationNumber);
            var scansControl = GetScansPerHourControl(locationNumber);

            scansControl.Minimum = 0;
            UpdateScansMaxValue(materialControl, scansControl);

            if (defaultValue >= scansControl.Minimum && defaultValue <= scansControl.Maximum)
                scansControl.Value = defaultValue;

            UpdateInterval(locationNumber);
        }


        /// <summary>
        /// Updates the Scan control's Maximum value property based on the Material selected
        /// </summary>
        private void UpdateScansMaxValue(ComboBox materialControl, NumericUpDown scansControl)
        {
            scansControl.Maximum = ((int)materialControl.SelectedValue == 0) ? 0 : MaxScans;
        }


        /// <summary>
        /// Set the default values for the User Fields
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <param name="materialId">Material Id</param>
        /// <param name="userField1">User Field 1 value</param>
        /// <param name="userField2">User Field 2 value</param>
        private void SetupUserFields(short locationNumber, int materialId, string userField1, string userField2)
        {
            var udf1Control = GetUserField1Control(locationNumber);
            udf1Control.Text = userField1;

            var udf2Control = GetUserField2Control(locationNumber);
            udf2Control.Text = userField2;
        }


        /// <summary>
        /// Look up which Sample Identifiers are used for the materials and set their names
        /// as the column headings for the User Fields
        /// </summary>
        private void SetUserFieldHeadings(int materialId)
        {
            var sampleIdentifiers = Helper.GetSampleIdentifiers(materialId).ToList();
            if (sampleIdentifiers.Count > 2)
            {
                lblColumnUserField1.Text = sampleIdentifiers[1].Name;
                lblColumnUserField2.Text = sampleIdentifiers[2].Name;
            }
        }


        /// <summary>
        /// Set the default value for the Active checkbox
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <param name="isActive">Whether the Schedule is Active or not</param>
        private void SetupActivate(short locationNumber, bool isActive)
        {
            var activeControl = GetActiveControl(locationNumber);
            activeControl.Checked = isActive;

            MarkAsCanBeActivated(locationNumber);
        }


        /// <summary>
        /// Locks or Unlocks all the controls for a specified Schedule
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <param name="locked">Whether the is to locked or not (unlocked)</param>
        private void LockSchedule(short locationNumber, bool locked)
        {
            var materialControl = GetMaterialControl(locationNumber);
            var udf1Control = GetUserField1Control(locationNumber);
            var udf2Control = GetUserField2Control(locationNumber);
            var scansControl = GetScansPerHourControl(locationNumber);

            materialControl.Enabled = !locked;
            udf1Control.Enabled = !locked;
            udf2Control.Enabled = !locked;
            scansControl.Enabled = !locked;
        }


        /// <summary>
        /// Update all fields dependent on the scan counts
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        private void UpdateScans(short locationNumber)
        {
            MarkAsCanBeActivated(locationNumber);

            UpdateInterval(locationNumber);
            UpdateScanCount();
        }


        /// <summary>
        /// Enable / Disable the Active checkbox
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        private void MarkAsCanBeActivated(short locationNumber)
        {
            var activeControl = GetActiveControl(locationNumber);
            var scansControl = GetScansPerHourControl(locationNumber);
            var materialControl = GetMaterialControl(locationNumber);
            var materialId = Convert.ToInt32(materialControl.SelectedValue);

            activeControl.Enabled = (scansControl.Value > 0 && materialId > 0);
        }


        /// <summary>
        /// Update the Scheduled and Available scan counts
        /// </summary>
        private void UpdateScanCount()
        {
            ScheduledScans.Text = ScanCount.ToString();
            var available = MaxScans - ScanCount;
            AvailableScans.Text = available.ToString();

            // Do not allow user to schedule more scans than allowed
            var foreColor = DefaultForeColor; 
            if (available < 0)
                foreColor = HighlightForeColor;

            ScheduledScans.ForeColor = foreColor;
            AvailableScans.ForeColor = foreColor;
        }


        /// <summary>
        /// Update the Interval value based on the Scans per Hour for the specified Schedule
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        private void UpdateInterval(short locationNumber)
        {
            var scansControl = GetScansPerHourControl(locationNumber);
            var intervalControl = GetIntervalControl(locationNumber);
            var scansPerHour = (short)scansControl.Value;
            var minutes = Helper.CalculateInterval(scansPerHour);

            intervalControl.Text = minutes.ToString("0.00");
        }


        /// <summary>
        /// Verify that the Schedule can be saved
        /// </summary>
        /// <returns>True, if the Schedule can be saved; False, otherwise.</returns>
        private bool CanScheduleBeSaved(short locationNumber, out string message)
        {
            if (ActiveScanCount > MaxScans)
            {
                message = ResourceHelper.Error_11201;
                return false;
            }

            var materialId = Convert.ToInt32(GetMaterialControl(locationNumber).SelectedValue);
            if (!ValidateIdentifySample(materialId, GetUserField1Control(locationNumber), GetUserField2Control(locationNumber), out message))
            {
                message += ResourceHelper.Error_11202;
                return false;
            }

            return true;
        }


        /// <summary>
        /// It will validate all sample identifiers displayed in the Identify Sample section.
        /// </summary>
        /// <returns></returns>
        private bool ValidateIdentifySample(int materialId, Control control1, Control control2, out string message)
        {
            bool isValid = true;
            message = string.Empty;

            var sampleIdentifiers = Helper.GetSampleIdentifiers(materialId).ToList();

            var si = sampleIdentifiers[1];
            if (si.Required)
            {
                if (control1.Text.Length == 0)
                {
                    // Invalid value
                    message += ResourceHelper.Error_11203.Replace("[0]", si.Name) + Environment.NewLine + Environment.NewLine;
                    isValid = false;
                }
            }

            si = sampleIdentifiers[2];
            if (si.Required)
            {
                if (control2.Text.Length == 0)
                {
                    // Invalid value
                    message += ResourceHelper.Error_11203.Replace("[0]", si.Name) + Environment.NewLine + Environment.NewLine;
                    isValid = false;
                }
            }

            return isValid;
        }


        /// <summary>
        /// Saves the State of the Schedules
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        private void SaveSchedule(short locationNumber)
        {
            var schedule = GetCurrentScheduleState(locationNumber);
            _repository.SaveSchedule(schedule);

            var isActive = schedule[0].IsActive;
            LockSchedule(locationNumber, isActive);

            if (isActive)
                AddScanToScheduler(schedule[0]);
            else
                RemoveScanFromScheduler(schedule[0]);

            UpdateScanCount();
        }


        /// <summary>
        /// Retrieve the Schedule data and return as a list of ScheduleItems
        /// </summary>
        /// <returns>List of ScheduleItems</returns>
        private List<ScheduleItem> GetCurrentScheduleState(short locationNumber)
        {
            short min = locationNumber > 0 ? locationNumber : (short) 1;
            short max = locationNumber > 0 ? locationNumber : (short) 6;

            var schedule = new List<ScheduleItem>();

            for (short i = min; i <= max; i++)
            {
                var activeControl = GetActiveControl(i);
                var materialControl = GetMaterialControl(i);
                var udf1Control = GetUserField1Control(i);
                var udf2Control = GetUserField2Control(i);
                var scansControl = GetScansPerHourControl(i);

                var item = new ScheduleItem
                               {
                                   UserName = Helper.CurrentUser.UserName,
                                   LocationNumber = i,
                                   IsActive = activeControl.Checked,
                                   MaterialId = Convert.ToInt32(materialControl.SelectedValue),
                                   UserField1 = udf1Control.Text,
                                   UserField2 = udf2Control.Text,
                                   ScansPerHour = Convert.ToInt16(scansControl.Value)
                               };

                schedule.Add(item);
            }

            return schedule;
        }


        /// <summary>
        /// Shows / Hides the Scanning Image for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <param name="isVisible">True, to show; False, to hide</param>
        private void UpdateScanningImage(short locationNumber, bool isVisible)
        {
            if (InvokeRequired)
                Invoke(new UpdateScanningImageDelegate(InternalUpdateScanningImage), new object[] { locationNumber, isVisible });
            else
                InternalUpdateScanningImage(locationNumber, isVisible);
        }


        /// <summary>
        /// Shows / Hides the Scanning Image for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <param name="isVisible">True, to show; False, to hide</param>
        private void InternalUpdateScanningImage(short locationNumber, bool isVisible)
        {
            var scanningControl = GetScanningImageControl(locationNumber);
            scanningControl.Visible = isVisible;
        }


        /// <summary>
        /// Append a message to the Scan History
        /// </summary>
        /// <param name="message">Message to Append</param>
        public void AppendToScanHistory(string message)
        {
            if (InvokeRequired)
                Invoke(new AppendToScanHistoryDelegate(InternalAppendToScanHistory), new object[] { message });
            else
                InternalAppendToScanHistory(message);
        }


        /// <summary>
        /// Append a message to the Scan History
        /// </summary>
        /// <param name="message">Message to Append</param>
        public void InternalAppendToScanHistory(string message)
        {
            tbScanHistory.Text = message + Environment.NewLine + tbScanHistory.Text;
        }

        #region Helpers for Retrieving Controls

        /// <summary>
        /// Get the Active CheckBox control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>Active CheckBox control</returns>
        private CheckBox GetActiveControl(short locationNumber)
        {
            return (CheckBox)Controls.Find(string.Format("Schedule{0}Active", locationNumber), true)[0];
        }


        /// <summary>
        /// Get the Location Label control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>Location Label control</returns>
        private Label GetLocationControl(short locationNumber)
        {
            return (Label)Controls.Find(string.Format("Schedule{0}Location", locationNumber), true)[0];
        }


        /// <summary>
        /// Get the Material ComboBox control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>Material ComboBox control</returns>
        private ComboBox GetMaterialControl(short locationNumber)
        {
            return (ComboBox)Controls.Find(string.Format("Schedule{0}Material", locationNumber), true)[0];
        }


        /// <summary>
        /// Get the UserField1 CheckBox control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>UserField1 CheckBox control</returns>
        private TextBox GetUserField1Control(short locationNumber)
        {
            return (TextBox)Controls.Find(string.Format("Schedule{0}UserField1", locationNumber), true)[0];
        }


        /// <summary>
        /// Get the UserField2 CheckBox control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>UserField2 CheckBox control</returns>
        private TextBox GetUserField2Control(short locationNumber)
        {
            return (TextBox)Controls.Find(string.Format("Schedule{0}UserField2", locationNumber), true)[0];
        }


        /// <summary>
        /// Get the ScansPerHour NumericUpDown control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>ScansPerHour NumericUpDown control</returns>
        private NumericUpDown GetScansPerHourControl(short locationNumber)
        {
            return (NumericUpDown)Controls.Find(string.Format("Schedule{0}Scans", locationNumber), true)[0];
        }


        /// <summary>
        /// Get the Interval Label control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>Interval Label control</returns>
        private Label GetIntervalControl(short locationNumber)
        {
            return (Label)Controls.Find(string.Format("Schedule{0}Interval", locationNumber), true)[0];
        }


        /// <summary>
        /// Get the Scanning Image control for the specified Location Number
        /// </summary>
        /// <param name="locationNumber">Location Number</param>
        /// <returns>Scanning Image control</returns>
        private PictureBox GetScanningImageControl(short locationNumber)
        {
            switch(locationNumber)
            {
                case 1:
                    return Schedule1Scanning;
                case 2:
                    return Schedule2Scanning;
                case 3:
                    return Schedule3Scanning;
                case 4:
                    return Schedule4Scanning;
                case 5:
                    return Schedule5Scanning;
                case 6:
                    return Schedule6Scanning;
                default:
                    return null;
            }
        }


        /// <summary>
        /// Get the Location Number from the Control's Name
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>Location Number</returns>
        private short GetLocationNumberFromControlName(Control control)
        {
            return short.Parse(control.Name.Replace("Schedule", "").Substring(0, 1));
        }

        #endregion

        #endregion

        #region Scheduler Methods

        /// <summary>
        /// Sets up the Scheduler
        /// </summary>
        private void InitializeScheduler()
        {
            // Load and start the Scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Start();

            // Add the Active Schedules to the Scheduler
            LoadSchedule();
        }


        /// <summary>
        /// Retrieves the list of Active Schedules and adds them to the Scheduler.
        /// The Scheduler will queue the scans when their scheduled time arrives.
        /// </summary>
        private void LoadSchedule()
        {
            var scheduleList = _repository.GetSchedule(Helper.CurrentUser.UserName)
                                          .Where(s => s.IsActive);

            foreach (var schedule in scheduleList)
                AddScanToScheduler(schedule);
        }


        /// <summary>
        /// Add a Scheduled Scan to the Scheduler
        /// </summary>
        private void AddScanToScheduler(ScheduleItem schedule)
        {
            // Create the Job
            IJobDetail job = JobBuilder.Create<ScheduledScanJob>()
                                       .WithIdentity(schedule.LocationNumber.ToString(), schedule.UserName)
                                       .Build();

            // Add the Scan details to the job
            job.JobDataMap.Add("schedule", schedule);


            // Create the Triggers
            var triggers = new Quartz.Collection.HashSet<ITrigger>();
            var interval = Helper.CalculateInterval(schedule.ScansPerHour);
            decimal startTime = 0;
            for (var i = 0; i < schedule.ScansPerHour; i++)
            {
                var minute = (short)startTime;
                short second = Helper.CalculateSecondsFromFraction(startTime);
                const string hour = "*";
                const string dayOfMonth = "*";
                const string month = "*";
                const string dayOfWeek = "?";

                var cronString = string.Format("{0} {1} {2} {3} {4} {5}", second, minute, hour, dayOfMonth, month, dayOfWeek);
                var triggerName = string.Format("{0}_{1}", schedule.LocationNumber, i);

                // Create a Trigger for each interval
                ITrigger trigger = TriggerBuilder.Create()
                                                 .WithIdentity(triggerName, schedule.LocationNumber.ToString())
                                                 .StartNow()
                                                 .WithCronSchedule(cronString)
                                                 .Build();       

                triggers.Add(trigger);
                startTime += interval;
            }

            // Add the Job and its Triggers to the Scheduler
            _scheduler.ScheduleJob(job, triggers, true);
        }


        /// <summary>
        /// Remove a Scheduled Scan from the Scheduler
        /// </summary>
        private void RemoveScanFromScheduler(ScheduleItem schedule)
        {
            var key = new JobKey(schedule.LocationNumber.ToString(), schedule.UserName);
            if (_scheduler.CheckExists(key))
                _scheduler.DeleteJob(key);
        }

        #endregion

        #region Queue Methods

        /// <summary>
        /// Initialize Processing of the ScanQueue
        /// </summary>
        private void InitializeScanQueue()
        {
            ScanRunner = new BackgroundWorker {WorkerSupportsCancellation = true};
            ScanRunner.DoWork += ScanRunnerDoWork;
            ScanRunner.RunWorkerCompleted += ScanRunnerCompleted;

            Helper.ScanQueue.ItemEnqueued += ScanQueue_ItemEnqueued;
        }


        /// <summary>
        /// Make sure that when an Item is added to the ScanQueue, that it is processed
        /// </summary>
        private void ScanQueue_ItemEnqueued(object sender, ScheduleQueueEventArgs e)
        {
            var locationNumber = e.Item.Location;
            AppendToScanHistory(string.Format(_scanHistoryMessageFormat, locationNumber, DateTime.Now, "Scan added to queue."));

            // Using a lock to make sure only one scan runs at a time
            lock (_queueLock)
            {
                if (_scanIsRunning)
                    return;

                ProcessQueueItem();
            }
        }


        /// <summary>
        /// Run the Scan for the Queued Scan job
        /// </summary>
        private void ProcessQueueItem()
        {
            _scanIsRunning = true;

            // Retrieve the next Sample to scan
            var queueItem = Helper.ScanQueue.Dequeue();

            // Set the current sample
            Helper.CurrentSample = queueItem.Sample;
            Helper.SelectedTraits = GetTraits(queueItem.Sample.MaterialId);

            // Update the UI
            var port = Helper.CurrentSample.PresentationName;
            var locationNumber = short.Parse(port.Replace("Port ", ""));

            UpdateScanningImage(locationNumber, true);

            ScanRunner.RunWorkerAsync();
        }


        /// <summary>
        /// Retrieve the list of Traits for the specified Material ID
        /// </summary>
        /// <param name="materialId">Material ID</param>
        /// <returns>List of Traits</returns>
        private List<Trait> GetTraits(int materialId)
        {
            return DAL.GetAllTrait().Where(t => t.MaterialId == materialId).ToList();
        }


        /// <summary>
        /// Handles the ScanRunner's DoWork event
        /// </summary>
        private void ScanRunnerDoWork(object sender, DoWorkEventArgs e)
        {
            RunScheduledScan();
        }


        /// <summary>
        /// Run the Scheduled Scan
        /// </summary>
        private void RunScheduledScan()
        {
            Trace.WriteLine("ScheduleForm.RunScheduledScan has been called.");

            var locationNumber = Helper.CurrentSample.PresentationName.Substring(5);
            AppendToScanHistory(string.Format(_scanHistoryMessageFormat, locationNumber, DateTime.Now, "Scan started."));

            Scan = Helper.GetWorkflowInstance();

            //Scan.RaiseMessageEvent += ScanRaiseMessageEvent;
            Scan.RaiseUIMessageEvent += ScanRaiseUiMessageEvent;
            Scan.RaiseCancelWorkflowEvent += ScanRaiseCancelWorkflowEvent;
            Scan.RaiseCancelCleanCheckWorkflowEvent += ScanRaiseCancelCleanCheckWorkflowEvent;

            Scan.ExecuteWorkflow();
        }


        /// <summary>
        /// Handles the ScanRunner's RunWorkerCompleted event
        /// </summary>
        private void ScanRunnerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RunNextScan();
        }


        /// <summary>
        /// Run the Next Scan in the Queue
        /// </summary>
        private void RunNextScan()
        {
            var locationNumber = Helper.CurrentSample.PresentationName.Substring(5);
            AppendToScanHistory(string.Format(_scanHistoryMessageFormat, locationNumber, DateTime.Now, "Scan completed."));

            for (short i = 1; i <= 6; i++)
                UpdateScanningImage(i, false);

            lock (_queueLock)
            {
                if (Helper.ScanQueue.Count > 0)
                    ProcessQueueItem();
                else
                    _scanIsRunning = false;
            }
        }


        //private void ScanRaiseMessageEvent(Object sender, MessageEventArgs e)
        //{
        //    ScanRunner.ReportProgress(0, e);
        //}


        /// <summary>
        /// Handles the Scan's RaiseUIMessageEvent
        /// </summary>
        private void ScanRaiseUiMessageEvent(Object sender, UIMessageEventArgs e)
        {
            Trace.WriteLine("Entering ScanRaiseUiMessageEvent - e.StepUIMessage = " + e.StepUIMessage);
            
            if (e.StepUIMessage == "BPUIMessage")
            {
                Helper.DisplayMessage("Background Scan", Helper.GetLocalizedResourceMessage(e.StepUIMessage), true);
            }
            else if (e.StepUIMessage == "AveragePrompt" ||
                     e.StepUIMessage == "AverageCleanCheckPrompt" ||
                     e.StepUIMessage == "CleanCheckShortMsg")
            {
                bool shouldCancel = false;

                string sampleid = string.Empty;

                if (e.StepUIMessage != "CleanCheckShortMsg")
                    sampleid = "\r\n\n Sample: " + Helper.AverageCurrentSample.ToString();

                Helper.DisplayMessageWithCancel(ResourceHelper.Workflow_MessageHeader, Helper.GetLocalizedResourceMessage(e.StepUIMessage) + sampleid, false, false, true, () => shouldCancel = true);
                Trace.WriteLine("ScanRaiseUiMessageEvent()... shouldCancel = " + shouldCancel.ToString());

                if (shouldCancel)
                {
                    Trace.WriteLine("ScanRaiseUiMessageEvent()..setting Helper.CancelAverage = true");
                    Helper.CancelAverage = true;
                }
            }
            else
            {
                Helper.DisplayMessage(ResourceHelper.Workflow_MessageHeader, Helper.GetLocalizedResourceMessage(e.StepUIMessage), false);
            }

            Trace.WriteLine("Leaving ScanRaiseUiMessageEvent()");
        }


        /// <summary>
        /// Handles the Scan's RaiseCancelCleanCheckWorkflowEvent
        /// </summary>
        private void ScanRaiseCancelCleanCheckWorkflowEvent(Object sender, CancelCleanCheckWorkflowEventArgs e)
        {
            Trace.WriteLine("Entering ScanRaiseCancelCleanCheckWorkflowEvent. e.DisplayMessage = " + e.DisplayMessage);
            string message = Helper.GetLocalizedResourceMessage(e.Message);

            if (!string.IsNullOrEmpty(e.ToAppend))
                message += Environment.NewLine + Environment.NewLine + e.ToAppend;

            IsCancelled = true;
            bool shouldCancel = false;

            if (e.DisplayMessage)
            {
                if (Helper.CleanCheckFail)
                {
                    Trace.WriteLine("if (Helper.CleanCheckFail).");
                    IsCancelled = false;
                    Trace.WriteLine("Before Helper.DisplayMessageWithCancel.");
                    Helper.DisplayMessageWithCancel(ResourceHelper.Workflow_ErrorMessageHeading, message, true, false, true, () => shouldCancel = true);
                    Trace.WriteLine("After Helper.DisplayMessageWithCancel.");
                    
                    if (shouldCancel)
                    {
                        Trace.WriteLine("ScanRaiseCancelCleanCheckWorkflowEvent()..setting Helper.CancelAverage = true");
                        Helper.CancelAverage = true;
                        IsCancelled = true;
                    }
                    else
                        Helper.CancelAverage = false;
                }
            }
            Trace.WriteLine("Leaving ScanRaiseCancelCleanCheckWorkflowEvent.");
        }


        /// <summary>
        /// Handles the Scan's RaiseCancelWorkflowEvent
        /// </summary>
        private void ScanRaiseCancelWorkflowEvent(Object sender, CancelWorkflowEventArgs e)
        {
            Trace.WriteLine("Entering ScanRaiseCancelWorkflowEvent.");
            string message = Helper.GetLocalizedResourceMessage(e.Message);

            if (!string.IsNullOrEmpty(e.ToAppend))
                message += Environment.NewLine + Environment.NewLine + e.ToAppend;

            IsCancelled = true;
            Helper.DisplayError(ResourceHelper.Workflow_ErrorMessageHeading, message);
            Trace.WriteLine("Leaving ScanRaiseCancelWorkflowEvent.");
        }


        public bool IsCancelled { get; private set; }

        #endregion
    }

    public delegate void AppendToScanHistoryDelegate(string message);
    public delegate void UpdateScanningImageDelegate(short locationNumber, bool isVisible);
}
