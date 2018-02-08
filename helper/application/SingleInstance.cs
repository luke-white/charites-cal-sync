// -----------------------------------------------------
// <copyright file="SingleInstance.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.helper.application
{
    using System;
    using System.Threading;

    /// <summary>
    /// SingleInstance utilises Mutex to ensure only one instance of the application will run at once.
    /// </summary>
    public static class SingleInstance
    {
        /// <summary>Show first instance</summary>
        public static readonly int WM_SHOWFIRSTINSTANCE = WinAPI.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);

        /// <summary>Mutex object to check for instances</summary>
        private static Mutex mutex;

        /// <summary>
        /// Setup Mutex when starting app for a single instance of the application
        /// </summary>
        /// <returns>If is the only instance of the application</returns>
        public static bool Start()
        {
            bool onlyInstance = false;

            string mutexName = string.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = string.Format("Global\\{0}", ProgramInfo.AssemblyGuid);
            mutex = new Mutex(true, mutexName, out onlyInstance);

            return onlyInstance;
        }

        /// <summary>
        /// Show the first instance of the application
        /// </summary>
        public static void ShowFirstInstance()
        {
            WinAPI.PostMessage(
                (IntPtr)WinAPI.HWND_BROADCAST,
                WM_SHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);
        }

        /// <summary>
        /// Clear the mutex when stopping appliction so that can start a new instance
        /// </summary>
        public static void Stop()
        {
            mutex.ReleaseMutex();
        }
    }
}
