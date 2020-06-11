using itdevgeek_charites.datatypes;
using itdevgeek_charites.helper.sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace itdevgeek_charites.helper.import
{
    class ImportHelper
    {

        /// <summary>class logger</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<SalonIrisAppointmentItem> importAppointmentList { get; set; }

        public static int koulaAppointments { get; set; }

        public static int lyshaieAppointments { get; set; }

        public static int totalAppointments { get; set; }

        public static int totalClients { get; set; }

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

                    string[] clientName = DBHelper.GetClientDetails(clientID);
                    string clientFirstname = "";
                    string clientLastname = "";

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
                    appointment.ClientEndName = clientLastname;

                    // Get start and end times and dates in Salon Iris formatted SQL dates
                    //string startDateString = String.Format("{0:yyyy-MM-dd}", icsStartTime);
                    string startDateString = icsStartTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    string startDateTimeString = icsStartTime.ToString("yyy-MM-dd HH:mm:ss");
                    string startTimeString = "1899-12-30 " + icsStartTime.ToString("HH:mm:ss");

                    string endDateString = icsEndTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    string endDateTimeString = icsEndTime.ToString("yyy-MM-dd HH:mm:ss");
                    string endTimeString = "1899-12-30 " + icsEndTime.ToString("HH:mm:ss");

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

                    appointment.ServiceID = "TR";
                    appointment.ServiceName = "Tip Rebalance";
                    appointment.ServiceCategory = "Acrylic Nails";

                    DateTime ticketDateTime = DateTime.Now;
                    appointment.TickedDateCreated = ticketDateTime.ToString("yyy-MM-dd") + "00:00:00";
                    appointment.TickedEditDateTime = ticketDateTime.ToString("yyy-MM-dd HH:mm:ss"); ;
                    appointment.TickedTimeCreated = "1899-12-30 " + ticketDateTime.ToString("HH:mm:ss");

                    appointment.TickedID = lastAppointmentID + 1;

                    importAppointmentList.Add(appointment);
                    

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

                totalAppointments = appointmentCount;
                totalClients = clientIDList.Count;
                koulaAppointments = koulaCount;
                lyshaieAppointments = lyshaieCount;
            }
            catch (Exception e)
            {
                log.Error("Error in XXX : " + e.Message);
            }

            log.Info("Finished XXX");
        }

    }
}
