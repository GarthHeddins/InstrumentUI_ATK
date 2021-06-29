namespace InstrumentUI_ATK.Controls
{
    partial class TraitCheck
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
            this.lblText = new System.Windows.Forms.Label();
            this.chkTrait = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.BackColor = System.Drawing.Color.White;
            this.lblText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblText.Location = new System.Drawing.Point(2, 0);
            this.lblText.Margin = new System.Windows.Forms.Padding(0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(175, 20);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "[Text]";
            this.lblText.UseMnemonic = false;
            // 
            // chkTrait
            // 
            this.chkTrait.AutoSize = true;
            this.chkTrait.BackColor = System.Drawing.Color.White;
            this.chkTrait.Location = new System.Drawing.Point(200, 3);
            this.chkTrait.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkTrait.Name = "chkTrait";
            this.chkTrait.Size = new System.Drawing.Size(15, 14);
            this.chkTrait.TabIndex = 0;
            this.chkTrait.TabStop = false;
            this.chkTrait.UseVisualStyleBackColor = false;
            this.chkTrait.CheckedChanged += new System.EventHandler(this.chkTrait_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_traits_underline;
            this.pictureBox1.Image = global::InstrumentUI_ATK.Properties.Resources.cognis_traits_underline;
            this.pictureBox1.Location = new System.Drawing.Point(5, 18);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(210, 1);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // TraitCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.chkTrait);
            this.Controls.Add(this.lblText);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TraitCheck";
            this.Size = new System.Drawing.Size(225, 20);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.CheckBox chkTrait;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
