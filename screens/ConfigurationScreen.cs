using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace itdevgeek_charites.screens
{
    public partial class ConfigurationScreen : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ConfigurationScreen()
        {
            InitializeComponent();

            dpUpdateYear.Value = DateTime.Now;

            CalendarListItem item = new CalendarListItem();
            item.Text = "Retrieving Calendars... Or None to Display.";
            item.Value = "";

            cbCalendarToUpdate.Items.Add(item);
            cbCalendarToUpdate.SelectedItem = item;
        }

        protected void loadValuesFromAppSettings()
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

        private void updateCalendarList()
        {
            if (txtCalendarOwner.Text.Trim().Length > 0)
            {
                CalendarListItem[] calendars = GCalHelper.getCalendars(txtCalendarOwner.Text.Trim());

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

        private void btnSaveSettings_Click(object sender, EventArgs e)
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

            if (allSettingsValid())
            {
                AppConfiguration.Default.Save();

                Charites._calendarOwner = AppConfiguration.Default.email_address;
                Charites._calendarId = AppConfiguration.Default.calendar_id;
                Charites._calendarYear = dpUpdateYear.Value;

                Charites._runInBackground = true;
                Charites._runInMinutes = AppConfiguration.Default.background_minutes;

                Charites._updatedSettings = true;

                Charites.updateProgramStatus("Updated program settings with new values.");

                Close();
            }
            else
            {
                MessageBox.Show("Please ensure all required settings have been provided.");
            }

            
        }

        private bool allSettingsValid()
        {
            bool valid = true;

            if (String.IsNullOrEmpty(txtCalendarOwner.Text.Trim()))
            {
                return false;
            }

            if (String.IsNullOrEmpty((cbCalendarToUpdate.SelectedItem as CalendarListItem).Value.ToString()))
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

        private void btnLoadCalendars_Click(object sender, EventArgs e)
        {
            updateCalendarList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void updateCalendarSelectionItems()
        {
            if (AppConfiguration.Default.calendar_id.Trim().Length > 0 && AppConfiguration.Default.calendar_name.Trim().Length > 0)
            {
                updateCalendarList();
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

        private void ConfigurationScreen_Load(object sender, EventArgs e)
        {
            loadValuesFromAppSettings();

            Task.Factory.StartNew
            (
                () =>
                {
                    Invoke(new Action(updateCalendarSelectionItems));
                }
            );
        }
    }
}
