// -----------------------------------------------------
// <copyright file="ConfigurationScreen.Designer.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.screens
{
    partial class ConfigurationScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationScreen));
            this.lblUpdateYear = new System.Windows.Forms.Label();
            this.dpUpdateYear = new System.Windows.Forms.DateTimePicker();
            this.btnLoadCalendars = new System.Windows.Forms.Button();
            this.cbCalendarToUpdate = new System.Windows.Forms.ComboBox();
            this.lblCalendarToUpdate = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCalendarOwner = new System.Windows.Forms.TextBox();
            this.lblCalendarOwner = new System.Windows.Forms.Label();
            this.cbAutoUpdateCalendars = new System.Windows.Forms.CheckBox();
            this.grpRunInBackground = new System.Windows.Forms.GroupBox();
            this.pnlUpdateMinuteSection = new System.Windows.Forms.Panel();
            this.rdoEightHours = new System.Windows.Forms.RadioButton();
            this.rdoFourHours = new System.Windows.Forms.RadioButton();
            this.rdoTwoHours = new System.Windows.Forms.RadioButton();
            this.rdoOneHour = new System.Windows.Forms.RadioButton();
            this.lblScheduleTime = new System.Windows.Forms.Label();
            this.btnResetCalendar = new System.Windows.Forms.Button();
            this.grpRunInBackground.SuspendLayout();
            this.pnlUpdateMinuteSection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUpdateYear
            // 
            this.lblUpdateYear.AutoSize = true;
            this.lblUpdateYear.Location = new System.Drawing.Point(419, 16);
            this.lblUpdateYear.Name = "lblUpdateYear";
            this.lblUpdateYear.Size = new System.Drawing.Size(86, 13);
            this.lblUpdateYear.TabIndex = 19;
            this.lblUpdateYear.Text = "Year To Update:";
            // 
            // dpUpdateYear
            // 
            this.dpUpdateYear.CustomFormat = "yyyy";
            this.dpUpdateYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpUpdateYear.Location = new System.Drawing.Point(511, 12);
            this.dpUpdateYear.MinDate = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.dpUpdateYear.Name = "dpUpdateYear";
            this.dpUpdateYear.Size = new System.Drawing.Size(61, 20);
            this.dpUpdateYear.TabIndex = 11;
            this.dpUpdateYear.Value = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            // 
            // btnLoadCalendars
            // 
            this.btnLoadCalendars.Location = new System.Drawing.Point(431, 107);
            this.btnLoadCalendars.Name = "btnLoadCalendars";
            this.btnLoadCalendars.Size = new System.Drawing.Size(106, 23);
            this.btnLoadCalendars.TabIndex = 2;
            this.btnLoadCalendars.Text = "&Load Calendars";
            this.btnLoadCalendars.UseVisualStyleBackColor = true;
            this.btnLoadCalendars.Click += new System.EventHandler(this.BtnLoadCalendars_Click);
            // 
            // cbCalendarToUpdate
            // 
            this.cbCalendarToUpdate.FormattingEnabled = true;
            this.cbCalendarToUpdate.Location = new System.Drawing.Point(177, 109);
            this.cbCalendarToUpdate.Name = "cbCalendarToUpdate";
            this.cbCalendarToUpdate.Size = new System.Drawing.Size(235, 21);
            this.cbCalendarToUpdate.TabIndex = 3;
            // 
            // lblCalendarToUpdate
            // 
            this.lblCalendarToUpdate.AutoSize = true;
            this.lblCalendarToUpdate.Location = new System.Drawing.Point(57, 112);
            this.lblCalendarToUpdate.Name = "lblCalendarToUpdate";
            this.lblCalendarToUpdate.Size = new System.Drawing.Size(102, 13);
            this.lblCalendarToUpdate.TabIndex = 15;
            this.lblCalendarToUpdate.Text = "Calendar to Update:";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(480, 326);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(92, 23);
            this.btnSaveSettings.TabIndex = 10;
            this.btnSaveSettings.Text = "&Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(399, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtCalendarOwner
            // 
            this.txtCalendarOwner.Location = new System.Drawing.Point(177, 71);
            this.txtCalendarOwner.Name = "txtCalendarOwner";
            this.txtCalendarOwner.Size = new System.Drawing.Size(235, 20);
            this.txtCalendarOwner.TabIndex = 1;
            // 
            // lblCalendarOwner
            // 
            this.lblCalendarOwner.AutoSize = true;
            this.lblCalendarOwner.Location = new System.Drawing.Point(57, 74);
            this.lblCalendarOwner.Name = "lblCalendarOwner";
            this.lblCalendarOwner.Size = new System.Drawing.Size(114, 13);
            this.lblCalendarOwner.TabIndex = 10;
            this.lblCalendarOwner.Text = "Calendar Owner Email:";
            // 
            // cbAutoUpdateCalendars
            // 
            this.cbAutoUpdateCalendars.AutoSize = true;
            this.cbAutoUpdateCalendars.Location = new System.Drawing.Point(287, 19);
            this.cbAutoUpdateCalendars.Name = "cbAutoUpdateCalendars";
            this.cbAutoUpdateCalendars.Size = new System.Drawing.Size(184, 17);
            this.cbAutoUpdateCalendars.TabIndex = 4;
            this.cbAutoUpdateCalendars.Text = "Update Calendars In Background";
            this.cbAutoUpdateCalendars.UseVisualStyleBackColor = true;
            // 
            // grpRunInBackground
            // 
            this.grpRunInBackground.BackColor = System.Drawing.SystemColors.Control;
            this.grpRunInBackground.Controls.Add(this.pnlUpdateMinuteSection);
            this.grpRunInBackground.Controls.Add(this.lblScheduleTime);
            this.grpRunInBackground.Controls.Add(this.cbAutoUpdateCalendars);
            this.grpRunInBackground.Location = new System.Drawing.Point(60, 155);
            this.grpRunInBackground.Name = "grpRunInBackground";
            this.grpRunInBackground.Size = new System.Drawing.Size(477, 156);
            this.grpRunInBackground.TabIndex = 21;
            this.grpRunInBackground.TabStop = false;
            this.grpRunInBackground.Text = "Automatically Update Settings:";
            // 
            // pnlUpdateMinuteSection
            // 
            this.pnlUpdateMinuteSection.Controls.Add(this.rdoEightHours);
            this.pnlUpdateMinuteSection.Controls.Add(this.rdoFourHours);
            this.pnlUpdateMinuteSection.Controls.Add(this.rdoTwoHours);
            this.pnlUpdateMinuteSection.Controls.Add(this.rdoOneHour);
            this.pnlUpdateMinuteSection.Location = new System.Drawing.Point(117, 40);
            this.pnlUpdateMinuteSection.Name = "pnlUpdateMinuteSection";
            this.pnlUpdateMinuteSection.Size = new System.Drawing.Size(154, 95);
            this.pnlUpdateMinuteSection.TabIndex = 22;
            // 
            // rdoEightHours
            // 
            this.rdoEightHours.AutoSize = true;
            this.rdoEightHours.Location = new System.Drawing.Point(16, 72);
            this.rdoEightHours.Name = "rdoEightHours";
            this.rdoEightHours.Size = new System.Drawing.Size(110, 17);
            this.rdoEightHours.TabIndex = 8;
            this.rdoEightHours.TabStop = true;
            this.rdoEightHours.Text = "Every Eight Hours";
            this.rdoEightHours.UseVisualStyleBackColor = true;
            // 
            // rdoFourHours
            // 
            this.rdoFourHours.AutoSize = true;
            this.rdoFourHours.Location = new System.Drawing.Point(16, 49);
            this.rdoFourHours.Name = "rdoFourHours";
            this.rdoFourHours.Size = new System.Drawing.Size(107, 17);
            this.rdoFourHours.TabIndex = 7;
            this.rdoFourHours.TabStop = true;
            this.rdoFourHours.Text = "Every Four Hours";
            this.rdoFourHours.UseVisualStyleBackColor = true;
            // 
            // rdoTwoHours
            // 
            this.rdoTwoHours.AutoSize = true;
            this.rdoTwoHours.Location = new System.Drawing.Point(16, 26);
            this.rdoTwoHours.Name = "rdoTwoHours";
            this.rdoTwoHours.Size = new System.Drawing.Size(107, 17);
            this.rdoTwoHours.TabIndex = 6;
            this.rdoTwoHours.TabStop = true;
            this.rdoTwoHours.Text = "Every Two Hours";
            this.rdoTwoHours.UseVisualStyleBackColor = true;
            // 
            // rdoOneHour
            // 
            this.rdoOneHour.AutoSize = true;
            this.rdoOneHour.Location = new System.Drawing.Point(16, 3);
            this.rdoOneHour.Name = "rdoOneHour";
            this.rdoOneHour.Size = new System.Drawing.Size(78, 17);
            this.rdoOneHour.TabIndex = 5;
            this.rdoOneHour.TabStop = true;
            this.rdoOneHour.Text = "Every Hour";
            this.rdoOneHour.UseVisualStyleBackColor = true;
            // 
            // lblScheduleTime
            // 
            this.lblScheduleTime.AutoSize = true;
            this.lblScheduleTime.Location = new System.Drawing.Point(18, 43);
            this.lblScheduleTime.Name = "lblScheduleTime";
            this.lblScheduleTime.Size = new System.Drawing.Size(93, 13);
            this.lblScheduleTime.TabIndex = 21;
            this.lblScheduleTime.Text = "Update Schedule:";
            // 
            // btnResetCalendar
            // 
            this.btnResetCalendar.BackColor = System.Drawing.Color.Red;
            this.btnResetCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetCalendar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnResetCalendar.Location = new System.Drawing.Point(13, 326);
            this.btnResetCalendar.Name = "btnResetCalendar";
            this.btnResetCalendar.Size = new System.Drawing.Size(120, 23);
            this.btnResetCalendar.TabIndex = 22;
            this.btnResetCalendar.TabStop = false;
            this.btnResetCalendar.Text = "Reset Calendar";
            this.btnResetCalendar.UseVisualStyleBackColor = false;
            this.btnResetCalendar.Click += new System.EventHandler(this.BtnResetCalendar_Click);
            // 
            // ConfigurationScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btnResetCalendar);
            this.Controls.Add(this.grpRunInBackground);
            this.Controls.Add(this.lblUpdateYear);
            this.Controls.Add(this.dpUpdateYear);
            this.Controls.Add(this.btnLoadCalendars);
            this.Controls.Add(this.cbCalendarToUpdate);
            this.Controls.Add(this.lblCalendarToUpdate);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtCalendarOwner);
            this.Controls.Add(this.lblCalendarOwner);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigurationScreen";
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.ConfigurationScreen_Load);
            this.grpRunInBackground.ResumeLayout(false);
            this.grpRunInBackground.PerformLayout();
            this.pnlUpdateMinuteSection.ResumeLayout(false);
            this.pnlUpdateMinuteSection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUpdateYear;
        private System.Windows.Forms.DateTimePicker dpUpdateYear;
        private System.Windows.Forms.Button btnLoadCalendars;
        private System.Windows.Forms.ComboBox cbCalendarToUpdate;
        private System.Windows.Forms.Label lblCalendarToUpdate;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtCalendarOwner;
        private System.Windows.Forms.Label lblCalendarOwner;
        private System.Windows.Forms.CheckBox cbAutoUpdateCalendars;
        private System.Windows.Forms.GroupBox grpRunInBackground;
        private System.Windows.Forms.Panel pnlUpdateMinuteSection;
        private System.Windows.Forms.RadioButton rdoEightHours;
        private System.Windows.Forms.RadioButton rdoFourHours;
        private System.Windows.Forms.RadioButton rdoTwoHours;
        private System.Windows.Forms.RadioButton rdoOneHour;
        private System.Windows.Forms.Label lblScheduleTime;
        private System.Windows.Forms.Button btnResetCalendar;
    }
}