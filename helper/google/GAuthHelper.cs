using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;

namespace itdevgeek_charites
{
    class GAuthHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string CLIENT_SECRET_FILE = "auth/client_secret.json";

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/salon-calendar-intg.json
        static string[] Scopes = { CalendarService.Scope.Calendar };

        public static UserCredential getCredential(string owner)
        {
            log.Debug("Starting Obtaining Google Credential");

            UserCredential credential;

            using (var stream = new FileStream(CLIENT_SECRET_FILE, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/salon-calendar-intg.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
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
