using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.Linq;
using System.Data.SqlServerCe;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using com.quinncurtis.chart2dnet;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.Common;
using System.Diagnostics;

namespace InstrumentUI_ATK.FormControls
{
    public partial class TrendGraphForm : Form
    {
        
        public TrendGraphForm()
        {
            InitializeComponent();
        }

        private void TrendGraphForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'instrumentUIDBDataSet.ResultHeader' table. You can move, or remove it, as needed.
            var rHeader = DAL.GetDistinctMaterials();
            this.materialsCombo.DataSource = rHeader;
            Helper.szMaterial = rHeader[0];
            DAL.GetAllGraphHeader(Helper.szMaterial);
            var rTraits = DAL.GetDistinctTraits();
            this.traitCombo.DataSource = rTraits;
            Helper.szTrait = rTraits[0];
        }

        private void materialsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.szMaterial = materialsCombo.SelectedItem.ToString();
            DAL.GetAllGraphHeader(Helper.szMaterial);
            var rTraits = DAL.GetDistinctTraits();
            this.traitCombo.DataSource = rTraits;
        }

        private void traitCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.szTrait = traitCombo.SelectedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //List<ResultHeader> resultHeader = DAL.GetAllResultAsending();
            List<double> dValue = new List<double>();
            List<ChartCalendar> dTime = new List<ChartCalendar>();

            int numResults = 0;
            String display = null;            

            foreach (var rID in DAL.requestIDs)
            {
                var rHeader = DAL.GetResultHeader(rID);
                var rDetails = DAL.GetResultDetails(rID);
                foreach (var details in rDetails)
                {
                    bool comp = details.DisplayText.Contains("Outlier");
                    if ((details.ModelGroupName == Helper.szTrait) && (comp == false))
                    {
                        display = details.DisplayText.Trim(new Char[] { '<', '>', '*'});
                        try
                        {
                            dValue.Add(Convert.ToDouble(display));
                            dTime.Add(ChartCalendar.Parse(rHeader.LocalTimeStamp.ToString("G")));
                            numResults++;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("Bad Conversion " + ex.ToString());
                        }                        
                    }
                }
            }
            int numPoints = numResults;
            if (numPoints > 0)
            {
                ChartCalendar[] x1 = new ChartCalendar[numPoints];
                double[] y1 = new double[numPoints];
                int i;
                for (i = 0; i < numPoints; i++)
                {
                    x1[i] = dTime[i];
                    y1[i] = dValue[i];

                }

                chartControl1.UpdateControlChart(x1, y1);
            }

        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartControl1.PageSetup(sender, e);
        }

        private void printerSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartControl1.PrinterSetup(sender, e);
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartControl1.PrintPreview(sender, e);
        }

        private void printPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartControl1.PrintPage(sender, e);
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartControl1.SaveAsFile(sender, e);
        }
   }
}
