// -----------------------------------------------------
// <copyright file="CalendarListItem.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites
{
    /// <summary>
    /// Custom list item for dislay of Google calendars with id and name in the settings screen
    /// </summary>
    public class CalendarListItem
    {
        /// <summary>Gets or sets calenar list item text</summary>
        public string Text { get; set; }

        /// <summary>Gets or sets Calendar list item value</summary>
        public object Value { get; set; }

        /// <summary>
        /// Override the to string method to return the text value
        /// </summary>
        /// <returns>The text for the calendar list item</returns>
        public override string ToString()
        {
            return this.Text;
        }
    }
}
