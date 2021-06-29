using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.Workflow;

namespace InstrumentUI_ATK.ModalForm
{
    public partial class WorkflowForm : Form
    {
        public event EventHandler OnInitPhaseCompleted;
        public event EventHandler OnAllPhasesCompleted;
        public event EventHandler<FaultMessageEventArgs> OnWfError;
        public WorkflowParser WorkflowParser;

        public bool IsInitPhase { get; set; }
        public bool IsCancelled { get; private set; }


        public WorkflowForm()
        {
            IsInitPhase = false;
            IsCancelled = false;
            InitializeComponent();
        }


        private void Workflow_Load(object sender, EventArgs e)
        {
            LocalizeResource();
        }


        public void StartUp()
        {
            bgWorkerInitWF.RunWorkerAsync();
        }


        /// <summary>
        /// This function replaces all the form control text from the relevent resource file
        /// </summary>
        private void LocalizeResource()
        {
            lblTitle.Text = ResourceHelper.Workflow_Title;
        }



        private void WorkflowParserRaiseMessageEvent(Object sender, MessageEventArgs e)
        {
            bgWorkerInitWF.ReportProgress(0, e);
        }


        private void WorkflowParserRaiseUiMessageEvent(Object sender, UIMessageEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Entering wfParser_RaiseUIMessageEvent - e.StepUIMessage = " + e.StepUIMessage); 
            //MessageBox.Show(this, GetLocalizedResourceMessage(e.StepUIMessage), "Workflow", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (e.StepUIMessage == "BPUIMessage")
            {
                Helper.DisplayMessage("Background Scan", Helper.GetLocalizedResourceMessage(e.StepUIMessage), true);
            }
            else if (e.StepUIMessage == "AveragePrompt" || 
                     e.StepUIMessage == "AverageCleanCheckPrompt" || 
                     e.StepUIMessage == "CleanCheckShortMsg")
            {
                bool shouldCancel = false;

                //string sampleid = "\r\n\n " + InstrumentUI_ATK.Common.ResourceHelper.Sample_ID + ": " + Helper.CurrentSample.SampleIdentifiers[0].Value;
                string sampleid = string.Empty;
                
                if(e.StepUIMessage != "CleanCheckShortMsg")
                    sampleid = "\r\n\n Sample: " + Helper.AverageCurrentSample.ToString();

                Helper.DisplayMessageWithCancel(ResourceHelper.Workflow_MessageHeader, Helper.GetLocalizedResourceMessage(e.StepUIMessage) + sampleid, false, false, true, () => shouldCancel = true);
                System.Diagnostics.Trace.WriteLine("wfParser_RaiceUIMessageEvent()... shouldCancel = " + shouldCancel.ToString());

                if (shouldCancel)
                {
                    System.Diagnostics.Trace.WriteLine("wfParser_RaiceUIMessageEvent()..setting Helper.CancelAverage = true");
                    Helper.CancelAverage = true;
                }
            }
            else
            {
                Helper.DisplayMessage(ResourceHelper.Workflow_MessageHeader, Helper.GetLocalizedResourceMessage(e.StepUIMessage), false);
            }

            System.Diagnostics.Trace.WriteLine("Leaving wfParser_RaiseUIMEssageEvent()");
        }


