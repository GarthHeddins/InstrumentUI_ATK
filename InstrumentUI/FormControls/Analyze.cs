using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using InstrumentUI_ATK.Controls;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.DataAccess.Model;
using InstrumentUI_ATK.Common;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.IO;
using DataService = InstrumentUI_ATK.DataService;
using System.Globalization;
using System.Configuration;

namespace InstrumentUI_ATK.FormControls
{
    public partial class Analyze : UserControl, IFormControl
    {
        //private int _addedTraitCount = 0;
        private bool _uncheckAll = true; // flag to decide whether to uncheck all traits or not
        private bool _isAutoFocus = false;

        // trait panel height constants
        private const int MAX_HEIGHT_PANEL_OUTER = 420;
        private const int MAX_HEIGHT_PANEL_INNER = 415;
        private const int MIN_HEIGHT_PANEL_OUTER = 116;
        private const int MIN_HEIGHT_PANEL_INNER = 110;

        ////SD: Generic event handler declaration
        //public event EventHandler OnAnalyzeFormValidationCompleted;

        public void SetAnalyzeButtonEnabled(bool isSelected)
        {
            this.btnAnalyze.Selected = isSelected;
        }

        public List<Trait> AllTraits { get; set; }

        // TODO: do we need it?
        //private List<Trait> _selectedTraits = new List<Trait>();
        //public List<Trait> SelectedTraits
        //{
        //    get
        //    {
        //        return selectedTraits;
        //    }
        //}

        public List<RecordedSampleIdentifier> PreviousSampleIdentifiers { get; set; }

        public DataService.EnumHelpCode HelpCode { get { return DataService.EnumHelpCode.INSTRUMENT_ANALYZE; } }

