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

namespace InstrumentUI_ATK.ModalForm
{
    public partial class Phone : Form
    {
        public Phone()
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
        private void Phone_Load(object sender, EventArgs e)
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
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        private void LocalizeResource()
        {
            btnOK.Text = ResourceHelper.Ok;
            lblCall.Text = ResourceHelper.Please_Call + Helper.COLON;
        }
    }
}
