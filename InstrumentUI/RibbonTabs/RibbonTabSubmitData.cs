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

namespace InstrumentUI_ATK.RibbonTabs
{
    public partial class RibbonTabSubmitData : UserControl, IRibbonTab
    {
        public RibbonIcon DefaultButton
        {
            get { return iconValidation; }
        }

        public RibbonTabSubmitData()
        {
            InitializeComponent();
        }

        public void DeSelectButtons()
        { }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            iconValidation.Text = ResourceHelper.Validation;
            iconFeasibility.Text = ResourceHelper.Feasibility;
            iconRoundRobin.Text = ResourceHelper.RoundRobin;
        }
    }
}
