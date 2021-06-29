using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.Common;
using InstrumentUI_ATK.Controls;

namespace InstrumentUI_ATK.FormControls
{
    public partial class Check : Analyze
    {
        //Generic event handler declaration
        public event EventHandler OnAnalyzeFormValidationCompleted;
        private string _SampleIdentifierName;

        private Check()
        {
            InitializeComponent();

            CheckBoxCheckSample.Visible = true;
            CheckBoxCheckSample.Checked = true;
            CheckBoxCheckSample.Enabled = false;

            ButtonInsertPrevious.Visible = false;
            ButtonClearAll.Visible = false;
        }

        public Check(List<Trait> allTraits) : this()
        {
            InitAnalyze(allTraits);
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public override void LocalizeResource()
        {
            base.LocalizeResource();

            CheckBoxCheckSample.Text = ResourceHelper.This_Is_Check_Sample;
        }

        /// <summary>
        /// Reset all values of Sample identifier section
        /// </summary>
        public override void ResetSampleIdentifiers()
        {
            if (this.MaterialId > 0)
            {
                var sampleIdentifiers = Helper.GetSampleIdentifiers(this.MaterialId);

                if (sampleIdentifiers != null && sampleIdentifiers.Count() > 0)
                {
                    foreach (var identifyControl in FlowLayoutPanelIdentifier.Controls.OfType<IdentifySample>())
                    {
                        var sampleIdentifier = sampleIdentifiers.FirstOrDefault(
                                                s => s.Name.Trim().ToLower() == identifyControl.DisplayText.Trim().ToLower());

                        if (sampleIdentifier != null)
                            identifyControl.Value = sampleIdentifier.Value;
                        else
                            identifyControl.Value = string.Empty;
                    }
                }
                //foreach (var identifyControl in FlowLayoutPanelIdentifier.Controls.OfType<IdentifySample>())
                //{
                //    identifyControl.Value = string.Empty;
                //}
            }
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
                currentSample.SampleTypeName = Helper.SAMPLE_CLASS_CHECK;

            // set the current sample
            Helper.CurrentSample = currentSample;

            //SD: Raising an event to start Workflow from Main
            if (OnAnalyzeFormValidationCompleted != null)
                OnAnalyzeFormValidationCompleted(this, new EventArgs());
        }

        /// <summary>
        /// Load Identify Sample section based on the specified material.
        /// </summary>
        /// <param name="materialId"></param>
        protected override void LoadIdentifySample(int materialId)
        {
            try
            {
                int sampleId = GetSampleId(materialId);
                

                FlowLayoutPanelIdentifier.Controls.Clear();

                // add an empty panel for some space on the top
                FlowLayoutPanelIdentifier.Controls.Add(PanelIdentifierTop);

                List<string> sampleIds = DAL.GetUniqueCheckSamples();
                sampleIds.Insert(0, string.Empty); // added an empty items

                IdentifySample identifySample = new IdentifySample(sampleId, _SampleIdentifierName, sampleIds.ToArray(), true, false, true);
                identifySample.OnTextValueChanged += new EventHandler(identifySample_OnTextValueChanged);

                FlowLayoutPanelIdentifier.Controls.Add(identifySample);

                AddOperatorField(FlowLayoutPanelIdentifier, materialId);
            }
            catch (Exception ex)
            {
                Helper.LogError("Check.LoadIdentifySample", "materialId" + materialId.ToString(), ex, true);
            }
        }

        private void AddOperatorField(FlowLayoutPanel flowLayoutPanelIdentifier, int materialId)
        {
            var sampleIdentifiers = Helper.GetSampleIdentifiers(materialId).ToList();
            if (sampleIdentifiers.Any(x => x.Description.Contains("Operator")))
            {
                var item = sampleIdentifiers.Where(x => x.Description.Contains("Operator")).First();
                var identifySample = new IdentifySample(item.Id, item.Name, item.Value, item.Required, item.Numeric);
                FlowLayoutPanelIdentifier.Controls.Add(identifySample);
                identifySample.OnTextValueChanged += new EventHandler(identifySample_OnTextValueChanged);
            }
        }

        void identifySample_OnTextValueChanged(object sender, EventArgs e)
        {
            ValidateInputs(false);
        }

        /// <summary>
        /// Find the Sample Identifier with name "Sample ID" and returns its Id, 
        /// if it doesn't exist then return 0
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        private int GetSampleId(int materialId)
        {
            var sampleIdentifiers = Helper.GetSampleIdentifiers(materialId);

            foreach (var sampleIdentifier in sampleIdentifiers)
            {
                //if (sampleIdentifier.Name.ToLower() == "sample id" || sampleIdentifier.Name.ToLower() == "samp id")
                if (sampleIdentifier.DisplayOrder == 1)
                {
                    _SampleIdentifierName = sampleIdentifier.Name;
                    return sampleIdentifier.Id;
                }
            }

            return 0;
        }
    }
}
