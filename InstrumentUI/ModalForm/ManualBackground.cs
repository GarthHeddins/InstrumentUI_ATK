using System;
using System.ComponentModel;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.Workflow;

namespace InstrumentUI_ATK.ModalForm
{
    public partial class ManualBackground : Form
    {
        private readonly string _fileName;

        public ManualBackground(string fileName)
        {
            _fileName = fileName;
            InitializeComponent();

            // make the corners rounded
            Region = System.Drawing.Region.FromHrgn(NativeMethod.CreateRoundRectRgn(-2, -2, Width + 2, Height + 2, 50, 40));
            timer1.Tick += timer1_Tick;
            timer1.Interval = 5000;
            timer1.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            RunWorkflow();

        }


        public void RunWorkflow()
        {
            WorkflowParser workflowParser = Helper.GetWorkflowInstance();

            workflowParser.RaiseMessageEvent += workflowParser_RaiseMessageEvent;
            workflowParser.RaiseUIMessageEvent += workflowParser_RaiseUIMessageEvent;
            workflowParser.ManualBackgroundComplete += workflowParser_ManualBackgroundComplete;
            workflowParser.ExecuteWorkflow(WorkflowParser.InstrumentPhase.ManualBackground, _fileName);
        }


        /// <summary>
        /// Load event of form.
        /// </summary>
        private void ManualBackground_Load(object sender, EventArgs e)
        {
            LocalizeResource();
        }


        public void StartUp()
        {
        }


        /// <summary>
        /// Close the Message dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        private void LocalizeResource()
        {
            btnOK.Text = ResourceHelper.Ok;
        }


        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkflowParser workflowParser = Helper.GetWorkflowInstance();

            workflowParser.RaiseMessageEvent += workflowParser_RaiseMessageEvent;
            workflowParser.RaiseUIMessageEvent += workflowParser_RaiseUIMessageEvent;
            workflowParser.ManualBackgroundComplete += workflowParser_ManualBackgroundComplete;
            workflowParser.ExecuteWorkflow(WorkflowParser.InstrumentPhase.ManualBackground);
        }


        private void workflowParser_ManualBackgroundComplete(object sender, EventArgs e)
        {
            Close();
        }


        private void workflowParser_RaiseUIMessageEvent(object sender, UIMessageEventArgs e)
        {
        }


        private void workflowParser_RaiseMessageEvent(object sender, MessageEventArgs e)
        {
        }
    }
}
