using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Source
{
    public static class ConsoleComands
    {

        public static void Execute(string command)
        {
            switch (command.ToLower())
            {
                case "clr":
                case "clear":
                    {
                        Clear();
                        return;
                    }
                case "x":
                case "exit":
                    {
                        Shutdown();
                        return;
                    }
                case "h":
                case "help":
                    {
                        Help();
                        return;
                    }
                case "reboot":
                    {
                        Reboot();
                        return;
                    }
                case "reset":
                    {
                        Reset();
                        return;
                    }
                case "settings":
                    {
                        Settings();
                        return;
                    }
                default:
                    break;
            }

            //Kein Kommando gefunden
            Console.WriteLine("Das eingegebene Kommando ist nicht bekannt! Für eine Liste aller Komandos tippen Sie bitte 'help' ein.");
            return;
        }


        #region Kommandos

        static void Settings()
        {
            Forms.Settings settings = new Forms.Settings();
            settings.ShowDialog();
        }

        static void Shutdown()
        {
            Environment.Exit(0);
        }

        static void Help()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("Liste aller verfügbaren Konsolen-Kommandos\n");
            sw.WriteLine("CLEAR                         Leer den Bildschirm");
            sw.WriteLine("CLR                           Alias für CLEAR");
            sw.WriteLine("EXIT                          Fährt den Server herunter");
            sw.WriteLine("HELP                          Zeigt diese Liste an.");
            sw.WriteLine("H                             Alias für HELP");
            sw.WriteLine("REBOOT                        Startet den Server komplett neu");
            sw.WriteLine("RESET                         Löscht die Einstellungsdatei des Servers");
            sw.WriteLine("SETTINGS                      Ruft den einstellungsdialog auf");
            sw.WriteLine("X                             Alias für EXIT");
            Console.WriteLine(sw);
        }

        static void Clear()
        {
            Console.Clear();
            Source.Startup.StartupText();
        }

        static void Reboot()
        {
            Application.Restart();
            Shutdown();
        }

        static void Reset()
        {
            File.Copy(Source.Settings.Filename, Source.Settings.Filename + "_old");
            File.Delete(Source.Settings.Filename);
            Application.Restart();
            Environment.Exit(0);
        }
        #endregion
    }
}
