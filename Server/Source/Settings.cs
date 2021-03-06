﻿using System;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Server.Source
{
    public class Settings
    {
        public string user;
        public string passwort;
        public static string Filename
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stundenplan_Server.xml"); }
        }

        public void User(string newUser)
        {
            string temp = null;
            foreach (char tempBuchstabe in newUser)
            {
                temp += (char)(tempBuchstabe - 1);
            }
            user = temp;
        }
        public string User()
        {
            if (user != null)
            {
                string temp = String.Empty;
                foreach (char tempBuchstabe in user)
                {
                    temp += (char)(tempBuchstabe + 1);
                }
                return temp;
            }
            else
                return "";
        }
        public void Passwort(string newPasswort)
        {
            string temp = null;
            foreach (char tempBuchstabe in newPasswort)
            {
                temp += (char)(tempBuchstabe - 1);
            }
            passwort = temp;
        }
        public string Passwort()
        {
            if (user != null)
            {
                string temp = String.Empty;
                foreach (char tempBuchstabe in passwort)
                {
                    temp += (char)(tempBuchstabe + 1);
                }
                return temp;
            }
            else
                return "";
        }
        public void Save(Settings Einstellungen)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            StreamWriter writer = null;
            try
            {
                writer = File.CreateText(Filename);
                serializer.Serialize(writer, Einstellungen);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Schreiben der Einstellungen:\n" + ex.Message, "Einstellungen speichern", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (writer != null) { writer.Close(); writer.Dispose(); }
            }
        }
        public static Settings Load()
        {
            //Datei exsistiert nicht
            if (!File.Exists(Filename))
            {
                return new Settings();
            }

            XmlSerializer serializer = serializer = new XmlSerializer(typeof(Settings));
            using (StreamReader reader = File.OpenText(Filename))
            {
                try
                {
                    return (Settings)serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Laden der Einstellungen:\n" + ex.Message, "Einstellungen laden", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
            }
            return new Settings();
        }
    }
}
