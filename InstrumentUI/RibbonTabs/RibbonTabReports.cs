using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.Controls;
using System.Threading;
using System.Diagnostics;
using InstrumentUI_ATK.DataService;
using InstrumentUI_ATK.FormControls;

namespace InstrumentUI_ATK.RibbonTabs
{
    public partial class RibbonTabReports : UserControl, IRibbonTab
    {
        //public event EventHandler OnCofAClick;
        //public event EventHandler OnSingleAnalysisClick;
        public event EventHandler OnWebReportClick;

        public RibbonIcon DefaultButton
        {
            get { return null; }
        }

        public RibbonTabReports()
        {
            InitializeComponent();

            // only admin can user R&D so this icon will remain invisible
            iconRnD.Visible = false;
        }

        public void DeSelectButtons()
        {
            iconCofA.Selected = false;
            iconSingleAnalysis.Selected = false;
            iconHistorical.Selected = false;
            iconTestCount.Selected = false;
            iconControlChart.Selected = false;
            iconTrendGraph.Selected = false;
            iconRnD.Selected = false;
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            iconCofA.Text = ResourceHelper.C_Of_A;
            iconSingleAnalysis.Text = ResourceHelper.Single_Analysis;
            iconHistorical.Text = ResourceHelper.Historical;
            iconTestCount.Text = ResourceHelper.Test_Count;
            iconControlChart.Text = ResourceHelper.Control_Chart;
            iconRnD.Text = ResourceHelper.R_n_D;
            iconTrendGraph.Text = ResourceHelper.Trend_Graph;
        }

        private void iconCofA_Click(object sender, EventArgs e)
        {
            GoToReportURL(EnumReportType.COFA, null);
            iconCofA.Selected = true;
        }

        private void iconSingleAnalysis_Click(object sender, EventArgs e)
        {
            GoToReportURL(EnumReportType.SA, null);
            iconSingleAnalysis.Selected = true;
        }

        /// <summary>
        /// Open Web report in the browser 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconTrendGraph_Click(object sender, EventArgs e)
        {
            TrendGraphForm rtChart = new TrendGraphForm();
            rtChart.ShowDialog();
            //rtChart.Dispose();            
            //GoToReportURL(EnumReportType.TRENDCHART, null);
            iconTrendGraph.Selected = true;
        }

        private void iconTestCount_Click(object sender, EventArgs e)
        {
            GoToReportURL(EnumReportType.TESTCOUNT, null);
            iconTestCount.Selected = true;
        }

        private void iconHistorical_Click(object sender, EventArgs e)
        {
            GoToReportURL(EnumReportType.HISTORY, null);
            iconHistorical.Selected = true;
        }

        private void iconRnD_Click(object sender, EventArgs e)
        {
            GoToReportURL(EnumReportType.RD, null);
            iconRnD.Selected = true;
        }

        private void iconControlChart_Click(object sender, EventArgs e)
        {
            GoToReportURL(EnumReportType.CONTROLCHART, null);
            iconControlChart.Selected = true;
        }

        private void GoToReportURL(EnumReportType reportType, byte[] spectralFile)
        {
            DeSelectButtons();

            InstrumentServiceClient serviceClient = null;
            bool isServiceClosed = false;

            try
            {
                serviceClient = Helper.GetServiceInstance();

                string token = Guid.NewGuid().ToString();
                
                // store token in the database
                int result = serviceClient.CreateAuthenticateToken(token, Helper.CurrentUser.UserName, Thread.CurrentThread.CurrentUICulture.Name, reportType, spectralFile, null);
                
                serviceClient.Close();
                isServiceClosed = true;

                // get the web report URL from the App Config file
                string reportUrl = System.Configuration.ConfigurationManager.AppSettings["ReportURL"] + "Report/Index?token=" + token;

                //Process.Start("IExplore.exe", fullUrl);
                System.Diagnostics.Process.Start(reportUrl);
            }
            catch (Exception ex)
            {
                Helper.LogError("RibbonTabReports.GoToReportURL", reportType + reportType.ToString(), ex, true);
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Abort();

                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }

            if (this.OnWebReportClick != null)
            {
                OnWebReportClick(this, new EventArgs());
            }
        }
    }
}
