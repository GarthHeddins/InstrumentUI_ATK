namespace InstrumentUI_ATK.FormControls
{
    partial class Analyze
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSelectMaterial = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.cbPresentation = new InstrumentUI_ATK.Controls.CustomComboBox();
            this.cbSubCategory = new InstrumentUI_ATK.Controls.CustomComboBox();
            this.cbCategory = new InstrumentUI_ATK.Controls.CustomComboBox();
            this.cbMaterial = new InstrumentUI_ATK.Controls.CustomComboBox();
            this.lblPresentation = new System.Windows.Forms.Label();
            this.lblSubCategory = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.pnlTraits = new System.Windows.Forms.Panel();
            this.flpTraits = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSelectAll = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.lblTraits = new System.Windows.Forms.Label();
            this.pnlIdentifySample = new System.Windows.Forms.Panel();
            this.flpIdentifySample = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlIdentifySampleTop = new System.Windows.Forms.Panel();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnInsertPrevious = new System.Windows.Forms.Button();
            this.chkCheckSample = new System.Windows.Forms.CheckBox();
            this.lblIdentifySample = new System.Windows.Forms.Label();
            this.btnAnalyze = new InstrumentUI_ATK.Controls.RibbonIcon();
            this.panel2.SuspendLayout();
            this.pnlTraits.SuspendLayout();
            this.pnlIdentifySample.SuspendLayout();
            this.flpIdentifySample.SuspendLayout();
            this.pnlIdentifySampleTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSelectMaterial
            // 
            this.lblSelectMaterial.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSelectMaterial.AutoSize = true;
            this.lblSelectMaterial.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSelectMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblSelectMaterial.Location = new System.Drawing.Point(-4, 16);
            this.lblSelectMaterial.Name = "lblSelectMaterial";
            this.lblSelectMaterial.Size = new System.Drawing.Size(129, 21);
            this.lblSelectMaterial.TabIndex = 1;
            this.lblSelectMaterial.Text = "Select Material";
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.lblMaterial);
            this.panel2.Controls.Add(this.cbPresentation);
            this.panel2.Controls.Add(this.cbSubCategory);
            this.panel2.Controls.Add(this.cbCategory);
            this.panel2.Controls.Add(this.cbMaterial);
            this.panel2.Controls.Add(this.lblPresentation);
            this.panel2.Controls.Add(this.lblSubCategory);
            this.panel2.Controls.Add(this.lblCategory);
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(500, 144);
            this.panel2.TabIndex = 0;
            // 
            // lblMaterial
            // 
            this.lblMaterial.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.BackColor = System.Drawing.Color.White;
            this.lblMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblMaterial.Location = new System.Drawing.Point(10, 16);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(101, 16);
            this.lblMaterial.TabIndex = 9;
            this.lblMaterial.Text = "Material Type";
            // 
            // cbPresentation
            // 
            this.cbPresentation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbPresentation.DisplayMember = "Name";
            this.cbPresentation.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbPresentation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPresentation.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbPresentation.ForeColor = System.Drawing.Color.Black;
            this.cbPresentation.FormattingEnabled = true;
            this.cbPresentation.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(236)))));
            this.cbPresentation.Location = new System.Drawing.Point(170, 105);
            this.cbPresentation.Name = "cbPresentation";
            this.cbPresentation.Size = new System.Drawing.Size(300, 25);
            this.cbPresentation.TabIndex = 3;
            this.cbPresentation.ValueMember = "Id";
            this.cbPresentation.SelectedIndexChanged += new System.EventHandler(this.cbPresentation_SelectedIndexChanged);
            this.cbPresentation.Enter += new System.EventHandler(this.cbPresentation_Enter);
            // 
            // cbSubCategory
            // 
            this.cbSubCategory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbSubCategory.DisplayMember = "Name";
            this.cbSubCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbSubCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubCategory.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbSubCategory.ForeColor = System.Drawing.Color.Black;
            this.cbSubCategory.FormattingEnabled = true;
            this.cbSubCategory.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(236)))));
            this.cbSubCategory.Location = new System.Drawing.Point(170, 75);
            this.cbSubCategory.Name = "cbSubCategory";
            this.cbSubCategory.Size = new System.Drawing.Size(300, 25);
            this.cbSubCategory.TabIndex = 2;
            this.cbSubCategory.ValueMember = "Id";
            this.cbSubCategory.SelectedIndexChanged += new System.EventHandler(this.cbSubCategory_SelectedIndexChanged);
            this.cbSubCategory.Enter += new System.EventHandler(this.cbSubCategory_Enter);
            // 
            // cbCategory
            // 
            this.cbCategory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbCategory.BackColor = System.Drawing.Color.White;
            this.cbCategory.DisplayMember = "Name";
            this.cbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbCategory.ForeColor = System.Drawing.Color.Black;
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(236)))));
            this.cbCategory.Location = new System.Drawing.Point(170, 44);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(300, 25);
            this.cbCategory.TabIndex = 1;
            this.cbCategory.ValueMember = "Id";
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.cbCategory_SelectedIndexChanged);
            this.cbCategory.Enter += new System.EventHandler(this.cbCategory_Enter);
            // 
            // cbMaterial
            // 
            this.cbMaterial.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMaterial.DisplayMember = "Name";
            this.cbMaterial.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterial.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbMaterial.ForeColor = System.Drawing.Color.Black;
            this.cbMaterial.FormattingEnabled = true;
            this.cbMaterial.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(236)))));
            this.cbMaterial.Location = new System.Drawing.Point(170, 13);
            this.cbMaterial.Name = "cbMaterial";
            this.cbMaterial.Size = new System.Drawing.Size(300, 25);
            this.cbMaterial.TabIndex = 0;
            this.cbMaterial.ValueMember = "Id";
            this.cbMaterial.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbMaterial_DrawItem);
            this.cbMaterial.SelectedIndexChanged += new System.EventHandler(this.cbMaterial_SelectedIndexChanged);
            this.cbMaterial.Enter += new System.EventHandler(this.cbMaterial_Enter);
            // 
            // lblPresentation
            // 
            this.lblPresentation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPresentation.AutoSize = true;
            this.lblPresentation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblPresentation.Location = new System.Drawing.Point(10, 108);
            this.lblPresentation.Name = "lblPresentation";
            this.lblPresentation.Size = new System.Drawing.Size(98, 16);
            this.lblPresentation.TabIndex = 4;
            this.lblPresentation.Text = "Presentation";
            // 
            // lblSubCategory
            // 
            this.lblSubCategory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSubCategory.AutoSize = true;
            this.lblSubCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblSubCategory.Location = new System.Drawing.Point(10, 78);
            this.lblSubCategory.Name = "lblSubCategory";
            this.lblSubCategory.Size = new System.Drawing.Size(98, 16);
            this.lblSubCategory.TabIndex = 3;
            this.lblSubCategory.Text = "SubCategory";
            // 
            // lblCategory
            // 
            this.lblCategory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCategory.AutoSize = true;
            this.lblCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblCategory.Location = new System.Drawing.Point(10, 47);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(71, 16);
            this.lblCategory.TabIndex = 1;
            this.lblCategory.Text = "Category";
            // 
            // pnlTraits
            // 
            this.pnlTraits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlTraits.Controls.Add(this.flpTraits);
            this.pnlTraits.Controls.Add(this.lblSelectAll);
            this.pnlTraits.Controls.Add(this.chkSelectAll);
            this.pnlTraits.Controls.Add(this.lblTraits);
            this.pnlTraits.Location = new System.Drawing.Point(525, 0);
            this.pnlTraits.Name = "pnlTraits";
            this.pnlTraits.Size = new System.Drawing.Size(250, 474);
            this.pnlTraits.TabIndex = 2;
            this.pnlTraits.Enter += new System.EventHandler(this.pnlTraits_Enter);
            // 
            // flpTraits
            // 
            this.flpTraits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flpTraits.AutoScroll = true;
            this.flpTraits.BackColor = System.Drawing.Color.White;
            this.flpTraits.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTraits.Location = new System.Drawing.Point(0, 40);
            this.flpTraits.Margin = new System.Windows.Forms.Padding(0);
            this.flpTraits.Name = "flpTraits";
            this.flpTraits.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.flpTraits.Size = new System.Drawing.Size(250, 427);
            this.flpTraits.TabIndex = 0;
            this.flpTraits.WrapContents = false;
            // 
            // lblSelectAll
            // 
            this.lblSelectAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSelectAll.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSelectAll.Location = new System.Drawing.Point(170, 2);
            this.lblSelectAll.Name = "lblSelectAll";
            this.lblSelectAll.Size = new System.Drawing.Size(59, 15);
            this.lblSelectAll.TabIndex = 3;
            this.lblSelectAll.Text = "Select All";
            this.lblSelectAll.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkSelectAll.Location = new System.Drawing.Point(235, 3);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(15, 14);
            this.chkSelectAll.TabIndex = 0;
            this.chkSelectAll.TabStop = false;
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // lblTraits
            // 
            this.lblTraits.AutoSize = true;
            this.lblTraits.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTraits.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblTraits.Location = new System.Drawing.Point(-4, 17);
            this.lblTraits.Name = "lblTraits";
            this.lblTraits.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTraits.Size = new System.Drawing.Size(55, 21);
            this.lblTraits.TabIndex = 1;
            this.lblTraits.Text = "Traits";
            // 
            // pnlIdentifySample
            // 
            this.pnlIdentifySample.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlIdentifySample.Controls.Add(this.flpIdentifySample);
            this.pnlIdentifySample.Controls.Add(this.chkCheckSample);
            this.pnlIdentifySample.Controls.Add(this.lblIdentifySample);
            this.pnlIdentifySample.Location = new System.Drawing.Point(0, 190);
            this.pnlIdentifySample.Name = "pnlIdentifySample";
            this.pnlIdentifySample.Size = new System.Drawing.Size(500, 284);
            this.pnlIdentifySample.TabIndex = 1;
            // 
            // flpIdentifySample
            // 
            this.flpIdentifySample.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flpIdentifySample.AutoScroll = true;
            this.flpIdentifySample.BackColor = System.Drawing.Color.White;
            this.flpIdentifySample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpIdentifySample.Controls.Add(this.pnlIdentifySampleTop);
            this.flpIdentifySample.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpIdentifySample.Location = new System.Drawing.Point(0, 27);
            this.flpIdentifySample.Name = "flpIdentifySample";
            this.flpIdentifySample.Size = new System.Drawing.Size(500, 250);
            this.flpIdentifySample.TabIndex = 4;
            this.flpIdentifySample.WrapContents = false;
            // 
            // pnlIdentifySampleTop
            // 
            this.pnlIdentifySampleTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlIdentifySampleTop.BackColor = System.Drawing.Color.White;
            this.pnlIdentifySampleTop.Controls.Add(this.btnClearAll);
            this.pnlIdentifySampleTop.Controls.Add(this.btnInsertPrevious);
            this.pnlIdentifySampleTop.Location = new System.Drawing.Point(3, 3);
            this.pnlIdentifySampleTop.Name = "pnlIdentifySampleTop";
            this.pnlIdentifySampleTop.Size = new System.Drawing.Size(459, 23);
            this.pnlIdentifySampleTop.TabIndex = 5;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearAll.FlatAppearance.BorderSize = 0;
            this.btnClearAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClearAll.Image = global::InstrumentUI_ATK.Properties.Resources.icon_negative;
            this.btnClearAll.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnClearAll.Location = new System.Drawing.Point(386, 2);
            this.btnClearAll.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(71, 20);
            this.btnClearAll.TabIndex = 3;
            this.btnClearAll.TabStop = false;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnClearAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnInsertPrevious
            // 
            this.btnInsertPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsertPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInsertPrevious.FlatAppearance.BorderSize = 0;
            this.btnInsertPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnInsertPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInsertPrevious.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnInsertPrevious.Image = global::InstrumentUI_ATK.Properties.Resources.icon_plus;
            this.btnInsertPrevious.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnInsertPrevious.Location = new System.Drawing.Point(278, 2);
            this.btnInsertPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.btnInsertPrevious.Name = "btnInsertPrevious";
            this.btnInsertPrevious.Size = new System.Drawing.Size(99, 20);
            this.btnInsertPrevious.TabIndex = 2;
            this.btnInsertPrevious.TabStop = false;
            this.btnInsertPrevious.Text = "Insert Previous";
            this.btnInsertPrevious.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnInsertPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInsertPrevious.UseVisualStyleBackColor = true;
            this.btnInsertPrevious.Click += new System.EventHandler(this.btnInsertPrevious_Click);
            // 
            // chkCheckSample
            // 
            this.chkCheckSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCheckSample.AutoSize = true;
            this.chkCheckSample.Enabled = false;
            this.chkCheckSample.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkCheckSample.Location = new System.Drawing.Point(340, 7);
            this.chkCheckSample.Name = "chkCheckSample";
            this.chkCheckSample.Size = new System.Drawing.Size(157, 19);
            this.chkCheckSample.TabIndex = 3;
            this.chkCheckSample.Text = "This is a Check Sample";
            this.chkCheckSample.UseVisualStyleBackColor = true;
            this.chkCheckSample.Visible = false;
            // 
            // lblIdentifySample
            // 
            this.lblIdentifySample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIdentifySample.AutoSize = true;
            this.lblIdentifySample.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblIdentifySample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblIdentifySample.Location = new System.Drawing.Point(-4, 3);
            this.lblIdentifySample.Name = "lblIdentifySample";
            this.lblIdentifySample.Size = new System.Drawing.Size(132, 21);
            this.lblIdentifySample.TabIndex = 1;
            this.lblIdentifySample.Text = "Identify Sample";
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.ActiveImage = global::InstrumentUI_ATK.Properties.Resources.button_analyze_active;
            this.btnAnalyze.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAnalyze.AutoSize = true;
            this.btnAnalyze.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(237)))));
            this.btnAnalyze.FlatAppearance.BorderSize = 0;
            this.btnAnalyze.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAnalyze.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAnalyze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyze.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnAnalyze.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnAnalyze.Image = global::InstrumentUI_ATK.Properties.Resources.button_analyze_inactive;
            this.btnAnalyze.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAnalyze.InActiveImage = global::InstrumentUI_ATK.Properties.Resources.button_analyze_inactive;
            this.btnAnalyze.InActiveTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnAnalyze.IsHoverActive = false;
            this.btnAnalyze.Location = new System.Drawing.Point(778, 375);
            this.btnAnalyze.Margin = new System.Windows.Forms.Padding(0);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Selected = false;
            this.btnAnalyze.Size = new System.Drawing.Size(92, 92);
            this.btnAnalyze.TabIndex = 3;
            this.btnAnalyze.TabStop = false;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAnalyze.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // Analyze
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblSelectMaterial);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.pnlIdentifySample);
            this.Controls.Add(this.pnlTraits);
            this.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.Name = "Analyze";
            this.Size = new System.Drawing.Size(900, 500);
            this.Load += new System.EventHandler(this.Analyze_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlTraits.ResumeLayout(false);
            this.pnlTraits.PerformLayout();
            this.pnlIdentifySample.ResumeLayout(false);
            this.pnlIdentifySample.PerformLayout();
            this.flpIdentifySample.ResumeLayout(false);
            this.pnlIdentifySampleTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSelectMaterial;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblPresentation;
        private System.Windows.Forms.Label lblSubCategory;
        private InstrumentUI_ATK.Controls.CustomComboBox cbPresentation;
        private InstrumentUI_ATK.Controls.CustomComboBox cbCategory;
        private InstrumentUI_ATK.Controls.CustomComboBox cbMaterial;
        private System.Windows.Forms.Panel pnlTraits;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Label lblTraits;
        private System.Windows.Forms.Label lblIdentifySample;
        private System.Windows.Forms.CheckBox chkCheckSample;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.FlowLayoutPanel flpIdentifySample;
        private System.Windows.Forms.Panel pnlIdentifySample;
        private InstrumentUI_ATK.Controls.CustomComboBox cbSubCategory;
        private InstrumentUI_ATK.Controls.RibbonIcon btnAnalyze;
        private System.Windows.Forms.FlowLayoutPanel flpTraits;
        private System.Windows.Forms.Label lblSelectAll;
        private System.Windows.Forms.Panel pnlIdentifySampleTop;
        private System.Windows.Forms.Button btnInsertPrevious;
        private System.Windows.Forms.Button btnClearAll;
    }
}
