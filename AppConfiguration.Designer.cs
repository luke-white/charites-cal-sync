﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Salon_Calendar_Integration {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0")]
    internal sealed partial class AppConfiguration : global::System.Configuration.ApplicationSettingsBase {
        
        private static AppConfiguration defaultInstance = ((AppConfiguration)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new AppConfiguration())));
        
        public static AppConfiguration Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string email_address {
            get {
                return ((string)(this["email_address"]));
            }
            set {
                this["email_address"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string calendar_name {
            get {
                return ((string)(this["calendar_name"]));
            }
            set {
                this["calendar_name"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string calendar_id {
            get {
                return ((string)(this["calendar_id"]));
            }
            set {
                this["calendar_id"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=REAPER;Initial Catalog=SalonIris;Integrated Security=SSPI;")]
        public string dbConnectionString {
            get {
                return ((string)(this["dbConnectionString"]));
            }
            set {
                this["dbConnectionString"] = value;
            }
        }
    }
}
