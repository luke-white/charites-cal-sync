// -----------------------------------------------------
// <copyright file="SalonIrisAppointmentItem.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.datatypes
{
    using System;

    /// <summary>
    /// Salon Iris Appointment Data Object
    /// Represents the salon appointment to be imported into the Salon Iris DB
    /// </summary>
    public class SalonIrisAppointmentItem
    {
        /// <summary>Gets or sets Ticket ID for the appointment</summary>
        public int TickedID { get; set; }

        /// <summary>Gets or sets Client ID for the appointment</summary>
        public int ClientID { get; set; }

        /// <summary>Gets or sets</summary>
        public string ClientFirstName { get; set; }

        /// <summary>Gets or sets</summary>
        public string ClientEndName { get; set; }

        /// <summary>Gets or sets Staff ID who the appointment is booked with</summary>
        public int StaffID { get; set; }

        public string StaffName { get; set; }

        /// <summary>Gets or sets</summary>
        public string StartDate { get; set; }

        /// <summary>Gets or sets Appointment start time</summary>
        public string StartTime { get; set; }

        /// <summary>Gets or sets</summary>
        public string StartDateTime { get; set; }

        /// <summary>Gets or sets</summary>
        public string EndDate { get; set; }

        /// <summary>Gets or sets Appointment end time</summary>
        public string EndTime { get; set; }

        /// <summary>Gets or sets</summary>
        public string EndDateTime { get; set; }

        /// <summary>Gets or sets</summary>
        public string DateScheduled { get; set; }

        /// <summary>Gets or sets Appointment duration in minutes</summary>
        public double DurationMinutes { get; set; }

        /// <summary>Gets or sets</summary>
        public string ServiceID { get; set; }

        /// <summary>Gets or sets</summary>
        public string ServiceName { get; set; }

        /// <summary>Gets or sets</summary>
        public string ServiceCategory { get; set; }

        /// <summary>Gets or sets</summary>
        public string TickedEditDateTime { get; set; }

        /// <summary>Gets or sets</summary>
        public string TickedDateCreated { get; set; }

        /// <summary>Gets or sets</summary>
        public string TickedTimeCreated { get; set; }


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

            SalonIrisAppointmentItem m = (SalonIrisAppointmentItem)obj;
            return ((this.TickedID == m.TickedID) &&
                    (this.ClientID == m.ClientID) &&
                    (this.StaffID == m.StaffID) && 
                    (this.StartDateTime == m.StartDateTime));
        }

        /// <summary>
        /// Override Hash Code
        /// </summary>
        /// <returns>salon calendar id</returns>
        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + TickedID.GetHashCode();
            hash = (hash * 7) + ClientID.GetHashCode();
            hash = (hash * 7) + StaffID.GetHashCode();
            hash = (hash * 7) + StartDateTime.GetHashCode();

            return hash;
        }
    }
}
