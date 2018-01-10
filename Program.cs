using Salon_Calendar_Integration.datatypes;
using Salon_Calendar_Integration.helper.sql;
using Salon_Calendar_Integration.screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Salon_Calendar_Integration
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<GCalEventItem> updatedEvents;
        public static List<GCalEventItem> deletedEvents;
        public static List<GCalEventItem> newEvents;

        // Create an instance of the splashscreen 
        public static SplashScreen splashscreen;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log.Info("Launching Salon Calendar Integration Application....");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainScreen());

            // Initialize and show splashscreen. Say the welcome message. 
            splashscreen = new SplashScreen();
            splashscreen.Show();

            // Create an instance of MainForm and hook into shown and closed events.
            MainScreen mainform = new MainScreen();
            mainform.Shown += main_Shown;
            mainform.FormClosed += main_FormClosed;

            System.Threading.Thread.Sleep(3000);   // Pause for 3 second.
            Application.Run(mainform);
        }

        static void main_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Hide the calling form  
            Form form = sender as Form;
            form.Hide();

            // Show the splash screen
            splashscreen.Show();
            System.Threading.Thread.Sleep(1000);   // Pause for a second.

            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

        static void main_Shown(object sender, EventArgs e)
        {
            // Hide the splashscreen. 
            splashscreen.Hide();
        }


        public static void updateGoogleCalendar(string owner, string calendarId, DateTime year)
        {
            log.Info("Starting the update Process...");

            // Get Google Calendar Events
            GCalHelper.getYearlyEvents(owner, calendarId, year, null);
            List<GCalEventItem> currentGoogEvents = GCalHelper.googleCalEvents;

            // Load Values From Salon Calendar
            DBHelper.initData(year.Year);
            List<GCalEventItem> currentSalonEvents = DBHelper.convertSQLTicketsToEvents();


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
                updatedEvents = (List<GCalEventItem>)currentSalonEvents.Except(newEvents).ToList();
                foreach (GCalEventItem e in updatedEvents)
                {
                    GCalEventItem ge = currentGoogEvents.Find(x => x.salonCalendarId == e.salonCalendarId);
                    if (ge != null)
                    {
                        e.eventId = ge.eventId;
                    }
                }
            }
            else
            {
                updatedEvents = new List<GCalEventItem>();
            }

            GCalHelper.updateGoogleCalendar(owner, calendarId, newEvents, updatedEvents, deletedEvents);

            log.Info("Finished the update Process...");
        }
    }
}
