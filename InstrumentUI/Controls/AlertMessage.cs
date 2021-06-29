using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InstrumentUI_ATK.Controls
{
    /// <summary>
    /// This control is used to disply a single Alert message with date, title and body
    /// </summary>
    public partial class AlertMessage : UserControl
    {
        private AlertMessage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create alert control with the specified date, title and description
        /// </summary>
        /// <param name="date"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        public AlertMessage(DateTime date, string title, string body) : this()
        {
            lblDate.Text = date.ToString("d", System.Threading.Thread.CurrentThread.CurrentUICulture);
            lblTitle.Text = title;
            lblBody.Text = body;
        }
    }
}
