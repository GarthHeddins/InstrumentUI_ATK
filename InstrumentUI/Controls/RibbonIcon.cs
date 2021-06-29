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
    /// A custom button control which will have mouse hover images and text on it and
    /// acts as Ribbon Tab icon
    /// </summary>
    public partial class RibbonIcon : Button
    {
        private Image _activeImage;
        private Image _inActiveImage;
        private bool _selected = false;
        private bool _isHoverActive = true;
        private Color? _activeTextColor = null;
        private Color? _inActiveTextColor = null;

        /// <summary>
        /// Image displayed on mouse enter
        /// </summary>
        public Image ActiveImage 
        {
            get { return _activeImage; }
            set 
            {
                this._activeImage = value;
            } 
        }

        /// <summary>
        /// Image displayed on mouse leave
        /// </summary>
        public Image InActiveImage
        {
            get { return _inActiveImage; }
            set
            {
                this.Image = value;
                this._inActiveImage = value;
            }
        }

        public bool Selected 
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (value)
                {
                    this.Image = ActiveImage;
                    if (_inActiveTextColor.HasValue)
                    {
                        this.ForeColor = _activeTextColor.Value;
                    }
                }
                else
                {
                    this.Image = InActiveImage;
                    if (_inActiveTextColor.HasValue)
                    {
                        this.ForeColor = _inActiveTextColor.Value;
                    }
                }
            }
        }

        public bool IsHoverActive
        {
            get { return _isHoverActive; }
            set { _isHoverActive = value; }
        }

        public Color? InActiveTextColor
        {
            get { return _inActiveTextColor; }
            set { _inActiveTextColor = value; }
        }


        public RibbonIcon()
        {
            InitializeComponent();

            // set the appearence of the button
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.ImageAlign = ContentAlignment.TopCenter;
            this.TextAlign = ContentAlignment.BottomCenter;
            this.TextImageRelation = TextImageRelation.ImageAboveText;
            this.ForeColor = Color.FromArgb(29, 56, 141);
            this.Size = new Size(66, 56);
            this.Margin = new Padding(0, 0, 0, 0);
            this.ForeColor = Color.FromArgb(29, 56, 141);
            this._activeTextColor = this.ForeColor;
        }


        /// <summary>
        /// Mouse Enter event, change button image on mouse enter as specified by user
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (_isHoverActive)
            {
                if (_activeImage != null && !_selected)
                {
                    this.Image = _activeImage;
                }
            }
        }

        /// <summary>
        /// Mouse Leave Event, change button image on mouse leave as specified by user 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (_isHoverActive)
            {
                if (_inActiveImage != null && !_selected)
                {
                    this.Image = _inActiveImage;
                }
            }
        }
        
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
