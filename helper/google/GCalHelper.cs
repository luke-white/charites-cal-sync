// -----------------------------------------------------
// <copyright file="GCalHelper.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Google.Apis.Calendar.v3;
    using Google.Apis.Calendar.v3.Data;
    using Google.Apis.Requests;
    using Google.Apis.Services;
    using itdevgeek_charites.datatypes;
    using static Google.Apis.Calendar.v3.Data.Event;

    /// <summary>
    /// Google Calendar Helper Class
    /// Interacts with the Google Calendar API
    /// </summary>
    public class GCalHelper
    {
        /// <summary>Class logger</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>application name used for Googel service</summary>
        private static string APP_NAME = "Salon Calendar Integration";

        /// <summary>Gets or sets the list of Google calendar events</summary>
        public static List<GCalEventItem> googleCalEvents { get; set; }

        /// <summary>
        /// Get the Google Calendar API service for a user
        /// </summary>
        /// <param name="owner">Google account to get service for</param>
        /// <returns>Google Calendar API service</returns>
        public static CalendarService GetCalendarService(string owner)
        {
            log.Debug("Get Google Calendar Service");
            
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GAuthHelper.GetCredential(owner),
                ApplicationName = APP_NAME,
            });

            return service;
        }

        /// <summary>
        /// Get the list of calendars in the Google account
        /// </summary>
        /// <param name="owner">Accoutn to return list of calendars for</param>
        /// <returns>List of google calendar names and id's associated with the Google account</returns>
        public static CalendarListItem[] GetCalendars(string owner)
        {
            log.Info("Starting Retrieval of Google Calendars for " + owner);

            CalendarListItem[] lstCalendars = new CalendarListItem[0];

            CalendarListResource.ListRequest request = GetCalendarService(owner).CalendarList.List();
            request.ShowDeleted = false;
            request.ShowHidden = false;
            request.MaxResults = 20;

            CalendarList calendars = request.Execute();

            if (calendars.Items != null && calendars.Items.Count > 0)
            {
                lstCalendars = new CalendarListItem[calendars.Items.Count];
                int count = 0;

                foreach (var calItem in calendars.Items)
                {
                    string calId = calItem.Id.ToString();
                    string calName = calItem.Summary.ToString();

                    CalendarListItem item = new CalendarListItem();
                    item.Text = calName;
                    item.Value = calId;

                    lstCalendars[count] = item;

                    count++;
                }
            }

            log.Info("Finished Retrieval of Google Calendars for " + owner);
            return lstCalendars;
        }

        /// <summary>
        /// Get the current Google calendar events for a year
        /// </summary>
        /// <param name="owner">Google account</param>
        /// <param name="calendarId">Calendar ID</param>
        /// <param name="updateYear">Year to get events for</param>
        /// <param name="pageToken">Whether there are more pages of events</param>
        public static void GetYearlyEvents(string owner, string calendarId, DateTime updateYear, string pageToken)
        {
            if (string.IsNullOrEmpty(pageToken))
            {
                log.Info("Starting Retrieval of Events from " + owner + "'s Google Calendar (" + calendarId + ") for " + updateYear.Year);
            }

            int year = updateYear.Year;

            DateTime minTime = DateTime.Parse("1/1/" + year + " 00:00:00");
            DateTime maxTime = DateTime.Parse("1/1/" + (year + 2) + " 00:00:00");

            // Define parameters of request.
            EventsResource.ListRequest request = GetCalendarService(owner).Events.List(calendarId);

            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 2500;
            request.TimeMin = minTime;
            request.TimeMax = maxTime;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            if (!string.IsNullOrEmpty(pageToken))
            {
                request.PageToken = pageToken;
            }

            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                if (googleCalEvents == null)
                {
                    googleCalEvents = new List<GCalEventItem>();
                }

                foreach (var eventItem in events.Items)
                {
                    GCalEventItem entry = new GCalEventItem();

                    entry.EventId = eventItem.Id;

                    string when = eventItem.Start.DateTime.ToString();
                    if (string.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }

                    string ending = eventItem.End.DateTime.ToString();
                    if (string.IsNullOrEmpty(ending))
                    {
                        ending = eventItem.End.Date;
                    }

                    entry.StartTime = DateTime.Parse(when);
                    entry.EndTime = DateTime.Parse(ending);

                    entry.DurationMinutes = (entry.EndTime - entry.StartTime).TotalMinutes;

                    List<KeyValuePair<string, string>> extraProps = eventItem.ExtendedProperties.Private__.ToList();
                    string key = "staffMember";
                    string staffMember = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();

                    if (!string.IsNullOrEmpty(staffMember))
                    {
                        if (staffMember.ToUpper().Equals(((NaNStaff.Employees)2).ToString()))
                        {
                            entry.StaffMember = NaNStaff.Employees.LYSHAIE;
                        }
                        else
                        {
                            entry.StaffMember = NaNStaff.Employees.KOULA;
                        }
                    }

                    key = "appointmentType";
                    string appointmentType = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();
                    if (!string.IsNullOrEmpty(appointmentType))
                    {
                        entry.AppointmentType = appointmentType;
                    }

                    key = "client";
                    string client = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();
                    if (!string.IsNullOrEmpty(client))
                    {
                        entry.Client = client;
                    }

                    key = "salonCalId";
                    string salonCalId = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();
                    if (!string.IsNullOrEmpty(salonCalId))
                    {
                        entry.SalonCalendarId = int.Parse(salonCalId);
                    }

                    googleCalEvents.Add(entry);
                }
            }
            else
            {
                log.Info("No upcoming events found in Google Calendar.");
            }

            // Response has more pages of events, get the next page for processing
            if (!string.IsNullOrEmpty(events.NextPageToken))
            {
                GetYearlyEvents(owner, calendarId, updateYear, events.NextPageToken);
            }

            log.Info("Finished Retrieval of Events from " + owner + "'s Google Calendar (" + calendarId + ") for " + updateYear.Year);
        }

        /// <summary>
        /// Remove all events currently in a calendar
        /// </summary>
        /// <param name="owner">Calendar owner</param>
        /// <param name="calendarId">Calendar ID</param>
        /// <param name="pageToken">Whether there are more pages of events to clear</param>
        public static async void ClearAllEventsFromCalendar(string owner, string calendarId, string pageToken)
        {
            EventsResource.ListRequest request = GetCalendarService(owner).Events.List(calendarId);

            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 1000;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            if (!string.IsNullOrEmpty(pageToken))
            {
                request.PageToken = pageToken;
            }

            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                await DeleteGoogleEventsAsync(owner, calendarId, events);
            }

            // If there are more pages of events continue removing next page
            if (!string.IsNullOrEmpty(events.NextPageToken))
            {
                ClearAllEventsFromCalendar(owner, calendarId, events.NextPageToken);
            }
        }

        /// <summary>
        /// Update the google calendar with provided event details
        /// </summary>
        /// <param name="owner">Calendar owner</param>
        /// <param name="calendarId">Calendar ID</param>
        /// <param name="newEvents">new events to add</param>
        /// <param name="updatedEvents">events to update</param>
        /// <param name="deletedEvents">events to remove</param>
        public static async void UpdateGoogleCalendar(string owner, string calendarId, List<GCalEventItem> newEvents, List<GCalEventItem> updatedEvents, List<GCalEventItem> deletedEvents)
        {
            log.Info("Starting Update Requests for Google Calendar");

            if (deletedEvents != null && deletedEvents.Count > 0)
            {
                await DeleteGoogleEventsAsync(owner, calendarId, deletedEvents);
            }

            if (newEvents != null && newEvents.Count > 0)
            {
                await CreateNewEventsAsync(owner, calendarId, newEvents);
            }

            if (updatedEvents != null && updatedEvents.Count > 0)
            {
                await UpdateGoogleEventsAsync(owner, calendarId, updatedEvents);
            }

            log.Info("Finished Update Requests for Google Calendar");
        }

        /// <summary>
        /// Create new events in the Google Calendar
        /// </summary>
        /// <param name="owner">Calendar owner</param>
        /// <param name="calendarId">Calendar id</param>
        /// <param name="events">list of events to create</param>
        /// <returns>async Task for running API in background</returns>
        private static async Task CreateNewEventsAsync(string owner, string calendarId, List<GCalEventItem> events)
        {
            try
            {
                int eventCount = 1;

                // Create a batch request.
                var request = new BatchRequest(GetCalendarService(owner));

                foreach (GCalEventItem e in events)
                {
                    Event tempEvent = new Event();

                    string summary = e.StaffMember.ToString() + ": " + e.Client;

                    tempEvent.Summary = summary;
                    tempEvent.Start = new EventDateTime() { DateTime = e.StartTime };
                    tempEvent.End = new EventDateTime() { DateTime = e.EndTime };

                    RemindersData eventReminders = new RemindersData();
                    List<EventReminder> eventReminder = new List<EventReminder>();
                    eventReminder.Add(new EventReminder { Method = "popup", Minutes = 30 });
                    eventReminders.Overrides = eventReminder;
                    eventReminders.UseDefault = false;

                    tempEvent.Reminders = eventReminders;

                    ExtendedPropertiesData extendedProps = new ExtendedPropertiesData();
                    extendedProps.Private__ = new Dictionary<string, string>();
                    extendedProps.Private__.Add("staffMember", e.StaffMember.ToString());
                    extendedProps.Private__.Add("appointmentType", e.AppointmentType);
                    extendedProps.Private__.Add("client", e.Client);
                    extendedProps.Private__.Add("salonCalId", e.SalonCalendarId.ToString());

                    tempEvent.ExtendedProperties = extendedProps;

                    if (eventCount < 900)
                    {
                        request.Queue<Event>(
                            GetCalendarService(owner).Events.Insert(tempEvent, calendarId), (content, error, i, message) =>
                            {
                            // Put your callback code here.
                            if (error != null)
                                {
                                    log.Error("Error creating new appointments in Google Calendar: " + error.Message);
                                }

                                if (content != null)
                                {
                                // do something
                                log.Debug("New event id = " + content.Id);
                                    e.EventId = content.Id;
                                }
                            });
                        eventCount++;
                    }
                    else
                    {
                        // Execute the batch request and create a new batch as cannot send more than 1000
                        await request.ExecuteAsync();

                        eventCount = 1;
                        request = new BatchRequest(GetCalendarService(owner));
                    }
                }

                // Execute the batch request
                await request.ExecuteAsync();

                if (googleCalEvents != null)
                {
                    googleCalEvents = (List<GCalEventItem>)googleCalEvents.Concat(events).ToList();
                }
                else
                {
                    googleCalEvents = events;
                }
            }
            catch (Exception e)
            {
                log.Error("Error creating new Google Events : " + e.Message);
            }
        }

        /// <summary>
        /// Update Google Events in Google Calenar
        /// </summary>
        /// <param name="owner">calendar owner</param>
        /// <param name="calendarId">calendar id</param>
        /// <param name="events">events to update</param>
        /// <returns>async task for the background Api tasks</returns>
        private static async Task UpdateGoogleEventsAsync(string owner, string calendarId, List<GCalEventItem> events)
        {
            try {
                int batchCount = 1;

                // Create a batch request.
                var request = new BatchRequest(GetCalendarService(owner));

                foreach (GCalEventItem e in events)
                {
                    Event tempEvent = new Event();

                    string summary = e.StaffMember.ToString() + ": " + e.Client;

                    tempEvent.Summary = summary;
                    tempEvent.Start = new EventDateTime() { DateTime = e.StartTime };
                    tempEvent.End = new EventDateTime() { DateTime = e.EndTime };

                    RemindersData eventReminders = new RemindersData();
                    List<EventReminder> eventReminder = new List<EventReminder>();
                    eventReminder.Add(new EventReminder { Method = "popup", Minutes = 30 });
                    eventReminders.Overrides = eventReminder;
                    eventReminders.UseDefault = false;

                    tempEvent.Reminders = eventReminders;

                    ExtendedPropertiesData extendedProps = new ExtendedPropertiesData();
                    extendedProps.Private__ = new Dictionary<string, string>();
                    extendedProps.Private__.Add("staffMember", e.StaffMember.ToString());
                    extendedProps.Private__.Add("appointmentType", e.AppointmentType);
                    extendedProps.Private__.Add("client", e.Client);
                    extendedProps.Private__.Add("salonCalId", e.SalonCalendarId.ToString());

                    tempEvent.ExtendedProperties = extendedProps;

                    if (batchCount < 900)
                    {
                        request.Queue<Event>(
                            GetCalendarService(owner).Events.Update(tempEvent, calendarId, e.EventId), (content, error, i, message) =>
                            {
                                // Put your callback code here.
                                if (error != null)
                                {
                                    log.Error("Error updating Google Calendar with new details: " + error.Message);
                                }
                            });
                        batchCount++;
                    }
                    else
                    {
                        // Execute the batch request and create a new batch as cannot send more than 1000
                        await request.ExecuteAsync();

                        batchCount = 1;
                        request = new BatchRequest(GetCalendarService(owner));
                    }
                }

                // Execute the batch request
                await request.ExecuteAsync();

                foreach (var ev in events)
                {
                    var entryToUpdate = googleCalEvents.FirstOrDefault(x => x.SalonCalendarId == ev.SalonCalendarId);
                    if (entryToUpdate != null)
                    {
                        entryToUpdate.EventId = ev.EventId;
                        entryToUpdate.AppointmentType = ev.AppointmentType;
                        entryToUpdate.Client = ev.Client;

                        entryToUpdate.StaffMember = ev.StaffMember;

                        entryToUpdate.StartTime = ev.StartTime;
                        entryToUpdate.EndTime = ev.EndTime;
                        entryToUpdate.DurationMinutes = ev.DurationMinutes;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("Error updating canged Google Events : " + e.Message);
            }
        }

        /// <summary>
        /// Delete Events from the Google Calendar
        /// </summary>
        /// <param name="owner">calendar owner</param>
        /// <param name="calendarId">calendar id</param>
        /// <param name="events">events to remove</param>
        /// <returns>async task for the background Api tasks</returns>
        private static async Task DeleteGoogleEventsAsync(string owner, string calendarId, List<GCalEventItem> events)
        {
            try
            {
                int batchCount = 1;

                // Create a batch request.
                var request = new BatchRequest(GetCalendarService(owner));

                foreach (GCalEventItem e in events)
                {
                    if (batchCount < 900)
                    {
                        request.Queue<Event>(
                            GetCalendarService(owner).Events.Delete(calendarId, e.EventId), (content, error, i, message) =>
                            {
                            // Put your callback code here.
                            if (error != null)
                                {
                                    log.Error("Error deleting removed appointments from Google Calendar: " + error.Message);
                                }
                            });
                        batchCount++;
                    }
                    else
                    {
                        // Execute the batch request and create a new batch as cannot send more than 1000
                        await request.ExecuteAsync();

                        batchCount = 1;
                        request = new BatchRequest(GetCalendarService(owner));
                    }
                }

                // Execute the batch request
                await request.ExecuteAsync();

                googleCalEvents = (List<GCalEventItem>)googleCalEvents.Except(events).ToList();
            }
            catch (Exception e)
            {
                log.Error("Error deleting Google Events : " + e.Message);
            }
        }

        /// <summary>
        /// Delete Google API Events from Google Calendar
        /// </summary>
        /// <param name="owner">calendar owner</param>
        /// <param name="calendarId">calendar id</param>
        /// <param name="events">Google API Events object</param>
        /// <returns>async task for the background Api tasks</returns>
        private static async Task DeleteGoogleEventsAsync(string owner, string calendarId, Events events)
        {
            try
            {
                int batchCount = 1;

                // Create a batch request.
                var request = new BatchRequest(GetCalendarService(owner));

                foreach (Event e in events.Items)
                {
                    if (batchCount < 900)
                    {
                        request.Queue<Event>(
                        GetCalendarService(owner).Events.Delete(calendarId, e.Id), (content, error, i, message) =>
                        {
                            // Put your callback code here.
                            if (error != null)
                            {
                                log.Error("Error deleting event from Google Calendar: " + error.Message);
                            }
                        });
                        batchCount++;
                    }
                    else
                    {
                        // Execute the batch request and create a new batch as cannot send more than 1000
                        await request.ExecuteAsync();

                        batchCount = 1;
                        request = new BatchRequest(GetCalendarService(owner));
                    }
                }

                // Execute the batch request
                await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                log.Error("Error deleting Google Events : " + e.Message);
            }
        }
    }
}
