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
    using System.Runtime.Versioning;
    using itdevgeek_charites.presenters;

    /// <summary>
    /// Main application screen
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainScreen : Form, IMainView
    {
        /// <summary>Main Presenter instance for handling business logic</summary>
        private MainPresenter _presenter;

        public event EventHandler UpdateRequested;
        public event EventHandler SettingsRequested;
        public event EventHandler ImportRequested;
        public event EventHandler ExitRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainScreen" /> class.
        /// </summary>
        public MainScreen()
        {
            this.InitializeComponent();

            sbpanelDateTime.Text = DateTime.Today.ToLongDateString();
            sbpanelDateTime.ToolTipText = "DateTime: " + DateTime.Today.ToLongDateString();

            sbpanelAppStatus.Text = "Application started. No action yet.";

            // Initialize presenter
            _presenter = new MainPresenter(this);
        }

        /// <summary>
        /// Update the status text
        /// </summary>
        /// <param name="updatedText">text to add to the status</param>
        public void UpdateStatusText(string updatedText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatusText), new object[] { updatedText });
                return;
            }
            this.sbpanelAppStatus.Text = updatedText.Trim();
        }

        public void EnableActions(bool enable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(EnableActions), new object[] { enable });
                return;
            }
            btnSettings.Enabled = enable;
            btnImport.Enabled = enable;
            btnUpdate.Enabled = enable;
            btnExit.Enabled = enable;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
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

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            UpdateRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Exit button click operation
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Settings button operation, load the settings screen
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnSettings_Click(object sender, EventArgs e)
        {
            SettingsRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Import button operation, load the import screen
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportRequested?.Invoke(this, EventArgs.Empty);
        }

    }
}
