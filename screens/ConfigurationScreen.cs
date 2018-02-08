// -----------------------------------------------------
// <copyright file="ConfigurationScreen.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.screens
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Configuration / Options screen
    /// </summary>
    public partial class ConfigurationScreen : Form
    {
        /// <summary>Class logger</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationScreen" /> class.
        /// </summary>
        public ConfigurationScreen()
        {
            this.InitializeComponent();

            dpUpdateYear.Value = DateTime.Now;

            CalendarListItem item = new CalendarListItem();
            item.Text = "Retrieving Calendars... Or None to Display.";
            item.Value = string.Empty;

            cbCalendarToUpdate.Items.Add(item);
            cbCalendarToUpdate.SelectedItem = item;
        }

        /// <summary>
        /// Load the initial settings from the AppConfiguration
        /// </summary>
        protected void LoadValuesFromAppSettings()
        {
            txtCalendarOwner.Text = AppConfiguration.Default.email_address.Trim();

            cbAutoUpdateCalendars.Checked = AppConfiguration.Default.run_in_background;

            int backgroundMins = AppConfiguration.Default.background_minutes;
            switch (backgroundMins)
            {
                case 60:
                    rdoOneHour.Checked = true;
                    break;
                case 120:
                    rdoTwoHours.Checked = true;
                    break;
                case 240:
                    rdoFourHours.Checked = true;
                    break;
                case 480:
                    rdoEightHours.Checked = true;
                    break;
                default:
                    rdoEightHours.Checked = true;
                    AppConfiguration.Default.background_minutes = 480;
                    AppConfiguration.Default.Save();
                    break;
            }
        }

        /// <summary>
        /// Update the calendar list dropdown from Google Calendar API
        /// </summary>
        private void UpdateCalendarList()
        {
            if (txtCalendarOwner.Text.Trim().Length > 0)
            {
                CalendarListItem[] calendars = GCalHelper.GetCalendars(txtCalendarOwner.Text.Trim());

                if (calendars != null && calendars.Length > 0)
                {
                    cbCalendarToUpdate.Items.Clear();

                    for (int i = 0; i < calendars.Length; i++)
                    {
                        cbCalendarToUpdate.Items.Add(calendars[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Save Settings button logic to save all options to the AppConfiguration
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            AppConfiguration.Default.email_address = txtCalendarOwner.Text.Trim();

            if (cbCalendarToUpdate.SelectedItem != null)
            {
                AppConfiguration.Default.calendar_id = (cbCalendarToUpdate.SelectedItem as CalendarListItem).Value.ToString();
                AppConfiguration.Default.calendar_name = (cbCalendarToUpdate.SelectedItem as CalendarListItem).Text.ToString();
            }

            AppConfiguration.Default.run_in_background = cbAutoUpdateCalendars.Checked;

            if (rdoEightHours.Checked)
            {
                AppConfiguration.Default.background_minutes = 480;
            }
            else if (rdoFourHours.Checked)
            {
                AppConfiguration.Default.background_minutes = 240;
            }
            else if (rdoTwoHours.Checked)
            {
                AppConfiguration.Default.background_minutes = 120;
            }
            else if (rdoOneHour.Checked)
            {
                AppConfiguration.Default.background_minutes = 60;
            }
            else
            {
                if (cbAutoUpdateCalendars.Checked)
                {
                    MessageBox.Show("Please select a valid update time period.");
                }
            }

            if (this.AllSettingsValid())
            {
                AppConfiguration.Default.Save();

                Charites._calendarOwner = AppConfiguration.Default.email_address;
                Charites._calendarId = AppConfiguration.Default.calendar_id;
                Charites._calendarYear = dpUpdateYear.Value;

                Charites._runInBackground = cbAutoUpdateCalendars.Checked;
                Charites._runInMinutes = AppConfiguration.Default.background_minutes;

                Charites.UpdatedSettings();

                Charites.UpdateProgramStatus("Updated program settings with new values.");

                this.Close();
            }
            else
            {
                MessageBox.Show("Please ensure all required settings have been provided.");
            }
        }

        /// <summary>
        /// Validate that all settins are entered and valid
        /// </summary>
        /// <returns>if all settings are valid</returns>
        private bool AllSettingsValid()
        {
            bool valid = true;

            if (string.IsNullOrEmpty(txtCalendarOwner.Text.Trim()))
            {
                return false;
            }

            if (string.IsNullOrEmpty((cbCalendarToUpdate.SelectedItem as CalendarListItem).Value.ToString()))
            {
                return false;
            }

            if (cbAutoUpdateCalendars.Checked)
            {
                if (!rdoEightHours.Checked && !rdoFourHours.Checked && !rdoTwoHours.Checked && !rdoOneHour.Checked)
                {
                    return false;
                }
            }

            return valid;
        }

        /// <summary>
        /// Load Calendar Button logic, update from google
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnLoadCalendars_Click(object sender, EventArgs e)
        {
            this.UpdateCalendarList();
        }

        /// <summary>
        /// Cancel Button, close form without saving settings
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Update the Calendar selection dropdoen items and set default display based on AppConfiguration
        /// </summary>
        private void UpdateCalendarSelectionItems()
        {
            if (AppConfiguration.Default.calendar_id.Trim().Length > 0 && AppConfiguration.Default.calendar_name.Trim().Length > 0)
            {
                this.UpdateCalendarList();
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

        /// <summary>
        /// Configuration screen loading operations, update based on current AppConfiguration
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void ConfigurationScreen_Load(object sender, EventArgs e)
        {
            this.LoadValuesFromAppSettings();

            Task.Factory.StartNew(() => { Invoke(new Action(UpdateCalendarSelectionItems)); });
        }

        /// <summary>
        /// Reset calendar button, remove all events from Google Calendar
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void BtnResetCalendar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCalendarOwner.Text.Trim()) || string.IsNullOrEmpty((cbCalendarToUpdate.SelectedItem as CalendarListItem).Value.ToString()))
            {
                MessageBox.Show("Cannot Reset Calendar as Owner and Calendar have not been selected.");
            }
            else
            {
                DialogResult answer = MessageBox.Show("Are you sure you wish to clear all events from the Calendar?", "caption", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (answer == DialogResult.Yes)
                {
                    GCalHelper.ClearAllEventsFromCalendar(txtCalendarOwner.Text.Trim(), (cbCalendarToUpdate.SelectedItem as CalendarListItem).Value.ToString(), null);
                    GCalHelper.googleCalEvents = null;
                }
            }
        }
    }
}
