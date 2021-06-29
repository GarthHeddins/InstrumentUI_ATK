using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.Controls;
using InstrumentUI_ATK.DataAccess;

namespace InstrumentUI_ATK.FormControls
{
    public partial class ScheduleSummaryForm : UserControl
    {
        /// <summary>
        /// Data Access Layer for the Schedule
        /// </summary>
        private readonly ScheduleRepository _repository;


        /// <summary>
        /// 
        /// </summary>
        private TableLayout tableLayout1;


        /// <summary>
        /// Event Handler for Back Button
        /// </summary>
        public event EventHandler OnBackClicked;


        /// <summary>
        /// Create a new instance of the ScheduleSummary class
        /// </summary>
        public ScheduleSummaryForm()
        {
            _repository = new ScheduleRepository();

            InitializeComponent();
            Initialize();
        }


        /// <summary>
        /// Initialize the control with default data
        /// </summary>
        private void Initialize()
        {
            LocalizeResource();
            var schedule = GenerateSchedule();
            DisplaySchedule(schedule);
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

            lblControlTitle.Text = ResourceHelper.ScheduleSummary;
            lblTime.Text = ResourceHelper.TimeMinSec;
            lblSampleLocations.Text = ResourceHelper.SampleLocation;
            btnBack.Text = ResourceHelper.Back;
        }


        /// <summary>
        /// Creates a Schedule of all the Scheduled Scan times 
        /// </summary>
        private SortedDictionary<string, LocationList> GenerateSchedule()
        {
            var scheduleList = _repository.GetSchedule(Helper.CurrentUser.UserName);

            var list = new SortedDictionary<string, LocationList>();

            foreach (var schedule in scheduleList)
            {
                // Only take in to account active schedules
                if (!schedule.IsActive)
                    continue;


                var interval = Helper.CalculateInterval(schedule.ScansPerHour);
                decimal startTime = 0;
                for (var i = 0; i < schedule.ScansPerHour; i++)
                {
                    var minute = (short)startTime;
                    short second = Helper.CalculateSecondsFromFraction(startTime);

                    var time = string.Format("{0}:{1}", minute.ToString("00"), second.ToString("00"));
                    LocationList line;
                    if (list.ContainsKey(time))
                    {
                        line = list[time];
                    }
                    else
                    {
                        line = new LocationList();
                        list.Add(time, line);
                    }

                    line[schedule.LocationNumber] = true;

                    startTime += interval;
                }
            }

            return list;
        }


        /// <summary>
        /// Display the schedule in a table
        /// </summary>
        /// <param name="schedule">Schedule Information</param>
        private void DisplaySchedule(SortedDictionary<string, LocationList> schedule)
        {
            CreateScheduleTableControl();

            foreach (var item in schedule)
            {
                var values = new string[7];
                values[0] = item.Key;
                foreach (var x in item.Value)
                    values[x.Key] = x.Value ? "x" : "";

                tableLayout1.AddRow(values);
            }
        }


        /// <summary>
        /// Create the TableLayout control which the schedule will be displayed in
        /// </summary>
        private void CreateScheduleTableControl()
        {
            tableLayout1 = new TableLayout(7, new[]{70,70,70,70,70,70,70});
            pnlScheduleDetails.Controls.Add(tableLayout1);

            // 
            // tableLayout1
            // 
            tableLayout1.BackColor = Color.White;
            tableLayout1.AlternateRowColor = Color.LightSteelBlue;
            tableLayout1.Padding = new Padding(0);
            tableLayout1.Margin = new Padding(0);
            tableLayout1.TextColor = Color.Black;
            //tableLayout1.TextFont = null;
        }


        /// <summary>
        /// Handles the Back button's click event
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (OnBackClicked != null)
                OnBackClicked(this, new EventArgs());
        }
    }

    internal class LocationList : Dictionary<short, bool>
    {
        public LocationList()
        {
            Add(1, false);
            Add(2, false);
            Add(3, false);
            Add(4, false);
            Add(5, false);
            Add(6, false);
        }
    }
}
