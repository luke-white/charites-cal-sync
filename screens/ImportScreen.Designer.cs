namespace itdevgeek_charites.screens
{
    partial class ImportScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportScreen));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImportCalendar = new System.Windows.Forms.Button();
            this.openICSFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenICSFile = new System.Windows.Forms.Button();
            this.txtImportCalendar = new System.Windows.Forms.TextBox();
            this.lblImportCalendar = new System.Windows.Forms.Label();
            this.btnParseICSFile = new System.Windows.Forms.Button();
            this.grpICSSummary = new System.Windows.Forms.GroupBox();
            this.txtKoulaAppointmentCount = new System.Windows.Forms.TextBox();
            this.txtClientCount = new System.Windows.Forms.TextBox();
            this.txtAppointmentCount = new System.Windows.Forms.TextBox();
            this.lblKoulaAppointmentCount = new System.Windows.Forms.Label();
            this.lblClients = new System.Windows.Forms.Label();
            this.lblAppointments = new System.Windows.Forms.Label();
            this.lblImportYear = new System.Windows.Forms.Label();
            this.dpImportYear = new System.Windows.Forms.DateTimePicker();
            this.txtLyshaieAppointmentCount = new System.Windows.Forms.TextBox();
            this.lblLyshaieAppointmentCount = new System.Windows.Forms.Label();
            this.grpICSSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(399, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImportCalendar
            // 
            this.btnImportCalendar.Location = new System.Drawing.Point(480, 326);
            this.btnImportCalendar.Name = "btnImportCalendar";
            this.btnImportCalendar.Size = new System.Drawing.Size(92, 23);
            this.btnImportCalendar.TabIndex = 11;
            this.btnImportCalendar.Text = "&Import Calendar";
            this.btnImportCalendar.UseVisualStyleBackColor = true;
            this.btnImportCalendar.Click += new System.EventHandler(this.BtnImportCalendar_Click);
            // 
            // openICSFileDialog
            // 
            this.openICSFileDialog.Filter = "ics files (*.ics)|*.ics|All files (*.*)|*.*\"";
            // 
            // btnOpenICSFile
            // 
            this.btnOpenICSFile.Location = new System.Drawing.Point(410, 69);
            this.btnOpenICSFile.Name = "btnOpenICSFile";
            this.btnOpenICSFile.Size = new System.Drawing.Size(106, 23);
            this.btnOpenICSFile.TabIndex = 13;
            this.btnOpenICSFile.Text = "&Select ICS File";
            this.btnOpenICSFile.UseVisualStyleBackColor = true;
            this.btnOpenICSFile.Click += new System.EventHandler(this.BtnOpenICSFile_Click);
            // 
            // txtImportCalendar
            // 
            this.txtImportCalendar.Location = new System.Drawing.Point(159, 71);
            this.txtImportCalendar.Name = "txtImportCalendar";
            this.txtImportCalendar.ReadOnly = true;
            this.txtImportCalendar.Size = new System.Drawing.Size(235, 20);
            this.txtImportCalendar.TabIndex = 12;
            // 
            // lblImportCalendar
            // 
            this.lblImportCalendar.AutoSize = true;
            this.lblImportCalendar.Location = new System.Drawing.Point(39, 74);
            this.lblImportCalendar.Name = "lblImportCalendar";
            this.lblImportCalendar.Size = new System.Drawing.Size(100, 13);
            this.lblImportCalendar.TabIndex = 14;
            this.lblImportCalendar.Text = "Calendar To Import:";
            // 
            // btnParseICSFile
            // 
            this.btnParseICSFile.Location = new System.Drawing.Point(410, 98);
            this.btnParseICSFile.Name = "btnParseICSFile";
            this.btnParseICSFile.Size = new System.Drawing.Size(106, 23);
            this.btnParseICSFile.TabIndex = 15;
            this.btnParseICSFile.Text = "Parse Import File";
            this.btnParseICSFile.UseVisualStyleBackColor = true;
            this.btnParseICSFile.Click += new System.EventHandler(this.BtnParseICSFile_Click);
            // 
            // grpICSSummary
            // 
            this.grpICSSummary.BackColor = System.Drawing.SystemColors.Control;
            this.grpICSSummary.Controls.Add(this.txtLyshaieAppointmentCount);
            this.grpICSSummary.Controls.Add(this.lblLyshaieAppointmentCount);
            this.grpICSSummary.Controls.Add(this.txtKoulaAppointmentCount);
            this.grpICSSummary.Controls.Add(this.txtClientCount);
            this.grpICSSummary.Controls.Add(this.txtAppointmentCount);
            this.grpICSSummary.Controls.Add(this.lblKoulaAppointmentCount);
            this.grpICSSummary.Controls.Add(this.lblClients);
            this.grpICSSummary.Controls.Add(this.lblAppointments);
            this.grpICSSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpICSSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpICSSummary.Location = new System.Drawing.Point(42, 151);
            this.grpICSSummary.Name = "grpICSSummary";
            this.grpICSSummary.Size = new System.Drawing.Size(474, 159);
            this.grpICSSummary.TabIndex = 16;
            this.grpICSSummary.TabStop = false;
            this.grpICSSummary.Text = "ICS Summary";
            // 
            // txtKoulaAppointmentCount
            // 
            this.txtKoulaAppointmentCount.Enabled = false;
            this.txtKoulaAppointmentCount.Location = new System.Drawing.Point(166, 85);
            this.txtKoulaAppointmentCount.Name = "txtKoulaAppointmentCount";
            this.txtKoulaAppointmentCount.ReadOnly = true;
            this.txtKoulaAppointmentCount.Size = new System.Drawing.Size(105, 20);
            this.txtKoulaAppointmentCount.TabIndex = 1;
            // 
            // txtClientCount
            // 
            this.txtClientCount.Enabled = false;
            this.txtClientCount.Location = new System.Drawing.Point(337, 41);
            this.txtClientCount.Name = "txtClientCount";
            this.txtClientCount.ReadOnly = true;
            this.txtClientCount.Size = new System.Drawing.Size(110, 20);
            this.txtClientCount.TabIndex = 1;
            // 
            // txtAppointmentCount
            // 
            this.txtAppointmentCount.Enabled = false;
            this.txtAppointmentCount.Location = new System.Drawing.Point(128, 41);
            this.txtAppointmentCount.Name = "txtAppointmentCount";
            this.txtAppointmentCount.ReadOnly = true;
            this.txtAppointmentCount.Size = new System.Drawing.Size(110, 20);
            this.txtAppointmentCount.TabIndex = 1;
            // 
            // lblKoulaAppointmentCount
            // 
            this.lblKoulaAppointmentCount.AutoSize = true;
            this.lblKoulaAppointmentCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKoulaAppointmentCount.Location = new System.Drawing.Point(23, 88);
            this.lblKoulaAppointmentCount.Name = "lblKoulaAppointmentCount";
            this.lblKoulaAppointmentCount.Size = new System.Drawing.Size(123, 13);
            this.lblKoulaAppointmentCount.TabIndex = 0;
            this.lblKoulaAppointmentCount.Text = "Koula Appointments:";
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClients.Location = new System.Drawing.Point(270, 44);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(49, 13);
            this.lblClients.TabIndex = 0;
            this.lblClients.Text = "Clients:";
            // 
            // lblAppointments
            // 
            this.lblAppointments.AutoSize = true;
            this.lblAppointments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppointments.Location = new System.Drawing.Point(23, 41);
            this.lblAppointments.Name = "lblAppointments";
            this.lblAppointments.Size = new System.Drawing.Size(87, 13);
            this.lblAppointments.TabIndex = 0;
            this.lblAppointments.Text = "Appointments:";
            // 
            // lblImportYear
            // 
            this.lblImportYear.AutoSize = true;
            this.lblImportYear.Location = new System.Drawing.Point(39, 103);
            this.lblImportYear.Name = "lblImportYear";
            this.lblImportYear.Size = new System.Drawing.Size(80, 13);
            this.lblImportYear.TabIndex = 21;
            this.lblImportYear.Text = "Year To Import:";
            // 
            // dpImportYear
            // 
            this.dpImportYear.CustomFormat = "yyyy";
            this.dpImportYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpImportYear.Location = new System.Drawing.Point(159, 96);
            this.dpImportYear.MinDate = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.dpImportYear.Name = "dpImportYear";
            this.dpImportYear.Size = new System.Drawing.Size(67, 20);
            this.dpImportYear.TabIndex = 20;
            this.dpImportYear.Value = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            // 
            // txtLyshaieAppointmentCount
            // 
            this.txtLyshaieAppointmentCount.Enabled = false;
            this.txtLyshaieAppointmentCount.Location = new System.Drawing.Point(166, 120);
            this.txtLyshaieAppointmentCount.Name = "txtLyshaieAppointmentCount";
            this.txtLyshaieAppointmentCount.ReadOnly = true;
            this.txtLyshaieAppointmentCount.Size = new System.Drawing.Size(105, 20);
            this.txtLyshaieAppointmentCount.TabIndex = 3;
            // 
            // lblLyshaieAppointmentCount
            // 
            this.lblLyshaieAppointmentCount.AutoSize = true;
            this.lblLyshaieAppointmentCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLyshaieAppointmentCount.Location = new System.Drawing.Point(23, 123);
            this.lblLyshaieAppointmentCount.Name = "lblLyshaieAppointmentCount";
            this.lblLyshaieAppointmentCount.Size = new System.Drawing.Size(134, 13);
            this.lblLyshaieAppointmentCount.TabIndex = 2;
            this.lblLyshaieAppointmentCount.Text = "Lyshaie Appointments:";
            // 
            // ImportScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.lblImportYear);
            this.Controls.Add(this.dpImportYear);
            this.Controls.Add(this.grpICSSummary);
            this.Controls.Add(this.btnParseICSFile);
            this.Controls.Add(this.btnOpenICSFile);
            this.Controls.Add(this.txtImportCalendar);
            this.Controls.Add(this.lblImportCalendar);
            this.Controls.Add(this.btnImportCalendar);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportScreen";
            this.Text = "Import Events";
            this.Load += new System.EventHandler(this.ImportScreen_Load);
            this.grpICSSummary.ResumeLayout(false);
            this.grpICSSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImportCalendar;
        private System.Windows.Forms.OpenFileDialog openICSFileDialog;
        private System.Windows.Forms.Button btnOpenICSFile;
        private System.Windows.Forms.TextBox txtImportCalendar;
        private System.Windows.Forms.Label lblImportCalendar;
        private System.Windows.Forms.Button btnParseICSFile;
        private System.Windows.Forms.GroupBox grpICSSummary;
        private System.Windows.Forms.TextBox txtKoulaAppointmentCount;
        private System.Windows.Forms.TextBox txtClientCount;
        private System.Windows.Forms.TextBox txtAppointmentCount;
        private System.Windows.Forms.Label lblKoulaAppointmentCount;
        private System.Windows.Forms.Label lblClients;
        private System.Windows.Forms.Label lblAppointments;
        private System.Windows.Forms.Label lblImportYear;
        private System.Windows.Forms.DateTimePicker dpImportYear;
        private System.Windows.Forms.TextBox txtLyshaieAppointmentCount;
        private System.Windows.Forms.Label lblLyshaieAppointmentCount;
    }
}