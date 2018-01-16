using CrystalJobScheduler;
using itdevgeek_charites.datatypes;
using itdevgeek_charites.helper.sql;
using itdevgeek_charites.screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace itdevgeek_charites
{
    static class Charites
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool _runInBackground { get; set; }
        public static int _runInMinutes { get; set; }

        public static string _calendarOwner { get; set; }
        public static string _calendarId { get; set; }
        public static DateTime _calendarYear { get; set; }

        public static List<GCalEventItem> _updatedEvents;
        public static List<GCalEventItem> _deletedEvents;
        public static List<GCalEventItem> _newEvents;

        // Create an instance of the splashscreen 
        public static SplashScreen splashScreen;
        public static MainScreen mainForm;

        public static JobScheduler backgroundUpdateScheduler;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log.Info("Launching Charites - Salon Calendar Integration Application....");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            getAppSettingValues();
            if (_runInBackground)
            {
                setUpBackgroundJob();
                if (backgroundUpdateScheduler != null)
                {
                    backgroundUpdateScheduler.Start();
                }
            }

            // Initialize and show splashscreen. Say the welcome message. 
            splashScreen = new SplashScreen();
            splashScreen.Show();

            // Create an instance of MainForm and hook into shown and closed events.
            mainForm = new MainScreen();
            mainForm.Shown += main_Shown;
            mainForm.Resize += main_Resize;
            mainForm.FormClosed += main_FormClosed;

            System.Threading.Thread.Sleep(3000);   // Pause for 3 second.
            Application.Run(mainForm);
        }

        static void main_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Hide the calling form  
            Form form = sender as Form;
            form.Hide();

            // Show the splash screen
            splashScreen.Show();
            System.Threading.Thread.Sleep(1000);   // Pause for a second.

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

        static void main_Shown(object sender, EventArgs e)
        {
            // Hide the splashscreen. 
            splashScreen.Hide();
        }

        static void main_Resize(object sender, EventArgs e)
        {
            if (mainForm.WindowState == FormWindowState.Minimized)
            {
                mainForm.Hide();
                mainForm.notifyIcon.Visible = true;
            }
        }

        private static void getAppSettingValues()
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

            if (!String.IsNullOrEmpty(AppConfiguration.Default.email_address.Trim()))
            {
                _calendarOwner = AppConfiguration.Default.email_address.Trim();
            }

            if (!String.IsNullOrEmpty(AppConfiguration.Default.calendar_id.Trim()))
            {
                _calendarId = AppConfiguration.Default.calendar_id.Trim();
            }

            _calendarYear = DateTime.Now;
        }

        public static void updateGoogleCalendar()
        {
            if (haveRequiredData())
            {
                log.Info("Starting the update Process...");
                mainForm.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                mainForm.notifyIcon.BalloonTipTitle = "Performing Update";
                mainForm.notifyIcon.BalloonTipText = "Updating the Google Calendar with latest Salon Appointments...";
                mainForm.notifyIcon.ShowBalloonTip(5000);

                // Get Google Calendar Events
                if (GCalHelper.googleCalEvents == null)
                {
                    GCalHelper.getYearlyEvents(_calendarOwner, _calendarId, _calendarYear, null);
                }
                List<GCalEventItem> currentGoogEvents = GCalHelper.googleCalEvents;

                // Load Values From Salon Calendar
                DBHelper.initData(_calendarYear.Year);
                List<GCalEventItem> currentSalonEvents = DBHelper.convertSQLTicketsToEvents();

                // get New Events to create
                if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                {
                    _newEvents = (List<GCalEventItem>)currentSalonEvents.Except(currentGoogEvents).ToList();
                }
                else
                {
                    _newEvents = currentSalonEvents;
                }

                // get events that need to delete
                if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                {
                    _deletedEvents = (List<GCalEventItem>)currentGoogEvents.Except(currentSalonEvents).ToList();
                }
                else
                {
                    _deletedEvents = new List<GCalEventItem>();
                }

                // get Events that need to update
                if (currentGoogEvents != null && currentGoogEvents.Count > 0)
                {
                    List<GCalEventItem> potentialUpdatedEvents = (List<GCalEventItem>)currentSalonEvents.Except(_newEvents).ToList();
                    //_updatedEvents = (List<GCalEventItem>)currentSalonEvents.Except(_newEvents).ToList();
                    foreach (GCalEventItem e in potentialUpdatedEvents)
                    {
                        GCalEventItem ge = currentGoogEvents.Find(x => x.salonCalendarId == e.salonCalendarId);
                        if (ge != null)
                        {
                            e.eventId = ge.eventId;
                        }
                    }
                    List<GCalEventItem> noGoogleIdEvents = potentialUpdatedEvents.FindAll(x => x.eventId == null).ToList();
                    if (noGoogleIdEvents != null && noGoogleIdEvents.Count > 0)
                    {
                        _newEvents = (List<GCalEventItem>)_newEvents.Concat(noGoogleIdEvents.Except(_newEvents)).ToList();
                        potentialUpdatedEvents = (List<GCalEventItem>)potentialUpdatedEvents.Except(noGoogleIdEvents).ToList();
                    }

                    List<GCalEventItem> unchangedEvents = new List<GCalEventItem>();
                    foreach (var ev in potentialUpdatedEvents)
                    {
                        var entryToCheck = currentGoogEvents.FirstOrDefault(x => x.salonCalendarId == ev.salonCalendarId);
                        if (entryToCheck != null)
                        {
                            if (entryToCheck.appointmentType != ev.appointmentType ||
                                entryToCheck.client != ev.client ||
                                entryToCheck.staffMember != ev.staffMember ||
                                entryToCheck.startTime != ev.startTime ||
                                entryToCheck.endTime != ev.endTime ||
                                entryToCheck.durationMinutes != ev.durationMinutes)
                            {
                                // need to update so leave in list
                                log.Debug("Need to update event ->" + entryToCheck.salonCalendarId);
                            }
                            else
                            {
                                unchangedEvents.Add(entryToCheck);
                            }
                        }
                    }
                    
                    _updatedEvents = (List<GCalEventItem>)potentialUpdatedEvents.Except(unchangedEvents).ToList();
                }
                else
                {
                    _updatedEvents = new List<GCalEventItem>();
                }

                GCalHelper.updateGoogleCalendar(_calendarOwner, _calendarId, _newEvents, _updatedEvents, _deletedEvents);

                log.Info("Finished the update Process...");
            }
            else
            {
                log.Error("Could not perform update as required data has not been set.");
                MessageBox.Show("Please provide the Calendar Owner and other Calendar details before updating.");
            }
        }

        public static void updateProgramStatus(string statusText)
        {
            mainForm.updateStatusText(statusText);
        }

        public static bool haveRequiredData()
        {
            if (String.IsNullOrEmpty(_calendarId) || String.IsNullOrEmpty(_calendarOwner) || _calendarYear == null )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void setUpBackgroundJob()
        {
            if (backgroundUpdateScheduler != null)
            {
                backgroundUpdateScheduler.Stop();
                backgroundUpdateScheduler = null;
            }
            if (_runInMinutes > 0)
            {
                backgroundUpdateScheduler = new JobScheduler(TimeSpan.FromMinutes(5), () =>
                {
                    updateGoogleCalendar();
                });
            }
        }

        public static void updatedSettings()
        {
            if (!_runInBackground)
            {
                if (backgroundUpdateScheduler != null)
                {
                    backgroundUpdateScheduler.Stop();
                    backgroundUpdateScheduler = null;
                }
            }
            else
            {
                setUpBackgroundJob();
                if (backgroundUpdateScheduler != null)
                {
                    backgroundUpdateScheduler.Start();
                }
            }
        }

        //public static 
        //// For each 1 hour execute 
        //JobScheduler jobScheduler = new JobScheduler(TimeSpan.FromHours(1), () =>
        //{
        //    // Your Action goes here 
        //    Console.WriteLine("Hello!");
        //});

        //jobScheduler.Start(); // To Start The Scheduler 

        //jobScheduler.Stop(); // To Stop The Scheduler
    }
}
