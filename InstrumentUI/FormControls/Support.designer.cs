namespace InstrumentUI_ATK.FormControls
{
    partial class Support
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
            this.lblDiagnosticsTitle = new System.Windows.Forms.Label();
            this.lnkHere = new System.Windows.Forms.LinkLabel();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblVersionTitle = new System.Windows.Forms.Label();
            this.lblVersionText = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblContactNo = new System.Windows.Forms.Label();
            this.lblContactEmail = new System.Windows.Forms.Label();
            this.lblLine2 = new System.Windows.Forms.Label();
            this.lblLine4 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblLine1 = new System.Windows.Forms.Label();
            this.lblContactInfoTitle = new System.Windows.Forms.Label();
            this.lblDiagnosticsText2 = new System.Windows.Forms.Label();
            this.lblClick = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRepairDB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDiagnosticsTitle
            // 
            this.lblDiagnosticsTitle.AutoSize = true;
            this.lblDiagnosticsTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDiagnosticsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDiagnosticsTitle.Location = new System.Drawing.Point(19, 152);
            this.lblDiagnosticsTitle.Name = "lblDiagnosticsTitle";
            this.lblDiagnosticsTitle.Size = new System.Drawing.Size(170, 16);
            this.lblDiagnosticsTitle.TabIndex = 0;
            this.lblDiagnosticsTitle.Text = "Instrument Diagnostics";
            // 
            // lnkHere
            // 
            this.lnkHere.AutoSize = true;
            this.lnkHere.BackColor = System.Drawing.Color.White;
            this.lnkHere.DisabledLinkColor = System.Drawing.Color.Blue;
            this.lnkHere.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lnkHere.LinkColor = System.Drawing.Color.Blue;
            this.lnkHere.Location = new System.Drawing.Point(71, 177);
            this.lnkHere.Name = "lnkHere";
            this.lnkHere.Size = new System.Drawing.Size(33, 16);
            this.lnkHere.TabIndex = 2;
            this.lnkHere.TabStop = true;
            this.lnkHere.Text = "here";
            this.lnkHere.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkHere.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDiagnostics_LinkClicked);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(316, 302);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(89, 29);
            this.btnOk.TabIndex = 3;
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblVersionTitle
            // 
            this.lblVersionTitle.AutoSize = true;
            this.lblVersionTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblVersionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblVersionTitle.Location = new System.Drawing.Point(19, 88);
            this.lblVersionTitle.Name = "lblVersionTitle";
            this.lblVersionTitle.Size = new System.Drawing.Size(62, 16);
            this.lblVersionTitle.TabIndex = 14;
            this.lblVersionTitle.Text = "Version";
            // 
            // lblVersionText
            // 
            this.lblVersionText.AutoSize = true;
            this.lblVersionText.BackColor = System.Drawing.Color.White;
            this.lblVersionText.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVersionText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblVersionText.Location = new System.Drawing.Point(38, 113);
            this.lblVersionText.Name = "lblVersionText";
            this.lblVersionText.Size = new System.Drawing.Size(134, 16);
            this.lblVersionText.TabIndex = 18;
            this.lblVersionText.Text = "This version of QTA is";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.White;
            this.lblVersion.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblVersion.Location = new System.Drawing.Point(168, 113);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(65, 16);
            this.lblVersion.TabIndex = 19;
            this.lblVersion.Text = "(Info here)";
            // 
            // lblContactNo
            // 
            this.lblContactNo.AutoSize = true;
            this.lblContactNo.BackColor = System.Drawing.Color.White;
            this.lblContactNo.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblContactNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblContactNo.Location = new System.Drawing.Point(41, 54);
            this.lblContactNo.Name = "lblContactNo";
            this.lblContactNo.Size = new System.Drawing.Size(86, 16);
            this.lblContactNo.TabIndex = 20;
            this.lblContactNo.Text = "800-xxx-xxxx";
            // 
            // lblContactEmail
            // 
            this.lblContactEmail.AutoSize = true;
            this.lblContactEmail.BackColor = System.Drawing.Color.White;
            this.lblContactEmail.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblContactEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblContactEmail.Location = new System.Drawing.Point(197, 54);
            this.lblContactEmail.Name = "lblContactEmail";
            this.lblContactEmail.Size = new System.Drawing.Size(139, 16);
            this.lblContactEmail.TabIndex = 21;
            this.lblContactEmail.Text = "emailaddresshere.com";
            // 
            // lblLine2
            // 
            this.lblLine2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLine2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLine2.Location = new System.Drawing.Point(81, 96);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(314, 1);
            this.lblLine2.TabIndex = 22;
            // 
            // lblLine4
            // 
            this.lblLine4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLine4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLine4.Location = new System.Drawing.Point(190, 160);
            this.lblLine4.Name = "lblLine4";
            this.lblLine4.Size = new System.Drawing.Size(206, 1);
            this.lblLine4.TabIndex = 24;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.White;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblMessage.ForeColor = System.Drawing.Color.Green;
            this.lblMessage.Location = new System.Drawing.Point(3, 6);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(67, 15);
            this.lblMessage.TabIndex = 25;
            this.lblMessage.Text = "[Message]";
            this.lblMessage.Visible = false;
            // 
            // lblLine1
            // 
            this.lblLine1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.lblLine1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLine1.Location = new System.Drawing.Point(165, 39);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(231, 1);
            this.lblLine1.TabIndex = 27;
            // 
            // lblContactInfoTitle
            // 
            this.lblContactInfoTitle.AutoSize = true;
            this.lblContactInfoTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblContactInfoTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblContactInfoTitle.Location = new System.Drawing.Point(19, 31);
            this.lblContactInfoTitle.Name = "lblContactInfoTitle";
            this.lblContactInfoTitle.Size = new System.Drawing.Size(146, 16);
            this.lblContactInfoTitle.TabIndex = 26;
            this.lblContactInfoTitle.Text = "Contact Information";
            // 
            // lblDiagnosticsText2
            // 
            this.lblDiagnosticsText2.AutoSize = true;
            this.lblDiagnosticsText2.BackColor = System.Drawing.Color.White;
            this.lblDiagnosticsText2.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDiagnosticsText2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDiagnosticsText2.Location = new System.Drawing.Point(102, 177);
            this.lblDiagnosticsText2.Name = "lblDiagnosticsText2";
            this.lblDiagnosticsText2.Size = new System.Drawing.Size(204, 16);
            this.lblDiagnosticsText2.TabIndex = 28;
            this.lblDiagnosticsText2.Text = " to access instrument diagnostics";
            // 
            // lblClick
            // 
            this.lblClick.AutoSize = true;
            this.lblClick.BackColor = System.Drawing.Color.White;
            this.lblClick.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClick.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblClick.Location = new System.Drawing.Point(38, 177);
            this.lblClick.Name = "lblClick";
            this.lblClick.Size = new System.Drawing.Size(37, 16);
            this.lblClick.TabIndex = 29;
            this.lblClick.Text = "Click";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(19, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "Repair Database";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(140, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 1);
            this.label1.TabIndex = 33;
            // 
            // btnRepairDB
            // 
            this.btnRepairDB.BackColor = System.Drawing.Color.Transparent;
            this.btnRepairDB.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.btnRepairDB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRepairDB.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRepairDB.FlatAppearance.BorderSize = 0;
            this.btnRepairDB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRepairDB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRepairDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRepairDB.Location = new System.Drawing.Point(165, 239);
            this.btnRepairDB.Name = "btnRepairDB";
            this.btnRepairDB.Size = new System.Drawing.Size(89, 29);
            this.btnRepairDB.TabIndex = 34;
            this.btnRepairDB.Text = "Repair DB";
            this.btnRepairDB.UseVisualStyleBackColor = false;
            this.btnRepairDB.Click += new System.EventHandler(this.btnRepairDB_Click);
            // 
            // Support
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.admin_support_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btnRepairDB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblClick);
            this.Controls.Add(this.lblDiagnosticsText2);
            this.Controls.Add(this.lblLine1);
            this.Controls.Add(this.lblContactInfoTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblLine4);
            this.Controls.Add(this.lblLine2);
            this.Controls.Add(this.lblContactEmail);
            this.Controls.Add(this.lblContactNo);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblVersionText);
            this.Controls.Add(this.lblVersionTitle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lnkHere);
            this.Controls.Add(this.lblDiagnosticsTitle);
            this.Location = new System.Drawing.Point(295, 88);
            this.Name = "Support";
            this.Size = new System.Drawing.Size(424, 347);
            this.Load += new System.EventHandler(this.Support_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDiagnosticsTitle;
        private System.Windows.Forms.LinkLabel lnkHere;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblVersionTitle;
        private System.Windows.Forms.Label lblVersionText;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblContactNo;
        private System.Windows.Forms.Label lblContactEmail;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblLine1;
        private System.Windows.Forms.Label lblContactInfoTitle;
        private System.Windows.Forms.Label lblClick;
        private System.Windows.Forms.Label lblDiagnosticsText2;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label lblLine2;
        private System.Windows.Forms.Label lblLine4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRepairDB;
    }
}
