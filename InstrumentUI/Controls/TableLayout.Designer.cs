namespace InstrumentUI_ATK.Controls
{
    partial class TableLayout
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
            this.tlpTable = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tlpTable
            // 
            this.tlpTable.AutoSize = true;
            this.tlpTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpTable.ColumnCount = 2;
            this.tlpTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tlpTable.Location = new System.Drawing.Point(0, 0);
            this.tlpTable.Name = "tlpTable";
            this.tlpTable.RowCount = 1;
            this.tlpTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpTable.Size = new System.Drawing.Size(138, 34);
            this.tlpTable.TabIndex = 0;
            // 
            // TableLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tlpTable);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TableLayout";
            this.Size = new System.Drawing.Size(138, 34);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpTable;
    }
}
