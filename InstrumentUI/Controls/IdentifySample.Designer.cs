namespace InstrumentUI_ATK.Controls
{
    partial class IdentifySample
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
            this.txtTextBox = new System.Windows.Forms.TextBox();
            this.cbDropDown = new InstrumentUI_ATK.Controls.CustomComboBox();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblText.Location = new System.Drawing.Point(3, 6);
            this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(130, 16);
            this.lblText.TabIndex = 16;
            this.lblText.Text = "[Text]";
            this.lblText.UseMnemonic = false;
            // 
            // txtTextBox
            // 
            this.txtTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.txtTextBox.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtTextBox.Location = new System.Drawing.Point(146, 3);
            this.txtTextBox.MaxLength = 28;
            this.txtTextBox.Name = "txtTextBox";
            this.txtTextBox.Size = new System.Drawing.Size(303, 24);
            this.txtTextBox.TabIndex = 18;
            this.txtTextBox.TextChanged += new System.EventHandler(this.identifierValue_TextChanged);
            // 
            // cbDropDown
            // 
            this.cbDropDown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDropDown.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbDropDown.ForeColor = System.Drawing.Color.Black;
            this.cbDropDown.FormattingEnabled = true;
            this.cbDropDown.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(236)))));
            this.cbDropDown.Location = new System.Drawing.Point(146, 3);
            this.cbDropDown.Name = "cbDropDown";
            this.cbDropDown.Size = new System.Drawing.Size(303, 25);
            this.cbDropDown.TabIndex = 17;
            this.cbDropDown.TextChanged += new System.EventHandler(this.identifierValue_TextChanged);
            // 
            // IdentifySample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTextBox);
            this.Controls.Add(this.cbDropDown);
            this.Controls.Add(this.lblText);
            this.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "IdentifySample";
            this.Size = new System.Drawing.Size(456, 30);
            this.Enter += new System.EventHandler(this.IdentifySample_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblText;
        private InstrumentUI_ATK.Controls.CustomComboBox cbDropDown;
        private System.Windows.Forms.TextBox txtTextBox;
    }
}
