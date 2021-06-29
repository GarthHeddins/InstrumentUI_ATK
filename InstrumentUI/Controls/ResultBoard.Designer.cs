namespace InstrumentUI_ATK.Controls
{
    partial class ResultBoard
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
            this.customFlpFirstRow = new InstrumentUI_ATK.Controls.CustomFlowLayoutPanel();
            this.lblFirst = new System.Windows.Forms.Label();
            this.lblSecond = new System.Windows.Forms.Label();
            this.customFlpFirstRow.SuspendLayout();
            this.SuspendLayout();
            // 
            // customFlpFirstRow
            // 
            this.customFlpFirstRow.BorderColor = System.Drawing.Color.Black;
            this.customFlpFirstRow.Controls.Add(this.lblFirst);
            this.customFlpFirstRow.Controls.Add(this.lblSecond);
            this.customFlpFirstRow.HasColorBorder = true;
            this.customFlpFirstRow.Location = new System.Drawing.Point(3, 3);
            this.customFlpFirstRow.Name = "customFlpFirstRow";
            this.customFlpFirstRow.Size = new System.Drawing.Size(147, 20);
            this.customFlpFirstRow.TabIndex = 1;
            this.customFlpFirstRow.WrapContents = false;
            // 
            // lblFirst
            // 
            this.lblFirst.Location = new System.Drawing.Point(3, 3);
            this.lblFirst.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(78, 14);
            this.lblFirst.TabIndex = 0;
            this.lblFirst.Text = "label1";
            this.lblFirst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSecond
            // 
            this.lblSecond.Location = new System.Drawing.Point(87, 3);
            this.lblSecond.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(78, 14);
            this.lblSecond.TabIndex = 1;
            this.lblSecond.Text = "label2";
            this.lblSecond.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ResultBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.customFlpFirstRow);
            this.Name = "ResultBoard";
            this.Size = new System.Drawing.Size(153, 41);
            this.customFlpFirstRow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFirst;
        private System.Windows.Forms.Label lblSecond;
        private CustomFlowLayoutPanel customFlpFirstRow;

    }
}
