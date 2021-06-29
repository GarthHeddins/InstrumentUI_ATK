using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.DataAccess;

namespace InstrumentUI_ATK.FormControls
{
    public partial class Routine : Analyze
    {
        //SD: Generic event handler declaration
        public event EventHandler OnAnalyzeFormValidationCompleted;

        private Routine()
        {
            InitializeComponent();

            CheckBoxCheckSample.Visible = false;

            ButtonInsertPrevious.Visible = true;
            ButtonClearAll.Visible = true;
        }

        public Routine(List<Trait> allTraits)
            : this()
        {
            InitAnalyze(allTraits);
        }

        /// <summary>
        /// Sets the Current Sample and raise completed event
        /// </summary>
        /// <param name="currentSample"></param>
        protected override void AfterAnalyzeClicked(Sample currentSample)
        {
            if (Helper.CurrentUser.IsDemo())
                currentSample.SampleTypeName = Helper.SAMPLE_CLASS_DEMO;
            else
                currentSample.SampleTypeName = Helper.SAMPLE_CLASS_ROUTINE;

            // set the current sample
            Helper.CurrentSample = currentSample;

            //SD: Raising an event to start Workflow from Main
            if (OnAnalyzeFormValidationCompleted != null)
                OnAnalyzeFormValidationCompleted(this, new EventArgs());
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public override void LocalizeResource()
        {
            base.LocalizeResource();

            ButtonInsertPrevious.Text = ResourceHelper.Insert_Previous;
            ButtonClearAll.Text = ResourceHelper.ClearAll;
        }
    }
}
