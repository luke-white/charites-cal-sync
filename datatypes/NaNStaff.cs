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

        /// <summary>
        /// Get the staff id for a given employee
        /// </summary>
        /// <param name="staffEnum">Employee to get ID for</param>
        /// <returns>Salon Iris staff ID. Return -1 if not found.</returns>
        public static int GetStaffID(Employees staffEnum)
        {
            if (staffEnum == Employees.LYSHAIE)
            {
                return 104;
            }
            else if (staffEnum == Employees.KOULA)
            {
                return 101;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Get the full name for a staff member
        /// </summary>
        /// <param name="staffEnum">Employee to get name for</param>
        /// <returns>Employee's full name. Return empty string if not found</returns>
        public static string GetStaffFullName(Employees staffEnum)
        {
            if (staffEnum == Employees.LYSHAIE)
            {
                return "Lyshaie White";
            }
            else if (staffEnum == Employees.KOULA)
            {
                return "Koula White";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Get the minimum appointment time for an employee
        /// </summary>
        /// <param name="staffEnum">Employee to get the time for</param>
        /// <returns>Minimum appointment time in minutes. Return 60 if not found</returns>
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
