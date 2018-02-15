// -----------------------------------------------------
// <copyright file="MainScreen.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using itdevgeek_charites.helper.application;
    using itdevgeek_charites.screens;

    /// <summary>
    /// Main application screen
    /// </summary>
    public partial class MainScreen : Form
    {
        /// <summary>Application configuration screen</summary>
        private static ConfigurationScreen settingsScreen;

        /// <summary>background worker to perform google calendar update when manually initiated</summary>
        private BackgroundWorker backgroundSynchWorker = new BackgroundWorker();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainScreen" /> class.
        /// </summary>
        public MainScreen()
        {
            this.InitializeComponent();

            sbpanelDateTime.Text = DateTime.Today.ToLongDateString();
            sbpanelDateTime.ToolTipText = "DateTime: " + DateTime.Today.ToLongDateString();

            sbpanelAppStatus.Text = "Application started. No action yet.";

            this.backgroundSynchWorker.DoWork += new DoWorkEventHandler(this.BgSynchWorker_DoWork);
            this.backgroundSynchWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BgSynchWorker_RunWorkerCompleted);
        }

        /// <summary>
        /// Update the status text
        /// </summary>
        /// <param name="updatedText">text to add to the status</param>
        public void UpdateStatusText(string updatedText)
        {
            // Update code for new event and async patter to allow updating of ui from threads
            //this.sbpanelAppStatus.Text = updatedText.Trim();

        }

        /// <summary>
        /// Shwo the current window
        /// </summary>
        public void ShowWindow()
        {
            // Insert code here to make your form show itself.
            WinAPI.ShowToFront(this.Handle);
        }

        /// <summary>
        /// Override WndProc to account for single instance
        /// </summary>
        /// <param name="message">App window message</param>
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                this.ShowWindow();
            }

            base.WndProc(ref message);
        }

        /// <summary>
        /// Action when notification tray icon is double clicked
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        /// <summary>
        /// Update button action, perform google calendar sync
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (Charites.HaveRequiredData())
            {
                // disable buttons while performing update
                btnSettings.Enabled = false;
                btnUpdate.Enabled = false;
                btnExit.Enabled = false;

                this.backgroundSynchWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please update program settings to enter an Account and select a Calendar to Update.");
            }
        }

        /// <summary>
        /// Exit button click operation
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Settings button operation, load the settings screen
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnSettings_Click(object sender, EventArgs e)
        {
            if (settingsScreen == null || settingsScreen.IsDisposed)
            {
                settingsScreen = new ConfigurationScreen();
            }

            settingsScreen.TopMost = true;
            settingsScreen.StartPosition = FormStartPosition.CenterScreen;
            settingsScreen.Show();
            settingsScreen.Refresh();
        }

        /// <summary>
        /// Background worker, work completed, re-enable screen buttons
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void BgSynchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSettings.Enabled = true;
            btnUpdate.Enabled = true;
            btnExit.Enabled = true;
        }

        /// <summary>
        /// Background update worker, update the google calendar
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void BgSynchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Charites.UpdateGoogleCalendar();
        }

    }
}
