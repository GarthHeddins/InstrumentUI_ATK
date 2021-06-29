namespace InstrumentUI_ATK.FormControls
{
    partial class Preferences
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
            this.lblChangePassTitle = new System.Windows.Forms.Label();
            this.chkAutoPrint = new System.Windows.Forms.CheckBox();
            this.lblClick = new System.Windows.Forms.Label();
            this.lnkHere = new System.Windows.Forms.LinkLabel();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblChangePassText = new System.Windows.Forms.Label();
            this.chkRememberUsername = new System.Windows.Forms.CheckBox();
            this.chkAutoSampleId = new System.Windows.Forms.CheckBox();
            this.chkSoundOn = new System.Windows.Forms.CheckBox();
            this.lblOptionsTitle = new System.Windows.Forms.Label();
            this.lblLanguageTitle = new System.Windows.Forms.Label();
            this.lblLineTop = new System.Windows.Forms.Label();
            this.lblLineMiddle = new System.Windows.Forms.Label();
            this.lblLineBottom = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.chkSpeedMode = new System.Windows.Forms.CheckBox();
            this.chkSpeedModeDualScan = new System.Windows.Forms.CheckBox();
            this.cbLanguage = new InstrumentUI_ATK.Controls.CustomComboBox();
            this.SuspendLayout();
            // 
            // lblChangePassTitle
            // 
            this.lblChangePassTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblChangePassTitle.AutoSize = true;
            this.lblChangePassTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblChangePassTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblChangePassTitle.Location = new System.Drawing.Point(19, 223);
            this.lblChangePassTitle.Name = "lblChangePassTitle";
            this.lblChangePassTitle.Size = new System.Drawing.Size(134, 16);
            this.lblChangePassTitle.TabIndex = 0;
            this.lblChangePassTitle.Text = "Change Password";
            this.lblChangePassTitle.Click += new System.EventHandler(this.control_Click);
            // 
            // chkAutoPrint
            // 
            this.chkAutoPrint.AutoSize = true;
            this.chkAutoPrint.BackColor = System.Drawing.Color.White;
            this.chkAutoPrint.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkAutoPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkAutoPrint.Location = new System.Drawing.Point(26, 158);
            this.chkAutoPrint.Name = "chkAutoPrint";
            this.chkAutoPrint.Size = new System.Drawing.Size(85, 20);
            this.chkAutoPrint.TabIndex = 4;
            this.chkAutoPrint.Text = "Auto Print";
            this.chkAutoPrint.UseVisualStyleBackColor = false;
            this.chkAutoPrint.Click += new System.EventHandler(this.control_Click);
            // 
            // lblClick
            // 
            this.lblClick.AutoSize = true;
            this.lblClick.BackColor = System.Drawing.Color.White;
            this.lblClick.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClick.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblClick.Location = new System.Drawing.Point(41, 246);
            this.lblClick.Name = "lblClick";
            this.lblClick.Size = new System.Drawing.Size(37, 16);
            this.lblClick.TabIndex = 5;
            this.lblClick.Text = "Click";
            this.lblClick.Click += new System.EventHandler(this.control_Click);
            // 
            // lnkHere
            // 
            this.lnkHere.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnkHere.AutoSize = true;
            this.lnkHere.BackColor = System.Drawing.Color.White;
            this.lnkHere.DisabledLinkColor = System.Drawing.Color.Blue;
            this.lnkHere.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lnkHere.Location = new System.Drawing.Point(72, 246);
            this.lnkHere.Name = "lnkHere";
            this.lnkHere.Size = new System.Drawing.Size(33, 16);
            this.lnkHere.TabIndex = 6;
            this.lnkHere.TabStop = true;
            this.lnkHere.Text = "here";
            this.lnkHere.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkHere.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHere_LinkClicked);
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
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.btnOk.Location = new System.Drawing.Point(306, 293);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(89, 29);
            this.btnOk.TabIndex = 7;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblChangePassText
            // 
            this.lblChangePassText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblChangePassText.AutoSize = true;
            this.lblChangePassText.BackColor = System.Drawing.Color.White;
            this.lblChangePassText.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblChangePassText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblChangePassText.Location = new System.Drawing.Point(101, 246);
            this.lblChangePassText.Name = "lblChangePassText";
            this.lblChangePassText.Size = new System.Drawing.Size(157, 16);
            this.lblChangePassText.TabIndex = 9;
            this.lblChangePassText.Text = "to change your password.";
            this.lblChangePassText.Click += new System.EventHandler(this.control_Click);
            // 
            // chkRememberUsername
            // 
            this.chkRememberUsername.AutoSize = true;
            this.chkRememberUsername.BackColor = System.Drawing.Color.White;
            this.chkRememberUsername.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkRememberUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkRememberUsername.Location = new System.Drawing.Point(205, 158);
            this.chkRememberUsername.Name = "chkRememberUsername";
            this.chkRememberUsername.Size = new System.Drawing.Size(153, 20);
            this.chkRememberUsername.TabIndex = 5;
            this.chkRememberUsername.Text = "Remember Username";
            this.chkRememberUsername.UseVisualStyleBackColor = false;
            this.chkRememberUsername.Click += new System.EventHandler(this.control_Click);
            // 
            // chkAutoSampleId
            // 
            this.chkAutoSampleId.AutoSize = true;
            this.chkAutoSampleId.BackColor = System.Drawing.Color.White;
            this.chkAutoSampleId.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkAutoSampleId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkAutoSampleId.Location = new System.Drawing.Point(205, 130);
            this.chkAutoSampleId.Name = "chkAutoSampleId";
            this.chkAutoSampleId.Size = new System.Drawing.Size(179, 20);
            this.chkAutoSampleId.TabIndex = 3;
            this.chkAutoSampleId.Text = "Autogenerate a Sample ID";
            this.chkAutoSampleId.UseVisualStyleBackColor = false;
            this.chkAutoSampleId.Click += new System.EventHandler(this.control_Click);
            // 
            // chkSoundOn
            // 
            this.chkSoundOn.AutoSize = true;
            this.chkSoundOn.BackColor = System.Drawing.Color.White;
            this.chkSoundOn.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkSoundOn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkSoundOn.Location = new System.Drawing.Point(26, 130);
            this.chkSoundOn.Name = "chkSoundOn";
            this.chkSoundOn.Size = new System.Drawing.Size(85, 20);
            this.chkSoundOn.TabIndex = 2;
            this.chkSoundOn.Text = "Sound On";
            this.chkSoundOn.UseVisualStyleBackColor = false;
            this.chkSoundOn.Click += new System.EventHandler(this.control_Click);
            // 
            // lblOptionsTitle
            // 
            this.lblOptionsTitle.AutoSize = true;
            this.lblOptionsTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblOptionsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblOptionsTitle.Location = new System.Drawing.Point(19, 108);
            this.lblOptionsTitle.Name = "lblOptionsTitle";
            this.lblOptionsTitle.Size = new System.Drawing.Size(63, 16);
            this.lblOptionsTitle.TabIndex = 14;
            this.lblOptionsTitle.Text = "Options";
            this.lblOptionsTitle.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLanguageTitle
            // 
            this.lblLanguageTitle.AutoSize = true;
            this.lblLanguageTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblLanguageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblLanguageTitle.Location = new System.Drawing.Point(19, 25);
            this.lblLanguageTitle.Name = "lblLanguageTitle";
            this.lblLanguageTitle.Size = new System.Drawing.Size(77, 16);
            this.lblLanguageTitle.TabIndex = 15;
            this.lblLanguageTitle.Text = "Language";
            this.lblLanguageTitle.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLineTop
            // 
            this.lblLineTop.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLineTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLineTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLineTop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLineTop.Location = new System.Drawing.Point(97, 33);
            this.lblLineTop.Name = "lblLineTop";
            this.lblLineTop.Size = new System.Drawing.Size(313, 1);
            this.lblLineTop.TabIndex = 17;
            this.lblLineTop.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLineMiddle
            // 
            this.lblLineMiddle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLineMiddle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLineMiddle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLineMiddle.Location = new System.Drawing.Point(85, 116);
            this.lblLineMiddle.Name = "lblLineMiddle";
            this.lblLineMiddle.Size = new System.Drawing.Size(325, 1);
            this.lblLineMiddle.TabIndex = 18;
            this.lblLineMiddle.Click += new System.EventHandler(this.control_Click);
            // 
            // lblLineBottom
            // 
            this.lblLineBottom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLineBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLineBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLineBottom.Location = new System.Drawing.Point(165, 231);
            this.lblLineBottom.Name = "lblLineBottom";
            this.lblLineBottom.Size = new System.Drawing.Size(255, 1);
            this.lblLineBottom.TabIndex = 19;
            this.lblLineBottom.Click += new System.EventHandler(this.control_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.White;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblMessage.ForeColor = System.Drawing.Color.Green;
            this.lblMessage.Location = new System.Drawing.Point(3, 4);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(94, 15);
            this.lblMessage.TabIndex = 39;
            this.lblMessage.Text = "Changes saved";
            this.lblMessage.Visible = false;
            // 
            // chkSpeedMode
            // 
            this.chkSpeedMode.AutoSize = true;
            this.chkSpeedMode.BackColor = System.Drawing.Color.White;
            this.chkSpeedMode.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkSpeedMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkSpeedMode.Location = new System.Drawing.Point(26, 184);
            this.chkSpeedMode.Name = "chkSpeedMode";
            this.chkSpeedMode.Size = new System.Drawing.Size(100, 20);
            this.chkSpeedMode.TabIndex = 40;
            this.chkSpeedMode.Text = "Speed Mode";
            this.chkSpeedMode.UseVisualStyleBackColor = false;
            this.chkSpeedMode.Click += new System.EventHandler(this.control_Click);
            this.chkSpeedMode.CheckedChanged += new System.EventHandler(this.chkSpeedMode_CheckedChanged);
            // 
            // chkSpeedModeDualScan
            // 
            this.chkSpeedModeDualScan.AutoSize = true;
            this.chkSpeedModeDualScan.BackColor = System.Drawing.Color.White;
            this.chkSpeedModeDualScan.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkSpeedModeDualScan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkSpeedModeDualScan.Location = new System.Drawing.Point(205, 184);
            this.chkSpeedModeDualScan.Name = "chkSpeedModeDualScan";
            this.chkSpeedModeDualScan.Size = new System.Drawing.Size(215, 20);
            this.chkSpeedModeDualScan.TabIndex = 41;
            this.chkSpeedModeDualScan.Text = "Speed Mode with Dual Scanning";
            this.chkSpeedModeDualScan.UseVisualStyleBackColor = false;
            this.chkSpeedModeDualScan.Click += new System.EventHandler(this.control_Click);
            // 
            // cbLanguage
            // 
            this.cbLanguage.BackColor = System.Drawing.Color.White;
            this.cbLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(236)))));
            this.cbLanguage.Location = new System.Drawing.Point(26, 50);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(320, 23);
            this.cbLanguage.TabIndex = 1;
            // 
            // Preferences
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.admin_preferences_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.chkSpeedModeDualScan);
            this.Controls.Add(this.chkSpeedMode);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblLineBottom);
            this.Controls.Add(this.lblLineMiddle);
            this.Controls.Add(this.lblLineTop);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.lblLanguageTitle);
            this.Controls.Add(this.lblOptionsTitle);
            this.Controls.Add(this.chkAutoSampleId);
            this.Controls.Add(this.chkSoundOn);
            this.Controls.Add(this.chkRememberUsername);
            this.Controls.Add(this.lblChangePassText);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lnkHere);
            this.Controls.Add(this.lblClick);
            this.Controls.Add(this.chkAutoPrint);
            this.Controls.Add(this.lblChangePassTitle);
            this.Name = "Preferences";
            this.Size = new System.Drawing.Size(442, 360);
            this.Load += new System.EventHandler(this.Preferences_Load);
            this.Click += new System.EventHandler(this.control_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChangePassTitle;
        private System.Windows.Forms.CheckBox chkAutoPrint;
        private System.Windows.Forms.Label lblClick;
        private System.Windows.Forms.LinkLabel lnkHere;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblChangePassText;
        private System.Windows.Forms.CheckBox chkRememberUsername;
        private System.Windows.Forms.CheckBox chkAutoSampleId;
        private System.Windows.Forms.CheckBox chkSoundOn;
        private System.Windows.Forms.Label lblOptionsTitle;
        private System.Windows.Forms.Label lblLanguageTitle;
        private InstrumentUI_ATK.Controls.CustomComboBox cbLanguage;
        private System.Windows.Forms.Label lblLineMiddle;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblLineTop;
        private System.Windows.Forms.Label lblLineBottom;
        private System.Windows.Forms.CheckBox chkSpeedMode;
        private System.Windows.Forms.CheckBox chkSpeedModeDualScan;
    }
}
