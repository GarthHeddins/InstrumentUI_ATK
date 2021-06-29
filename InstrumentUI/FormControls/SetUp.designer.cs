namespace InstrumentUI_ATK.FormControls
{
    partial class SetUp
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
            this.lblEmailOptionsTitle = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblDefaultReportDirTitle = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblAddLogo = new System.Windows.Forms.Label();
            this.lblMailTo = new System.Windows.Forms.Label();
            this.lblDefaultReportTitle = new System.Windows.Forms.Label();
            this.lblCustomizeTitle = new System.Windows.Forms.Label();
            this.txtLogo = new System.Windows.Forms.TextBox();
            this.chkAddAlgoVersion = new System.Windows.Forms.CheckBox();
            this.lblLine1 = new System.Windows.Forms.Label();
            this.lblLine2 = new System.Windows.Forms.Label();
            this.lblLine3 = new System.Windows.Forms.Label();
            this.lblLine4 = new System.Windows.Forms.Label();
            this.openFileDialogForLogo = new System.Windows.Forms.OpenFileDialog();
            this.txtReportAddress1 = new System.Windows.Forms.TextBox();
            this.txtMailTo = new System.Windows.Forms.TextBox();
            this.txtReportAddress2 = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.txtReportDirectory = new System.Windows.Forms.TextBox();
            this.btnReportDirectory = new System.Windows.Forms.Button();
            this.folderBrowserDialogForReport = new System.Windows.Forms.FolderBrowserDialog();
            this.cbDefaultReport = new InstrumentUI_ATK.Controls.CustomComboBox();
            this.SuspendLayout();
            // 
            // lblEmailOptionsTitle
            // 
            this.lblEmailOptionsTitle.AutoSize = true;
            this.lblEmailOptionsTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblEmailOptionsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblEmailOptionsTitle.Location = new System.Drawing.Point(19, 322);
            this.lblEmailOptionsTitle.Name = "lblEmailOptionsTitle";
            this.lblEmailOptionsTitle.Size = new System.Drawing.Size(104, 16);
            this.lblEmailOptionsTitle.TabIndex = 0;
            this.lblEmailOptionsTitle.Text = "Email Options";
            this.lblEmailOptionsTitle.Click += new System.EventHandler(this.control_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(308, 387);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(89, 29);
            this.btnOk.TabIndex = 10;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblDefaultReportDirTitle
            // 
            this.lblDefaultReportDirTitle.AutoSize = true;
            this.lblDefaultReportDirTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDefaultReportDirTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDefaultReportDirTitle.Location = new System.Drawing.Point(19, 175);
            this.lblDefaultReportDirTitle.Name = "lblDefaultReportDirTitle";
            this.lblDefaultReportDirTitle.Size = new System.Drawing.Size(178, 16);
            this.lblDefaultReportDirTitle.TabIndex = 15;
            this.lblDefaultReportDirTitle.Text = "Default Report Directory";
            this.lblDefaultReportDirTitle.Click += new System.EventHandler(this.control_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.btnBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(308, 44);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(89, 29);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblAddLogo
            // 
            this.lblAddLogo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblAddLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblAddLogo.Location = new System.Drawing.Point(3, 51);
            this.lblAddLogo.Name = "lblAddLogo";
            this.lblAddLogo.Size = new System.Drawing.Size(100, 15);
            this.lblAddLogo.TabIndex = 20;
            this.lblAddLogo.Text = "Add Logo:";
            this.lblAddLogo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAddLogo.Click += new System.EventHandler(this.control_Click);
            // 
            // lblMailTo
            // 
            this.lblMailTo.AutoSize = true;
            this.lblMailTo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMailTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblMailTo.Location = new System.Drawing.Point(37, 350);
            this.lblMailTo.Name = "lblMailTo";
            this.lblMailTo.Size = new System.Drawing.Size(45, 15);
            this.lblMailTo.TabIndex = 22;
            this.lblMailTo.Text = "Mail to:";
            this.lblMailTo.Click += new System.EventHandler(this.control_Click);
            // 
            // lblDefaultReportTitle
            // 
            this.lblDefaultReportTitle.AutoSize = true;
            this.lblDefaultReportTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDefaultReportTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDefaultReportTitle.Location = new System.Drawing.Point(19, 249);
            this.lblDefaultReportTitle.Name = "lblDefaultReportTitle";
            this.lblDefaultReportTitle.Size = new System.Drawing.Size(110, 16);
            this.lblDefaultReportTitle.TabIndex = 25;
            this.lblDefaultReportTitle.Text = "Default Report";
            this.lblDefaultReportTitle.Click += new System.EventHandler(this.control_Click);
            // 
            // lblCustomizeTitle
            // 
            this.lblCustomizeTitle.AutoSize = true;
            this.lblCustomizeTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblCustomizeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblCustomizeTitle.Location = new System.Drawing.Point(19, 23);
            this.lblCustomizeTitle.Name = "lblCustomizeTitle";
            this.lblCustomizeTitle.Size = new System.Drawing.Size(81, 16);
            this.lblCustomizeTitle.TabIndex = 27;
            this.lblCustomizeTitle.Text = "Customize";
            this.lblCustomizeTitle.Click += new System.EventHandler(this.control_Click);
            // 
            // txtLogo
            // 
            this.txtLogo.BackColor = System.Drawing.Color.White;
            this.txtLogo.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.txtLogo.Location = new System.Drawing.Point(103, 48);
            this.txtLogo.Name = "txtLogo";
            this.txtLogo.Size = new System.Drawing.Size(197, 22);
            this.txtLogo.TabIndex = 1;
            this.txtLogo.Click += new System.EventHandler(this.control_Click);
            // 
            // chkAddAlgoVersion
            // 
            this.chkAddAlgoVersion.AutoSize = true;
            this.chkAddAlgoVersion.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkAddAlgoVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkAddAlgoVersion.Location = new System.Drawing.Point(105, 136);
            this.chkAddAlgoVersion.Name = "chkAddAlgoVersion";
            this.chkAddAlgoVersion.Size = new System.Drawing.Size(200, 19);
            this.chkAddAlgoVersion.TabIndex = 5;
            this.chkAddAlgoVersion.Text = "Add Algorithm version to reports";
            this.chkAddAlgoVersion.UseVisualStyleBackColor = true;
            this.chkAddAlgoVersion.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLine1
            // 
            this.lblLine1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLine1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLine1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLine1.Location = new System.Drawing.Point(99, 31);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(296, 1);
            this.lblLine1.TabIndex = 30;
            this.lblLine1.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLine2
            // 
            this.lblLine2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLine2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLine2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLine2.Location = new System.Drawing.Point(196, 184);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(201, 1);
            this.lblLine2.TabIndex = 31;
            this.lblLine2.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLine3
            // 
            this.lblLine3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLine3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLine3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLine3.Location = new System.Drawing.Point(130, 258);
            this.lblLine3.Name = "lblLine3";
            this.lblLine3.Size = new System.Drawing.Size(267, 1);
            this.lblLine3.TabIndex = 32;
            this.lblLine3.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLine4
            // 
            this.lblLine4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLine4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLine4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLine4.Location = new System.Drawing.Point(122, 331);
            this.lblLine4.Name = "lblLine4";
            this.lblLine4.Size = new System.Drawing.Size(275, 1);
            this.lblLine4.TabIndex = 34;
            this.lblLine4.Click += new System.EventHandler(this.control_Click);
            // 
            // openFileDialogForLogo
            // 
            this.openFileDialogForLogo.Filter = "Bitmap|*.bmp|GIF|*.gif|JPG|*.jpg|JPEG|*.jpeg";
            // 
            // txtReportAddress1
            // 
            this.txtReportAddress1.BackColor = System.Drawing.Color.White;
            this.txtReportAddress1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportAddress1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.txtReportAddress1.Location = new System.Drawing.Point(103, 79);
            this.txtReportAddress1.Name = "txtReportAddress1";
            this.txtReportAddress1.Size = new System.Drawing.Size(294, 22);
            this.txtReportAddress1.TabIndex = 3;
            this.txtReportAddress1.Click += new System.EventHandler(this.control_Click);
            // 
            // txtMailTo
            // 
            this.txtMailTo.BackColor = System.Drawing.Color.White;
            this.txtMailTo.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMailTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.txtMailTo.Location = new System.Drawing.Point(86, 347);
            this.txtMailTo.Name = "txtMailTo";
            this.txtMailTo.Size = new System.Drawing.Size(311, 22);
            this.txtMailTo.TabIndex = 9;
            this.txtMailTo.Click += new System.EventHandler(this.control_Click);
            // 
            // txtReportAddress2
            // 
            this.txtReportAddress2.BackColor = System.Drawing.Color.White;
            this.txtReportAddress2.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportAddress2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.txtReportAddress2.Location = new System.Drawing.Point(103, 108);
            this.txtReportAddress2.Name = "txtReportAddress2";
            this.txtReportAddress2.Size = new System.Drawing.Size(294, 22);
            this.txtReportAddress2.TabIndex = 4;
            this.txtReportAddress2.Click += new System.EventHandler(this.control_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.White;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblMessage.ForeColor = System.Drawing.Color.Green;
            this.lblMessage.Location = new System.Drawing.Point(3, 5);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(94, 15);
            this.lblMessage.TabIndex = 38;
            this.lblMessage.Text = "Changes saved";
            this.lblMessage.Visible = false;
            // 
            // lblAddress1
            // 
            this.lblAddress1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblAddress1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblAddress1.Location = new System.Drawing.Point(17, 83);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(86, 15);
            this.lblAddress1.TabIndex = 39;
            this.lblAddress1.Text = "Address 1:";
            this.lblAddress1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAddress2
            // 
            this.lblAddress2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblAddress2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblAddress2.Location = new System.Drawing.Point(17, 112);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(86, 15);
            this.lblAddress2.TabIndex = 40;
            this.lblAddress2.Text = "Address 2:";
            this.lblAddress2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtReportDirectory
            // 
            this.txtReportDirectory.BackColor = System.Drawing.Color.White;
            this.txtReportDirectory.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportDirectory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.txtReportDirectory.Location = new System.Drawing.Point(37, 203);
            this.txtReportDirectory.Name = "txtReportDirectory";
            this.txtReportDirectory.ReadOnly = true;
            this.txtReportDirectory.Size = new System.Drawing.Size(259, 22);
            this.txtReportDirectory.TabIndex = 6;
            // 
            // btnReportDirectory
            // 
            this.btnReportDirectory.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.btnReportDirectory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnReportDirectory.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnReportDirectory.FlatAppearance.BorderSize = 0;
            this.btnReportDirectory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReportDirectory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReportDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportDirectory.Location = new System.Drawing.Point(308, 200);
            this.btnReportDirectory.Name = "btnReportDirectory";
            this.btnReportDirectory.Size = new System.Drawing.Size(89, 29);
            this.btnReportDirectory.TabIndex = 7;
            this.btnReportDirectory.UseVisualStyleBackColor = true;
            this.btnReportDirectory.Click += new System.EventHandler(this.btnReportDirectory_Click);
            // 
            // cbDefaultReport
            // 
            this.cbDefaultReport.BackColor = System.Drawing.Color.White;
            this.cbDefaultReport.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbDefaultReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultReport.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbDefaultReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.cbDefaultReport.FormattingEnabled = true;
            this.cbDefaultReport.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(236)))));
            this.cbDefaultReport.Location = new System.Drawing.Point(37, 275);
            this.cbDefaultReport.Name = "cbDefaultReport";
            this.cbDefaultReport.Size = new System.Drawing.Size(360, 23);
            this.cbDefaultReport.TabIndex = 8;
            this.cbDefaultReport.Click += new System.EventHandler(this.control_Click);
            // 
            // SetUp
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.admin_setup_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btnReportDirectory);
            this.Controls.Add(this.txtReportDirectory);
            this.Controls.Add(this.lblAddress2);
            this.Controls.Add(this.lblAddress1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtReportAddress2);
            this.Controls.Add(this.txtMailTo);
            this.Controls.Add(this.txtReportAddress1);
            this.Controls.Add(this.lblLine4);
            this.Controls.Add(this.lblLine3);
            this.Controls.Add(this.lblLine2);
            this.Controls.Add(this.lblLine1);
            this.Controls.Add(this.chkAddAlgoVersion);
            this.Controls.Add(this.txtLogo);
            this.Controls.Add(this.lblCustomizeTitle);
            this.Controls.Add(this.cbDefaultReport);
            this.Controls.Add(this.lblDefaultReportTitle);
            this.Controls.Add(this.lblMailTo);
            this.Controls.Add(this.lblAddLogo);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblDefaultReportDirTitle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblEmailOptionsTitle);
            this.Name = "SetUp";
            this.Size = new System.Drawing.Size(424, 433);
            this.Load += new System.EventHandler(this.SetUp_Load);
            this.Click += new System.EventHandler(this.control_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEmailOptionsTitle;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblDefaultReportDirTitle;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblAddLogo;
        private System.Windows.Forms.Label lblMailTo;
        private InstrumentUI_ATK.Controls.CustomComboBox cbDefaultReport;
        private System.Windows.Forms.Label lblDefaultReportTitle;
        private System.Windows.Forms.Label lblCustomizeTitle;
        private System.Windows.Forms.TextBox txtLogo;
        private System.Windows.Forms.CheckBox chkAddAlgoVersion;
        private System.Windows.Forms.Label lblLine2;
        private System.Windows.Forms.Label lblLine3;
        private System.Windows.Forms.Label lblLine4;
        private System.Windows.Forms.OpenFileDialog openFileDialogForLogo;
        private System.Windows.Forms.TextBox txtReportAddress1;
        private System.Windows.Forms.TextBox txtMailTo;
        private System.Windows.Forms.TextBox txtReportAddress2;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.TextBox txtReportDirectory;
        private System.Windows.Forms.Button btnReportDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogForReport;
        private System.Windows.Forms.Label lblLine1;
    }
}
