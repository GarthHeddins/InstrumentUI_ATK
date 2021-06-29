using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.DataService;
using InstrumentUI_ATK.Common;

namespace InstrumentUI_ATK.FormControls
{
    public partial class SingleAnalysis : UserControl, IFormControl
    {
        public SingleAnalysis()
        {
            InitializeComponent();
        }

        #region IFormControl Members

        public EnumHelpCode HelpCode
        {
            get { throw new NotImplementedException(); }
        }

        public void LocalizeResource()
        {
            lblSingleAnalysisText.Text = ResourceHelper.Single_Analysis_Report;
            lblSelectText.Text = ResourceHelper.Select;
            lblSampleIdText.Text = ResourceHelper.Sample_ID + Helper.COLON;
            lblOrText.Text = ResourceHelper.Or;
            lblAnalysisIdText.Text = ResourceHelper.Analysis_ID + Helper.COLON;
            btnSubmit.Text = ResourceHelper.Submit;
        }

        #endregion

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}
