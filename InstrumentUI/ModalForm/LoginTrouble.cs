using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace InstrumentUI_ATK.ModalForm
{
    public partial class LoginTrouble : Form
    {
        public LoginTrouble()
        {
            InitializeComponent();

            // make the corners rounded
            this.Region = System.Drawing.Region.FromHrgn(NativeMethod.CreateRoundRectRgn(-2, -2, Width + 2, Height + 2, 50, 40)); 
        }

        /// <summary>
        /// Load event of form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginTrouble_Load(object sender, EventArgs e)
        {
            LocalizeResource();
        }

        /// <summary>
        /// Close the Message dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Chat button click. It opens a url specified for chat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChat_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Helper.SupportPath);
            }
            catch (Exception ex)
            {
                Helper.LogError("btnChat_Click", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        private void LocalizeResource()
        {
            lblMessage1.Text = ResourceHelper.Login_Trouble_Message_1.Replace("[0]", ConfigurationManager.AppSettings[Helper.CONTACT_NUMBER]);
            lblMessage2.Text = ResourceHelper.Login_Trouble_Message_2;

            btnOK.Text = ResourceHelper.Ok;
            btnChat.Text = ResourceHelper.Chat;
        }
    }
}
