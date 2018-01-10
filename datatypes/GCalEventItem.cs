using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salon_Calendar_Integration.datatypes
{
    class GCalEventItem
    {
        public string eventId { get; set; }

        public int salonCalendarId { get; set; }

        public string client { get; set; }
        public string appointmentType { get; set; }

        public NaNStaff.Employees staffMember { get; set; }

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public double durationMinutes { get; set; }


        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;
            GCalEventItem m = (GCalEventItem)obj;
            return (salonCalendarId == m.salonCalendarId);
        }

        public override int GetHashCode()
        {
            return salonCalendarId;
        }
    }
}
