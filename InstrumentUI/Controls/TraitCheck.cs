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
    /// <summary>
    /// A custom control to represent a trait with select ability and contain a Lable and a checkbox.
    /// </summary>
    public partial class TraitCheck : UserControl
    {
        public event EventHandler OnCheckChanged;

        //flag to raise or not to raise checkchanged event
        private bool _isRaise = true; 

        /// <summary>
        /// Id of the Trait
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the trait
        /// </summary>
        public string DisplayText 
        {
            get { return lblText.Text; }
            set { lblText.Text = value; } 
        }

        /// <summary>
        /// Get or Set check box checked status but do not raise CheckedChanged event
        /// </summary>
        public bool Checked
        {
            get { return chkTrait.Checked; }
            set 
            {
                _isRaise = false; // set the flag to not raise checkchanged event
                chkTrait.Checked = value;
                _isRaise = true; // reset the flag
            }
        }

        /// <summary>
        /// check or uncheck check box and raise CheckChanged event
        /// </summary>
        public bool CheckednRaise
        {
            set { chkTrait.Checked = value; }
        }

        private TraitCheck()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Create control with the help of Id and Text specified
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public TraitCheck(int id, string text)
            : this()
        {
            this.Id = id;
            this.DisplayText = text;
        }

        private void chkTrait_CheckedChanged(object sender, EventArgs e)
        {
            if (_isRaise && this.OnCheckChanged != null)
                this.OnCheckChanged(this, new EventArgs());

            // reset the flag
            _isRaise = true;
        }
    }
}
