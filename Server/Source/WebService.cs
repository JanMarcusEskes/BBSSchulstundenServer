using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Server.Source
{
    public static class WebService
    {
        static string url;

        public static bool checkCredentials()
        {
            bool credentials = true;
            try
            {
                //Festlegen URL
                WebRequest request = WebRequest.Create("http://bbs-ahrweiler.de/vertretungen/V_DH_001.html");
                //Einrichten des Proxy
                request.Proxy = WebRequest.GetSystemWebProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                //Einrichten der Zugangsdaten
                CredentialCache myCache = new CredentialCache();
                myCache.Add(new Uri("http://bbs-ahrweiler.de/vertretungen/V_DH_001.html"), "Basic", new NetworkCredential(Program.settings.User(), Program.settings.Passwort()));
                request.Credentials = myCache;
                //abfragen der .html Datei
                WebResponse response = request.GetResponse();
            }
            catch (Exception error)
            {
                credentials = false;
            }
            return credentials;
        }
    }
}
