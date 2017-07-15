﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Server
{
    class Program
    {
        static List<string> startupArgs = new List<string>();
        static bool error = false;
        static bool updated = false;
        static bool warning = false;

        static void Main(string[] args)
        {
            getArgs(args);
            startupText();
            startup();
            Console.Read();
        }


        static void startup()
        {
            Console.WriteLine("Eingie Prüfungen vorab:\n");

            Console.WriteLine("-Internetverbindung prüfen");
            checkInternetConnection();
            checkForError();

            Console.WriteLine("-Porterreichbarkeit einrichten");
            yellow("NICHT EINGERICHTET");
            checkForError();

            Console.WriteLine("-Auf Aktualisierung prüfen");
            checkForUpdate();
            checkForError();


        }
        static void checkInternetConnection()
        {
            Ping ping = new Ping();
            try
            {
                PingReply reply = ping.Send("www.google.de", 1000);
                if (reply.Status == IPStatus.Success)
                    green("OK");
                else
                    red("Es ist keine Verbindung zum Internet möglich!\nBitte überprüfen Sie die Firewall- und Netzwerkeinstellungen.");
            }
            catch 
            {
                red("Es ist keine Verbindung zum Internet möglich!\nBitte überprüfen Sie die Firewall- und Netzwerkeinstellungen.");
            }



        }
        static void checkForUpdate()
        {
            WebClient downloadService = new WebClient();
            Version onlineVersion = null;
            Version installedVersion = null;
            try
            {
                downloadService.DownloadFile("http://janmarcus.eskes.de/BBSProjekt/BBSSchulstundenServer.exe", Path.GetTempPath() + "BBSSchulstundenServer.version");
                FileVersionInfo online = FileVersionInfo.GetVersionInfo(Path.GetTempPath() + "BBSSchulstundenServer.version");
                FileVersionInfo installed = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

                onlineVersion = new Version(string.Format("{0}.{1}.{2}.{3}", online.FileMajorPart, online.FileMinorPart, online.FileBuildPart, online.FilePrivatePart));
                installedVersion = new Version(string.Format("{0}.{1}.{2}.{3}", installed.FileMajorPart, installed.FileMinorPart, installed.FileBuildPart, installed.FilePrivatePart));
            }
            catch (Exception ex)
            {
                yellow("Es ist ein Fehler beim Update aufgetreten!\nMeldung: " + ex.Message);
                return;
            }
            if (updated && onlineVersion == installedVersion)
            {
                green("Update wurde erfolgreich instaliert");
                return;
            }
            else if (onlineVersion > installedVersion)
            {
                yellow("Es gibt eine neuere Version. Wollen Sie den Server aktualisieren? (Y/N)");
                string antwort = Console.ReadLine();
                if (antwort == "y" || antwort == "Y")
                {
                    warning = false;
                    updateServer();
                }
                else
                {
                    yellow("Update wurde übersprungen");
                }
            }
            else
            {
                green("Dies ist die Aktuelle Version");
            }
        }
        static void updateServer()
        {
            string filename_exe = Assembly.GetExecutingAssembly().Location;
            string ordner = Path.GetTempPath();

            string filename_update = Path.GetTempPath() + "BBSSchulstundenServer.version";
            string filename_VBS = Path.Combine(ordner, "update.vbs");

            // auf Schreibrechte im exe-Verzeichnis prüfen
            bool schreibrechte = false;
            try
            {
                FileStream stream = File.Create(filename_exe + "_test");
                stream.Close();
                File.Delete(filename_exe + "_test");
                schreibrechte = true;
            }
            catch (UnauthorizedAccessException)
            { }

            // update.vbs erzeugen
            try
            {
                FileStream stream = new FileStream(filename_VBS, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("Option Explicit");
                writer.WriteLine();
                writer.WriteLine("Dim Filename_exe");
                writer.WriteLine("Dim Filename_update");
                writer.WriteLine("Dim FSO");
                writer.WriteLine("Dim Shell");
                writer.WriteLine("Dim Zusatz");
                writer.WriteLine();
                writer.WriteLine("Set FSO = CreateObject(\"Scripting.FileSystemObject\")");
                writer.WriteLine("Set Shell = WScript.CreateObject(\"WScript.Shell\")");
                writer.WriteLine();
                writer.WriteLine("WScript.Sleep(2000)");
                writer.WriteLine("Filename_exe = \"" + filename_exe + "\"");
                writer.WriteLine("Filename_update = \"" + filename_update + "\"");
                writer.WriteLine();
                writer.WriteLine("If Not FSO.FileExists(Filename_update) Then");
                writer.WriteLine("	MsgBox \"Die Update-Datei '\" & Filename_update & \"' existiert nicht.\", vbOkOnly & vbCritical, WScript.ScriptName");
                writer.WriteLine("	WScript.Quit");
                writer.WriteLine("End If");
                writer.WriteLine();
                writer.WriteLine("FSO.CopyFile Filename_update, Filename_exe, true");
                writer.WriteLine("FSO.DeleteFile Filename_update, true");
                if (schreibrechte)
                {
                    writer.WriteLine("MsgBox \"Update erfolgreich\" & vbCrLf & \"Die Anwendung wird neu gestartet.\", vbOkOnly & vbInformation, WScript.ScriptName");
                    writer.WriteLine("Shell.Run Chr(34) & Filename_exe & Chr(34)");
                }
                else
                {
                    writer.WriteLine("MsgBox \"Update erfolgreich\" & vbCrLf & \"Bitte starten Sie die Anwendung neu.\" & vbCrLf & \"(erfolgt aus Berechtigungsgründen nicht automatisiert)\", vbOkOnly & vbInformation, WScript.ScriptName");
                }
                writer.Close();
                stream.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Fehler beim Schreiben der Datei " + filename_VBS + System.Environment.NewLine + System.Environment.NewLine + ex.ToString(), "Update durchführen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ... und dann 'update.vbs' ausführen
            //MessageBox.Show("Schreibrechte = " + schreibrechte.ToString());
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "wscript.exe";
            psi.Arguments = filename_VBS;
            psi.WorkingDirectory = Path.GetDirectoryName(filename_VBS);
            psi.UseShellExecute = true;
            if (schreibrechte == false) psi.Verb = "runas";
            Process process = Process.Start(psi);

            // diese Anwendung beenden
            global::System.Environment.Exit(0);
        }
        static void red(String eingabe)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(eingabe);
            Console.ForegroundColor = ConsoleColor.White;
            error = true;
        }
        static void green(String eingabe)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(eingabe);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void yellow(String eingabe)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(eingabe);
            Console.ForegroundColor = ConsoleColor.White;
            warning = true;
        }
        static void startupText()
        {
            Console.Title = "Vertretungsplan Server";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 Schulsystem zur Raum- und Stundensuche BBS-Neuenahr                       ║");
            Console.WriteLine("║                            Instalierte Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "                                   ║");
            Console.WriteLine("║                                                                                           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void checkForError()
        {

            if (error)
            {
                red("Es ist ein kritischer Systemfehler aufgetreten");
                Console.Read();
                Environment.Exit(0x0001);
            }
        }
        static void getArgs(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (string argument in args)
                {
                    startupArgs.Add(argument);
                    if (argument.Contains("updated")) updated = true;
                }
            }
        }
    }
}
