// -----------------------------------------------------
// <copyright file="ICSHelper.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.helper.ics
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using Ical.Net;
    using Ical.Net.CalendarComponents;
    using itdevgeek_charites.datatypes;
    using itdevgeek_charites.helper.sql;

    /// <summary>
    /// ICS Calendar File Helper to get appointments from an exported Google Calendar
    /// </summary>
    class ICSHelper
    {
        /// <summary>class logger</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Parse ICS file for appointments
        /// </summary>
        /// <returns>Events from the ICS File</returns>
        public static List<ICSCalEventItem> ParseICSCalendarFile(string filename)
        {
            log.Info("Starting Reading of Salon Calendar Data from ICS file");

            List<ICSCalEventItem> icsEvents = new List<ICSCalEventItem>();

            try
            {
                // Load the calendar file
                StreamReader file = new StreamReader(filename);
                string content = file.ReadToEnd();
                file.Close();

                // Read the content into the ics Calendar object
                Calendar calendar = Calendar.Load(content);

                foreach (CalendarEvent calEntry in calendar.Events)
                {
                    ICSCalEventItem newEvent = new ICSCalEventItem();

                    string appointmentSummary = calEntry.Summary;
                    string[] appointmentDetails = appointmentSummary.Split(':');

                    string staffMember = appointmentDetails[0].Trim();
                    string client = appointmentDetails[1].Trim();

                    var startTime = calEntry.DtStart;
                    var duration = calEntry.Duration;

                    switch (staffMember.ToLower())
                    {
                        case "lyshaie":
                            newEvent.StaffMember = NaNStaff.Employees.LYSHAIE;
                            break;
                        default:
                            newEvent.StaffMember = NaNStaff.Employees.KOULA;
                            break;
                    }

                    newEvent.StartTime = startTime.Date;
                    newEvent.StartTime = newEvent.StartTime.Date + new TimeSpan(startTime.Hour, startTime.Minute, 0);

                    newEvent.DurationMinutes = duration.TotalMinutes;

                    if (newEvent.StaffMember == NaNStaff.Employees.LYSHAIE)
                    {
                        // check duration is minimum two hours
                        if (newEvent.DurationMinutes < NaNStaff.GetStaffMinimumAppointmentDuration(NaNStaff.Employees.LYSHAIE))
                        {
                            newEvent.DurationMinutes = NaNStaff.GetStaffMinimumAppointmentDuration(NaNStaff.Employees.LYSHAIE);
                        }

                    }
                    else if(newEvent.StaffMember == NaNStaff.Employees.KOULA)
                    {
                        // check duration is at least 90 minutes
                        if (newEvent.DurationMinutes < NaNStaff.GetStaffMinimumAppointmentDuration(NaNStaff.Employees.KOULA))
                        {
                            newEvent.DurationMinutes = NaNStaff.GetStaffMinimumAppointmentDuration(NaNStaff.Employees.KOULA);
                        }
                    }
                    else
                    {
                        if (newEvent.DurationMinutes < NaNStaff.GetStaffMinimumAppointmentDuration(newEvent.StaffMember))
                        {
                            newEvent.DurationMinutes = NaNStaff.GetStaffMinimumAppointmentDuration(newEvent.StaffMember);
                        }
                    }
                    // Dictionary<int, string> clientList = DBHelper.GetClientsInSystem();

                    icsEvents.Add(newEvent);
                }
            }
            catch (Exception e)
            {
                log.Error("Error in ICS Read : " + e.Message);
            }

            log.Info("Finished Reading ICS Calendar File Data");
            return icsEvents;
        }
    }
}
