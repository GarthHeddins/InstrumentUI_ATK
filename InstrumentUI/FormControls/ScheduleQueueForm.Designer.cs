namespace InstrumentUI_ATK.FormControls
{
    partial class ScheduleQueueForm
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
            this.pnlScheduleQueue = new System.Windows.Forms.Panel();
            this.pnlQueue = new System.Windows.Forms.Panel();
            this.pnlQueueDetails = new System.Windows.Forms.Panel();
            this.tlpHeaders = new System.Windows.Forms.TableLayoutPanel();
            this.lblRemove = new System.Windows.Forms.Label();
            this.lblScheduledTime = new System.Windows.Forms.Label();
            this.lblSampleLocation = new System.Windows.Forms.Label();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.lblControlTitle = new System.Windows.Forms.Label();
            this.btnBack = new InstrumentUI_ATK.Controls.RibbonIcon();
            this.pnlScheduleQueue.SuspendLayout();
            this.pnlQueue.SuspendLayout();
            this.tlpHeaders.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlScheduleQueue
            // 
            this.pnlScheduleQueue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlScheduleQueue.Controls.Add(this.pnlQueue);
            this.pnlScheduleQueue.Controls.Add(this.lblControlTitle);
            this.pnlScheduleQueue.Location = new System.Drawing.Point(34, 0);
            this.pnlScheduleQueue.Name = "pnlScheduleQueue";
            this.pnlScheduleQueue.Size = new System.Drawing.Size(610, 300);
            this.pnlScheduleQueue.TabIndex = 0;
            // 
            // pnlQueue
            // 
            this.pnlQueue.AutoScroll = true;
            this.pnlQueue.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.routine_selectmaterial_background;
            this.pnlQueue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlQueue.Controls.Add(this.pnlQueueDetails);
            this.pnlQueue.Controls.Add(this.tlpHeaders);
            this.pnlQueue.Location = new System.Drawing.Point(3, 32);
            this.pnlQueue.Name = "pnlQueue";
            this.pnlQueue.Padding = new System.Windows.Forms.Padding(5, 10, 20, 20);
            this.pnlQueue.Size = new System.Drawing.Size(601, 256);
            this.pnlQueue.TabIndex = 5;
            // 
            // pnlQueueDetails
            // 
            this.pnlQueueDetails.AutoScroll = true;
            this.pnlQueueDetails.BackColor = System.Drawing.Color.Transparent;
            this.pnlQueueDetails.Location = new System.Drawing.Point(6, 37);
            this.pnlQueueDetails.Name = "pnlQueueDetails";
            this.pnlQueueDetails.Size = new System.Drawing.Size(575, 195);
            this.pnlQueueDetails.TabIndex = 1;
            // 
            // tlpHeaders
            // 
            this.tlpHeaders.BackColor = System.Drawing.Color.Transparent;
            this.tlpHeaders.ColumnCount = 4;
            this.tlpHeaders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlpHeaders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpHeaders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlpHeaders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tlpHeaders.Controls.Add(this.lblRemove, 0, 0);
            this.tlpHeaders.Controls.Add(this.lblScheduledTime, 1, 0);
            this.tlpHeaders.Controls.Add(this.lblSampleLocation, 2, 0);
            this.tlpHeaders.Controls.Add(this.lblMaterial, 3, 0);
            this.tlpHeaders.Location = new System.Drawing.Point(6, 6);
            this.tlpHeaders.Name = "tlpHeaders";
            this.tlpHeaders.RowCount = 1;
            this.tlpHeaders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeaders.Size = new System.Drawing.Size(550, 30);
            this.tlpHeaders.TabIndex = 0;
            // 
            // lblRemove
            // 
            this.lblRemove.AutoSize = true;
            this.lblRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemove.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblRemove.ForeColor = System.Drawing.Color.Black;
            this.lblRemove.Location = new System.Drawing.Point(3, 0);
            this.lblRemove.Name = "lblRemove";
            this.lblRemove.Size = new System.Drawing.Size(69, 30);
            this.lblRemove.TabIndex = 0;
            this.lblRemove.Text = "Remove";
            this.lblRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScheduledTime
            // 
            this.lblScheduledTime.AutoSize = true;
            this.lblScheduledTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScheduledTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblScheduledTime.ForeColor = System.Drawing.Color.Black;
            this.lblScheduledTime.Location = new System.Drawing.Point(78, 0);
            this.lblScheduledTime.Name = "lblScheduledTime";
            this.lblScheduledTime.Size = new System.Drawing.Size(144, 30);
            this.lblScheduledTime.TabIndex = 1;
            this.lblScheduledTime.Text = "Scheduled Time";
            this.lblScheduledTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSampleLocation
            // 
            this.lblSampleLocation.AutoSize = true;
            this.lblSampleLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSampleLocation.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSampleLocation.ForeColor = System.Drawing.Color.Black;
            this.lblSampleLocation.Location = new System.Drawing.Point(228, 0);
            this.lblSampleLocation.Name = "lblSampleLocation";
            this.lblSampleLocation.Size = new System.Drawing.Size(69, 30);
            this.lblSampleLocation.TabIndex = 2;
            this.lblSampleLocation.Text = "Sample Location";
            this.lblSampleLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMaterial
            // 
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaterial.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblMaterial.ForeColor = System.Drawing.Color.Black;
            this.lblMaterial.Location = new System.Drawing.Point(303, 0);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(244, 30);
            this.lblMaterial.TabIndex = 3;
            this.lblMaterial.Text = "Material";
            this.lblMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblControlTitle
            // 
            this.lblControlTitle.AutoSize = true;
            this.lblControlTitle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblControlTitle.Location = new System.Drawing.Point(6, 6);
            this.lblControlTitle.Name = "lblControlTitle";
            this.lblControlTitle.Size = new System.Drawing.Size(144, 21);
            this.lblControlTitle.TabIndex = 0;
            this.lblControlTitle.Text = "Schedule Queue";
            // 
            // btnBack
            // 
            this.btnBack.ActiveImage = global::InstrumentUI_ATK.Properties.Resources.button_analyze_active;
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnBack.Image = global::InstrumentUI_ATK.Properties.Resources.button_analyze_active;
            this.btnBack.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBack.InActiveImage = global::InstrumentUI_ATK.Properties.Resources.button_analyze_active;
            this.btnBack.InActiveTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnBack.IsHoverActive = true;
            this.btnBack.Location = new System.Drawing.Point(586, 284);
            this.btnBack.Margin = new System.Windows.Forms.Padding(0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Selected = false;
            this.btnBack.Size = new System.Drawing.Size(90, 90);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Back";
            this.btnBack.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // ScheduleQueueForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.pnlScheduleQueue);
            this.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.Name = "ScheduleQueueForm";
            this.Size = new System.Drawing.Size(676, 374);
            this.pnlScheduleQueue.ResumeLayout(false);
            this.pnlScheduleQueue.PerformLayout();
            this.pnlQueue.ResumeLayout(false);
            this.tlpHeaders.ResumeLayout(false);
            this.tlpHeaders.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlScheduleQueue;
        private System.Windows.Forms.Label lblControlTitle;
        private System.Windows.Forms.Panel pnlQueue;
        private InstrumentUI_ATK.Controls.RibbonIcon btnBack;
        private System.Windows.Forms.Panel pnlQueueDetails;
        private System.Windows.Forms.TableLayoutPanel tlpHeaders;
        private System.Windows.Forms.Label lblRemove;
        private System.Windows.Forms.Label lblScheduledTime;
        private System.Windows.Forms.Label lblSampleLocation;
        private System.Windows.Forms.Label lblMaterial;
    }
}
