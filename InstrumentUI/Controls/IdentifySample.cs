using System;
using System.Drawing;
using System.Windows.Forms;
using InstrumentUI_ATK.Common;

namespace InstrumentUI_ATK.Controls
{
    /// <summary>
    /// A control which represents single sample identiifer and contains a Label and a textbox or dropdown
    /// </summary>
    public partial class IdentifySample : UserControl
    {
        #region Properties

        /// <summary>
        /// Whether the control is displayed as a DropDownList (otherwise, TextBox)
        /// </summary>
        public bool IsDropdown { get; set; }

        /// <summary>
        /// Whether a value is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Whether the value must be numeric
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// The control's ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Text to be displayed before the control
        /// </summary>
        public string DisplayText
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        /// <summary>
        /// Whether the value is Automatically Generated
        /// </summary>
        public bool IsAutoGenerated { get; private set; }

        /// <summary>
        /// Whether the control is ReadOnly
        /// </summary>
        public bool IsReadOnly 
        {
            get 
            {
                if (IsDropdown)
                    return !cbDropDown.Enabled;
                else
                    return !txtTextBox.Enabled;
            }
            set
            {
                if (IsDropdown)
                    cbDropDown.Enabled = !value;
                else
                    txtTextBox.Enabled = !value;
            }
        }

        /// <summary>
        /// get the value(text in case of TextBox and SelectedValue incase of dropdown
        /// </summary>
        public string Value
        {
            get
            {
                if (IsDropdown)
                {
                    if (cbDropDown.SelectedIndex >= 0)
                        return cbDropDown.SelectedValue.ToString();
                    else
                        return cbDropDown.Text;
                }
                else
                {
                    return txtTextBox.Text.Trim();
                }
            }
            set
            {
                if (IsDropdown)
                {
                    if (string.IsNullOrEmpty(value))
                        cbDropDown.SelectedIndex = 0;
                    else
                        cbDropDown.SelectedItem = value;
                }
                else
                {
                    txtTextBox.Text = string.IsNullOrEmpty(value) ? string.Empty : value;

                    if (!txtTextBox.Enabled)
                        txtTextBox.Enabled = true;
                }
            }
        }

        /// <summary>
        /// The back color of required controls
        /// </summary>
        public Color RequiredBackColor
        {
            get { return _requiredBackColor; }
            set { _requiredBackColor = value; }
        }

        private Color _requiredBackColor = Color.FromArgb(239, 239, 239);

        #endregion

        #region Events & Event Handlers

        public event EventHandler OnTextValueChanged;


        private void IdentifySample_Enter(object sender, EventArgs e)
        {
            if (IsDropdown)
                cbDropDown.Focus();
            else
                txtTextBox.Focus();
        }


        /// <summary>
        /// Raises text changed event
        /// </summary>
        private void identifierValue_TextChanged(object sender, EventArgs e)
        {
            if (OnTextValueChanged != null)
                OnTextValueChanged(this, new EventArgs());
        }

        #endregion

        /// <summary>
        /// Create a new instance of the IdentifySample class
        /// </summary>
        private IdentifySample()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Creates a Label and a DropDownList beside it
        /// </summary>
        /// <param name="id">Control's ID</param>
        /// <param name="labelText">Label Text</param>
        /// <param name="ddlSource">data source for the dropdown</param>
        /// <param name="isRequired">Whether a Value is Required</param>
        /// <param name="isNumeric">Whether Value must be Numeric</param>
        /// <param name="isDropdownEditable">Whether the DropDownList allows free form text</param>
        public IdentifySample(int id, string labelText, string[] ddlSource, bool isRequired, bool isNumeric, bool isDropdownEditable) : this()
        {
            CreateControl(id, labelText, null, isRequired, isNumeric, true, ddlSource, isDropdownEditable);
        }


        /// <summary>
        /// Creates a Label and a TextBox beside it
        /// </summary>
        /// <param name="id">Control's ID</param>
        /// <param name="labelText">Label Text</param>
        /// <param name="value">Control's Value</param>
        /// <param name="isRequired">Whether a Value is Required</param>
        /// <param name="isNumeric">Whether Value must be Numeric</param>
        public IdentifySample(int id, string labelText, string value, bool isRequired, bool isNumeric) : this()
        {
            CreateControl(id, labelText, value, isRequired, isNumeric, false, null, false);
        }


