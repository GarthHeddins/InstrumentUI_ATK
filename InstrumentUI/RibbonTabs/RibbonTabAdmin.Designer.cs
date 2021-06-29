namespace InstrumentUI_ATK.RibbonTabs
{
    partial class RibbonTabAdmin
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
            this.flpBody = new System.Windows.Forms.FlowLayoutPanel();
            this.iconPreferences = new InstrumentUI_ATK.Controls.RibbonIcon();
            this.iconSupport = new InstrumentUI_ATK.Controls.RibbonIcon();
            this.iconSetup = new InstrumentUI_ATK.Controls.RibbonIcon();
            this.flpBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpBody
            // 
            this.flpBody.Controls.Add(this.iconPreferences);
            this.flpBody.Controls.Add(this.iconSupport);
            this.flpBody.Controls.Add(this.iconSetup);
            this.flpBody.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpBody.Location = new System.Drawing.Point(-5, -4);
            this.flpBody.Margin = new System.Windows.Forms.Padding(0);
            this.flpBody.Name = "flpBody";
            this.flpBody.Size = new System.Drawing.Size(580, 85);
            this.flpBody.TabIndex = 15;
            this.flpBody.Click += new System.EventHandler(this.flpBody_Click);
            // 
            // iconPreferences
            // 
            this.iconPreferences.ActiveImage = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_preferences_active;
            this.iconPreferences.FlatAppearance.BorderSize = 0;
            this.iconPreferences.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.iconPreferences.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.iconPreferences.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconPreferences.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconPreferences.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.iconPreferences.Image = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_preferences;
            this.iconPreferences.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconPreferences.InActiveImage = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_preferences;
            this.iconPreferences.IsHoverActive = true;
            this.iconPreferences.Location = new System.Drawing.Point(0, 0);
            this.iconPreferences.Margin = new System.Windows.Forms.Padding(0);
            this.iconPreferences.Name = "iconPreferences";
            this.iconPreferences.Selected = false;
            this.iconPreferences.Size = new System.Drawing.Size(75, 57);
            this.iconPreferences.TabIndex = 12;
            this.iconPreferences.TabStop = false;
            this.iconPreferences.Text = "Preferences";
            this.iconPreferences.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconPreferences.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.iconPreferences.UseVisualStyleBackColor = true;
            this.iconPreferences.Click += new System.EventHandler(this.iconPreferences_Click);
            // 
            // iconSupport
            // 
            this.iconSupport.ActiveImage = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_support_active;
            this.iconSupport.FlatAppearance.BorderSize = 0;
            this.iconSupport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.iconSupport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.iconSupport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconSupport.Font = new System.Drawing.Font("Arial", 6.75F);
            this.iconSupport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.iconSupport.Image = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_support;
            this.iconSupport.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconSupport.InActiveImage = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_support;
            this.iconSupport.IsHoverActive = true;
            this.iconSupport.Location = new System.Drawing.Point(75, 0);
            this.iconSupport.Margin = new System.Windows.Forms.Padding(0);
            this.iconSupport.Name = "iconSupport";
            this.iconSupport.Selected = false;
            this.iconSupport.Size = new System.Drawing.Size(61, 57);
            this.iconSupport.TabIndex = 13;
            this.iconSupport.TabStop = false;
            this.iconSupport.Text = "Support";
            this.iconSupport.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconSupport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.iconSupport.UseVisualStyleBackColor = true;
            this.iconSupport.Click += new System.EventHandler(this.iconSupport_Click);
            // 
            // iconSetup
            // 
            this.iconSetup.ActiveImage = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_setup_active;
            this.iconSetup.FlatAppearance.BorderSize = 0;
            this.iconSetup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.iconSetup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.iconSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconSetup.Font = new System.Drawing.Font("Arial", 6.75F);
            this.iconSetup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(56)))), ((int)(((byte)(141)))));
            this.iconSetup.Image = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_setup;
            this.iconSetup.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconSetup.InActiveImage = global::InstrumentUI_ATK.Properties.Resources.ribbon_icon_admin_setup;
            this.iconSetup.IsHoverActive = true;
            this.iconSetup.Location = new System.Drawing.Point(136, 0);
            this.iconSetup.Margin = new System.Windows.Forms.Padding(0);
            this.iconSetup.Name = "iconSetup";
            this.iconSetup.Selected = false;
            this.iconSetup.Size = new System.Drawing.Size(61, 57);
            this.iconSetup.TabIndex = 14;
            this.iconSetup.TabStop = false;
            this.iconSetup.Text = "Set Up";
            this.iconSetup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconSetup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.iconSetup.UseVisualStyleBackColor = true;
            this.iconSetup.Click += new System.EventHandler(this.iconSetup_Click);
            // 
            // RibbonTabAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flpBody);
            this.Name = "RibbonTabAdmin";
            this.Size = new System.Drawing.Size(586, 95);
            this.flpBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private InstrumentUI_ATK.Controls.RibbonIcon iconPreferences;
        private InstrumentUI_ATK.Controls.RibbonIcon iconSupport;
        private InstrumentUI_ATK.Controls.RibbonIcon iconSetup;
        private System.Windows.Forms.FlowLayoutPanel flpBody;
    }
}
