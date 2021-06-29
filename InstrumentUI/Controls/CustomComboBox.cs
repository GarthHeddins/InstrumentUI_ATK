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
    /// A combobox with some customization required for this project
    /// </summary>
    public partial class CustomComboBox : ComboBox
    {
        private Color _hoverColor = Color.FromArgb(210, 222, 236);

        /// <summary>
        /// Color which appears when we take mouse over any item
        /// </summary>
        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }

        public CustomComboBox()
        {
            InitializeComponent();
            this.DrawMode = DrawMode.OwnerDrawVariable;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <summary>
        /// Change the Hover Color
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(this.HoverColor), e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
                }

                e.Graphics.DrawString(this.GetItemText(this.Items[e.Index]), e.Font, new SolidBrush(this.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }
        }
    }
}
