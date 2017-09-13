using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace Server.Source
{
    public static class WebService
    {
        public readonly static List<List<string>> Tage = new List<List<string>>();

        public static bool CheckCredentials()
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
        public static void GetCode()
        {
            Tage.Clear();
            int tag_id = 1;
            string line;
            for (int i = 0; i < 6; i++)
            {
                WebRequest request = null;
                WebResponse response = null;
                CredentialCache myCache = null;
                StreamReader reader = null;
                List<string> ServerAntwort = new List<string>();

                //Einbinden des Webclients
                using (WebClient client = new WebClient())
                {

                    try
                    {
                        //Festlegen URL
                        request = WebRequest.Create("http://bbs-ahrweiler.de/vertretungen/V_DH_00" + tag_id + ".html");
                        //Einrichten des Proxy
                        request.Proxy = WebRequest.GetSystemWebProxy();
                        request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                        //Einrichten der Zugangsdaten
                        myCache = new CredentialCache();
                        myCache.Add(new Uri("http://bbs-ahrweiler.de/vertretungen/V_DH_00" + tag_id + ".html"), "Basic", new NetworkCredential(Program.settings.User(), Program.settings.Passwort()));
                        request.Credentials = myCache;
                        //abfragen der .html Datei
                        response = request.GetResponse();
                    }
                    //Abfragen ob Fehler besteht
                    catch (Exception ex)
                    {
                        StringWriter sw = new StringWriter();
                        sw.WriteLine("Beim Herunterladen der Datei ist ein Fehler aufgetreten.");
                        sw.WriteLine("Meldung: " + ex.Message);
                        sw.WriteLine("URL: " + "http://bbs-ahrweiler.de/vertretungen/V_DH_00" + tag_id + ".html");
                        MessageBox.Show(sw.ToString(), "Infodatei herunterladen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Source.Startup.red("Der Quellcode der BBS-Internetseite konnte nicht geladen werden!");
                        return;
                    }

                    //Einbinden der .html als stream
                    using (Stream stream = response.GetResponseStream())
                    {
                        reader = new StreamReader(stream);
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line != @"</td>" && line != @"<td>") ServerAntwort.Add(line);
                        }
                        reader = null;

                        //Findet Datum des Codes herraus
                        /*for (int d = 25; d < 40; d++)
                            if (ServerAntwort[d].ToString().Contains(DateTime.Now.Year.ToString()))
                                NamemRadioButtons.Add(ServerAntwort[d].ToString());
                        */


                    }
                    Tage.Add(ServerAntwort.ToList<string>());
                    ServerAntwort.Clear();
                }
                tag_id++;
            }
            Source.Startup.green("Quellcode erfolgreich herruntergeladen");
        }
    }
}
