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
using System.Configuration;

namespace InstrumentUI_ATK.RibbonTabs
{
    public partial class RibbonTabAdmin : UserControl, IRibbonTab
    {
        public event EventHandler OnPreferencesClick;
        public event EventHandler OnSupportClick;
        public event EventHandler OnSetupClick;
        public event EventHandler OnBodyClick;

        public RibbonIcon DefaultButton 
        { 
            get { return iconPreferences; } 
        }

        public RibbonTabAdmin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            iconPreferences.Text = ResourceHelper.Preferences;
            iconSupport.Text = ResourceHelper.Support;
            iconSetup.Text = ResourceHelper.SetUp;
        }

        private void iconPreferences_Click(object sender, EventArgs e)
        {
            DeSelectButtons();
            if (ConfigurationManager.AppSettings["AllowPreferences"].ToLower() == "true")
            {
                iconPreferences.Selected = true;
                if (null != this.OnPreferencesClick)
                    OnPreferencesClick(this, new EventArgs());
            }
        }

        public void DeSelectButtons()
        {
            iconPreferences.Selected = false;
            iconSupport.Selected = false;
            iconSetup.Selected = false;
        }

        private void iconSupport_Click(object sender, EventArgs e)
        {
            DeSelectButtons();
            iconSupport.Selected = true;

            if (null != this.OnSupportClick)
                OnSupportClick(this, new EventArgs());
        }

        private void iconSetup_Click(object sender, EventArgs e)
        {
            DeSelectButtons();
            iconSetup.Selected = true;

            if (null != this.OnSetupClick)
                OnSetupClick(this, new EventArgs());
        }

        private void flpBody_Click(object sender, EventArgs e)
        {
            if (this.OnBodyClick != null)
                OnBodyClick(this, new EventArgs());
        }
    }
}
