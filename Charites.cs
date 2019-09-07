// -----------------------------------------------------
// <copyright file="Charites.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using Chroniton;
    using Chroniton.Jobs;
    using Chroniton.Schedules;
    using itdevgeek_charites.datatypes;
    using itdevgeek_charites.helper.application;
    using itdevgeek_charites.helper.sql;
    using itdevgeek_charites.screens;

    /// <summary>
    /// Charites salon calendar synchronisation application.
    /// Synch Salon Iris appointments with Google Calendar.
    /// Allows salon to have a shared google calendar with staff so they can all see upcoming appointments.
    /// </summary>
    public static class Charites
    {
        /// <summary>Log provider</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>List of updated events</summary>
        private static List<GCalEventItem> updatedEvents;

        /// <summary>List of deleted events</summary>
        private static List<GCalEventItem> deletedEvents;

        /// <summary>List of newly created events</summary>
        private static List<GCalEventItem> newEvents;

        /// <summary>Splash screen form</summary>
        private static SplashScreen splashScreen;

        /// <summary>Main application form</summary>
        private static MainScreen mainForm;

        /// <summary>Background job scheduler</summary>
        private static ISingularity backgroundUpdateScheduler;

        private static SimpleJob backgroundUpdateJob;

        private static IScheduledJob scheduledUpdateJob;

        /// <summary>Gets or sets run in background setting</summary>
        public static bool _runInBackground { get; set; }

        /// <summary>Gets or sets how many minutes to wait between background runs</summary>
        public static int _runInMinutes { get; set; }

        /// <summary>Gets or sets Google calendar owner email address</summary>
        public static string _calendarOwner { get; set; }

        /// <summary>Gets or sets Google calendar ID</summary>
        public static string _calendarId { get; set; }

        /// <summary>Gets or sets Year to perform update/sync for</summary>
        public static DateTime _calendarYear { get; set; }

        /// <summary>
        /// The main entry point for the application.<br/>
        ///     - Verify that only one instance of the application is running at any one time.<br/>
        ///     - Set up background update scheduler and start if run in background is turned on in options.<br/>
        /// </summary>
        [STAThread]
        public static void Main()
        {
            log.Info("Launching Charites - Salon Calendar Integration Application....");

            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // get the current app settings
                GetAppSettingValues();
                if (_runInBackground)
                {
                    SetUpBackgroundJob();
                }

                // Initialize and show splashscreen.
                splashScreen = new SplashScreen();
                splashScreen.Show();

                // Create an instance of MainForm and hook into window/form shown and closed events.
                mainForm = new MainScreen();
                mainForm.Shown += Main_Shown;
                mainForm.Resize += Main_Resize;
                mainForm.FormClosed += Main_FormClosed;

                Thread.Sleep(3000);   // Pause for 3 second to show splashscreen
                Application.Run(mainForm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            SingleInstance.Stop();
        }

        /// <summary>
        /// Show splash screen when closing application then exit
        /// </summary>
        /// <param name="sender">The form being closed</param>
        /// <param name="e">event arguments</param>
        public static void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Hide the calling form  
            Form form = sender as Form;
            form.Hide();

            // Show the splash screen
            splashScreen.Show();
            System.Threading.Thread.Sleep(1000);   // Pause for a second to show splash

            if (_runInBackground)
            {
                EndBackgroundJob();
            }

            if (Application.MessageLoop)
            {
                // WinForms app
                log.Info("Closing Charites - Salon Calendar Integration Application.");
                Application.Exit();
            }
            else
            {
                // Console app
                log.Info("Closing Charites - Salon Calendar Integration Application.");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Display the main application window and hide the splash screen.
        /// </summary>
        /// <param name="sender">Form being displayed causing the event</param>
        /// <param name="e">The event arguments</param>
        public static void Main_Shown(object sender, EventArgs e)
        {
            // Hide the splashscreen. 
            splashScreen.Hide();
        }

        /// <summary>
        /// Handle minimising the form to the tray icon.
        /// </summary>
        /// <param name="sender">Form being resized causing the event</param>
        /// <param name="e">Event arguments</param>
        public static void Main_Resize(object sender, EventArgs e)
        {
            // Minimise to the tray icon 
            if (mainForm.WindowState == FormWindowState.Minimized)
            {
                mainForm.Hide();
                mainForm.notifyIcon.Visible = true;
            }
            else
            {
                if (mainForm.notifyIcon.Visible)
                {
                    mainForm.notifyIcon.Visible = false;
                }
            }
        }

        /// <summary>
        /// Find window function to allow locating application if launched more than once.
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="windowName">Window name</param>
        /// <returns>Window pointer</returns>
        [DllImportAttribute("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowName);

        /// <summary>
        /// Display the window of an application.
        /// </summary>
        /// <param name="wnd">Pointer to the window</param>
        /// <param name="cmdShow">Show window value</param>
        /// <returns>Success value</returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr wnd, int cmdShow);

        /// <summary>
        /// Bring window to foreground. Used to display infront if app already running and launched again.
        /// </summary>
        /// <param name="wnd">Pointer to the application window</param>
        /// <returns>Success value</returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr wnd);

        /// <summary>
        /// Bring the application instance to front, used if launched more than once.
        /// </summary>
        /// <param name="windowName">Window name to bring to front</param>
        public static void ShowToFront(string windowName)
        {
            IntPtr firstInstance = FindWindow(null, windowName);
            ShowWindow(firstInstance, 1);
            SetForegroundWindow(firstInstance);
        }

        /// <summary>
        /// Perform the update of the Google Calendar
        /// </summary>
        public static void UpdateGoogleCalendar()
        {
            if (HaveRequiredData())
            {
                UpdateProgramStatus("Performing calendar update...");
                log.Info("Starting the update Process...");

                // Show tool tip in case application is minimised to tray
                mainForm.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                mainForm.notifyIcon.BalloonTipTitle = "Performing Update";
                mainForm.notifyIcon.BalloonTipText = "Updating the Google Calendar with latest Salon Appointments...";
                mainForm.notifyIcon.ShowBalloonTip(5000);

                // Get Google Calendar Events
                if (GCalHelper.googleCalEvents == null)
                {
                    GCalHelper.GetYearlyEvents(_calendarOwner, _calendarId, _calendarYear, null);
                }

                List<GCalEventItem> currentGoogEvents = GCalHelper.googleCalEvents;

                // Load Values From Salon Calendar
                DBHelper.InitData(_calendarYear.Year);
                List<GCalEventItem> currentSalonEvents = DBHelper.ConvertSQLTicketsToEvents();
                if (currentSalonEvents != null && currentSalonEvents.Count > 0)
                {
                    // get New Events to create
                    if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                    {
                        newEvents = (List<GCalEventItem>)currentSalonEvents.Except(currentGoogEvents).ToList();
                    }
                    else
                    {
                        newEvents = currentSalonEvents;
                    }

                    // get events that need to delete
                    if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                    {
                        deletedEvents = (List<GCalEventItem>)currentGoogEvents.Except(currentSalonEvents).ToList();
                    }
                    else
                    {
                        deletedEvents = new List<GCalEventItem>();
                    }

                    // get Events that need to update
                    if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                    {
                        List<GCalEventItem> potentialUpdatedEvents = (List<GCalEventItem>)currentSalonEvents.Except(newEvents).ToList();
                        foreach (GCalEventItem e in potentialUpdatedEvents)
                        {
                            GCalEventItem ge = currentGoogEvents.Find(x => x.SalonCalendarId == e.SalonCalendarId);
                            if (ge != null)
                            {
                                e.EventId = ge.EventId;
                            }
                        }

                        List<GCalEventItem> noGoogleIdEvents = potentialUpdatedEvents.FindAll(x => x.EventId == null).ToList();
                        if (noGoogleIdEvents != null && noGoogleIdEvents.Count > 0)
                        {
                            newEvents = (List<GCalEventItem>)newEvents.Concat(noGoogleIdEvents.Except(newEvents)).ToList();
                            potentialUpdatedEvents = (List<GCalEventItem>)potentialUpdatedEvents.Except(noGoogleIdEvents).ToList();
                        }

                        List<GCalEventItem> unchangedEvents = new List<GCalEventItem>();
                        foreach (var ev in potentialUpdatedEvents)
                        {
                            var entryToCheck = currentGoogEvents.FirstOrDefault(x => x.SalonCalendarId == ev.SalonCalendarId);
                            if (entryToCheck != null)
                            {
                                if (entryToCheck.AppointmentType != ev.AppointmentType ||
                                    entryToCheck.Client != ev.Client ||
                                    entryToCheck.StaffMember != ev.StaffMember ||
                                    entryToCheck.StartTime != ev.StartTime ||
                                    entryToCheck.EndTime != ev.EndTime ||
                                    entryToCheck.DurationMinutes != ev.DurationMinutes)
                                {
                                    // need to update so leave in list
                                    log.Debug("Need to update event ->" + entryToCheck.SalonCalendarId);
                                }
                                else
                                {
                                    unchangedEvents.Add(entryToCheck);
                                }
                            }
                        }

                        updatedEvents = (List<GCalEventItem>)potentialUpdatedEvents.Except(unchangedEvents).ToList();
                    }
                    else
                    {
                        updatedEvents = new List<GCalEventItem>();
                    }

                    GCalHelper.UpdateGoogleCalendar(_calendarOwner, _calendarId, newEvents, updatedEvents, deletedEvents);

                    log.Info("Finished the update Process...");
                    UpdateProgramStatus("Calendar update completed - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));

                    // Show tool tip in case application is minimised to tray
                    mainForm.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                    mainForm.notifyIcon.BalloonTipTitle = "Update Complete";
                    mainForm.notifyIcon.BalloonTipText = "Update with latest Salon Appointments complete - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
                    mainForm.notifyIcon.ShowBalloonTip(5000);
                }
                else
                {
                    log.Info("Finished the update Process...");
                    UpdateProgramStatus("Calendar update error - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));

                    // Show tool tip in case application is minimised to tray
                    mainForm.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                    mainForm.notifyIcon.BalloonTipTitle = "Update Error";
                    mainForm.notifyIcon.BalloonTipText = "Could not update with latest Salon Appointments - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
                    mainForm.notifyIcon.ShowBalloonTip(5000);
                }
            }
            else
            {
                log.Error("Could not perform update as required data has not been set.");
                MessageBox.Show("Please provide the Calendar Owner and other Calendar details before updating.");
            }
        }

        /// <summary>
        /// Update the status bar text on the main form.
        /// </summary>
        /// <param name="statusText">New status text to display</param>
        public static void UpdateProgramStatus(string statusText)
        {
            mainForm.UpdateStatusText(statusText);
        }

        /// <summary>
        /// Verify all required Google calendar information has been provided for an update
        /// </summary>
        /// <returns>Whether application has initialised with required data</returns>
        public static bool HaveRequiredData()
        {
            if (string.IsNullOrEmpty(_calendarId) || string.IsNullOrEmpty(_calendarOwner) || _calendarYear == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Update the background scheduler if application settings have been modified.
        /// Stop scheduler if run in background is no longer enabled or vise versa.
        /// </summary>
        public static void UpdatedSettings()
        {
            if (!_runInBackground)
            {
                EndBackgroundJob();
            }
            else
            {
                SetUpBackgroundJob();
            }
        }

        /// <summary>
        /// Configure the background scheduler that updates the calendar in the background
        /// </summary>
        private static void SetUpBackgroundJob()
        {
            if (backgroundUpdateScheduler == null)
            {
                ISingularityFactory factory = new SingularityFactory();
                backgroundUpdateScheduler = factory.GetSingularity();
            }

            if (backgroundUpdateJob == null)
            {
                backgroundUpdateJob = new SimpleJob(scheduledTime => UpdateGoogleCalendar());
            }

            if (scheduledUpdateJob != null)
            {
                backgroundUpdateScheduler.StopScheduledJob(scheduledUpdateJob);
                scheduledUpdateJob = null;
                backgroundUpdateScheduler.Stop();
            }

            if (_runInMinutes > 0)
            {
                var schedule = new EveryXTimeSchedule(TimeSpan.FromMinutes(_runInMinutes));
                scheduledUpdateJob = backgroundUpdateScheduler.ScheduleJob(schedule, backgroundUpdateJob, true);
            }

            backgroundUpdateScheduler.Start();
        }

        /// <summary>
        /// Stops the background scheduler that updates the calendar in the background
        /// </summary>
        private static void EndBackgroundJob()
        {
            if (scheduledUpdateJob != null)
            {
                if (backgroundUpdateScheduler != null)
                {
                    backgroundUpdateScheduler.StopScheduledJob(scheduledUpdateJob);
                }
                scheduledUpdateJob = null;
            }

            if (backgroundUpdateScheduler != null)
            {
                backgroundUpdateScheduler.Stop();
                backgroundUpdateScheduler = null;
            }
        }

        /// <summary>
        /// Load the initial application settings from app configuration file on launch.
        /// </summary>
        private static void GetAppSettingValues()
        {
            _runInBackground = false;
            if (AppConfiguration.Default.run_in_background)
            {
                _runInBackground = true;
            }

            _runInMinutes = AppConfiguration.Default.background_minutes;
            if (_runInMinutes != 60 && _runInMinutes != 120 && _runInMinutes != 240 && _runInMinutes != 480)
            {
                // default to every 8 hours
                _runInMinutes = 480;
                AppConfiguration.Default.background_minutes = _runInMinutes;
                AppConfiguration.Default.Save();
            }

            if (!string.IsNullOrEmpty(AppConfiguration.Default.email_address.Trim()))
            {
                _calendarOwner = AppConfiguration.Default.email_address.Trim();
            }

            if (!string.IsNullOrEmpty(AppConfiguration.Default.calendar_id.Trim()))
            {
                _calendarId = AppConfiguration.Default.calendar_id.Trim();
            }

            _calendarYear = DateTime.Now;
        }
    }
}
