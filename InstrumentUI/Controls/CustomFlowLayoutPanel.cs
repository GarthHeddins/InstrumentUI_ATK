using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InstrumentUI_ATK.Controls
{
    /// <summary>
    /// This control has ability to draw border with color specified by user
    /// </summary>
    public partial class CustomFlowLayoutPanel : FlowLayoutPanel
    {
        private Color _borderColor = Color.Black;
        private bool _hasColorBorder = false;

        /// <summary>
        /// Color of the border
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        /// <summary>
        /// true, if colored border is required. If true then it will set the BorderStyle to none
        /// </summary>
        public bool HasColorBorder
        {
            get { return _hasColorBorder; }
            set
            {
                // Either BorderStyle Or HasColorBorder can be set at one time 
                // becasue if BorderStyle is not None and HasBorderColor is true 
                // then it will create dual Border
                if (value)
                    this.BorderStyle = BorderStyle.None;
                _hasColorBorder = value;
            }
        }

        public CustomFlowLayoutPanel()
        {
            InitializeComponent();

            // Either BorderStyle Or HasColorBorder can be set at one time 
            // becasue if BorderStyle is not None and HasBorderColor is true 
            // then it will create dual Border
            if (this.HasColorBorder && this.BorderStyle != BorderStyle.None)
            {
                this.BorderStyle = BorderStyle.None;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (this.HasColorBorder)
            {
                ControlPaint.DrawBorder(pe.Graphics, this.DisplayRectangle, this.BorderColor, ButtonBorderStyle.Solid);
            }
        }
    }
}
