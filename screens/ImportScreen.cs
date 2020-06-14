using itdevgeek_charites.datatypes;
using itdevgeek_charites.helper.ics;
using itdevgeek_charites.helper.import;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace itdevgeek_charites.screens
{
    public partial class ImportScreen : Form
    {
        /// <summary>background worker to perform calendar import when manually initiated</summary>
        private BackgroundWorker backgroundImportWorker = new BackgroundWorker();

        /// <summary>background worker to perform calendar parsing when manually initiated</summary>
        private BackgroundWorker backgroundParseWorker = new BackgroundWorker();

        /// <summary>whether the ICS file has been selected to import from</summary>
        private bool selectedICSFile;

        /// <summary>whether the ICS file has been parsed into memory</summary>
        private bool parsedICSFile;


        public ImportScreen()
        {
            InitializeComponent();

            this.backgroundImportWorker.DoWork += new DoWorkEventHandler(this.BgImportWorker_DoWork);
            this.backgroundImportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BgImportWorker_RunWorkerCompleted);

            this.backgroundParseWorker.DoWork += new DoWorkEventHandler(this.BgParseWorker_DoWork);
            this.backgroundParseWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BgParseWorker_RunWorkerCompleted);
        }

        private void ImportScreen_Load(object sender, EventArgs e)
        {
            selectedICSFile = false;
            parsedICSFile = false;

            // Set initial data directory
            string initialDirectory = Directory.GetCurrentDirectory() + @"\data";

            if (!Directory.Exists(initialDirectory))
            {
                openICSFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            else
            {
                openICSFileDialog.InitialDirectory = initialDirectory;
            }
        }

        /// <summary>
        /// Select the ICS file to load data from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenICSFile_Click(object sender, EventArgs e)
        {
            selectedICSFile = false;
            if (openICSFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtImportCalendar.Text = openICSFileDialog.FileName;
                selectedICSFile = true;
            }
            else
            {
                txtImportCalendar.Text = "";
            }

        }

        /// <summary>
        /// Import button action, perform calendar import
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnImportCalendar_Click(object sender, EventArgs e)
        {
            if (selectedICSFile && parsedICSFile)
            {
                // disable buttons while performing import
                btnOpenICSFile.Enabled = false;
                btnParseICSFile.Enabled = false;
                btnImportCalendar.Enabled = false;
                btnCancel.Enabled = false;

                this.backgroundImportWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please select a calendar to import and parse the initial data.");
            }
        }

        /// <summary>
        /// Cancel Button, close form without importing anything
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Parse Button action, parse and read ICS file into memory
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnParseICSFile_Click(object sender, EventArgs e)
        {
            if (selectedICSFile)
            {
                // disable buttons while performing import
                btnOpenICSFile.Enabled = false;
                btnParseICSFile.Enabled = false;
                btnImportCalendar.Enabled = false;
                btnCancel.Enabled = false;

                txtAppointmentCount.Text = "-";
                txtClientCount.Text = "-";
                txtKoulaAppointmentCount.Text = "-";
                txtLyshaieAppointmentCount.Text = "-";

                this.backgroundParseWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please select a calendar to parse.");
            }
        }

        /// <summary>
        /// Background worker, parsing work completed, re-enable screen buttons
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void BgParseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            txtAppointmentCount.Text = ImportHelper.totalAppointments.ToString();
            txtClientCount.Text = ImportHelper.totalClients.ToString();
            txtKoulaAppointmentCount.Text = ImportHelper.koulaAppointments.ToString();
            txtLyshaieAppointmentCount.Text = ImportHelper.lyshaieAppointments.ToString();

            btnOpenICSFile.Enabled = true;
            btnParseICSFile.Enabled = true;
            btnImportCalendar.Enabled = true;
            btnCancel.Enabled = true;
        }

        /// <summary>
        /// Background import worker, import the loaded details into Salon Iris
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void BgParseWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<ICSCalEventItem> icsEvents = ICSHelper.ParseICSCalendarFile(txtImportCalendar.Text);
            ImportHelper.ConvertICSEvents(icsEvents);
        }

        /// <summary>
        /// Background worker, import work completed, re-enable screen buttons
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void BgImportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnOpenICSFile.Enabled = true;
            btnParseICSFile.Enabled = true;
            btnImportCalendar.Enabled = true;
            btnCancel.Enabled = true;
        }

        /// <summary>
        /// Background import worker, import the loaded details into Salon Iris
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void BgImportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //TODO: Charites.UpdateGoogleCalendar();
            //ImportHelper.
        }

    }
}
