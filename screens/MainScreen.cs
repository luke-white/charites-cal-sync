using itdevgeek_charites.helper.application;
using itdevgeek_charites.screens;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace itdevgeek_charites
{
    public partial class MainScreen : Form
    {
        private BackgroundWorker bgSynchWorker = new BackgroundWorker();

        private static ConfigurationScreen settingsScreen;

        public MainScreen()
        {
            InitializeComponent();

            sbpanelDateTime.Text = DateTime.Today.ToLongDateString();
            sbpanelDateTime.ToolTipText = "DateTime: " + DateTime.Today.ToLongDateString();

            sbpanelAppStatus.Text = "Application started. No action yet.";

            bgSynchWorker.DoWork += new DoWorkEventHandler(bgSynchWorker_DoWork);
            bgSynchWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgSynchWorker_RunWorkerCompleted);
        }

        void bgSynchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSettings.Enabled = true;
            btnUpdate.Enabled = true;
            btnExit.Enabled = true;
        }

        void bgSynchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Charites.updateGoogleCalendar();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Charites.haveRequiredData())
            {
                btnSettings.Enabled = false;
                btnUpdate.Enabled = false;
                btnExit.Enabled = false;

                bgSynchWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please update program settings to enter an Account and select a Calendar to Update.");
            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSettings_Click(object sender, EventArgs e)
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

        public void updateStatusText(string updatedText)
        {
            sbpanelAppStatus.Text = updatedText.Trim();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }


        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }
            base.WndProc(ref message);
        }

        public void ShowWindow()
        {
            // Insert code here to make your form show itself.
            WinAPI.ShowToFront(this.Handle);
        }


    }
}
