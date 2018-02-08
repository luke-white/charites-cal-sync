// -----------------------------------------------------
// <copyright file="GAuthHelper.cs" company="IT Dev Geek">
//     IT Dev Geek. All rights reserved.
// </copyright>
// <author>Luke White</author>
// -----------------------------------------------------
namespace itdevgeek_charites
{
    using System.IO;
    using System.Threading;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Calendar.v3;
    using Google.Apis.Util.Store;

    /// <summary>
    /// Google Authorisation Helper
    /// </summary>
    public class GAuthHelper
    {
        /// <summary>Path and filename used for Google Client API file</summary>
        private const string CLIENT_SECRET_FILE = "auth/client_secret.json";

        /// <summary>Class Logger</summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/salon-calendar-intg.json

        /// <summary>Authorisation scopes needed by the application</summary>
        private static string[] googleScopes = { CalendarService.Scope.Calendar };

        /// <summary>
        /// Get the user Google credentials using the browser if not already authorised
        /// </summary>
        /// <param name="owner">Calendar owner id to authorise</param>
        /// <returns>The Google user credential for API calls</returns>
        public static UserCredential GetCredential(string owner)
        {
            log.Debug("Starting Obtaining Google Credential");

            UserCredential credential;

            using (var stream = new FileStream(CLIENT_SECRET_FILE, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/salon-calendar-intg.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    googleScopes,
                    owner,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                log.Debug("Credential file saved to: " + credPath);
            }

            log.Debug("Finished Obtaining Google Credential");
            return credential;
        }
    }
}
