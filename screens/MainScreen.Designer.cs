// -----------------------------------------------------
// <copyright file="MainScreen.Designer.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites
{
    partial class MainScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainScreen));
            this.btnExit = new FontAwesome.Sharp.IconButton();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.sbpanelAppStatus = new System.Windows.Forms.StatusBarPanel();
            this.sbpanelDateTime = new System.Windows.Forms.StatusBarPanel();
            this.btnSettings = new FontAwesome.Sharp.IconButton();
            this.rtxtAppInfo = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.sbpanelAppStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpanelDateTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnExit.IconChar = FontAwesome.Sharp.IconChar.SignOutAlt;
            this.btnExit.IconColor = System.Drawing.Color.Black;
            this.btnExit.IconSize = 25;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(492, 224);
            this.btnExit.Name = "btnExit";
            this.btnExit.Rotation = 0D;
            this.btnExit.Size = new System.Drawing.Size(80, 50);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "E&xit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(158, 280);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(182, 54);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "&Update Salon Calendar Now";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Charites:  Salon Calendar Integration";
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 339);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbpanelAppStatus,
            this.sbpanelDateTime});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(584, 22);
            this.statusBar.TabIndex = 10;
            // 
            // sbpanelAppStatus
            // 
            this.sbpanelAppStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbpanelAppStatus.Name = "sbpanelAppStatus";
            this.sbpanelAppStatus.ToolTipText = "Last Activity";
            this.sbpanelAppStatus.Width = 557;
            // 
            // sbpanelDateTime
            // 
            this.sbpanelDateTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbpanelDateTime.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Raised;
            this.sbpanelDateTime.Name = "sbpanelDateTime";
            this.sbpanelDateTime.Width = 10;
            // 
            // btnSettings
            // 
            this.btnSettings.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnSettings.IconChar = FontAwesome.Sharp.IconChar.Cog;
            this.btnSettings.IconColor = System.Drawing.Color.Black;
            this.btnSettings.IconSize = 25;
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSettings.Location = new System.Drawing.Point(492, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Rotation = 0D;
            this.btnSettings.Size = new System.Drawing.Size(80, 45);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "&Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // rtxtAppInfo
            // 
            this.rtxtAppInfo.BackColor = System.Drawing.SystemColors.Control;
            this.rtxtAppInfo.Location = new System.Drawing.Point(12, 12);
            this.rtxtAppInfo.Name = "rtxtAppInfo";
            this.rtxtAppInfo.ReadOnly = true;
            this.rtxtAppInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtAppInfo.Size = new System.Drawing.Size(474, 262);
            this.rtxtAppInfo.TabIndex = 12;
            this.rtxtAppInfo.TabStop = false;
            this.rtxtAppInfo.Text = resources.GetString("rtxtAppInfo.Text");
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.rtxtAppInfo);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Charites:  Salon Iris Calendar Integration";
            ((System.ComponentModel.ISupportInitialize)(this.sbpanelAppStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpanelDateTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.StatusBar statusBar;
        private FontAwesome.Sharp.IconButton btnSettings;
        private FontAwesome.Sharp.IconButton btnExit;
        private System.Windows.Forms.RichTextBox rtxtAppInfo;
        protected System.Windows.Forms.StatusBarPanel sbpanelAppStatus;
        protected System.Windows.Forms.StatusBarPanel sbpanelDateTime;
        public System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

