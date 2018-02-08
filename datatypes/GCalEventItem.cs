// -----------------------------------------------------
// <copyright file="GCalEventItem.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.datatypes
{
    using System;

    /// <summary>
    /// Google Calendar Event Data Object
    /// Represents the salon appointment in Google Calendar
    /// </summary>
    public class GCalEventItem
    {
        /// <summary>Gets or sets Google Calendar Event item unique ID</summary>
        public string EventId { get; set; }

        /// <summary>Gets or sets Salon Iris Calendar appointment unique ID</summary>
        public int SalonCalendarId { get; set; }

        /// <summary>Gets or sets Client name for the appointment</summary>
        public string Client { get; set; }

        /// <summary>Gets or sets Service that the appointment is for</summary>
        public string AppointmentType { get; set; }

        /// <summary>Gets or sets Staff member who the appointment is booked with</summary>
        public NaNStaff.Employees StaffMember { get; set; }

        /// <summary>Gets or sets Appointment start time</summary>
        public DateTime StartTime { get; set; }

        /// <summary>Gets or sets Appointment end time</summary>
        public DateTime EndTime { get; set; }

        /// <summary>Gets or sets Appointment duration in minutes</summary>
        public double DurationMinutes { get; set; }

        /// <summary>
        /// Equal method to validate event is equal based on the Salon Iris Calendar ID
        /// </summary>
        /// <param name="obj">Object to compare with the object</param>
        /// <returns>Whether the objects are equal</returns>
        public override bool Equals(object obj)
        {
            // Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            GCalEventItem m = (GCalEventItem)obj;
            return this.SalonCalendarId == m.SalonCalendarId;
        }

        /// <summary>
        /// Override Hash Code to use the unique salon iris calendar id
        /// </summary>
        /// <returns>salon calendar id</returns>
        public override int GetHashCode()
        {
            return this.SalonCalendarId;
        }
    }
}
