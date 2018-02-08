// -----------------------------------------------------
// <copyright file="ProgramInfo.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.helper.application
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Application Program information class
    /// </summary>
    public static class ProgramInfo
    {
        /// <summary>
        /// Gets the application GUID value
        /// </summary>
        public static string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
            }
        }
    }
}
