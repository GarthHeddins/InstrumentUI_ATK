using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InstrumentUI_ATK.Controls
{
    public partial class ResultListItem : UserControl
    {
        public event EventHandler OnItemClicked;
        private bool _isSelected;

        #region Properties
        
        /// <summary>
        /// Uniqueidentifier for the list item
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Background color of selected item
        /// </summary>
        public Color SelectedColor { get; set; }

        /// <summary>
        /// Get or set the item selected
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;

                if (value)
                    this.BackColor = this.SelectedColor;
                else
                    this.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// It contains previous result item
        /// </summary>
        public ResultListItem LastResultItem { get; set; }

        /// <summary>
        /// It contains next result item
        /// </summary>
        public ResultListItem NextResultItem { get; set; }

        #endregion

        #region Constructors

        public ResultListItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create list item based on the specified display text
        /// </summary>
        /// <param name="displayText"></param>
        public ResultListItem(string displayText)
            : this(string.Empty, displayText)
        {

        }

        /// <summary>
        /// Create list item based on the serial number and the display text
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="displayText"></param>
        public ResultListItem(string serialNumber, string displayText)
            : this()
        {
            if (string.IsNullOrEmpty(serialNumber.Trim()))
                lblSerialNumber.Text = string.Empty;
            else
                lblSerialNumber.Text = serialNumber + ".";

            lblDisplayText.Text = displayText;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Click on the list item, raise the OnItemClicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDisplayText_Click(object sender, EventArgs e)
        {
            if (this.OnItemClicked != null)
                this.OnItemClicked(this, new EventArgs());
        }

        #endregion
    }
}
