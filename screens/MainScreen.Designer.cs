namespace Salon_Calendar_Integration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainScreen));
            this.lblCalendarOwner = new System.Windows.Forms.Label();
            this.tbCalendarOwner = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblCalendarToUpdate = new System.Windows.Forms.Label();
            this.cbCalendarToUpdate = new System.Windows.Forms.ComboBox();
            this.btnLoadCalendars = new System.Windows.Forms.Button();
            this.dpUpdateYear = new System.Windows.Forms.DateTimePicker();
            this.lblUpdateYear = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCalendarOwner
            // 
            this.lblCalendarOwner.AutoSize = true;
            this.lblCalendarOwner.Location = new System.Drawing.Point(42, 87);
            this.lblCalendarOwner.Name = "lblCalendarOwner";
            this.lblCalendarOwner.Size = new System.Drawing.Size(114, 13);
            this.lblCalendarOwner.TabIndex = 0;
            this.lblCalendarOwner.Text = "Calendar Owner Email:";
            // 
            // tbCalendarOwner
            // 
            this.tbCalendarOwner.Location = new System.Drawing.Point(162, 84);
            this.tbCalendarOwner.Name = "tbCalendarOwner";
            this.tbCalendarOwner.Size = new System.Drawing.Size(235, 20);
            this.tbCalendarOwner.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(478, 301);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(380, 301);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(92, 23);
            this.btnSaveSettings.TabIndex = 3;
            this.btnSaveSettings.Text = "&Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(191, 218);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(182, 54);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update Salon Calendar";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblCalendarToUpdate
            // 
            this.lblCalendarToUpdate.AutoSize = true;
            this.lblCalendarToUpdate.Location = new System.Drawing.Point(42, 125);
            this.lblCalendarToUpdate.Name = "lblCalendarToUpdate";
            this.lblCalendarToUpdate.Size = new System.Drawing.Size(102, 13);
            this.lblCalendarToUpdate.TabIndex = 5;
            this.lblCalendarToUpdate.Text = "Calendar to Update:";
            // 
            // cbCalendarToUpdate
            // 
            this.cbCalendarToUpdate.FormattingEnabled = true;
            this.cbCalendarToUpdate.Location = new System.Drawing.Point(162, 122);
            this.cbCalendarToUpdate.Name = "cbCalendarToUpdate";
            this.cbCalendarToUpdate.Size = new System.Drawing.Size(235, 21);
            this.cbCalendarToUpdate.TabIndex = 6;
            // 
            // btnLoadCalendars
            // 
            this.btnLoadCalendars.Location = new System.Drawing.Point(416, 120);
            this.btnLoadCalendars.Name = "btnLoadCalendars";
            this.btnLoadCalendars.Size = new System.Drawing.Size(106, 23);
            this.btnLoadCalendars.TabIndex = 7;
            this.btnLoadCalendars.Text = "Load Calendars";
            this.btnLoadCalendars.UseVisualStyleBackColor = true;
            this.btnLoadCalendars.Click += new System.EventHandler(this.btnLoadCalendars_Click);
            // 
            // dpUpdateYear
            // 
            this.dpUpdateYear.CustomFormat = "yyyy";
            this.dpUpdateYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpUpdateYear.Location = new System.Drawing.Point(466, 24);
            this.dpUpdateYear.MinDate = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.dpUpdateYear.Name = "dpUpdateYear";
            this.dpUpdateYear.Size = new System.Drawing.Size(73, 20);
            this.dpUpdateYear.TabIndex = 8;
            this.dpUpdateYear.Value = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            // 
            // lblUpdateYear
            // 
            this.lblUpdateYear.AutoSize = true;
            this.lblUpdateYear.Location = new System.Drawing.Point(403, 28);
            this.lblUpdateYear.Name = "lblUpdateYear";
            this.lblUpdateYear.Size = new System.Drawing.Size(32, 13);
            this.lblUpdateYear.TabIndex = 9;
            this.lblUpdateYear.Text = "Year:";
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 339);
            this.Controls.Add(this.lblUpdateYear);
            this.Controls.Add(this.dpUpdateYear);
            this.Controls.Add(this.btnLoadCalendars);
            this.Controls.Add(this.cbCalendarToUpdate);
            this.Controls.Add(this.lblCalendarToUpdate);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tbCalendarOwner);
            this.Controls.Add(this.lblCalendarOwner);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Salon Iris Calendar Integration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCalendarOwner;
        private System.Windows.Forms.TextBox tbCalendarOwner;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblCalendarToUpdate;
        private System.Windows.Forms.ComboBox cbCalendarToUpdate;
        private System.Windows.Forms.Button btnLoadCalendars;
        private System.Windows.Forms.DateTimePicker dpUpdateYear;
        private System.Windows.Forms.Label lblUpdateYear;
    }
}

