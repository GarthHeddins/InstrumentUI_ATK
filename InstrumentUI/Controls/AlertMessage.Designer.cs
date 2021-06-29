namespace InstrumentUI_ATK.Controls
{
    partial class AlertMessage
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
            this.lblDate = new System.Windows.Forms.Label();
            this.lblBody = new System.Windows.Forms.Label();
            this.flpMessage = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.flpMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(113)))), ((int)(((byte)(141)))));
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(90, 20);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "label1";
            // 
            // lblBody
            // 
            this.lblBody.AutoSize = true;
            this.lblBody.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(113)))), ((int)(((byte)(141)))));
            this.lblBody.Location = new System.Drawing.Point(0, 0);
            this.lblBody.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblBody.Name = "lblBody";
            this.lblBody.Size = new System.Drawing.Size(46, 16);
            this.lblBody.TabIndex = 2;
            this.lblBody.Text = "label3";
            this.lblBody.UseMnemonic = false;
            // 
            // flpMessage
            // 
            this.flpMessage.AutoSize = true;
            this.flpMessage.Controls.Add(this.lblBody);
            this.flpMessage.Location = new System.Drawing.Point(97, 21);
            this.flpMessage.MaximumSize = new System.Drawing.Size(400, 2000);
            this.flpMessage.Name = "flpMessage";
            this.flpMessage.Size = new System.Drawing.Size(400, 17);
            this.flpMessage.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblTitle.Location = new System.Drawing.Point(97, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 20);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "label1";
            this.lblTitle.UseMnemonic = false;
            // 
            // AlertMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.flpMessage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblDate);
            this.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AlertMessage";
            this.Size = new System.Drawing.Size(500, 47);
            this.flpMessage.ResumeLayout(false);
            this.flpMessage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblBody;
        private System.Windows.Forms.FlowLayoutPanel flpMessage;
        private System.Windows.Forms.Label lblTitle;
    }
}
