// -----------------------------------------------------
// <copyright file="WinAPI.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites.screens
{
    using System.Windows.Forms;

    /// <summary>
    /// Spash screen, displayed during app laod and close
    /// </summary>
    public partial class SplashScreen : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen" /> class.
        /// </summary>
        public SplashScreen()
        {
            this.InitializeComponent();
        }
    }
}
