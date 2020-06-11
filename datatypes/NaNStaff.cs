// -----------------------------------------------------
// <copyright file="NaNStaff.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.datatypes
{
    /// <summary>
    /// Class to hold the Salon staff for lookups
    /// </summary>
    public class NaNStaff
    {
        /// <summary>Enumerated list of available staff names</summary>
        public enum Employees
        {
            /// <summary>Koula staff member</summary>
            KOULA,

            /// <summary>Lyshaie staff member</summary>
            LYSHAIE
        }

        public static int GetStaffID(Employees staffEnum)
        {
            if (staffEnum == Employees.LYSHAIE)
            {
                return 104;
            }
            else
            {
                return 101;
            }
        }

        public static string GetStaffFullName(Employees staffEnum)
        {
            if (staffEnum == Employees.LYSHAIE)
            {
                return "Lyshaie White";
            }
            else
            {
                return "Koula White";
            }
        }

        public static int GetStaffMinimumAppointmentDuration(Employees staffEnum)
        {
            if (staffEnum == Employees.LYSHAIE)
            {
                return 120;
            }
            else if (staffEnum == Employees.KOULA)
            {
                return 90;
            }
            else
            {
                return 60;
            }
        }
    }
}
