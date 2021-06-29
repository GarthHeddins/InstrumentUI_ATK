using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using InstrumentUI_ATK.Common;

namespace InstrumentUI_ATK.Workflow
{
    #region Event Handler

    public delegate void NextStepEventHandler(object sender, NextStepEventArgs e);

    public class NextStepEventArgs : EventArgs
    {
    }

    #endregion


    public class WorkflowParser
    {
        #region Enumerator

        public enum InstrumentPhase
        {
            [StringValue("all")]
            All = 0,
            [StringValue("instrumentinit")]
            InstrumentInit = 1,
            [StringValue("prescanprocessing")]
            PreScanProcessing = 2,
            [StringValue("scanbackground")]
            ScanBackground = 3,
            [StringValue("scansample")]
            ScanSample = 4,
            [StringValue("postscanprocessing")]
            PostScanProcessing = 5,
            [StringValue("preresultsprocessing")]
            PreResultsProcessing = 6,
            [StringValue("manualbackground")]
            ManualBackground = 7
        }

        #endregion

        #region Members

        private string _fileName = string.Empty;
        private string _filePath = string.Empty;
        private FileStream _fileStream;
        protected XmlTextReader _fileReader;


        public string CurrentRequestId
        {
            get { return _currentRequestId; }
        }
        protected string _currentRequestId;


        public bool IsWorkflowCancelled
        {
            get { return _isWorkflowCancelled; }
        }
        protected bool _isWorkflowCancelled;


        public string WorkflowCancelledMessageTitle
        {
            get { return _WorkflowCancelledMessageTitle; }
        }
        protected string _WorkflowCancelledMessageTitle = "Initialization Error";


        public string WorkflowCancelledMessage
        {
            get { return _WorkflowCancelledMessage; }
        }
        protected string _WorkflowCancelledMessage;


        /// <summary>
        /// Spectrometer Type Id
        /// </summary>
        public string SpectTypeId
        {
            get { return _spectTypeId; }
        }
        protected string _spectTypeId;

        #endregion

        #region Events

        public delegate void UIMessageEventHandler<UIMessageEventArgs>(Object sender, UIMessageEventArgs e);
        public event UIMessageEventHandler<UIMessageEventArgs> RaiseUIMessageEvent;
        protected virtual void OnRaiseUIMessageEvent(UIMessageEventArgs e)
        {
            if (RaiseUIMessageEvent != null)
                RaiseUIMessageEvent(this, e);
        }


        public delegate void MessageEventHandler<MessageEventArgs>(Object sender, MessageEventArgs e);
        public event MessageEventHandler<MessageEventArgs> RaiseMessageEvent;
        protected virtual void OnRaiseMessageEvent(MessageEventArgs e)
        {
            if (RaiseMessageEvent != null)
                RaiseMessageEvent(this, e);
        }


        public event EventHandler StartPreResultsProcessing;
        protected void DoStartPreResultsProcessing()
        {
            if (StartPreResultsProcessing != null)
                StartPreResultsProcessing(this, EventArgs.Empty);
        }


        public event EventHandler ManualBackgroundComplete;
        protected virtual void OnManualBackgroundComplete(EventArgs e)
        {
            if (ManualBackgroundComplete != null)
                ManualBackgroundComplete(this, e);
        }


        public delegate void FaultMessageEventHandler<FaultMessageEventArgs>(Object sender, FaultMessageEventArgs e);
        public event FaultMessageEventHandler<FaultMessageEventArgs> RaiseFaultMessageEvent;
        protected virtual void OnRaiseFaultMessageEvent(FaultMessageEventArgs e)
        {
            if (RaiseFaultMessageEvent != null)
                RaiseFaultMessageEvent(this, e);
        }


        public delegate void CancelCleanCheckWorkflowEventHandler<CancelCleanCheckWorkflowEventArgs>(Object sender, CancelCleanCheckWorkflowEventArgs e);
        public event CancelCleanCheckWorkflowEventHandler<CancelCleanCheckWorkflowEventArgs> RaiseCancelCleanCheckWorkflowEvent;
        protected virtual void OnRaiseCancelCleanCheckWorkflowEvent(CancelCleanCheckWorkflowEventArgs e)
        {
            if (RaiseCancelCleanCheckWorkflowEvent != null)
                RaiseCancelCleanCheckWorkflowEvent(this, e);
        }


        public delegate void CancelWorkflowEventHandler<CancelWorkflowEventArgs>(Object sender, CancelWorkflowEventArgs e);
        public event CancelWorkflowEventHandler<CancelWorkflowEventArgs> RaiseCancelWorkflowEvent;
        protected virtual void OnRaiseCancelWorkflowEvent(CancelWorkflowEventArgs e)
        {
            if (RaiseCancelWorkflowEvent != null)
                RaiseCancelWorkflowEvent(this, e);
        }

        #endregion

        #region Public Methods

        public WorkflowParser()
        {
            LoadWorkflowFile();
        }

        private void LoadWorkflowFile()
        {
            _filePath = Helper.FOLDER_PATH_FILES; 
            // Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\InstrumentUI_ATK\\Files\\";
            _fileName = Helper.CurrentSpectrometer.WFFileName;
            Trace.WriteLine(_filePath + _fileName);
            _fileStream = new FileStream(_filePath + _fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            _fileReader = new XmlTextReader(_fileStream);

            // Retrieve the SpectTypeId so we know which type of parser to use
            _fileReader.Read();

            if (_fileReader.IsStartElement("spectrometertype"))
            {
                if (_fileReader.HasAttributes)
                {
                    _spectTypeId = _fileReader.GetAttribute("specttypeid");
                    Helper.SpectromterType = _fileReader.GetAttribute("specttypeid"); 
                }
            }
        }

        public virtual void ExecuteWorkflow() { }

        public virtual void ExecuteWorkflow(InstrumentPhase instrumentPhase) { }

        public virtual void ExecuteWorkflow(InstrumentPhase instrumentPhase, string fileName) { }

        public void CancelWorkflow(string code, string message)
        {
            _isWorkflowCancelled = true;

            if (Helper.MultiScanRunCount > 0)
                Helper.MultiScanRunCount = 0;

            OnRaiseMessageEvent(new MessageEventArgs(message));
        }

        public void CancelWorkflow()
        {
            _isWorkflowCancelled = true;

            if (Helper.SpeedMode)
            {
                if (Helper.MultiScanRunCount > 0)
                    Helper.MultiScanRunCount = 0;
            }
        }

        #endregion
    }
}