using Salon_Calendar_Integration.screens;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Salon_Calendar_Integration
{
    public partial class MainScreen : Form
    {

        private BackgroundWorker bkWorker = new BackgroundWorker();
        private static ProcessingScreen loadingScreen;

        private DateTime year;
        private string owner;
        private string calendarId;

        public MainScreen()
        {
            InitializeComponent();
            dpUpdateYear.Value = DateTime.Now;
            loadValues();

            bkWorker.DoWork += new DoWorkEventHandler(bkWorker_DoWork);
            bkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkWorker_RunWorkerCompleted);
        }

        protected void loadValues()
        {
            tbCalendarOwner.Text = AppConfiguration.Default.email_address.Trim();
            updateCalendarList();

            if (AppConfiguration.Default.calendar_id.Trim().Length > 0 && AppConfiguration.Default.calendar_name.Trim().Length > 0)
            {
                
                foreach (CalendarListItem item in cbCalendarToUpdate.Items)
                {
                    if (item.Value.ToString() == AppConfiguration.Default.calendar_id.Trim())
                    {
                        cbCalendarToUpdate.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            AppConfiguration.Default.email_address = tbCalendarOwner.Text.Trim();

            if (cbCalendarToUpdate.SelectedItem != null)
            {
                AppConfiguration.Default.calendar_id = (cbCalendarToUpdate.SelectedItem as CalendarListItem).Value.ToString();
                AppConfiguration.Default.calendar_name = (cbCalendarToUpdate.SelectedItem as CalendarListItem).Text.ToString();
            }

            AppConfiguration.Default.Save();
        }

        void bkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingScreen.Close();
        }

        void bkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!String.IsNullOrEmpty(owner) && !String.IsNullOrEmpty(calendarId) && year != null)
                Program.updateGoogleCalendar(owner, calendarId, year);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (tbCalendarOwner.Text.Trim().Length > 0 && cbCalendarToUpdate.SelectedItem != null)
            {
                if (loadingScreen == null || loadingScreen.IsDisposed)
                {
                    loadingScreen = new ProcessingScreen();
                }
                
                loadingScreen.TopMost = true;
                loadingScreen.StartPosition = FormStartPosition.CenterScreen;
                loadingScreen.Show();
                loadingScreen.Refresh();

                // force the wait window to display for at least 700ms so it doesn't just flash on the screen
                System.Threading.Thread.Sleep(700);

                year = dpUpdateYear.Value;
                owner = tbCalendarOwner.Text.Trim();
                calendarId = (cbCalendarToUpdate.SelectedItem as CalendarListItem).Value.ToString();

                bkWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please enter an email and select a Calendar to Update.");
            }
            
        }

        private void btnLoadCalendars_Click(object sender, EventArgs e)
        {
            updateCalendarList();
        }

        private void updateCalendarList()
        {
            if (tbCalendarOwner.Text.Trim().Length > 0)
            {
                CalendarListItem[] calendars = GCalHelper.getCalendars(tbCalendarOwner.Text.Trim());

                cbCalendarToUpdate.Items.Clear();

                for (int i = 0; i < calendars.Length; i++)
                {
                    cbCalendarToUpdate.Items.Add(calendars[i]);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            Close();
        }

    }
}
