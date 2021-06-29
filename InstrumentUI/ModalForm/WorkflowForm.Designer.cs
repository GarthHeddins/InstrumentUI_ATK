using System.Drawing;

namespace InstrumentUI_ATK.ModalForm
{
    partial class WorkflowForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.bgWorkerInitWF = new System.ComponentModel.BackgroundWorker();
            this.pbWorkflowStatus = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbWorkflowStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblTitle.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(40, 90);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(640, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Workflow Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.lblMessage.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(75, 190);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(570, 29);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Workflow Messages";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.Visible = false;
            // 
            // bgWorkerInitWF
            // 
            this.bgWorkerInitWF.WorkerReportsProgress = true;
            this.bgWorkerInitWF.WorkerSupportsCancellation = true;
            this.bgWorkerInitWF.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerInitWF_DoWork);
            this.bgWorkerInitWF.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerInitWF_ProgressChanged);
            this.bgWorkerInitWF.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerInitWF_RunWorkerCompleted);
            // 
            // pbWorkflowStatus
            // 
            this.pbWorkflowStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.pbWorkflowStatus.ErrorImage = null;
            this.pbWorkflowStatus.Image = global::InstrumentUI_ATK.Properties.Resources.statusindicator_workflow;
            this.pbWorkflowStatus.Location = new System.Drawing.Point(190, 145);
            this.pbWorkflowStatus.Name = "pbWorkflowStatus";
            this.pbWorkflowStatus.Size = new System.Drawing.Size(340, 50);
            this.pbWorkflowStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbWorkflowStatus.TabIndex = 1;
            this.pbWorkflowStatus.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(713, 311);
            this.panel1.TabIndex = 3;
            // 
            // WorkflowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.BackgroundImage = global::InstrumentUI_ATK.Properties.Resources.cognis_popup_workflow_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(735, 333);
            this.ControlBox = false;
            this.Controls.Add(this.pbWorkflowStatus);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkflowForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.Ivory;
            this.Load += new System.EventHandler(this.Workflow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbWorkflowStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbWorkflowStatus;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Panel panel1;
        private System.ComponentModel.BackgroundWorker bgWorkerInitWF;
    }
}