using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Source
{
    public class Schulstunde
    {
        public string Tag;
        public string Position;
        public string Lehrer;
        public string Fach;
        public string Raum;
        public string Klasse;
        public string Vertreter;
        public string Art;
        public string Info;
		    public static List<Schulstunde> Schulstunden = new List<Schulstunde>();
		
        override public string ToString()
        {
            string ausgabe = Position + "," + Lehrer + "," + Fach + "," + Raum + "," + Klasse + "," + Vertreter + "," + Art + "," + Info;
            return ausgabe;
        }
		
    }
}
