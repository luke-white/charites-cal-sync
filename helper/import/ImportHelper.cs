// -----------------------------------------------------
// <copyright file="ICSCalEventItem.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.helper.import
{
    using itdevgeek_charites.datatypes;
    using itdevgeek_charites.helper.sql;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Import Helper Class
    /// Provides helper methods to convert and import ICS events to Salon Iris appointments
    /// </summary>
    class ImportHelper
    {
        /// <summary>class logger</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>The list of appointments with data needed to import into Salon Iris DB</summary>
        private static List<SalonIrisAppointmentItem> importAppointmentList { get; set; }

        /// <summary>The total number of appointments with Koula in the import data</summary>
        public static int koulaAppointments { get; set; }

        /// <summary>The total number of appointments with Lyshaie in the import data</summary>
        public static int lyshaieAppointments { get; set; }

        /// <summary>The total number of appointments in the data ready to import</summary>
        public static int totalAppointments { get; set; }

        /// <summary>The total number of unique clients with appointments in the data ready for import</summary>
        public static int totalClients { get; set; }

        /// <summary>The current client list to used for matching events with</summary>
        private static Dictionary<int, string> clientList;

        /// <summary>
        /// Convert ICS events to SalonIris Appointment items
        /// </summary>
        /// <returns>Events from the ICS File</returns>
        public static void ConvertICSEvents(List<ICSCalEventItem> icsEvents)
        {
            log.Info("Starting Conversion of ICS Events to Salon Iris Appointments");

            try
            {
                // get client list from system if haven't loaded already
                if (clientList == null || clientList.Count() == 0)
                {
                    clientList = DBHelper.GetClientsInSystem();
                }

                importAppointmentList = new List<SalonIrisAppointmentItem>();
                int lastAppointmentID = DBHelper.GetLastAppintmentID();

                int appointmentCount = 0;
                int koulaCount = 0;
                int lyshaieCount = 0;

                List<int> clientIDList = new List<int>();

                foreach ( ICSCalEventItem icsEvent in icsEvents)
                {
                    SalonIrisAppointmentItem appointment = new SalonIrisAppointmentItem();

                    string icsClientName = icsEvent.Client;

                    // find client name in client list
                    int clientID = clientList.FirstOrDefault(x => x.Value == icsClientName).Key;
                    string clientFirstname = "";
                    string clientLastname = "";

                    if (clientID != 0)
                    {
                        string[] clientName = DBHelper.GetClientDetails(clientID);

                        if (clientName != null && clientName.Length == 2)
                        {
                            clientFirstname = clientName[0];
                            clientLastname = clientName[1];
                        }

                        NaNStaff.Employees staffMember = icsEvent.StaffMember;
                        DateTime icsStartTime = icsEvent.StartTime;
                        DateTime icsEndTime = icsEvent.EndTime;

                        double icsEventDuration = icsEvent.DurationMinutes;

                        appointment.ClientID = clientID;
                        appointment.ClientFirstName = clientFirstname;
                        appointment.ClientLastName = clientLastname;

                        // Get start and end times and dates in Salon Iris formatted SQL dates
                        string startDateString = icsStartTime.ToString("yyyy-MM-dd") + " 00:00:00";
                        string startDateTimeString = icsStartTime.ToString("yyy-MM-dd HH:mm:ss");
                        string startTimeString = "1899-12-30 " + icsStartTime.ToString("HH:mm:ss");

                        string endDateString = icsEndTime.ToString("yyyy-MM-dd") + " 00:00:00";
                        string endDateTimeString = icsEndTime.ToString("yyy-MM-dd HH:mm:ss");
                        string endTimeString = "1899-12-30 " + icsEndTime.ToString("HH:mm:ss");

                        // set the data for the appointment
                        appointment.DateScheduled = startDateString;
                        appointment.DurationMinutes = icsEventDuration;

                        appointment.EndDate = endDateString;
                        appointment.EndDateTime = endDateTimeString;
                        appointment.EndTime = endTimeString;

                        appointment.StartDate = startDateString;
                        appointment.StartDateTime = startDateTimeString;
                        appointment.StartTime = startTimeString;

                        appointment.StaffID = NaNStaff.GetStaffID(staffMember);
                        appointment.StaffName = NaNStaff.GetStaffFullName(staffMember);

                        // set the default service details for the appointment
                        // TODO: Update this to come from cofig/fields
                        appointment.ServiceID = "TR";
                        appointment.ServiceName = "Tip Rebalance";
                        appointment.ServiceCategory = "Acrylic Nails";

                        DateTime ticketDateTime = DateTime.Now;
                        appointment.TicketDateCreated = ticketDateTime.ToString("yyy-MM-dd") + "00:00:00";
                        appointment.TicketEditDateTime = ticketDateTime.ToString("yyy-MM-dd HH:mm:ss"); ;
                        appointment.TicketTimeCreated = "1899-12-30 " + ticketDateTime.ToString("HH:mm:ss");

                        // set the ticket id to the next id based off last id number
                        appointment.TicketID = lastAppointmentID + 1;

                        importAppointmentList.Add(appointment);

                        // update running totals for the appointment data values
                        appointmentCount++;
                        if (staffMember == NaNStaff.Employees.KOULA)
                        {
                            koulaCount++;
                        }
                        if (staffMember == NaNStaff.Employees.LYSHAIE)
                        {
                            lyshaieCount++;
                        }
                        if (!clientIDList.Contains(clientID))
                        {
                            clientIDList.Add(clientID);
                        }
                    }
                    else
                    {
                        log.Warn("Could not find Client in system for appointment with client name of " + icsClientName);
                    }
                }

                // update overall totals for the parsed ics events
                totalAppointments = appointmentCount;
                totalClients = clientIDList.Count;
                koulaAppointments = koulaCount;
                lyshaieAppointments = lyshaieCount;
            }
            catch (Exception e)
            {
                log.Error("Error in conversion of ICS events to Salon Iris appointments : " + e.Message);
            }

            log.Info("Finished  conversion of ICS events to Salon Iris appointments");
        }


        public static void ImportAppointments()
        {
            log.Info("Starting Import of appointments to Salon Iris");

            if (importAppointmentList != null && importAppointmentList.Count > 0)
            {
                // TODO: DO import
            }
            else
            {
                log.Error("No appointments to import");
            }

            log.Info("Finished Import of appointments to Salon Iris");
        }

    }
}