        /// <summary>
        /// Selected Material Id
        /// </summary>
        public int MaterialId
        {
            get
            {
                return int.Parse(cbMaterial.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Selected Category Id
        /// </summary>
        public int? CategoryId
        {
            get
            {
                if (cbCategory.SelectedValue == null)
                    return null;
                else
                    return int.Parse(cbCategory.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Selected Sub Category Id
        /// </summary>
        public int? SubCategoryId
        {
            get
            {
                if (cbSubCategory.SelectedValue == null)
                    return null;
                else
                    return int.Parse(cbSubCategory.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Selected Presentation
        /// </summary>
        public int? PresentationId
        {
            get
            {
                if (cbPresentation.SelectedValue == null)
                    return null;
                else
                    return int.Parse(cbPresentation.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// List of selected traits
        /// </summary>
        public List<Trait> SelectedTraits
        {
            get
            {
                List<Trait> lstTraits = new List<Trait>();

                foreach (var tc in flpTraits.Controls.OfType<TraitCheck>())
                {
                    if (tc.Checked)
                    {
                        lstTraits.AddRange(AllTraits.Where(t => t.Id == tc.Id));
                    }
                }

                return lstTraits;
            }
        }

        protected CheckBox CheckBoxCheckSample
        { 
            get { return chkCheckSample; } 
        }

        protected Button ButtonInsertPrevious
        {
            get { return btnInsertPrevious; }
        }

        protected Button ButtonClearAll
        {
            get { return btnClearAll; }
        }

        protected FlowLayoutPanel FlowLayoutPanelIdentifier
        {
            get { return flpIdentifySample; }
        }

        protected Panel PanelIdentifierTop
        {
            get { return pnlIdentifySampleTop; }
        }

        protected Analyze()
        {
            InitializeComponent();
        }

        ///// <summary>
        ///// Create Analyze control based on the traits specified
        ///// </summary>
        ///// <param name="allTraits"></param>
        //public Analyze(List<Trait> allTraits)
        //    : this()
        //{
        //    this.AllTraits = allTraits;

        //    //Fill material dropdown
        //    FillMaterial();
        //}

        protected void InitAnalyze(List<Trait> allTraits)
        {
            this.AllTraits = allTraits;

            //Fill material dropdown
            FillMaterial();
        }

        public void Init(CacheSample sample)
        {
            if (sample != null)
            {
                // select Material, Category, Subcategory and Presentation
                cbMaterial.SelectedValue = sample.MaterialId;

                if (cbCategory.SelectedValue != null)
                    cbCategory.SelectedValue = sample.CategoryId;

                if (cbSubCategory.SelectedValue != null)
                    cbSubCategory.SelectedValue = sample.SubCategoryId;

                if (cbPresentation.SelectedValue != null)
                    cbPresentation.SelectedValue = sample.PresentationId;

                if (cbMaterial.SelectedValue == null)
                    cbMaterial.SelectedIndex = 0;

                // If any Material is selected then only select the traits and set the Sample Identifiers
                if (cbMaterial.SelectedValue != null && int.Parse(cbMaterial.SelectedValue.ToString()) > 0)
                {
                    if (string.IsNullOrEmpty(sample.Traits)) // no trait is selected
                    {
                        chkSelectAll.Checked = false;
                    }
                    else
                    {
                        string[] traitIds = sample.Traits.Split(','); // create collection of trait ids from comma seperated string

                        // select the traits specified by user last time
                        foreach (var traitControl in flpTraits.Controls.OfType<TraitCheck>())
                        {
                            if (traitIds.FirstOrDefault(t => t == traitControl.Id.ToString()) != null)
                                traitControl.Checked = true;
                            else
                            {
                                traitControl.Checked = false;
                                UncheckSelectAll();
                            }
                        }
                    }
                }
            }
        }

        public void SetFocus()
        {
            _isAutoFocus = true;
            cbMaterial.Focus();
            _isAutoFocus = false;

            //if (!btnAnalyze.Selected)
             if (ValidateInputs(false))
                btnAnalyze.Selected = true;

            if (this.ParentForm != null)
                this.ParentForm.AcceptButton = btnAnalyze;
        }

        /// <summary>
        /// Set up the Analyze screen for Repeat Sample, fill all values which were used in the last sample
        /// analysis.
        /// </summary>
        public void RepeatSample(bool isNextSample)
        {
            if (Helper.CurrentSample != null)
            {
                // set material, category, subcategory and presentation which were used in the last analysis
                cbMaterial.SelectedValue = (int?)Helper.CurrentSample.MaterialId;
                cbCategory.SelectedValue = (int?)Helper.CurrentSample.CategoryId;
                cbSubCategory.SelectedValue = (int?)Helper.CurrentSample.SubCategoryId;
                cbPresentation.SelectedValue = (int?)Helper.CurrentSample.PresentationId;

                int cnt = 0;

                if (!isNextSample) // Repeat Sample
                {
                    // set values of sample identifiers which were used in the last analysis
                    foreach (var identifyControl in flpIdentifySample.Controls.OfType<IdentifySample>())
                    {
                        identifyControl.Value = Helper.CurrentSample.SampleIdentifiers[cnt++].Value;
                    }
                }

                // selects the all trits which were used in the last analysis
                foreach (var traitControl in flpTraits.Controls.OfType<TraitCheck>())
                {
                    if (Helper.CurrentSample.Traits.Exists(t => t.Id == traitControl.Id))
                        traitControl.Checked = true;
                }

                // if it is Repeat Sample then start the Analysis automatically
                if (!isNextSample)
                    btnAnalyze_Click(this, new EventArgs());
            }
        }

        #region Virtual Functions
        /// <summary>
        /// Reset all values of Sample identifier section
        /// </summary>
        public virtual void ResetSampleIdentifiers()
        {
            if (this.MaterialId > 0)
            {
                var sampleIdentifiers = Helper.GetSampleIdentifiers(this.MaterialId);
                int counter = 0;

                if (sampleIdentifiers != null && sampleIdentifiers.Count() > 0)
                {
                    foreach (var identifyControl in flpIdentifySample.Controls.OfType<IdentifySample>())
                    {
                        var sampleIdentifier = sampleIdentifiers.FirstOrDefault(
                                                s => s.Name.Trim().ToLower() == identifyControl.DisplayText.Trim().ToLower());

                        if (sampleIdentifier != null && counter != 0)
                            identifyControl.Value = sampleIdentifier.Value;
                        else
                            identifyControl.Value = string.Empty;

                        if (counter == 0)
                        {
                            if (Helper.IsAutoSampleId)
                                identifyControl.AutoGenerate(this.MaterialId);
                        }

                        counter++;
                    }
                }

            }
        }

        /// <summary>
        /// This function must override by base class and provide after click functionality
        /// </summary>
        /// <param name="currentSample"></param>
        protected virtual void AfterAnalyzeClicked(Sample currentSample)
        {
            // this method should not be called. Each child class must override this method
            throw new NotSupportedException();
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public virtual void LocalizeResource()
        {
            if (ResourceHelper.Culture.Name.ToLower() == Helper.CULTURE_NAME_ENGLISH.ToLower())
            {
                AdjustEnglish();
            }
            else if (ResourceHelper.Culture.Name.ToLower() == Helper.CULTURE_NAME_SPANISH.ToLower())
            {
                AdjustSpanish();
            }

            lblSelectMaterial.Text = ResourceHelper.Select_Material;
            lblMaterial.Text = ResourceHelper.Material_Type;
            lblCategory.Text = ResourceHelper.Category;
            lblSubCategory.Text = ResourceHelper.SubCategory;
            lblPresentation.Text = ResourceHelper.Presentation;

            lblIdentifySample.Text = ResourceHelper.Identify_Sample;
            //chkCheckSample.Text = ResourceHelper.This_Is_Check_Sample;

            lblTraits.Text = ResourceHelper.Traits;
            lblSelectAll.Text = ResourceHelper.SelectAll;            

            btnAnalyze.Text = ResourceHelper.Analyze;

            //btnInsertPrevious.Text = ResourceHelper.Insert_Previous;
            //btnClearAll.Text = ResourceHelper.ClearAll;

            if (cbMaterial.Items.Count > 0)
                ((IdNamePair)cbMaterial.Items[0]).Name = ResourceHelper.Select;

            if (cbCategory.Items.Count > 0)
                ((IdNamePair)cbCategory.Items[0]).Name = ResourceHelper.Select;

            if (cbSubCategory.Items.Count > 0)
                ((IdNamePair)cbSubCategory.Items[0]).Name = ResourceHelper.Select;

            if (cbPresentation.Items.Count > 0)
                ((IdNamePair)cbPresentation.Items[0]).Name = ResourceHelper.Select;
        }

        /// <summary>
        /// Load Identify Sample section based on the specified material.
        /// </summary>
        /// <param name="materialId"></param>
        protected virtual void LoadIdentifySample(int materialId)
        {
            try
            {
                var sampleIdentifiers = Helper.GetSampleIdentifiers(materialId);

                flpIdentifySample.Controls.Clear();

                // add the panel which contains insert previous and clear all
                flpIdentifySample.Controls.Add(pnlIdentifySampleTop);

                IdentifySample identifySample = null;

                if (sampleIdentifiers.Count() > 0)
                {
                    int counter = 0;
                    foreach (var item in sampleIdentifiers)
                    {
                        if (item.DropDown)
                        {
                            identifySample = new IdentifySample(item.Id, item.Name, item.MultiValues.Split(',')
                                                                    , item.Required, item.Numeric, false);
                        }
                        else
                        {
                            identifySample = new IdentifySample(item.Id, item.Name, item.Value, item.Required, item.Numeric);
                        }

                        identifySample.OnTextValueChanged += new EventHandler(identifySample_OnTextValueChanged);

                        if (counter == 0)
                        {
                            if (Helper.IsAutoSampleId)
                                identifySample.AutoGenerate(materialId);
                        }

                        flpIdentifySample.Controls.Add(identifySample);

                        counter++;
                    }
                }
                else
                {
                    Helper.DisplayError(ResourceHelper.Error_10501); // sample identifiers dosen't exists
                }
            }
            catch (Exception ex)
            {
                Helper.LogError("Analyze.LoadIdentifySample", "materialId" + materialId.ToString(), ex, true);
            }
        }
        #endregion

        #region Control Events

        /// <summary>
        /// Analyze control load event. Localize all texts on the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Analyze_Load(object sender, EventArgs e)
        {
            LocalizeResource();

            if (System.Configuration.ConfigurationManager.AppSettings["AverageScans"].ToLower() == "true")
                Helper.IsAveragingOn = true;

            if (System.Configuration.ConfigurationManager.AppSettings["AverageScanCleanCheck"].ToLower() == "true")
                Helper.IsAverageScanCleanCheckOn = true;

        }
            

        /// <summary>
        /// Check All(traits) click event. It selects or Deselects all traits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            //1. on uncheck, unselect trait only if there is atleast one slected trait
            //2. on check, select all traits only if there is atleast one unselected trait
            if (chkSelectAll.Checked && !this.AllTraitsSelected()
                || (!chkSelectAll.Checked && this.TraitSelected() && _uncheckAll))
            {
                foreach (var traitControl in flpTraits.Controls.OfType<TraitCheck>())
                {
                    traitControl.Checked = chkSelectAll.Checked;
                }

                ValidateInputs(false);
            }
        }

        /// <summary>
        /// Analyze button click event
        /// </summary>
        public void btnAnalyze_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInputs(true))
                {
                    // create object of Sample class which will hold all values specified by user
                    Sample currentSample = new Sample();
                    currentSample.MaterialId = int.Parse(cbMaterial.SelectedValue.ToString());
                    currentSample.MaterialName = cbMaterial.Text;

                    currentSample.CategoryId = int.Parse(cbCategory.SelectedValue.ToString());
                    currentSample.CategoryName = cbCategory.Text;

                    currentSample.SubCategoryId = int.Parse(cbSubCategory.SelectedValue.ToString());
                    currentSample.SubCategoryName = cbSubCategory.Text;

                    currentSample.PresentationId = int.Parse(cbPresentation.SelectedValue.ToString());
                    currentSample.PresentationName = cbPresentation.Text;

                    currentSample.SampleIdentifiers = new List<SampleIdentifier>();
                    currentSample.Traits = new List<IdNamePair>();

                    foreach (var identifyControl in flpIdentifySample.Controls.OfType<IdentifySample>())
                    {
                        currentSample.SampleIdentifiers.Add(new SampleIdentifier
                                                                {
                                                                    Id = identifyControl.Id,
                                                                    Name = identifyControl.DisplayText,
                                                                    Value = identifyControl.Value
                                                                });
                    }

                    if (Helper.SpeedMode && GetContainerControl().ToString() == "InstrumentUI_ATK.FormControls.Routine")
                    {                        
                        DAL.InsertRecordedSampleIdentifiers(currentSample, currentSample.MaterialId);
                    }
                    

                    foreach (var traitControl in flpTraits.Controls.OfType<TraitCheck>())
                    {
                        if (traitControl.Checked)
                            currentSample.Traits.Add(new IdNamePair { Id = traitControl.Id, Name = traitControl.DisplayText });
                    }

                    // Disable button so user should not click/enter at this button again
                    // once workflow is started and currently running.
                    btnAnalyze.Selected = false;
                    this.Update();
                    Helper.currentSample = currentSample;
                    
                    Thread.Sleep(2000);

                    AfterAnalyzeClicked(currentSample);
                }
            }
            catch (Exception ex)
            {
                Helper.LogError("Analyze.btnAnalyze_Click", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// Load the previous sample identifiers(if any) and their according to the selected material.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnInsertPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMaterial.SelectedIndex > 0)
                {
                    int materialId = int.Parse(cbMaterial.SelectedValue.ToString());

                    var sampleIdentifiers = DAL.GetPreviousSampleIdentifiersByMaterial(materialId);
                    int counter = 0;

                    if (sampleIdentifiers != null && sampleIdentifiers.Count() > 0)
                    {
                        foreach (var identifyControl in flpIdentifySample.Controls.OfType<IdentifySample>())
                        {
                            var sampleIdentifier = sampleIdentifiers.FirstOrDefault(
                                                    s => s.Name.Trim().ToLower() == identifyControl.DisplayText.Trim().ToLower());

                            if (sampleIdentifier != null)
                                identifyControl.Value = sampleIdentifier.Value;
                            else
                                identifyControl.Value = string.Empty;

                            if (counter == 0)
                            {
                                if (Helper.IsAutoSampleId)
                                    identifyControl.AutoGenerate(this.MaterialId);
                            }

                            counter++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogError("Analyze.btnInsertPrevious_Click", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// Clear the values of all the sample identifiers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            foreach (var control in flpIdentifySample.Controls)
            {
                if (control is IdentifySample)
                {
                    var identifyControl = (IdentifySample)control;

                    // do not clear auto generated control
                    if (!identifyControl.IsAutoGenerated)
                        identifyControl.ClearValue();
                }
            }
        }

        /// <summary>
        /// Validate all fields when user changes any trait selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkTrait_OnCheckChanged(object sender, EventArgs e)
        {
            // if any trait is unchecked then set the flag to not uncheck all trait if the Select All is unchecked
            if (!((TraitCheck)sender).Checked)
                _uncheckAll = false;

            ValidateInputs(false);
            VerifySelectAll();
        }

        /// <summary>
        /// Validate all fields when user changes any sample identifier value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void identifySample_OnTextValueChanged(object sender, EventArgs e)
        {
            ValidateInputs(false);
        }

        /// <summary>
        /// To change the selection color of the combo box, override this method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMaterial_DrawItem(object sender, DrawItemEventArgs e)
        {
            //ComboBox combo = sender as ComboBox;
            //if (e.Index >= 0)
            //{
            //    var item = combo.Items[e.Index] as IdNamePair;

            //    if (item != null)
            //    {
            //        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            //        {
            //            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(210, 222, 236)), e.Bounds);
            //        }
            //        else
            //        {
            //            e.Graphics.FillRectangle(new SolidBrush(combo.BackColor), e.Bounds);
            //        }

            //        e.Graphics.DrawString(item.Name, e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            //    }
            //}
        }

        /// <summary>
        /// it shifts focus back to material
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlTraits_Enter(object sender, EventArgs e)
        {
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
        }

        private void cbMaterial_Enter(object sender, EventArgs e)
        {
            if (_isAutoFocus)
            {
                if (cbMaterial.SelectedIndex > 0)
                    cbCategory.Focus();
            }
        }

        private void cbCategory_Enter(object sender, EventArgs e)
        {
            if (_isAutoFocus)
            {
                if (cbCategory.SelectedIndex > 0)
                    cbSubCategory.Focus();
            }
        }

        private void cbSubCategory_Enter(object sender, EventArgs e)
        {
            if (_isAutoFocus)
            {
                if (cbSubCategory.SelectedIndex > 0)
                    cbPresentation.Focus();
            }
        }

        private void cbPresentation_Enter(object sender, EventArgs e)
        {
            bool isSampleIdentifierActive = false;

            if (_isAutoFocus)
            {
                _isAutoFocus = false;

                if (cbPresentation.SelectedIndex > 0)
                {
                    // select the traits specified by user last time
                    foreach (var sampleIdentifier in flpIdentifySample.Controls.OfType<IdentifySample>())
                    {
                        if (!sampleIdentifier.IsAutoGenerated)
                        {
                            if (string.IsNullOrEmpty(sampleIdentifier.Value))
                            {
                                sampleIdentifier.Focus();

                                isSampleIdentifierActive = true;
                                break;
                            }
                        }
                    }

                    if (!isSampleIdentifierActive)
                    {
                        cbMaterial.Focus();
                    }
                }
            }
        }

        #endregion

        #region Dropdowns(Material Type, category, SubCategory, Presentation) selected index changed event
        /// <summary>
        /// seleced material changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbMaterial.SelectedIndex > 0)
                {

                    flpTraits.Visible = false; // hide the panel to reduce the flickering(height change)
                    pnlIdentifySample.Visible = true;

                    int materialId = int.Parse(cbMaterial.SelectedValue.ToString());
                    LoadIdentifySample(materialId);
                    FillCategory(materialId);
                    //FillSubCategory(materialId, null);
                    //FillPresentation(materialId, null, null);
                    FillTraits(materialId, null, null, null);

                    flpTraits.Visible = true; // display the panel after all filtered trait is loaded

                }
                else if (cbMaterial.SelectedIndex == 0)
                {
                    // hide the Identify Sample and Trait section and clear category, subcategory and presentaion dropdowns
                    pnlIdentifySample.Visible = false;
                    pnlTraits.Visible = false;

                    cbCategory.DataSource = new IdNamePair[] { new IdNamePair() { Id = 0, Name = ResourceHelper.Select } }; ;
                    cbSubCategory.DataSource = new IdNamePair[] { new IdNamePair() { Id = 0, Name = ResourceHelper.Select } }; ;
                    cbPresentation.DataSource = new IdNamePair[] { new IdNamePair() { Id = 0, Name = ResourceHelper.Select } }; ;
                }

                ValidateInputs(false);
            }
            catch (Exception ex)
            {
                Helper.LogError("Analyze.cbCategory_SelectedIndexChanged", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// Selected category changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool hideTraits = false;

                if (cbMaterial.SelectedIndex >= 0)
                {
                    if (flpTraits.Visible)
                    {
                        hideTraits = true;
                        flpTraits.Visible = false; // hide the panel to reduce the flickering(height change)
                    }

                    int materialId = int.Parse(cbMaterial.SelectedValue.ToString());

                    if (cbCategory.SelectedIndex > 0)
                    {
                        int categoryId = int.Parse(cbCategory.SelectedValue.ToString());
                        FillTraits(materialId, categoryId, null, null);
                        FillSubCategory(materialId, categoryId);
                    }
                    else if (cbCategory.SelectedIndex == 0)
                    {
                        FillTraits(materialId, null, null, null);
                        FillSubCategory(materialId, null);
                    }

                    if (hideTraits)
                        flpTraits.Visible = true; // display the panel after all filtered trait is loaded
                }

                ValidateInputs(false);
            }
            catch (Exception ex)
            {
                Helper.LogError("Analyze.cbCategory_SelectedIndexChanged", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// Selected sub category changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool hideTraits = false;

                if (cbMaterial.SelectedIndex >= 0 && cbCategory.SelectedIndex >= 0)
                {
                    if (flpTraits.Visible)
                    {
                        hideTraits = true;
                        flpTraits.Visible = false; // hide the panel to reduce the flickering(height change)
                    }

                    int materialId = int.Parse(cbMaterial.SelectedValue.ToString());
                    int categoryId = int.Parse(cbCategory.SelectedValue.ToString());

                    if (cbSubCategory.SelectedIndex > 0)
                    {
                        int subCategoryId = int.Parse(cbSubCategory.SelectedValue.ToString());
                        FillTraits(materialId, categoryId, subCategoryId, null);
                        FillPresentation(materialId, categoryId, subCategoryId);
                    }
                    else if (cbSubCategory.SelectedIndex == 0)
                    {
                        FillTraits(materialId, categoryId, null, null);
                        FillPresentation(materialId, categoryId, null);
                    }

                    if (hideTraits)
                        flpTraits.Visible = true; // display the panel after all filtered trait is loaded
                }

                ValidateInputs(false);
            }
            catch (Exception ex)
            {
                Helper.LogError("Analyze.cbSubCategory_SelectedIndexChanged", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// Selected presentation changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPresentation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool hideTraits = false;

                if (cbMaterial.SelectedIndex >= 0 && cbCategory.SelectedIndex >= 0 && cbSubCategory.SelectedIndex >= 0)
                {
                    if (flpTraits.Visible)
                    {
                        hideTraits = true;
                        flpTraits.Visible = false; // hide the panel to reduce the flickering(height change)
                    }

                    int materialId = int.Parse(cbMaterial.SelectedValue.ToString());
                    int categoryId = int.Parse(cbCategory.SelectedValue.ToString());
                    int subCategoryId = int.Parse(cbSubCategory.SelectedValue.ToString());

                    if (cbPresentation.SelectedIndex > 0)
                    {
                        int presentationId = int.Parse(cbPresentation.SelectedValue.ToString());
                        FillTraits(materialId, categoryId, subCategoryId, presentationId);
                    }
                    else if (cbPresentation.SelectedIndex == 0)
                    {
                        FillTraits(materialId, categoryId, subCategoryId, null);
                    }

                    if (hideTraits)
                        flpTraits.Visible = true; // display the panel after all filtered trait is loaded
                }

                ValidateInputs(false);
            }
            catch (Exception ex)
            {
                Helper.LogError("Analyze.cbPresentation_SelectedIndexChanged", string.Empty, ex, true);
            }
        }

        /// <summary>
        /// Analyze control size changed event. Move Analyze button half way between the trait 
        /// panel and the right side of the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Analyze_SizeChanged(object sender, EventArgs e)
        {
            // get the right location of trait panel
            int leftX = pnlTraits.Location.X + pnlTraits.Width;

            // get the right of screen
            int rightX = this.Width;

            // calculate the required position(midway) of Analyze button
            int locationX = (leftX + rightX) / 2 - (btnAnalyze.Width / 2);

            btnAnalyze.Location = new Point(locationX, btnAnalyze.Location.Y);
        }
        #endregion

        #region Dropdown(Material Type, category, SubCategory, Presentation) check box list(Traits) Fill methods
        /// <summary>
        /// Fill material type dropdown with materials
        /// </summary>
        protected void FillMaterial()
        {
            // get all distict materials
            var materials = AllTraits.Where(m => m.MaterialId > 0 && m.ModelGroupStageName.ToUpper() == "Commercialized Q2".ToUpper())
                                        .Select(t => new IdNamePair() { Id = t.MaterialId, Name = t.MaterialName })
                                        .Distinct((m1, m2) => m1.Id == m2.Id)
                                        .OrderBy(m => m.Name);

            IEnumerable<IdNamePair> materialsWithSelect = new IdNamePair[] { new IdNamePair() { Id = 0, Name = ResourceHelper.Select } };

            if (materials.Count() > 0)
            {
                materialsWithSelect = materialsWithSelect.Concat(materials);
            }

            cbMaterial.DataSource = materialsWithSelect.ToList();
            
            // if there is one material then select that material
            if (materials.Count() == 1)
            {
                cbMaterial.SelectedIndex = 1;
            }
            else if (materials.Count() == 0)
            {
                Helper.DisplayError(ResourceHelper.Error_10401);
            }
        }

        /// <summary>
        /// Fill category dropdown based on specified material id 
        /// </summary>
        /// <param name="materialId"></param>
        private void FillCategory(int materialId)
        {
            var categories = AllTraits.Where(t => t.MaterialId == materialId && t.CategoryId > 0)
                                     .Select(t => new IdNamePair() { Id = t.CategoryId, Name = t.CategoryName })
                                     .Distinct((c1, c2) => c1.Id == c2.Id)
                                     .OrderBy(c => c.Name);

            IEnumerable<IdNamePair> categoriesWithSelect = new IdNamePair[] { new IdNamePair() { Id = 0, Name = ResourceHelper.Select } };

            if (categories.Count() > 0)
            {
                categoriesWithSelect = categoriesWithSelect.Concat(categories);
                cbCategory.TabStop = true;
            }
            else
            {
                cbCategory.TabStop = false;
            }

            cbCategory.DataSource = categoriesWithSelect.ToList();

            // if there is one category then select that category
            if (categories.Count() == 1)
            {
                cbCategory.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// Fill sub category dropdown based on specified category id 
        /// </summary>
        /// <param name="materialId"></param>
        private void FillSubCategory(int materialId, int? categoryId)
        {
            IEnumerable<IdNamePair> finalSubCategories  = new List<IdNamePair>();

            if (categoryId.HasValue)
            {
                var subCategories = AllTraits.Where(delegate(Trait t)
                {
                    bool filter = false;

                    filter = t.MaterialId == materialId && t.SubcategoryId > 0;

                    if (categoryId.HasValue)
                        filter = filter && t.CategoryId == categoryId;

                    return filter;
                });

                finalSubCategories = subCategories.Select(t => new IdNamePair() { Id = t.SubcategoryId, Name = t.SubcategoryName })
                                         .Distinct((s1, s2) => s1.Id == s2.Id)
                                         .OrderBy(s => s.Name);
            }

            IEnumerable<IdNamePair> subCategoriesWithSelect = new IdNamePair[] { new IdNamePair() { Id = 0, Name = ResourceHelper.Select } };

            if (finalSubCategories.Count() > 0)
            {
                subCategoriesWithSelect = subCategoriesWithSelect.Concat(finalSubCategories);
                cbSubCategory.TabStop = true;
            }
            else
            {
                cbSubCategory.TabStop = false;
            }

            cbSubCategory.DataSource = subCategoriesWithSelect.ToList();

            // if there is one sub category and category is selected then select that sub category
            if (finalSubCategories.Count() == 1 && (cbCategory.SelectedIndex > 0 || cbCategory.Items.Count == 1))
            {
                cbSubCategory.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// Fill presentation dropdown based on specified sub category id 
        /// </summary>
        /// <param name="materialId"></param>
        private void FillPresentation(int materialId, int? categoryId, int? subCategoryId)
        {
            IEnumerable<IdNamePair> finalPresentations = new List<IdNamePair>();

            if (subCategoryId.HasValue)
            {
                var presentations = AllTraits.Where(delegate(Trait t)
                {
                    bool filter = false;

                    filter = t.MaterialId == materialId && t.PresentationId > 0;

                    if (categoryId.HasValue)
                        filter = filter && t.CategoryId == categoryId;

                    if (subCategoryId.HasValue)
                        filter = filter && t.SubcategoryId == subCategoryId;

                    return filter;
                });

                finalPresentations = presentations.Select(t => new IdNamePair() { Id = t.PresentationId, Name = t.PresentationName })
                                         .Distinct((p1, p2) => p1.Id == p2.Id)
                                         .OrderBy(p => p.Name);
            }

            IEnumerable<IdNamePair> presentationsWithSelect = new IdNamePair[] { new IdNamePair() { Id = 0, Name = ResourceHelper.Select } };

            if (finalPresentations.Count() > 0)
            {
                presentationsWithSelect = presentationsWithSelect.Concat(finalPresentations);
                cbPresentation.TabStop = true;
            }
            else
            {
                cbPresentation.TabStop = false;
            }

            cbPresentation.DataSource = presentationsWithSelect.ToList();

            // if there is one presentation and subcategory is selected then select that presentation
            if (finalPresentations.Count() == 1 && (cbSubCategory.SelectedIndex > 0 || cbSubCategory.Items.Count == 1))
            {
                cbPresentation.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// Display all traits based on the specified presentation
        /// </summary>
        /// <param name="presentationId"></param>
        private void FillTraits(int materialId, int? categoryId, int? subCategoryId, int? presentationId)
        {
            var traits = AllTraits.Where(delegate (Trait t)
            {
                bool filter = false;

                filter = t.MaterialId == materialId;

                filter = filter && t.ModelGroupStageName.Trim().ToLower()
                        == Helper.MODEL_STAGE_ROUTINE.Trim().ToLower();

                if (categoryId > 0)
                    filter = filter && t.CategoryId == categoryId;

                if (subCategoryId > 0)
                    filter = filter && t.SubcategoryId == subCategoryId;

                if (presentationId > 0)
                    filter = filter && t.PresentationId == presentationId;

                return filter;
            }
                                          );

            var displayTraits = traits.OrderBy(t => t.MGOrder)
                             .Select(t => new IdNamePair() { Id = t.Id, Name = t.Name })
                             .Distinct((t1, t2) => t1.Id == t2.Id);

            flpTraits.Controls.Clear();

            int count = 0;

            foreach (var trait in displayTraits)
            {
                TraitCheck chkTrait = new TraitCheck(trait.Id.Value, trait.Name);
                chkTrait.Checked = true;
                chkTrait.OnCheckChanged += new EventHandler(chkTrait_OnCheckChanged);

                // Sets beginning margin to account for panel margin issues when scrolling
                if (count == 0)
                {
                    chkTrait.Margin = new Padding(0, 10, 0, 0);
                }
                else
                {
                    chkTrait.Margin = new Padding(0, 6, 0, 0);
                }

                flpTraits.Controls.Add(chkTrait);

                count++;
            }

            int traitCount = flpTraits.Controls.OfType<TraitCheck>().Count();

            /*int requiredHeight = 23 * traitCount + 5; // 23 pixel per trait is required. Padding is 5 pixel.

            // fix the height of trait panel
            if (requiredHeight > MAX_HEIGHT_PANEL_INNER) // if required height exceed max height(430) then set the max height(scroll bar will be visible)
            {
                // set the max height
                flpTraits.Height = MAX_HEIGHT_PANEL_OUTER;
                flpTraits.Height = MAX_HEIGHT_PANEL_INNER;
            }
            else if (requiredHeight < MIN_HEIGHT_PANEL_INNER)
            {
                // set the minimum height
                flpTraits.Height = MIN_HEIGHT_PANEL_OUTER;
                flpTraits.Height = MIN_HEIGHT_PANEL_INNER;
            }
            else
            {
                flpTraits.Height = requiredHeight + 5; // 5 pixel is the margin with outer panel(flptrait)
                flpTraits.Height = requiredHeight;
            }*/

            //pnlTraitBottom.Location = new Point(pnlTraitBottom.Location.X, pnlTraitCheckbox.Location.Y + pnlTraitCheckbox.Height);

            if (displayTraits.Count() > 0 && !chkSelectAll.Checked)
            {
                chkSelectAll.Checked = true;
            }

            pnlTraits.Visible = true;
        }

        #endregion

        #region Class Level Private Functions

        /// <summary>
        /// Validate all fields on the form
        /// </summary>
        /// <param name="isDisplayMessage"></param>
        /// <returns></returns>
        protected bool ValidateInputs(bool isDisplayMessage)
        {
            if (cbMaterial.SelectedIndex > 0 && (cbCategory.SelectedIndex > 0 || cbCategory.Items.Count == 1 )
                        && (cbSubCategory.SelectedIndex > 0 || cbSubCategory.Items.Count == 1) 
                        && (cbPresentation.SelectedIndex > 0 || cbPresentation.Items.Count == 1))
                pnlTraits.Visible = true;
            else
                pnlTraits.Visible = false;

            // Material Must be selected
            if (cbMaterial.SelectedIndex <= 0)
            {
                if (isDisplayMessage)
                    Helper.DisplayError(ResourceHelper.Material_Required);
                else
                    btnAnalyze.Selected = false;

                return false;
            }

            // All sample identifiers must have valid values
            string errorMessage = ValidateIdentifySample();

            if (errorMessage.Length > 0)
            {
                if (isDisplayMessage)
                    Helper.DisplayError(errorMessage);//Display error message
                else
                    btnAnalyze.Selected = false;

                return false;
            }

            // Atleast on trait must be selected and trait panel must be visible
            if (!(pnlTraits.Visible && this.TraitSelected()))
            {
                if (isDisplayMessage)
                    Helper.DisplayError(ResourceHelper.Error_10402);// select atleast on trait
                else
                    btnAnalyze.Selected = false;

                return false;
            }

            btnAnalyze.Selected = true;
            return true;
        }

        /// <summary>
        /// It will validate all sample identifiers displayed in the Identify Sample section.
        /// </summary>
        /// <returns></returns>
        private string ValidateIdentifySample()
        {
            string message = string.Empty;

            foreach (var control in flpIdentifySample.Controls)
            {
                if (control is IdentifySample)
                {
                    var identifyControl = (IdentifySample)control;

                    if (identifyControl.IsRequired)
                    {
                        if (identifyControl.Value.Length == 0)
                        {
                            //invalid value
                            message += ResourceHelper.Error_10502.Replace("[0]", identifyControl.DisplayText) + Environment.NewLine;
                        }
                        else
                        {
                            if (identifyControl.DisplayText=="Samp ID")
                            {
                                Helper.SampleID = identifyControl.Value.ToString();
                            }
                        }
                    }
                    //Added by GDH
                    if (identifyControl.DisplayText == "Analysis Type")
                    {
                        if (!Helper.IsAveragingOn)
                        {
                            identifyControl.Value = "Single Scan";
                        }
                        else if (Helper.IsAveragingOn)
                            identifyControl.Value = "Averaged Scans";
                    }

                    if (identifyControl.IsNumeric && identifyControl.Value.Length > 0)
                    {
                        if (!identifyControl.Value.IsNumeric())
                        {
                            //invalid value
                            message += ResourceHelper.Error_10502.Replace("[0]", identifyControl.DisplayText) + Environment.NewLine;
                        }
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// Returns true if any trait is selected otherwise false
        /// </summary>
        /// <returns></returns>
        private bool TraitSelected()
        {
            foreach (var item in flpTraits.Controls.OfType<TraitCheck>())
            {
                if (item.Checked)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if all traits are selected otherwise false
        /// </summary>
        /// <returns></returns>
        private bool AllTraitsSelected()
        {
            foreach (var item in flpTraits.Controls.OfType<TraitCheck>())
            {
                if (!item.Checked)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Uncheck Select all check box without unchecking all the traits
        /// </summary>
        private void UncheckSelectAll()
        {
            if (chkSelectAll.Checked)
            {
                _uncheckAll = false; // set the flag

                chkSelectAll.Checked = false;

                _uncheckAll = true; // reset the flag
            }

        }

        /// <summary>
        /// It checked Select All check box if all trait is selected 
        /// and uncheck Select all if any trait is unchecked.
        /// </summary>
        /// <returns></returns>
        private void VerifySelectAll()
        {
            if (this.AllTraitsSelected() && !chkSelectAll.Checked)
                chkSelectAll.Checked = true;
            else if (!this.AllTraitsSelected() && chkSelectAll.Checked)
                chkSelectAll.Checked = false;

            _uncheckAll = true; // reset the flag
        }

        /// <summary>
        /// Adjust the size/location of lables according to the Spanish text.
        /// </summary>
        private void AdjustSpanish()
        {
            this.btnInsertPrevious.Location = new System.Drawing.Point(262, 2);
            this.btnInsertPrevious.Size = new System.Drawing.Size(104, 20);

            this.btnClearAll.Location = new System.Drawing.Point(375, 2);
            this.btnClearAll.Size = new System.Drawing.Size(82, 20);
        }

        /// <summary>
        /// Adjust the size/location of lables back to the English text.
        /// </summary>
        private void AdjustEnglish()
        {
            this.btnInsertPrevious.Location = new System.Drawing.Point(278, 2);
            this.btnInsertPrevious.Size = new System.Drawing.Size(99, 20);

            this.btnClearAll.Location = new System.Drawing.Point(386, 2);
            this.btnClearAll.Size = new System.Drawing.Size(71, 20);
        }

        #endregion

    }
}
