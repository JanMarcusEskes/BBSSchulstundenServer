using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

namespace Server
{
    class Program
    {
        public static Source.Settings settings = Source.Settings.Load();
        static void Main(string[] args)
        {
            Source.Startup.InitStartup(settings);

            bool schleife = true;
            while (schleife)
            {
                Console.Write("\n--> ");
                string eingabe = Console.ReadLine();
                Source.ConsoleComands.Execute(eingabe);

            }
        }
    }
}
