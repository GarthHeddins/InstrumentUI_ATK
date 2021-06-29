using System;
using System.Drawing;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess.Model;

namespace InstrumentUI_ATK.FormControls
{
    public partial class ScheduleQueueForm : UserControl
    {
        /// <summary>
        /// Table Layout Panel which displays the Queue
        /// </summary>
        private TableLayoutPanel tlpQueue;


        /// <summary>
        /// Event Handler for Back Button
        /// </summary>
        public event EventHandler OnBackClicked;


        /// <summary>
        ///  Create a new instance of the ScheduleQueue class
        /// </summary>
        public ScheduleQueueForm()
        {
            InitializeComponent();
            Initialize();
        }


        /// <summary>
        /// Set up the form with default values
        /// </summary>
        private void Initialize()
        {
            LocalizeResource();
            PopulateQueue();

            Helper.ScanQueue.ItemDequeued += ScanQueueChanged;
            Helper.ScanQueue.ItemEnqueued += ScanQueueChanged;
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

            lblControlTitle.Text = ResourceHelper.QueuedScans;
            lblRemove.Text = ResourceHelper.Remove;
            lblScheduledTime.Text = ResourceHelper.ScheduledTime;
            lblSampleLocation.Text = ResourceHelper.SampleLocation;
            lblMaterial.Text = ResourceHelper.Material;
            btnBack.Text = ResourceHelper.Back;
        }


        /// <summary>
        /// Create controls to display the Schedule Queue
        /// </summary>
        private void PopulateQueue()
        {
            SuspendLayout();

            CreateQueueTablePanelLayout();
            foreach (ScheduleQueueItem item in Helper.ScanQueue)
                AddTableRow(item);

            ResumeLayout();
        }


        #region Queue Table Panel Layout

        private Font tf = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
        private Color c = Color.Black;


        /// <summary>
        /// Create the Table Panel Layout control to display the Queue
        /// </summary>
        private void CreateQueueTablePanelLayout()
        {
            tlpQueue = new TableLayoutPanel();
            pnlQueueDetails.Controls.Clear();
            pnlQueueDetails.Controls.Add(tlpQueue);


            // 
            // tlpQueue
            // 
            tlpQueue.Name = "tlpQueue";
            tlpQueue.TabIndex = 0;
            tlpQueue.AutoSize = true;
            tlpQueue.Dock = DockStyle.Top;
            tlpQueue.Size = new Size(791, 0);
            tlpQueue.Location = new Point(0, 0);
            tlpQueue.BackColor = Color.Transparent;

            tlpQueue.ColumnCount = 4;
            tlpQueue.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tlpQueue.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpQueue.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tlpQueue.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
        }


        /// <summary>
        /// Add a new row in the table with specified column values
        /// </summary>
        /// <param name="item">Schedule Queue Item</param>
        public void AddTableRow(ScheduleQueueItem item)
        {
            // add a new row
            tlpQueue.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            tlpQueue.RowCount++;
            tlpQueue.Height += 25;
            

            // Add the value to the table row
            tlpQueue.Controls.Add(NewDeleteControl(item.Id), 0, tlpQueue.RowCount - 1);
            tlpQueue.Controls.Add(NewLabel(item.ScheduledTime.ToString("HH:mm:ss"), tf, c), 1, tlpQueue.RowCount - 1);
            tlpQueue.Controls.Add(NewLabel(item.Location, tf, c), 2, tlpQueue.RowCount - 1);
            tlpQueue.Controls.Add(NewLabel(item.Material, tf, c), 3, tlpQueue.RowCount - 1);
        }


        /// <summary>
        /// Create a Control that will be added to the Queue for removing Queue items
        /// </summary>
        /// <returns>Delete Control</returns>
        private Button NewDeleteControl(int id)
        {
            var button = new Button {Text = ResourceHelper.RemoveButton, Name = string.Format("Remove_{0}", id)};
            button.Click += RemoveButtonClicked;

            return button;
        }


        /// <summary>
        /// Create a new lable control
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="font">Font</param>
        /// <param name="foreColor">Text Color</param>
        /// <returns></returns>
        private Label NewLabel(object text, Font font, Color foreColor)
        {
            var label = new Label
            {
                BackColor = Color.Transparent,
                Font = font,
                ForeColor = foreColor,
                Text = text.ToString(),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            return label;
        }

        #endregion


        /// <summary>
        /// Handles the RemoveButton's Clicked event
        /// </summary>
        private void RemoveButtonClicked(object sender, EventArgs e)
        {
            var id = int.Parse(((Button)sender).Name.Replace("Remove_", ""));
            Helper.ScanQueue.RemoveBySequence(id);

            PopulateQueue();
        }


        /// <summary>
        /// When the ScanQueue gets updated, update the display
        /// </summary>
        private void ScanQueueChanged(object sender, ScheduleQueueEventArgs e)
        {
            if (InvokeRequired)
                Invoke(new Delegate(PopulateQueue));
            else
                PopulateQueue();
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

    public delegate void Delegate();
}
