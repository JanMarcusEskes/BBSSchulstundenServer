using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hausaufgabenplan
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

        /*public ListViewItem ToListViewItem()
        {
            ListViewItem lstvItem = new ListViewItem();
            lstvItem.Text = Position;
            lstvItem.Name = Position + ';' + Klasse;
            lstvItem.SubItems.Add(Lehrer);
            lstvItem.SubItems.Add(Fach);
            lstvItem.SubItems.Add(Raum);
            lstvItem.SubItems.Add(Klasse);
            lstvItem.SubItems.Add(Vertreter);
            lstvItem.SubItems.Add(Art);
            lstvItem.SubItems.Add(Info);
            return lstvItem;

            //   string ausgabe = Position + "    " + Lehrer + "    " + Fach + "    " + Raum + "    " + Klasse + "    " + Vertreter + "    " + Art + "    " + Info;
            //   return ausgabe;
        }*/
    }

}
