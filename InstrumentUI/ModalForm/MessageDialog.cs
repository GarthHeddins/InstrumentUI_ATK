using System;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using System.Configuration;
using InstrumentUI_ATK.Properties;
using System.Diagnostics;

namespace InstrumentUI_ATK.ModalForm
{
    /// <summary>
    /// Dialog for displaying a Message
    /// </summary>
    public partial class MessageDialog : Form
    {
        private Action _closeAction;
        private Action _cancelAction;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private MessageDialog()
        {
            InitializeComponent();
            if (Helper.CleanCheckFail && (Helper.CleanCheckFailCount > Helper.CleanCheckLimit))
            {
                newbkgBtn.Enabled = true;
                newbkgBtn.Visible = true;
            }
            else
            {
                newbkgBtn.Enabled = false;
                newbkgBtn.Visible = false;
            }

            // make the corners rounded
            //Region = Region.FromHrgn(NativeMethod.CreateRoundRectRgn(-2, -2, Width + 2, Height + 2, 50, 40));
            //Region = new Region(RoundedRectangle.Create(-2, -2, Width+2, Height+2, 25));
        }


        /// <summary>
        /// Create Message dialog from the specified title and message text
        /// </summary>
        /// <param name="title">Message dialog title</param>
        /// <param name="messageText">Message</param>
        public MessageDialog(string title, string messageText) : this()
        {
            lblTitle.Text = title;
            lblMessageText.Text = messageText;
        }


        /// <summary>
        /// Create Message dialog from the specified title and message text with option to hide the icon image
        /// </summary>
        /// <param name="title"></param>
        /// <param name="messageText"></param>
        /// <param name="isDisplayIcon"></param>
        public MessageDialog(string title, string messageText, bool isDisplayIcon)
            : this()
        {
            lblTitle.Text = title;
            lblMessageText.Text = messageText;
            picIcon.Visible = isDisplayIcon;
        }

        public MessageDialog(string title, string messageText, bool isDisplayIcon, bool Cancelbtn, Action cancelAction)
            : this()
        {
            lblTitle.Text = title;
            lblMessageText.Text = messageText;
            var samps = Helper.CurrentSample.SampleIdentifiers.Count;
            picIcon.Visible = isDisplayIcon;
            btnCancel.Visible = true;
            btnContinue.Location = new System.Drawing.Point(163, 286);
            btnCancel.Location = new System.Drawing.Point(308, 286);
            //btnContinue.Location = new System.Drawing.Point(238, 286); //Default/original value
            _cancelAction = cancelAction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="messageText"></param>
        /// <param name="isDisplayIcon"></param>
        /// <param name="closeAction"></param>
        public MessageDialog(string title, string messageText, bool isDisplayIcon, Action closeAction) : this()
        {
            lblTitle.Text = title;
            lblMessageText.Text = messageText;
            picIcon.Visible = isDisplayIcon;
            _closeAction = closeAction;
        }


        /// <summary>
        /// Allows overriding the Form's Background Image with an alternate.
        /// Used to handle issues with rounded corners and different underlying form colors.
        /// </summary>
        public void UseAlternateBackground()
        {
            BackgroundImage = Resources.alerts_background_on_darkblue;
        }


        /// <summary>
        /// Load event of form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageDialog_Load(object sender, EventArgs e)
        {
            LocalizeResource();
        }


        /// <summary>
        /// Close the Message dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinue_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now; 
            Trace.WriteLine("MessageDialog.btnContinue_Click(). " + startTime.Day + " " + startTime.Hour + ":" + startTime.Minute + ":" + startTime.Second + ":" + startTime.Millisecond);
            //Helper.MPAVBPUIDialog = true;
            Close();
            if (_closeAction != null) 
            {
                Trace.WriteLine("MessageDialog.cs - btnContinue_Click() - _closeAction.Invoke()");
                _closeAction.Invoke();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Notify Workflow that the process has been cancelled
            //isCancelled = true;
            try
            {
                Helper.Parser.CancelWorkflow();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error caught in Helper.DisplayMessage() error: " + ex);
            }

            Close();
            if (_cancelAction != null)
                _cancelAction.Invoke();
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        private void LocalizeResource()
        {
            lblBottom.Text = ResourceHelper.Additional_Assistance_Message + ConfigurationManager.AppSettings[Helper.CONTACT_NUMBER];
            btnContinue.Text = ResourceHelper.Continue;
        }

        private void newbkgBtn_Click(object sender, EventArgs e)
        {
            Helper.NewCleanBkg = true;
            btnContinue_Click(sender, e);
        }


    }
}