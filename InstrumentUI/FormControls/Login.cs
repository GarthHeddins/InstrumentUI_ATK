using System;
using System.Configuration;
using System.Windows.Forms;
using InstrumentUI_ATK.DataService;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.ModalForm;

namespace InstrumentUI_ATK.FormControls
{
    public partial class Login : UserControl, IFormControl
    {
        private const string USERNAME = "Username";
        public event EventHandler OnUserAuthenticated;

        public EnumHelpCode HelpCode { get { return EnumHelpCode.INSTRUMENT_LOGIN; } }
        
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reset all values of controls
        /// </summary>
        public void ResetValues()
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;

            chkRememberMe.Checked = false;
        }

        /// <summary>
        /// Get the Username from the config file(if any) and set the active button of the form 
        /// </summary>
        public void InitValues()
        {
            string username = ConfigurationManager.AppSettings[USERNAME];

            if (!username.IsNullOrWhiteSpace())
            {
                txtUsername.Text = username;
                chkRememberMe.Checked = true;
                this.ActiveControl = txtPassword;
                this.txtPassword.Focus();
                //this.txtPassword.Text = "password";
            }
            else
            {
                this.ActiveControl = txtUsername;
                this.txtUsername.Focus();
            }
            if (null != this.ParentForm)
                this.ParentForm.AcceptButton = btnLogin;

        }

        /// <summary>
        /// Form Load event. Get the Username from the config file(if any) 
        /// and replace all text on the form from the resource file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginControl_Load(object sender, EventArgs e)
        {
            LocalizeResource();
            InitValues();
        }

        /// <summary>
        /// Login button click event. Validate the user credentials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (!txtUsername.Text.IsNullOrWhiteSpace())
            {
                string inputUsername = txtUsername.Text.Trim();

                try
                {
                    Configuration appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    appSettings.AppSettings.Settings.Remove(USERNAME);
                    if (chkRememberMe.Checked)
                        appSettings.AppSettings.Settings.Add(USERNAME, inputUsername);
                    appSettings.Save();
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch (ConfigurationErrorsException cee)
                {
                    Helper.LogError("Login.btnLogin_Click", string.Empty, cee, true);
                }

                if (!txtPassword.Text.IsNullOrWhiteSpace())
                {
                    DataService.InstrumentServiceClient serviceClient = null;
                    bool isServiceClosed = false;
                    User currentUser = null;

                    try
                    {
                        serviceClient = Helper.GetServiceInstance();

                        currentUser = serviceClient.AuthenticateUser(txtUsername.Text.Trim(), txtPassword.Text.Trim());

                        serviceClient.Close();
                        isServiceClosed = true;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException)
                    {
                        Helper.DisplayError(ResourceHelper.Error_10100);
                        return;
                    }
                    catch (System.ServiceModel.FaultException ex)
                    {
                        if (ex.Code.Equals("OE_12518") || ex.Code.Equals("OE_12514") || ex.Code.Equals("OE_01017")
                            || ex.Code.Equals("OE_12541") || ex.Code.Equals("OE_12170"))
                            Helper.DisplayError(ResourceHelper.Error_10100);
                        else
                            Helper.DisplayError(ResourceHelper.Error);
                        return;
                    }
                    finally
                    {
                        if (!isServiceClosed)
                            serviceClient.Abort();

                        if (Helper.ContextScope != null)
                            Helper.ContextScope.Dispose();
                    }
                    
                    if (currentUser.UserRole == null)
                        Helper.DisplayError(ResourceHelper.Error_10101);
                    else if (!currentUser.Status.Equals(EnumStatus.ACTIVE))
                        Helper.DisplayError(ResourceHelper.Error_10101);
                    else if (!currentUser.UserRole.RoleName.Equals(Helper.ROLE_INSTRUMENT_USER, StringComparison.InvariantCultureIgnoreCase))
                        Helper.DisplayError(ResourceHelper.Error_10101);
                    else
                    {
                        Helper.CurrentUser = currentUser;

                        if (null != OnUserAuthenticated)
                            OnUserAuthenticated(this, new EventArgs());
                    }
                }
                else
                    Helper.DisplayError(ResourceHelper.Error_Invalid_Password);
            }
            else
                Helper.DisplayError(ResourceHelper.Error_Invalid_User_Name);
        }
        
        /// <summary>
        /// This function replaces all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            lblUsername.Text = ResourceHelper.User_Name;
            lblPassword.Text = ResourceHelper.Password;
            chkRememberMe.Text = ResourceHelper.Remember_Me;            
            btnLogin.Text = ResourceHelper.Login;
        }
    }
}
