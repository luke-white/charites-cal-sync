using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using itdevgeek_charites.datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Calendar.v3.Data.Event;

namespace itdevgeek_charites
{
    class GCalHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string APP_NAME = "Salon Calendar Integration";

        public static List<GCalEventItem> googleCalEvents { get; set; }

        public static CalendarService getCalendarService(string owner)
        {
            log.Debug("Get Google Calendar Service");
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GAuthHelper.getCredential(owner),
                ApplicationName = APP_NAME,
            });

            return service;
        }

        public static CalendarListItem[] getCalendars(string owner)
        {
            log.Info("Starting Retrieval of Google Calendars for " + owner);

            CalendarListItem[] oCalendars = new CalendarListItem[0];

            CalendarListResource.ListRequest request = getCalendarService(owner).CalendarList.List();
            request.ShowDeleted = false;
            request.ShowHidden = false;
            request.MaxResults = 20;

            CalendarList calendars = request.Execute();

            if (calendars.Items != null && calendars.Items.Count > 0)
            {
                oCalendars = new CalendarListItem[calendars.Items.Count];
                int count = 0;

                foreach (var calItem in calendars.Items)
                {
                    string calId = calItem.Id.ToString();
                    string calName = calItem.Summary.ToString();

                    CalendarListItem item = new CalendarListItem();
                    item.Text = calName;
                    item.Value = calId;

                    oCalendars[count] = item;

                    count++;
                }
            }

            log.Info("Finished Retrieval of Google Calendars for " + owner);
            return oCalendars;
        }

        public static void getYearlyEvents(string owner, string calendarId, DateTime updateYear, string pageToken)
        {
            if (String.IsNullOrEmpty(pageToken))
                log.Info("Starting Retrieval of Events from " + owner + "'s Google Calendar (" + calendarId + ") for " + updateYear.Year);


            int year = updateYear.Year;

            DateTime minTime = DateTime.Parse("1/1/" + year + " 00:00:00");
            DateTime maxTime = DateTime.Parse("1/1/" + (year + 1) + " 00:00:00");

            // Define parameters of request.
            EventsResource.ListRequest request = getCalendarService(owner).Events.List(calendarId);

            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 2500;
            request.TimeMin = minTime;
            request.TimeMax = maxTime;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            if (!String.IsNullOrEmpty(pageToken))
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

                    entry.eventId = eventItem.Id;

                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    string ending = eventItem.End.DateTime.ToString();
                    if (String.IsNullOrEmpty(ending))
                    {
                        ending = eventItem.End.Date;
                    }

                    entry.startTime = DateTime.Parse(when);
                    entry.endTime = DateTime.Parse(ending);

                    entry.durationMinutes = (entry.endTime - entry.startTime).TotalMinutes;

                    //entry.appointmentType = eventItem.Summary;

                    List<KeyValuePair<string, string>> extraProps = eventItem.ExtendedProperties.Private__.ToList();
                    string key = "staffMember";
                    string staffMember = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();

                    if (!String.IsNullOrEmpty(staffMember))
                    {
                        if (staffMember.ToUpper().Equals(((NaNStaff.Employees)2).ToString()))
                            entry.staffMember = NaNStaff.Employees.LYSHAIE;
                        else if (staffMember.ToUpper().Equals(((NaNStaff.Employees)1).ToString()))
                            entry.staffMember = NaNStaff.Employees.EMMA;
                        else
                            entry.staffMember = NaNStaff.Employees.KOULA;
                    }

                    key = "appointmentType";
                    string appointmentType = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();
                    if (!String.IsNullOrEmpty(appointmentType))
                    {
                        entry.appointmentType = appointmentType;
                    }

                    key = "client";
                    string client = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();
                    if (!String.IsNullOrEmpty(client))
                    {
                        entry.client = client;
                    }

                    key = "salonCalId";
                    string salonCalId = (from kvp in extraProps where kvp.Key == key select kvp.Value).Single();
                    if (!String.IsNullOrEmpty(salonCalId))
                    {
                        entry.salonCalendarId = int.Parse(salonCalId);
                    }

                    googleCalEvents.Add(entry);

                }
            }
            else
            {
                log.Info("No upcoming events found in Google Calendar.");
            }

            if (!String.IsNullOrEmpty(events.NextPageToken))
            {
                getYearlyEvents(owner, calendarId, updateYear, events.NextPageToken);
            }

            log.Info("Finished Retrieval of Events from " + owner + "'s Google Calendar (" + calendarId + ") for " + updateYear.Year);
        }

        public static async void clearAllEventsFromCalendar(string owner, string calendarId, string pageToken)
        {
            EventsResource.ListRequest request = getCalendarService(owner).Events.List(calendarId);

            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 2500;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            if (!String.IsNullOrEmpty(pageToken))
            {
                request.PageToken = pageToken;
            }
            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                await deleteGoogleEventsAsync(owner, calendarId, events);
            }

            if (!String.IsNullOrEmpty(events.NextPageToken))
            {
                clearAllEventsFromCalendar(owner, calendarId, events.NextPageToken);
            }
        }


        public static async void updateGoogleCalendar(string owner, string calendarId, List<GCalEventItem> newEvents, List<GCalEventItem> updatedEvents, List<GCalEventItem> deletedEvents)
        {
            log.Info("Starting Update Requests for Google Calendar");

            if (deletedEvents != null && deletedEvents.Count > 0)
            {
                await deleteGoogleEventsAsync(owner, calendarId, deletedEvents);
            }

            if (newEvents != null && newEvents.Count > 0)
            {
                await createNewEventsAsync(owner, calendarId, newEvents);
            }

            if (updatedEvents != null && updatedEvents.Count > 0)
            {
                await updateGoogleEventsAsync(owner, calendarId, updatedEvents);
            }

            log.Info("Finished Update Requests for Google Calendar");

        }


        private static async Task createNewEventsAsync(string owner, string calendarId, List<GCalEventItem> events)
        {
            // Create a batch request.
            var request = new BatchRequest(getCalendarService(owner));
            
            foreach (GCalEventItem e in events)
            {
                Event tempEvent = new Event();

                string summary = e.staffMember.ToString() + ": " + e.client;

                tempEvent.Summary = summary;
                tempEvent.Start = new EventDateTime() { DateTime = e.startTime };
                tempEvent.End = new EventDateTime() { DateTime = e.endTime };

                RemindersData eventReminders = new RemindersData();
                List<EventReminder> eventReminder = new List<EventReminder>();
                eventReminder.Add(new EventReminder { Method = "popup", Minutes = 30 });
                eventReminders.Overrides = eventReminder;
                eventReminders.UseDefault = false;

                tempEvent.Reminders = eventReminders;

                ExtendedPropertiesData exProps = new ExtendedPropertiesData();
                exProps.Private__ = new Dictionary<string, string>();
                exProps.Private__.Add("staffMember", e.staffMember.ToString());
                exProps.Private__.Add("appointmentType", e.appointmentType);
                exProps.Private__.Add("client", e.client);
                exProps.Private__.Add("salonCalId", e.salonCalendarId.ToString());

                tempEvent.ExtendedProperties = exProps;

                request.Queue<Event>(
                    getCalendarService(owner).Events.Insert(tempEvent, calendarId), (content, error, i, message) =>
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
                            e.eventId = content.Id;
                        }
                    }
                );
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

        private static async Task updateGoogleEventsAsync(string owner, string calendarId, List<GCalEventItem> events)
        {
            // Create a batch request.
            var request = new BatchRequest(getCalendarService(owner));

            foreach (GCalEventItem e in events)
            {
                Event tempEvent = new Event();

                string summary = e.staffMember.ToString() + ": " + e.client;

                tempEvent.Summary = summary;
                tempEvent.Start = new EventDateTime() { DateTime = e.startTime };
                tempEvent.End = new EventDateTime() { DateTime = e.endTime };

                RemindersData eventReminders = new RemindersData();
                List<EventReminder> eventReminder = new List<EventReminder>();
                eventReminder.Add(new EventReminder { Method = "popup", Minutes = 30 });
                eventReminders.Overrides = eventReminder;
                eventReminders.UseDefault = false;

                tempEvent.Reminders = eventReminders;

                ExtendedPropertiesData exProps = new ExtendedPropertiesData();
                exProps.Private__ = new Dictionary<string, string>();
                exProps.Private__.Add("staffMember", e.staffMember.ToString());
                exProps.Private__.Add("appointmentType", e.appointmentType);
                exProps.Private__.Add("client", e.client);
                exProps.Private__.Add("salonCalId", e.salonCalendarId.ToString());

                tempEvent.ExtendedProperties = exProps;

                request.Queue<Event>(
                    getCalendarService(owner).Events.Update(tempEvent, calendarId, e.eventId), (content, error, i, message) =>
                    {
                        // Put your callback code here.
                        if (error != null)
                        {
                            log.Error("Error updating Google Calendar with new details: " + error.Message);
                        }
                    }
                );
            }

            // Execute the batch request
            await request.ExecuteAsync();

            foreach (var ev in events)
            {
                var entryToUpdate = googleCalEvents.FirstOrDefault(x => x.salonCalendarId == ev.salonCalendarId);
                if (entryToUpdate != null)
                {
                    entryToUpdate.eventId = ev.eventId;
                    entryToUpdate.appointmentType = ev.appointmentType;
                    entryToUpdate.client = ev.client;

                    entryToUpdate.staffMember = ev.staffMember;

                    entryToUpdate.startTime = ev.startTime;
                    entryToUpdate.endTime = ev.endTime;
                    entryToUpdate.durationMinutes = ev.durationMinutes;
                }
            }
        }

        private static async Task deleteGoogleEventsAsync(string owner, string calendarId, List<GCalEventItem> events)
        {
            // Create a batch request.
            var request = new BatchRequest(getCalendarService(owner));

            foreach (GCalEventItem e in events)
            {
                request.Queue<Event>(
                    getCalendarService(owner).Events.Delete(calendarId, e.eventId), (content, error, i, message) =>
                    {
                        // Put your callback code here.
                        if (error != null)
                        {
                            log.Error("Error deleting removed appointments from Google Calendar: " + error.Message);
                        }
                    }
                );
            }

            // Execute the batch request
            await request.ExecuteAsync();

            googleCalEvents = (List<GCalEventItem>)googleCalEvents.Except(events).ToList();
        }

        private static async Task deleteGoogleEventsAsync(string owner, string calendarId, Events events)
        {
            // Create a batch request.
            var request = new BatchRequest(getCalendarService(owner));

            foreach (Event e in events.Items)
            {
                request.Queue<Event>(
                    getCalendarService(owner).Events.Delete(calendarId, e.Id), (content, error, i, message) =>
                    {
                        // Put your callback code here.
                        if (error != null)
                        {
                            log.Error("Error deleting event from Google Calendar: " + error.Message);
                        }
                    }
                );
            }

            // Execute the batch request
            await request.ExecuteAsync();
        }
    }
}
