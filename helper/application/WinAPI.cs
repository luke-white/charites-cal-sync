// -----------------------------------------------------
// <copyright file="WinAPI.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.helper.application
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Windows API Helper used for single instance and restoring other process if one already running
    /// </summary>
    public static class WinAPI
    {
        /// <summary>Broadcast value</summary>
        public const int HWND_BROADCAST = 0xffff;

        /// <summary>Normal window status value</summary>
        public const int SW_SHOWNORMAL = 1;

        /// <summary>
        /// Register window message unique in system
        /// </summary>
        /// <param name="message">window message to register</param>
        /// <returns>message identifier</returns>
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        /// <summary>
        /// Register the window message which is unique in system
        /// </summary>
        /// <param name="format">message to register</param>
        /// <param name="args">arguments to replace in message</param>
        /// <returns>message identifier</returns>
        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = string.Format(format, args);
            return RegisterWindowMessage(message);
        }

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="hwnd">window handler</param>
        /// <param name="msg">message to post</param>
        /// <param name="wparam">additional message info</param>
        /// <param name="lparam">more additional message info</param>
        /// <returns>if message was successfully posted</returns>
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        /// <summary>
        /// Show a window provided by the pointer
        /// </summary>
        /// <param name="hwnd">window pointer</param>
        /// <param name="cmdShow">window show type</param>
        /// <returns>whether the window was shown</returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int cmdShow);

        /// <summary>
        /// Sets a window to the foreground
        /// </summary>
        /// <param name="wnd">window pointer</param>
        /// <returns>whether window was brought to foreground</returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr wnd);

        /// <summary>
        /// Display window and bring to front
        /// </summary>
        /// <param name="window">window pointer to show and bring to front</param>
        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetForegroundWindow(window);
        }
    }
}
