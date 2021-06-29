using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;

namespace InstrumentUI_ATK.FormControls
{
    public partial class CofA : UserControl, IFormControl
    {
        public CofA()
        {
            InitializeComponent();
        }

        #region IFormControl Members

        public InstrumentUI_ATK.DataService.EnumHelpCode HelpCode
        {
            get { throw new NotImplementedException(); }
        }

        public void LocalizeResource()
        {
            lblCofAText.Text = ResourceHelper.C_of_A_Report;
            lblSelectText.Text = ResourceHelper.Select;
            lblSampleIdText.Text = ResourceHelper.Sample_ID + Helper.COLON;
            lblOrText.Text = ResourceHelper.Or;
            lblAnalysisIdText.Text = ResourceHelper.Analysis_ID + Helper.COLON;
            btnSubmit.Text = ResourceHelper.Submit;
        }

        #endregion
    }
}