        private void WorkflowParserRaiseCancelCleanCheckWorkflowEvent(Object sender, CancelCleanCheckWorkflowEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Entering wfParser_RaiseCancelCleanCheckWorkflowEvent. e.DisplayMessage = " + e.DisplayMessage.ToString());
            string message = Helper.GetLocalizedResourceMessage(e.Message);

            if (!string.IsNullOrEmpty(e.ToAppend))
            {
                message += Environment.NewLine + Environment.NewLine + e.ToAppend;
            }
            IsCancelled = true;
            bool shouldCancel = false;

            if (e.DisplayMessage)
            {
                if (Helper.CleanCheckFail)
                {
                    System.Diagnostics.Trace.WriteLine("if (Helper.CleanCheckFail).");
                    IsCancelled = false;
                    System.Diagnostics.Trace.WriteLine("B4 Helper.DisplayMessageWithCancel.");
                    Helper.DisplayMessageWithCancel(ResourceHelper.Workflow_ErrorMessageHeading, message, true, false, true, () => shouldCancel = true);
                    System.Diagnostics.Trace.WriteLine("After Helper.DisplayMessageWithCancel.");
                    if (shouldCancel)
                    {
                        System.Diagnostics.Trace.WriteLine("wfParser_RaiseCancelWorkflowEvent()..setting Helper.CancelAverage = true");
                        Helper.CancelAverage = true;
                        IsCancelled = true;
                    }
                    else
                        Helper.CancelAverage = false;
                }
            }
            System.Diagnostics.Trace.WriteLine("Leaving wfParser_RaiseCancelCleanCheckWorkflowEvent."); 
        }


        private void WorkflowParserRaiseCancelWorkflowEvent(Object sender, CancelWorkflowEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Entering wfParser_RaiseCancelWorkflowEvent.");
            string message = Helper.GetLocalizedResourceMessage(e.Message);

            if (!string.IsNullOrEmpty(e.ToAppend))
            {
                message += Environment.NewLine + Environment.NewLine + e.ToAppend;
            }

            IsCancelled = true;
            Helper.DisplayError(ResourceHelper.Workflow_ErrorMessageHeading, message);
            System.Diagnostics.Trace.WriteLine("Leaving wfParser_RaiseCancelWorkflowEvent.");
            //MessageBox.Show(this, GetLocalizedResourceMessage(e.Message), "Workflow", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }


        private void bgWorkerInitWF_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                WorkflowParser = Helper.GetWorkflowInstance();
                WorkflowParser.RaiseMessageEvent += WorkflowParserRaiseMessageEvent;
                WorkflowParser.RaiseUIMessageEvent += WorkflowParserRaiseUiMessageEvent;
                WorkflowParser.RaiseCancelWorkflowEvent += WorkflowParserRaiseCancelWorkflowEvent;
                WorkflowParser.RaiseCancelCleanCheckWorkflowEvent += WorkflowParserRaiseCancelCleanCheckWorkflowEvent;

                if (IsInitPhase)
                    WorkflowParser.ExecuteWorkflow(WorkflowParser.InstrumentPhase.InstrumentInit);
                else
                {
                    WorkflowParser.StartPreResultsProcessing += WorkflowParserStartPreResultsProcessing;
                    WorkflowParser.ExecuteWorkflow();
                }
            }
            catch (FileNotFoundException exception)
            {
                Helper.LogError("bgWorkerInitWF_DoWork", string.Empty, exception, false);

                if (OnWfError != null)
                {
                    OnWfError(this, new FaultMessageEventArgs(string.Format(ResourceHelper.Error_10503, exception.FileName)));
                }
            }
            catch (Exception exception)
            {
                Helper.LogError("bgWorkerInitWF_DoWork", string.Empty, exception, false);

                if (OnWfError != null)
                {
                    OnWfError(this, new FaultMessageEventArgs(string.Format("Unable to open the XML file: {0}.  Application restarting", exception.Message)));
                }
            }
        }


        void WorkflowParserStartPreResultsProcessing(object sender, EventArgs e)
        {
        }


        private void bgWorkerInitWF_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsInitPhase)
            {
                if (OnInitPhaseCompleted != null)
                    OnInitPhaseCompleted(this, new EventArgs());
            }
            else
            {
                if (OnAllPhasesCompleted != null)
                    OnAllPhasesCompleted(this, new EventArgs());
            }
        }


        private void bgWorkerInitWF_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblTitle.Text = Helper.GetLocalizedResourceMessage(((MessageEventArgs)e.UserState).StepMessage);
        }
    }
}
