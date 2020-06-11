// -----------------------------------------------------
// <copyright file="DBHelper.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.helper.sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using itdevgeek_charites.datatypes;

    /// <summary>
    /// SQL DB Helper to get Salon Iris appointments
    /// </summary>
    public class DBHelper
    {
        /// <summary>class logger</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>Gets or sets SQL Dataset</summary>
        public static DataSet ds { get; set; }

        /// <summary>
        /// initialise the data connection and dataset
        /// </summary>
        /// <param name="workingYear">year to get data for</param>
        public static void InitData(int workingYear)
        {
            log.Info("Starting Initialising the SQL DB Data");

            ds = new DataSet();
            try
            {
                AppConfiguration.Default.Reload();
                string connectionString = AppConfiguration.Default.dbConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "Data Source=localhost;Initial Catalog=SalonIris;Integrated Security=SSPI;";
                }

                // Create a new adapter and give it a query to fetch
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT " +
                        "s.fldTicketID as 'id', s.fldClientID as 'clientId', s.fldFirstName as 'clientFirstName', s.fldLastName as 'clientLastName', " +
                        "r.fldEmployeeName as 'staff', r.fldStartDate as 'startDate', r.fldStartTime as 'startTime', " +
                        "r.fldEndDate as 'endDate', r.fldEndTime as 'endTime', r.fldDescription as 'serviceDescription' " +
                    "FROM SalonIris.dbo.tblTicketsSummary s, SalonIris.dbo.tblTicketsRow r " +
                    "WHERE " +
                        "s.fldTicketID = r.fldTicketID " +
                        "AND r.fldID NOT LIKE '.C%' " +
                        "AND (DATEPART(YEAR, r.fldStartDate) = @year OR DATEPART(YEAR, r.fldStartDate) = @nextyear)" +
                        "AND (DATEPART(YEAR, r.fldEndDate) = @year OR DATEPART(YEAR, r.fldEndDate) = @nextyear)" +
                    "ORDER BY r.fldStartDate, r.fldStartTime",
                    connectionString);

                // Add table mappings.
                da.SelectCommand.Parameters.AddWithValue("@year", workingYear);
                da.SelectCommand.Parameters.AddWithValue("@nextyear", workingYear + 1);
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

        /// <summary>
        /// Convert SQL appointment data to custom event data structure GCalEventItem
        /// </summary>
        /// <returns>Events from the DB</returns>
        public static List<GCalEventItem> ConvertSQLTicketsToEvents()
        {
            log.Info("Starting Reading of Salon SQL Calendar Data");

            List<GCalEventItem> ticketEvents = new List<GCalEventItem>();

            if (ds != null && ds.Tables.Count > 0)
            {
                try
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

                        newEvent.SalonCalendarId = ticketID;

                        newEvent.AppointmentType = RemoveSpecialCharacters(appointmentQuery.Select(x => x.ServiceDescription).First());
                        string clientName = RemoveSpecialCharacters(appointmentQuery.Select(x => x.FirstName).First().Trim());
                        if (!string.IsNullOrEmpty(appointmentQuery.Select(x => x.LastName).First()))
                        {
                            clientName += " ";
                            clientName += RemoveSpecialCharacters(appointmentQuery.Select(x => x.LastName).First().Trim());
                        }

                        newEvent.Client = clientName;

                        string employee = RemoveSpecialCharacters(appointmentQuery.Select(x => x.Employee).FirstOrDefault().ToLower().Trim());
                        if (string.IsNullOrEmpty(employee))
                        {
                            employee = ((NaNStaff.Employees)0).ToString().ToLower();
                        }

                        switch (employee.ToLower())
                        {
                            case "lyshaie white":
                                newEvent.StaffMember = NaNStaff.Employees.LYSHAIE;
                                break;
                            default:
                                newEvent.StaffMember = NaNStaff.Employees.KOULA;
                                break;
                        }

                        DateTime appStart = appointmentQuery.Min(x => x.StartDate);
                        DateTime appStartTime = appointmentQuery.Min(x => x.StartTime);
                        DateTime appEnd = appointmentQuery.Max(x => x.EndDate);
                        DateTime appEndTime = appointmentQuery.Max(x => x.EndTime);

                        newEvent.StartTime = DateTime.Parse(appStart.ToShortDateString() + " " + appStartTime.TimeOfDay);
                        newEvent.EndTime = DateTime.Parse(appEnd.ToShortDateString() + " " + appEndTime.TimeOfDay);

                        newEvent.DurationMinutes = (newEvent.EndTime - newEvent.StartTime).TotalMinutes;

                        ticketEvents.Add(newEvent);
                    }
                }
                catch (Exception e)
                {
                    log.Error("Error in SQL Read : " + e.Message);
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

        /// <summary>
        /// Get the Client List from the Salon DB
        /// </summary>
        /// <returns>Dictionary with clientid and  client name</returns>
        public static Dictionary<int, string> GetClientsInSystem()
        {
            log.Info("Starting Reading of Salon SQL Client Data");

            Dictionary<int, string> clientList = new Dictionary<int, string>();
            DataSet clientDS = new DataSet();
            try
            {
                AppConfiguration.Default.Reload();
                string connectionString = AppConfiguration.Default.dbConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "Data Source=localhost;Initial Catalog=SalonIris;Integrated Security=SSPI;";
                }

                // Create a new adapter and give it a query to fetch
                SqlDataAdapter clientDA = new SqlDataAdapter(
                    "SELECT " +
                        "c.fldClientID as 'id', c.fldFirstName as 'clientFirstName', c.fldLastName as 'clientLastName' " +
                    "FROM SalonIris.dbo.tblClients c " +
                    "ORDER BY c.fldClientID",
                    connectionString);

                // Add table mappings.
                clientDA.TableMappings.Add("Client", "Clients");

                // Fill the DataSet.
                clientDA.Fill(clientDS);
            }
            catch (SqlException ex)
            {
                log.Error("SQL exception occurred: " + ex.Message);
            }
            
            if (clientDS != null && clientDS.Tables.Count > 0)
            {
                try
                {
                    DataTable clients = clientDS.Tables["Clients"];

                    var clientQuery = from client in clients.AsEnumerable()
                                      select new
                                      {
                                          ClientId = client.Field<int>("id"),
                                          FirstName = client.Field<string>("clientFirstName"),
                                          LastName = client.Field<string>("clientLastName")
                                      };

                    var clientResults = clientQuery.Select(x => x).ToList();

                    foreach (var clientDetails in clientResults)
                    {
                        if (clientDetails.ClientId > 0)
                        {
                            int id = clientDetails.ClientId;
                            string clientName = "";
                            if (!string.IsNullOrEmpty(clientDetails.FirstName))
                            {
                                clientName += RemoveSpecialCharacters(clientDetails.FirstName);
                            }
                            if (!string.IsNullOrEmpty(clientDetails.LastName))
                            {
                                clientName += " " + RemoveSpecialCharacters(clientDetails.LastName);
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    log.Error("Error in SQL Read : " + e.Message);
                }
            }
            else
            {
                // Error have not initialized Data
                log.Error("ERROR: Reading of SQL Data attempted before initialisation has been performed.");
            }

            log.Info("Finished Reading Salon SQL Client Data");
            return clientList;

        }

        /// <summary>
        /// Get the Client's name from the system based off the provided Client ID
        /// </summary>
        /// <param name="clientID">Client ID</param>
        /// <returns>Firstname and Lastname in an array</returns>
        public static string[] GetClientDetails(int clientID)
        {
            log.Info("Starting Reading of Salon SQL Client Data");

            string[] clientNames = new string[2];
            DataSet clientDS = new DataSet();
            try
            {
                AppConfiguration.Default.Reload();
                string connectionString = AppConfiguration.Default.dbConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "Data Source=localhost;Initial Catalog=SalonIris;Integrated Security=SSPI;";
                }

                // Create a new adapter and give it a query to fetch
                SqlDataAdapter clientDA = new SqlDataAdapter(
                    "SELECT " +
                        "c.fldClientID as 'id', c.fldFirstName as 'clientFirstName', c.fldLastName as 'clientLastName' " +
                    "FROM SalonIris.dbo.tblClients c " +
                    "ORDER BY c.fldClientID",
                    connectionString);

                // Add table mappings.
                clientDA.TableMappings.Add("Client", "Clients");

                // Fill the DataSet.
                clientDA.Fill(clientDS);
            }
            catch (SqlException ex)
            {
                log.Error("SQL exception occurred: " + ex.Message);
            }

            if (clientDS != null && clientDS.Tables.Count > 0)
            {
                try
                {
                    DataTable clients = clientDS.Tables["Clients"];

                    var clientQuery = from client in clients.AsEnumerable()
                                      where client.Field<int>("id") == clientID 
                                      select new
                                      {
                                          FirstName = client.Field<string>("clientFirstName"),
                                          LastName = client.Field<string>("clientLastName")
                                      };

                    string firstname = clientQuery.Select(x => x.FirstName).First();
                    string lastname = clientQuery.Select(x => x.FirstName).First();

                    if (!string.IsNullOrEmpty(firstname))
                    {
                        clientNames[0] = firstname;
                    }
                    else
                    {
                        clientNames[0] = "";
                    }

                    if (!string.IsNullOrEmpty(lastname))
                    {
                        clientNames[1] = lastname;
                    }
                    else
                    {
                        clientNames[1] = "";
                    }

                }
                catch (Exception e)
                {
                    log.Error("Error in SQL Read : " + e.Message);
                }
            }
            else
            {
                // Error have not initialized Data
                log.Error("ERROR: Reading of SQL Data attempted before initialisation has been performed.");
            }

            log.Info("Finished Reading Salon SQL Client Data");
            return clientNames;
        }


        public static int GetLastAppintmentID()
        {
            log.Info("Starting Reading of Salon SQL Appointment ID Data");

            int lastAppointmentID = -1;
            
            DataSet appointmentDS = new DataSet();
            try
            {
                AppConfiguration.Default.Reload();
                string connectionString = AppConfiguration.Default.dbConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "Data Source=localhost;Initial Catalog=SalonIris;Integrated Security=SSPI;";
                }

                // Create a new adapter and give it a query to fetch
                SqlDataAdapter appointmentDA = new SqlDataAdapter("SELECT t.fldTicketID as 'id' FROM SalonIris.dbo.tblTicketsRow t ", connectionString);

                // Add table mappings.
                appointmentDA.TableMappings.Add("TicketID", "TicketIDs");

                // Fill the DataSet.
                appointmentDA.Fill(appointmentDS);
            }
            catch (SqlException ex)
            {
                log.Error("SQL exception occurred: " + ex.Message);
            }

            if (appointmentDS != null && appointmentDS.Tables.Count > 0)
            {
                try
                {
                    DataTable ticketIDs = appointmentDS.Tables["TicketIDs"];

                    int maxValue = (int) ticketIDs.Compute("MAX([id])", ""); //computing the max value

                    lastAppointmentID = maxValue;
                }
                catch (Exception e)
                {
                    log.Error("Error in SQL Read : " + e.Message);
                }
            }
            else
            {
                // Error have not initialized Data
                log.Error("ERROR: Reading of SQL Data attempted before initialisation has been performed.");
            }

            log.Info("Finished Reading Salon SQL Appointment ID Data");
            return lastAppointmentID;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            char[] buffer = new char[str.Length];
            int idx = 0;

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') 
                    || (c >= 'A' && c <= 'Z')
                    || (c >= 'a' && c <= 'z') 
                    || (c == '.') || (c == '_') || (c == ' '))
                {
                    buffer[idx] = c;
                    idx++;
                }
            }

            return new string(buffer, 0, idx);
        }
    }
}
