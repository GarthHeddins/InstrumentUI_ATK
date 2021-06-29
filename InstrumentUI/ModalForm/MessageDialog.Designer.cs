namespace InstrumentUI_ATK.ModalForm
{
    partial class MessageDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.lblBottom = new System.Windows.Forms.Label();
            this.flpMessage = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMessageText = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.newbkgBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.flpMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            this.picIcon.Image = global::InstrumentUI_ATK.Properties.Resources.icon_alert_warning01;
            this.picIcon.Location = new System.Drawing.Point(23, 12);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(35, 34);
            this.picIcon.TabIndex = 1;
            this.picIcon.TabStop = false;
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.White;
            this.btnContinue.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.btnContinue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnContinue.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnContinue.FlatAppearance.BorderSize = 0;
            this.btnContinue.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnContinue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnContinue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.btnContinue.Location = new System.Drawing.Point(238, 286);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(89, 29);
            this.btnContinue.TabIndex = 6;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // lblBottom
            // 
            this.lblBottom.BackColor = System.Drawing.Color.White;
            this.lblBottom.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblBottom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblBottom.Location = new System.Drawing.Point(95, 327);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(378, 15);
            this.lblBottom.TabIndex = 7;
            this.lblBottom.Text = "For additional assistance, please call 1-866-968-7782";
            // 
            // flpMessage
            // 
            this.flpMessage.AutoScroll = true;
            this.flpMessage.BackColor = System.Drawing.Color.White;
            this.flpMessage.Controls.Add(this.lblMessageText);
            this.flpMessage.Location = new System.Drawing.Point(98, 75);
            this.flpMessage.Name = "flpMessage";
            this.flpMessage.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.flpMessage.Size = new System.Drawing.Size(367, 197);
            this.flpMessage.TabIndex = 8;
            // 
            // lblMessageText
            // 
            this.lblMessageText.AutoSize = true;
            this.lblMessageText.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMessageText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblMessageText.Location = new System.Drawing.Point(0, 0);
            this.lblMessageText.Margin = new System.Windows.Forms.Padding(0);
            this.lblMessageText.Name = "lblMessageText";
            this.lblMessageText.Size = new System.Drawing.Size(113, 19);
            this.lblMessageText.TabIndex = 0;
            this.lblMessageText.Text = "Message Text";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblTitle.Location = new System.Drawing.Point(64, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(133, 21);
            this.lblTitle.TabIndex = 9;
            this.lblTitle.Text = "Error: Error title";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.btnCancel.Location = new System.Drawing.Point(360, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 29);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // newbkgBtn
            // 
            this.newbkgBtn.BackColor = System.Drawing.Color.Crimson;
            this.newbkgBtn.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_admin_button_blank_large;
            this.newbkgBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.newbkgBtn.Enabled = false;
            this.newbkgBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.newbkgBtn.FlatAppearance.BorderSize = 0;
            this.newbkgBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.newbkgBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.newbkgBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newbkgBtn.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newbkgBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.newbkgBtn.Location = new System.Drawing.Point(238, 19);
            this.newbkgBtn.Name = "newbkgBtn";
            this.newbkgBtn.Size = new System.Drawing.Size(89, 27);
            this.newbkgBtn.TabIndex = 11;
            this.newbkgBtn.Text = "Clean Bkg";
            this.newbkgBtn.UseVisualStyleBackColor = false;
            this.newbkgBtn.Visible = false;
            this.newbkgBtn.Click += new System.EventHandler(this.newbkgBtn_Click);
            // 
            // MessageDialog
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.alerts_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(562, 351);
            this.ControlBox = false;
            this.Controls.Add(this.newbkgBtn);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.flpMessage);
            this.Controls.Add(this.lblBottom);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.picIcon);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Message";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.Load += new System.EventHandler(this.MessageDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.flpMessage.ResumeLayout(false);
            this.flpMessage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.FlowLayoutPanel flpMessage;
        private System.Windows.Forms.Label lblMessageText;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button newbkgBtn;
    }
}