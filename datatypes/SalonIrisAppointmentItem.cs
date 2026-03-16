// -----------------------------------------------------
// <copyright file="SalonIrisAppointmentItem.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.datatypes
{
    /// <summary>
    /// Salon Iris Appointment Data Object
    /// Represents the salon appointment to be imported into the Salon Iris DB
    /// </summary>
    public class SalonIrisAppointmentItem
    {
        /// <summary>Gets or sets Ticket ID for the appointment</summary>
        public int TicketID { get; set; }

        /// <summary>Gets or sets Client ID for the appointment</summary>
        public int ClientID { get; set; }

        /// <summary>Gets or sets client first name</summary>
        public string ClientFirstName { get; set; }

        /// <summary>Gets or setsclient last name</summary>
        public string ClientLastName { get; set; }

        /// <summary>Gets or sets Staff ID who the appointment is booked with</summary>
        public int StaffID { get; set; }

        /// <summary>Gets or sets the Staff member's name</summary>
        public string StaffName { get; set; }

        /// <summary>Gets or sets the appointment start date</summary>
        public string StartDate { get; set; }

        /// <summary>Gets or sets appointment start time</summary>
        public string StartTime { get; set; }

        /// <summary>Gets or sets the appointment start date and time</summary>
        public string StartDateTime { get; set; }

        /// <summary>Gets or sets the appointment end date</summary>
        public string EndDate { get; set; }

        /// <summary>Gets or sets Appointment end time</summary>
        public string EndTime { get; set; }

        /// <summary>Gets or sets the appointment end date and time</summary>
        public string EndDateTime { get; set; }

        /// <summary>Gets or sets the appointment scheduled date</summary>
        public string DateScheduled { get; set; }

        /// <summary>Gets or sets Appointment duration in minutes</summary>
        public double DurationMinutes { get; set; }

        /// <summary>Gets or sets the appointment service id</summary>
        public string ServiceID { get; set; }

        /// <summary>Gets or sets the appointment service name</summary>
        public string ServiceName { get; set; }

        /// <summary>Gets or setsthe appointment service category</summary>
        public string ServiceCategory { get; set; }

        /// <summary>Gets or sets the appointment last edit date</summary>
        public string TicketEditDateTime { get; set; }

        /// <summary>Gets or sets the appointment creation date</summary>
        public string TicketDateCreated { get; set; }

        /// <summary>Gets or sets the appointment creation time</summary>
        public string TicketTimeCreated { get; set; }


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
            return ((this.TicketID == m.TicketID) &&
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

            hash = (hash * 7) + TicketID.GetHashCode();
            hash = (hash * 7) + ClientID.GetHashCode();
            hash = (hash * 7) + StaffID.GetHashCode();
            hash = (hash * 7) + StartDateTime.GetHashCode();

            return hash;
        }
    }
}