        /// <summary>
        /// This function will create the control based on the inputs provided by user
        /// </summary>
        /// <param name="id">Control's ID</param>
        /// <param name="labelText">Label Text</param>
        /// <param name="value">Control's Value</param>
        /// <param name="isRequired">Whether a Value is Required</param>
        /// <param name="isNumeric">Whether Value must be Numeric</param>
        /// <param name="isDropdown">Whether Control is a DropDownList (otherwise, TextBox)</param>
        /// <param name="dropdownSource">Data source for the DropDownList</param>
        /// <param name="isDropdownEditable">Whether the DropDownList allows free form text</param>
        private void CreateControl(int id, string labelText, string value, bool isRequired, bool isNumeric,
                                   bool isDropdown, string[] dropdownSource, bool isDropdownEditable)
        {
            IsDropdown = isDropdown;
            Id = id;
            DisplayText = labelText;
            IsRequired = isRequired;
            IsNumeric = isNumeric;

            // If it is a DropDownList then hide the TextBox and set the DataSource of 
            // DropDown as provided by user
            if (isDropdown && dropdownSource != null)
            {
                txtTextBox.Visible = false;
                cbDropDown.Visible = true;

                if (IsRequired)
                    cbDropDown.BackColor = RequiredBackColor;

                cbDropDown.DropDownStyle = isDropdownEditable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;

                if (dropdownSource.Length > 0)
                {
                    if (dropdownSource[0].Trim() != "")
                    {
                        bool bDefault = false;
                        string szDefault = "";

                        bDefault = dropdownSource[0].Contains("<*>");
                        if (bDefault)
                        {
                            szDefault = dropdownSource[0].Remove(0, 3);
                            dropdownSource[0] = szDefault;
                            cbDropDown.DataSource = dropdownSource;
                        }
                        else
                        {
                            var ds = new string[dropdownSource.Length + 1];
                            for (var i = ds.Length - 1; i > 0; i--)
                                ds[i] = dropdownSource[i - 1];

                            ds[0] = string.Empty;
                            cbDropDown.DataSource = ds;
                        }
                    }
                    else
                        cbDropDown.DataSource = dropdownSource;
                }
            }
            else // Otherwise hide the DropDownList and display the TextBox
            {
                txtTextBox.Visible = true;
                cbDropDown.Visible = false;

                txtTextBox.Text = string.IsNullOrEmpty(value) ? string.Empty : value;

                if (IsRequired)
                    txtTextBox.BackColor = RequiredBackColor;
            }
        }


        /// <summary>
        /// Clear the value. if Label then clear its text and if dropdown then reset its index to 0
        /// </summary>
        public void ClearValue()
        {
            if (IsDropdown)
                cbDropDown.SelectedIndex = 0;
            else
                txtTextBox.Text = string.Empty;
        }


        /// <summary>
        /// It is used to set the sample id as autogenerated
        /// </summary>
        /// <param name="materialId">Material ID</param>
        public void AutoGenerate(int materialId)
        {
            if (Helper.CurrentOwner.FormCurrentAnalyze is FormControls.Routine)
            {
                IsAutoGenerated = true;
                string materialCode = Helper.GetMaterialCode(materialId);

                if (string.IsNullOrEmpty(materialCode))
                    materialCode = string.Empty;

                string sampleId = string.Format("{0}{1}_{2}",
                                                Helper.CurrentUser.Location.LocationId,
                                                materialCode,
                                                DateTime.Now.ToString("yyMMddHHmm"));

                if (IsDropdown)
                {
                    cbDropDown.Text = sampleId;
                    cbDropDown.Enabled = false;
                }
                else
                {
                    txtTextBox.Text = sampleId;
                    txtTextBox.Enabled = true;
                }
            }
        }


        /// <summary>
        /// The mode that the IdentifySample control should be displayed
        /// </summary>
        /// <remarks>
        /// Compact - Displays with the control only, no label.
        /// Expanded - Displays with the Label and Control in a single row.
        /// </remarks>
        public enum Mode
        {
            Compact,
            Expanded
        }
    }
}
