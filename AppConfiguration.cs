// -----------------------------------------------------
// <copyright file="AppConfiguration.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites
{
    /// <summary>
    /// Application configuration
    /// </summary>
    /// <remarks>
    ///     This class allows you to handle specific events on the settings class:
    ///     The SettingChanging event is raised before a setting's value is changed.
    ///     The PropertyChanged event is raised after a setting's value is changed.
    ///     The SettingsLoaded event is raised after the setting values are loaded.
    ///     The SettingsSaving event is raised before the setting values are saved.
    /// </remarks>
    internal sealed partial class AppConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfiguration" /> class.
        /// </summary>
        public AppConfiguration()
        {
            // To add event handlers for saving and changing settings, uncomment the lines below:
            // this.SettingChanging += this.SettingChangingEventHandler;
            // this.SettingsSaving += this.SettingsSavingEventHandler;
        }
        
        /// <summary>
        /// Events when a setting has changed
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }
        
        /// <summary>
        /// Events with a setting has saved
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }
    }
}
