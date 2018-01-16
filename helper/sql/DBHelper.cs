using itdevgeek_charites.datatypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace itdevgeek_charites.helper.sql
{
    class DBHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static DataSet ds { get; set; }

        public static void initData(int workingYear)
        {
            log.Info("Starting Initialising the SQL DB Data");

            ds = new DataSet();
            try
            {
                string connectionString = AppConfiguration.Default.dbConnectionString;

                if (String.IsNullOrEmpty(connectionString))
                    connectionString = "Data Source=localhost;Initial Catalog=SalonIris;Integrated Security=SSPI;";

                // Create a new adapter and give it a query to fetch
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT " +
                        "s.fldTicketID as 'id', s.fldClientID as 'clientId', s.fldFirstName as 'clientFirstName', s.fldLastName as 'clientLastName', " +
                        "r.fldEmployeeName as 'staff', r.fldStartDate as 'startDate', r.fldStartTime as 'startTime', " +
                        "r.fldEndDate as 'endDate', r.fldEndTime as 'endTime', r.fldDescription as 'serviceDescription' " +
                    "FROM SalonIris.dbo.tblTicketsSummary s, SalonIris.dbo.tblTicketsRow r " +
                    "WHERE " +
                        "s.fldTicketID = r.fldTicketID " +
                        "AND DATEPART(YEAR, r.fldStartDate) = @year " +
                        "AND DATEPART(YEAR, r.fldEndDate) = @year " +
                    "ORDER BY r.fldStartDate, r.fldStartTime",
                connectionString);

                // Add table mappings.
                da.SelectCommand.Parameters.AddWithValue("@year", workingYear);
                da.TableMappings.Add("Table", "Tickets");

                // Fill the DataSet.
                da.Fill(ds);

            }
            catch (SqlException ex)
            {
                log.Error("SQL exception occurred: " + ex.Message);
            }

            log.Info("Finished Initialising the SQL DB Data");
        }


        public static List<GCalEventItem> convertSQLTicketsToEvents()
        {
            log.Info("Starting Reading of Salon SQL Calendar Data");

            List<GCalEventItem> ticketEvents = new List<GCalEventItem>();

            if (ds != null)
            {
                DataTable tickets = ds.Tables["Tickets"];
                var ticketIDQuery = from ticket in tickets.AsEnumerable() select ticket.Field<int>("id");

                List<int> ticketIDs = ticketIDQuery.GroupBy(x => x).Select(y => y.First()).OrderBy(z => z).ToList();

                foreach (int ticketID in ticketIDs)
                {
                    GCalEventItem newEvent = new GCalEventItem();

                    var appointmentQuery = from ticket in tickets.AsEnumerable()
                    where ticket.Field<int>("id") == ticketID
                    select new
                    {
                        TicketId = ticket.Field<int>("id"),
                        ClientId = ticket.Field<int>("clientId"),
                        FirstName = ticket.Field<string>("clientFirstName"),
                        LastName = ticket.Field<string>("clientLastName"),
                        Employee = ticket.Field<string>("staff"),
                        StartDate = ticket.Field<DateTime>("startDate"),
                        EndDate = ticket.Field<DateTime>("endDate"),
                        StartTime = ticket.Field<DateTime>("startTime"),
                        EndTime = ticket.Field<DateTime>("endTime"),
                        ServiceDescription = ticket.Field<string>("serviceDescription")
                    };


                    newEvent.salonCalendarId = ticketID;

                    newEvent.appointmentType = appointmentQuery.Select(x => x.ServiceDescription).First();
                    string clientName = appointmentQuery.Select(x => x.FirstName).First().Trim();
                    if (!String.IsNullOrEmpty(appointmentQuery.Select(x => x.LastName).First()))
                    {
                        clientName += " ";
                        clientName += appointmentQuery.Select(x => x.LastName).First().Trim();
                    }
                    newEvent.client = clientName;

                    string employee = appointmentQuery.Select(x => x.Employee).FirstOrDefault().ToLower().Trim();
                    if (String.IsNullOrEmpty(employee))
                    {
                        employee = ((NaNStaff.Employees) 0).ToString().ToLower();
                    }
                    switch (employee.ToLower())
                    {
                        case "lyshaie white":
                            newEvent.staffMember = NaNStaff.Employees.LYSHAIE;
                            break;
                        case "emma lee":
                            newEvent.staffMember = NaNStaff.Employees.EMMA;
                            break;
                        default:
                            newEvent.staffMember = NaNStaff.Employees.KOULA;
                            break;
                    }

                    DateTime appStart = appointmentQuery.Min(x => x.StartDate);
                    DateTime appStartTime = appointmentQuery.Min(x => x.StartTime);
                    DateTime appEnd = appointmentQuery.Max(x => x.EndDate);
                    DateTime appEndTime = appointmentQuery.Max(x => x.EndTime);

                    newEvent.startTime = DateTime.Parse(appStart.ToShortDateString() + " " + appStartTime.TimeOfDay);
                    newEvent.endTime = DateTime.Parse(appEnd.ToShortDateString() + " " + appEndTime.TimeOfDay);

                    newEvent.durationMinutes = (newEvent.endTime - newEvent.startTime).TotalMinutes;

                    ticketEvents.Add(newEvent);
                }

            }
            else
            {
                // Error have not initialized Data
                log.Error("ERROR: Reading of SQL Data attempted before initialisation has been performed.");
            }

            log.Info("Finished Reading Salon SQL Calendar Data");
            return ticketEvents;
        }


    }
}
