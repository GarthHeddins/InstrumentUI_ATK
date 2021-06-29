using System;
using System.Configuration;
using System.Windows.Forms;
using InstrumentUI_ATK.DataService;
using InstrumentUI_ATK.Common;

namespace InstrumentUI_ATK.FormControls
{
    public partial class ChangePassword : UserControl, IFormControl
    {
        public event EventHandler OnChangePasswordCancelClicked;

        public EnumHelpCode HelpCode { get { return EnumHelpCode.INSTRUMENT_CHANGE_PASSWORD; } }

        public ChangePassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form Load event. Get the Username from the config file(if any) 
        /// and replace all text on the form from the resource file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginControl_Load(object sender, EventArgs e)
        {
            try
            {
                LocalizeResource();
            }
            catch (Exception ex)
            {
                Helper.LogError("LoginControl_Load", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        /// <summary>
        /// Login button click event. Validate the user credentials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtPassword.Text.IsNullOrWhiteSpace())
                {
                    string oldPassword = txtPassword.Text.Trim();

                    if (!txtNewPassword.Text.IsNullOrWhiteSpace())
                    {
                        string newPassword = txtNewPassword.Text.Trim();

                        if (txtConfirmPassword.Text.Equals(txtNewPassword.Text))
                        {
                            DataService.InstrumentServiceClient serviceClient = null;
                            bool isServiceClosed = false;
                            int result = 0;

                            try
                            {
                                serviceClient = Helper.GetServiceInstance();

                                result = serviceClient.ChangePassword(Helper.CurrentUser.Id, txtPassword.Text.Trim(), txtNewPassword.Text.Trim());

                                serviceClient.Close();
                                isServiceClosed = true;
                            }
                            catch (System.ServiceModel.EndpointNotFoundException ex)
                            {
                                Helper.DisplayError(ResourceHelper.Error_10100);
                                Helper.LogError("btnOk_Click", string.Format("sender = {0}, e = {1}", sender, e), ex, false);
                                return;
                            }
                            catch (System.ServiceModel.FaultException ex)
                            {
                                if (ex.Code.Equals("OE_12518") || ex.Code.Equals("OE_12514") || ex.Code.Equals("OE_01017")
                                    || ex.Code.Equals("OE_12541") || ex.Code.Equals("OE_12170"))
                                    Helper.DisplayError(ResourceHelper.Error_10100);
                                else
                                    Helper.DisplayError(ex.Message);
                                    return;
                            }
                            finally
                            {
                                if (!isServiceClosed)
                                    serviceClient.Abort();

                                if (Helper.ContextScope != null)
                                    Helper.ContextScope.Dispose();
                            }

                            if (result <= 0)
                                Helper.DisplayError(ResourceHelper.Password_Change_Failed);
                            else
                            {
                                Helper.DisplayError(ResourceHelper.Password_Changed)  ;
                                if (null != OnChangePasswordCancelClicked)
                                    OnChangePasswordCancelClicked(this, new EventArgs());
                            }
                        }
                        else
                            Helper.DisplayError(ResourceHelper.Error_Invalid_Confirm_Password);
                    }
                    else
                        Helper.DisplayError(ResourceHelper.Error_Invalid_New_Password);
                }
                else
                    Helper.DisplayError(ResourceHelper.Error_Invalid_Password);
            }
            catch (Exception ex)
            {
                Helper.LogError("btnOk_Click", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }

        /// <summary>
        /// This function replaces all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            lblPassword.Text = ResourceHelper.Password + Helper.COLON;
            lblNewPassword.Text = ResourceHelper.New_Password + Helper.COLON;
            lblConfirmPassword.Text = ResourceHelper.Confirm_Password + Helper.COLON;
            btnOk.Text = ResourceHelper.Ok;
            btnCancel.Text = ResourceHelper.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != OnChangePasswordCancelClicked)
                    OnChangePasswordCancelClicked(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Helper.LogError("btnCancel_Click", string.Format("sender = {0}, e = {1}", sender, e), ex, true);
            }
        }
    }
}
