using System;

namespace InstrumentUI_ATK.Workflow
{
    /// <summary>
    /// Event Arguments for a Message
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        public string StepMessage { get; set; }


        /// <summary>
        /// Create a new instance of the MessageEventArgs class
        /// </summary>
        /// <param name="stepMsg"></param>
        public MessageEventArgs(string stepMsg)
        {
            StepMessage = stepMsg;
        }
    }


    /// <summary>
    /// Event Arguments for a UI Message
    /// </summary>
    public class UIMessageEventArgs : EventArgs
    {
        public string StepUIMessage { get; set; }


        /// <summary>
        /// Create a new instance of the UIMessageEventArgs class
        /// </summary>
        /// <param name="stepUIMessage"></param>
        public UIMessageEventArgs(string stepUIMessage)
        {
            StepUIMessage = stepUIMessage;
        }
    }


    /// <summary>
    /// Event Arguments for a Fault Message
    /// </summary>
    public class FaultMessageEventArgs : EventArgs
    {
        public string StepFaultMessage { get; set; }


        /// <summary>
        /// Create a new instance of the FaultMessageEventArgs class
        /// </summary>
        /// <param name="stepFaultMsg">Step Fault Message</param>
        public FaultMessageEventArgs(string stepFaultMessage)
        {
            StepFaultMessage = stepFaultMessage;
        }
    }


    /// <summary>
    /// Event Arguments for Cancelling a Workflow
    /// </summary>
    public class CancelWorkflowEventArgs : EventArgs
    {
        public string ToAppend { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }


        /// <summary>
        /// Create a new instance of the CancelWorkflowEventArgs class
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="message">Message</param>
        public CancelWorkflowEventArgs(string code, string message)
        {
            Code = code;
            Message = message;
        }


        /// <summary>
        /// Create a new instance of the CancelWorkflowEventArgs class
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="message">Message</param>
        /// <param name="toAppend">Text To Append</param>
        public CancelWorkflowEventArgs(string code, string message, string toAppend)
        {
            Code = code;
            Message = message;
            ToAppend = toAppend;
        }
    }


    /// <summary>
    /// Event Arguments for Cancelling a Clean Check Workflow 
    /// </summary>
    public class CancelCleanCheckWorkflowEventArgs : EventArgs
    {
        public bool DisplayMessage { get; set; }
        public string ToAppend { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        
        /// <summary>
        /// Create a new instance of the CancelCleanCheckWorkflowEventArgs class
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="message">Message</param>
        /// <param name="displayMessage">Display Message</param>
        public CancelCleanCheckWorkflowEventArgs(string code, string message, bool displayMessage)
        {
            Code = code;
            Message = message;
            DisplayMessage = displayMessage;
        }

        
        /// <summary>
        /// Create a new instance of the CancelCleanCheckWorkflowEventArgs class
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="message">Message</param>
        /// <param name="toAppend">Text To Append</param>
        public CancelCleanCheckWorkflowEventArgs(string code, string message, string toAppend)
        {
            Code = code;
            Message = message;
            ToAppend = toAppend;
            DisplayMessage = true;
        }
    }
}
