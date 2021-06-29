using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.Controls;

namespace InstrumentUI_ATK.RibbonTabs
{
    public partial class RibbonTabAnalyze : UserControl, IRibbonTab
    {
        public event EventHandler OnRoutineClick;
        public event EventHandler OnCheckClick;
        public event EventHandler OnScheduledClick;


        public bool HasSampleClasses { get; private set; }


        public RibbonIcon DefaultButton
        {
            get { return iconRoutine; }
        }


        public RibbonTabAnalyze()
        {
            InitializeComponent();
            BackColor = Color.White;
        }


        public void DisplayTabIcon(List<SampleClass> sampleClasses)
        {
            foreach (var item in sampleClasses)
            {
                if (item.Name == Helper.SAMPLE_CLASS_ROUTINE)
                    iconRoutine.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_IDENTIFY)
                    iconIdentify.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_SCHEDULED)
                    iconScheduled.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_AUTOSAMPLER)
                    iconAutoSampler.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_CHECK)
                    iconCheck.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_VALIDATION)
                    iconValidation.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_FEASIBILITY)
                    iconFeasibility.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_TEST)
                    iconTest.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_ROUNDROBIN)
                    iconRoundRobin.Visible = true;
                else if (item.Name == Helper.SAMPLE_CLASS_DEMO)
                    iconDemo.Visible = true;

                if (!HasSampleClasses)
                    HasSampleClasses = true;
            }

            // Determine the default active image
            if (sampleClasses.Any(c => c.Name == Helper.SAMPLE_CLASS_SCHEDULED))
            {
                //iconScheduled.Image = iconScheduled.ActiveImage;
                iconScheduled.Selected = true;
                //iconScheduled.Select();
            }
            else
            {
                //iconRoutine.Image = iconRoutine.ActiveImage;
                iconRoutine.Selected = true;
                //iconRoutine.Select();
            }
        }


        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            iconRoutine.Text = ResourceHelper.Routine;
            iconIdentify.Text = ResourceHelper.Identify;
            iconScheduled.Text = ResourceHelper.Scheduled;
            iconAutoSampler.Text = ResourceHelper.Auto_Sampler;
            iconCheck.Text = ResourceHelper.Check;
            iconValidation.Text = ResourceHelper.Validation;
            iconFeasibility.Text = ResourceHelper.Feasibility;
            iconTest.Text = ResourceHelper.Test;
            iconRoundRobin.Text = ResourceHelper.RoundRobin;
            iconDemo.Text = ResourceHelper.Demo;
        }


        /// <summary>
        /// Handle the Routine button's OnClick event
        /// </summary>
        private void iconRoutine_Click(object sender, EventArgs e)
        {
            DeSelectButtons();
            iconRoutine.Selected = true;

            if (OnRoutineClick != null)
                OnRoutineClick(this, new EventArgs());
        }


        /// <summary>
        /// Handle the Check button's OnClick event
        /// </summary>
        private void iconCheck_Click(object sender, EventArgs e)
        {
            DeSelectButtons();
            iconCheck.Selected = true;

            if (OnCheckClick != null)
                OnCheckClick(this, new EventArgs());
        }


        /// <summary>
        /// Handle the Scheduled button's OnClick event
        /// </summary>
        private void iconScheduled_Click(object sender, EventArgs e)
        {
            DeSelectButtons();
            iconScheduled.Selected = true;

            if (OnScheduledClick != null)
                OnScheduledClick(this, new EventArgs());
        }



        public void DeSelectButtons()
        {
            iconRoutine.Selected = false;
            iconIdentify.Selected = false;
            iconScheduled.Selected = false;
            iconAutoSampler.Selected = false;
            iconCheck.Selected = false;
            iconValidation.Selected = false;
            iconFeasibility.Selected = false;
            iconTest.Selected = false;
            iconRoundRobin.Selected = false;
            iconDemo.Selected = false;
        }
    }
}
