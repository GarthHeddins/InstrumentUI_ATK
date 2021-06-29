using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.Controls;
using System.Configuration;
using System.Runtime.InteropServices;

namespace InstrumentUI_ATK.ModalForm
{
    public partial class Alerts : Form
    {
        private bool _hasAlerts = false;

        /// <summary>
        /// returns true if there are any alerts otherwise false
        /// </summary>
        public bool HasAlerts
        {
            get { return _hasAlerts; }
        }

        public Alerts()
        {
            InitializeComponent();

            // make the corners rounded
            this.Region = System.Drawing.Region.FromHrgn(NativeMethod.CreateRoundRectRgn(-2, -2, Width + 2, Height + 2, 50, 40)); 

            LoadAlerts();
        }

        private void Alerts_Load(object sender, EventArgs e)
        {
            LocalizeResource();
        }

        /// <summary>
        /// Get all alerts from the service
        /// </summary>
        /// <param name="isPopupOnly"></param>
        private void LoadAlerts()
        {
            DataService.InstrumentServiceClient serviceClient = null;
            bool isServiceClosed = false;

            try
            {
                List<DataService.Alert> alerts = null;

                serviceClient = Helper.GetServiceInstance();

                // Get all alerts from the service
                alerts = serviceClient.GetAlerts(Helper.CurrentUser.Id);

                AlertMessage alertMessage;
                foreach (var alert in alerts)
                {
                    _hasAlerts = true;

                    // create one alert control for each alert and then add it on the panel
                    alertMessage = new AlertMessage(alert.CreationDate, alert.Title, alert.MessageText);

                    flpAlert.Controls.Add(alertMessage);
                }

                serviceClient.Close();
                isServiceClosed = true;
            }
            catch (Exception ex)
            {
                Helper.LogError("Alerts.LoadAlerts", string.Empty, ex, false);
                Helper.DisplayError(ResourceHelper.Error_10004); // error while retrieving alerts
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Abort();

                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        private void LocalizeResource()
        {
            lblHeader.Text = ResourceHelper.Your_QTA_Alerts;
            btnClose.Text = ResourceHelper.Close;
            lblBottom.Text = ResourceHelper.Additional_Assistance_Message + ConfigurationManager.AppSettings[Helper.CONTACT_NUMBER];
        }

        /// <summary>
        /// Close the Alert dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
