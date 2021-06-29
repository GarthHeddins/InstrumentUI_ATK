namespace InstrumentUI_ATK.Controls
{
    partial class ResultListItem
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
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.lblDisplayText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.Font = new System.Drawing.Font("Arial", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSerialNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblSerialNumber.Location = new System.Drawing.Point(0, 0);
            this.lblSerialNumber.Margin = new System.Windows.Forms.Padding(0);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(30, 13);
            this.lblSerialNumber.TabIndex = 0;
            this.lblSerialNumber.Text = "123";
            // 
            // lblDisplayText
            // 
            this.lblDisplayText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDisplayText.Font = new System.Drawing.Font("Arial", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblDisplayText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDisplayText.Location = new System.Drawing.Point(28, 0);
            this.lblDisplayText.Name = "lblDisplayText";
            this.lblDisplayText.Size = new System.Drawing.Size(171, 13);
            this.lblDisplayText.TabIndex = 1;
            this.lblDisplayText.Text = "label2";
            this.lblDisplayText.UseMnemonic = false;
            this.lblDisplayText.Click += new System.EventHandler(this.lblDisplayText_Click);
            // 
            // ResultListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblDisplayText);
            this.Controls.Add(this.lblSerialNumber);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ResultListItem";
            this.Size = new System.Drawing.Size(204, 13);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Label lblDisplayText;
    }
}
