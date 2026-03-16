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
    using System.Runtime.Versioning;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Chroniton;
    using Chroniton.Jobs;
    using Chroniton.Schedules;
    using datatypes;
    using helper.application;
    using helper.sql;
    using screens;

    /// <summary>
    /// Charites salon calendar synchronisation application.
    /// Synch Salon Iris appointments with Google Calendar.
    /// Allows salon to have a shared google calendar with staff so they can all see upcoming appointments.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class Charites
    {
        /// <summary>Log provider</summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>List of updated events</summary>
        private static List<GCalEventItem> _updatedEvents;

        /// <summary>List of deleted events</summary>
        private static List<GCalEventItem> _deletedEvents;

        /// <summary>List of newly created events</summary>
        private static List<GCalEventItem> _newEvents;

        /// <summary>Splash screen form</summary>
        private static SplashScreen _splashScreen;

        /// <summary>Main application form</summary>
        private static MainScreen _mainForm;

        /// <summary>Background job scheduler</summary>
        private static ISingularity _backgroundUpdateScheduler;

        private static SimpleJob _backgroundUpdateJob;

        private static IScheduledJob _scheduledUpdateJob;

        /// <summary>Gets or sets run in background setting</summary>
        public static bool RunInBackground { get; set; }

        /// <summary>Gets or sets how many minutes to wait between background runs</summary>
        public static int RunInMinutes { get; set; }

        /// <summary>Gets or sets Google calendar owner email address</summary>
        public static string CalendarOwner { get; set; }

        /// <summary>Gets or sets Google calendar ID</summary>
        public static string CalendarId { get; set; }

        /// <summary>Gets or sets Year to perform update/sync for</summary>
        public static DateTime CalendarYear { get; set; }

        /// <summary>
        /// The main entry point for the application.<br/>
        ///     - Verify that only one instance of the application is running at any one time.<br/>
        ///     - Set up background update scheduler and start if run in background is turned on in options.<br/>
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Log.Info("Launching Charites - Salon Calendar Integration Application....");

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
                if (RunInBackground)
                {
                    SetUpBackgroundJob();
                }

                // Initialize and show splashscreen.
                _splashScreen = new SplashScreen();
                _splashScreen.Show();

                // Create an instance of MainForm and hook into window/form shown and closed events.
                _mainForm = new MainScreen();
                _mainForm.Shown += Main_Shown;
                _mainForm.Resize += Main_Resize;
                _mainForm.FormClosed += Main_FormClosed;

                Thread.Sleep(3000);   // Pause for 3 second to show splashscreen
                Application.Run(_mainForm);
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
            if (sender is Form form) form.Hide();

            // Show the splash screen
            _splashScreen.Show();
            System.Threading.Thread.Sleep(1000);   // Pause for a second to show splash

            if (RunInBackground)
            {
                EndBackgroundJob();
            }

            if (Application.MessageLoop)
            {
                // WinForms app
                Log.Info("Closing Charites - Salon Calendar Integration Application.");
                Application.Exit();
            }
            else
            {
                // Console app
                Log.Info("Closing Charites - Salon Calendar Integration Application.");
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
            _splashScreen.Hide();
        }

        /// <summary>
        /// Handle minimising the form to the tray icon.
        /// </summary>
        /// <param name="sender">Form being resized causing the event</param>
        /// <param name="e">Event arguments</param>
        public static void Main_Resize(object sender, EventArgs e)
        {
            // Minimise to the tray icon 
            if (_mainForm.WindowState == FormWindowState.Minimized)
            {
                _mainForm.Hide();
                _mainForm.notifyIcon.Visible = true;
            }
            else
            {
                if (_mainForm.notifyIcon.Visible)
                {
                    _mainForm.notifyIcon.Visible = false;
                }
            }
        }

        /// <summary>
        /// Find window function to allow locating application if launched more than once.
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="windowName">Window name</param>
        /// <returns>Window pointer</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowName);

        /// <summary>
        /// Display the window of an application.
        /// </summary>
        /// <param name="wnd">Pointer to the window</param>
        /// <param name="cmdShow">Show window value</param>
        /// <returns>Success value</returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr wnd, int cmdShow);

        /// <summary>
        /// Bring window to foreground. Used to display infront if app already running and launched again.
        /// </summary>
        /// <param name="wnd">Pointer to the application window</param>
        /// <returns>Success value</returns>
        [DllImport("user32.dll")]
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
        public static async Task UpdateGoogleCalendarAsync()
        {
            if (HaveRequiredData())
            {
                UpdateProgramStatus("Performing calendar update...");
                Log.Info("Starting the update Process...");

                // Show tool tip in case application is minimised to tray
                _mainForm.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                _mainForm.notifyIcon.BalloonTipTitle = "Performing Update";
                _mainForm.notifyIcon.BalloonTipText = "Updating the Google Calendar with latest Salon Appointments...";
                _mainForm.notifyIcon.ShowBalloonTip(5000);

                // Get Google Calendar Events
                if (GCalHelper.googleCalEvents == null)
                {
                    await GCalHelper.GetYearlyEventsAsync(CalendarOwner, CalendarId, CalendarYear, null);
                }

                List<GCalEventItem> currentGoogEvents = GCalHelper.googleCalEvents;

                // Load Values From Salon Calendar
                DBHelper.InitData(CalendarYear.Year);
                List<GCalEventItem> currentSalonEvents = DBHelper.ConvertSQLTicketsToEvents();
                if (currentSalonEvents != null && currentSalonEvents.Count > 0)
                {
                    // get New Events to create
                    if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                    {
                        _newEvents = currentSalonEvents.Except(currentGoogEvents).ToList();
                    }
                    else
                    {
                        _newEvents = currentSalonEvents;
                    }

                    // get events that need to delete
                    if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                    {
                        _deletedEvents = currentGoogEvents.Except(currentSalonEvents).ToList();
                    }
                    else
                    {
                        _deletedEvents = new List<GCalEventItem>();
                    }

                    // get Events that need to update
                    if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                    {
                        List<GCalEventItem> potentialUpdatedEvents = currentSalonEvents.Except(_newEvents).ToList();
                        foreach (GCalEventItem e in potentialUpdatedEvents)
                        {
                            GCalEventItem ge = currentGoogEvents.Find(x => x.SalonCalendarId == e.SalonCalendarId);
                            if (ge != null)
                            {
                                e.EventId = ge.EventId;
                            }
                        }

                        List<GCalEventItem> noGoogleIdEvents = potentialUpdatedEvents.FindAll(x => x.EventId == null);
                        if (noGoogleIdEvents.Count > 0)
                        {
                            _newEvents = _newEvents.Concat(noGoogleIdEvents.Except(_newEvents)).ToList();
                            potentialUpdatedEvents = potentialUpdatedEvents.Except(noGoogleIdEvents).ToList();
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
                                    Math.Abs(entryToCheck.DurationMinutes - ev.DurationMinutes) > 0.001)
                                {
                                    // need to update so leave in list
                                    Log.Debug("Need to update event ->" + entryToCheck.SalonCalendarId);
                                }
                                else
                                {
                                    unchangedEvents.Add(entryToCheck);
                                }
                            }
                        }

                        _updatedEvents = potentialUpdatedEvents.Except(unchangedEvents).ToList();
                    }
                    else
                    {
                        _updatedEvents = new List<GCalEventItem>();
                    }

                    await GCalHelper.UpdateGoogleCalendarAsync(CalendarOwner, CalendarId, _newEvents, _updatedEvents, _deletedEvents);

                    Log.Info("Finished the update Process...");
                    UpdateProgramStatus("Calendar update completed - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));

                    // Show tool tip in case application is minimised to tray
                    _mainForm.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                    _mainForm.notifyIcon.BalloonTipTitle = "Update Complete";
                    _mainForm.notifyIcon.BalloonTipText = "Update with latest Salon Appointments complete - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
                    _mainForm.notifyIcon.ShowBalloonTip(5000);
                }
                else
                {
                    Log.Info("Finished the update Process...");
                    UpdateProgramStatus("Calendar update error - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));

                    // Show tool tip in case application is minimised to tray
                    _mainForm.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                    _mainForm.notifyIcon.BalloonTipTitle = "Update Error";
                    _mainForm.notifyIcon.BalloonTipText = "Could not update with latest Salon Appointments - " + DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
                    _mainForm.notifyIcon.ShowBalloonTip(5000);
                }
            }
            else
            {
                Log.Error("Could not perform update as required data has not been set.");
                MessageBox.Show("Please provide the Calendar Owner and other Calendar details before updating.");
            }
        }

        /// <summary>
        /// Update the status bar text on the main form.
        /// </summary>
        /// <param name="statusText">New status text to display</param>
        public static void UpdateProgramStatus(string statusText)
        {
            _mainForm.UpdateStatusText(statusText);
        }

        /// <summary>
        /// Verify all required Google calendar information has been provided for an update
        /// </summary>
        /// <returns>Whether application has initialised with required data</returns>
        public static bool HaveRequiredData()
        {
            if (string.IsNullOrEmpty(CalendarId) || string.IsNullOrEmpty(CalendarOwner))
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
            if (!RunInBackground)
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
            if (_backgroundUpdateScheduler == null)
            {
                ISingularityFactory factory = new SingularityFactory();
                _backgroundUpdateScheduler = factory.GetSingularity();
            }

            if (_scheduledUpdateJob != null)
            {
                _backgroundUpdateScheduler.StopScheduledJob(_scheduledUpdateJob);
                _scheduledUpdateJob = null;
                _backgroundUpdateScheduler.Stop();
            }

            if (RunInMinutes > 0)
            {
                var schedule = new EveryXTimeSchedule(TimeSpan.FromMinutes(RunInMinutes));
                _backgroundUpdateJob ??= new SimpleJob(_ => UpdateGoogleCalendarAsync().Wait());
                _scheduledUpdateJob = _backgroundUpdateScheduler.ScheduleJob(schedule, _backgroundUpdateJob, false);
            }

            _backgroundUpdateScheduler.Start();
        }

        /// <summary>
        /// Stops the background scheduler that updates the calendar in the background
        /// </summary>
        private static void EndBackgroundJob()
        {
            if (_scheduledUpdateJob != null)
            {
                if (_backgroundUpdateScheduler != null)
                {
                    _backgroundUpdateScheduler.StopScheduledJob(_scheduledUpdateJob);
                }
                _scheduledUpdateJob = null;
            }

            if (_backgroundUpdateScheduler != null)
            {
                _backgroundUpdateScheduler.Stop();
                _backgroundUpdateScheduler = null;
            }
        }

        /// <summary>
        /// Load the initial application settings from app configuration file on launch.
        /// </summary>
        private static void GetAppSettingValues()
        {
            RunInBackground = AppConfiguration.Default.run_in_background;

            RunInMinutes = AppConfiguration.Default.background_minutes;
            if (RunInMinutes != 60 && RunInMinutes != 120 && RunInMinutes != 240 && RunInMinutes != 480)
            {
                // default to every 8 hours
                RunInMinutes = 480;
                AppConfiguration.Default.background_minutes = RunInMinutes;
                AppConfiguration.Default.Save();
            }

            if (!string.IsNullOrEmpty(AppConfiguration.Default.email_address.Trim()))
            {
                CalendarOwner = AppConfiguration.Default.email_address.Trim();
            }

            if (!string.IsNullOrEmpty(AppConfiguration.Default.calendar_id.Trim()))
            {
                CalendarId = AppConfiguration.Default.calendar_id.Trim();
            }

            CalendarYear = DateTime.Now;
        }
    }
}
