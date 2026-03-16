// -----------------------------------------------------
// <copyright file="ICSCalEventItem.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.datatypes
{
    using System;

    /// <summary>
    /// ICS Calendar Event Data Object
    /// Represents the salon appointment loaded from Google Calendar ICS Export File
    /// </summary>
    public class ICSCalEventItem
    {
        /// <summary>Gets or sets Client name for the appointment</summary>
        public string Client { get; set; }

        /// <summary>Gets or sets Staff member who the appointment is booked with</summary>
        public NaNStaff.Employees StaffMember { get; set; }

        /// <summary>Gets or sets Appointment start time</summary>
        public DateTime StartTime { get; set; }

        /// <summary>Gets or sets Appointment end time</summary>
        public DateTime EndTime { get; set; }

        /// <summary>Gets or sets Appointment duration in minutes</summary>
        public double DurationMinutes { get; set; }

        /// <summary>
        /// Equal method to validate event is equal based on combined fields
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

            ICSCalEventItem m = (ICSCalEventItem)obj;
            return ((this.Client == m.Client) &&
                    (this.StaffMember == m.StaffMember) &&
                    (this.StartTime == m.StartTime) &&
                    (this.DurationMinutes == m.DurationMinutes));
        }

        /// <summary>
        /// Override Hash Code
        /// </summary>
        /// <returns>salon calendar id</returns>
        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + Client.GetHashCode();
            hash = (hash * 7) + StaffMember.GetHashCode();
            hash = (hash * 7) + StartTime.GetHashCode();
            hash = (hash * 7) + DurationMinutes.GetHashCode();

            return hash;
        }
    }
}
