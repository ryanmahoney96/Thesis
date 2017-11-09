using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace citadel_wpf
{
    public class Entity
    {

        public static void AddRecord()
        {
            //bool alreadyPresent = NoteRecords.Where(s => s.GetName().Equals(g.GetName())).Count() > 0 ? true : false;
            //if (!alreadyPresent)
            //{
            //    NoteRecords.Add(g);
            //}

            //return !alreadyPresent;
        }

        public static string CharacterToXML(string name, string gender, string description)
        {
            StringBuilder s = new StringBuilder();

            s.Append("<character>\n\t\t");
            s.Append("<name>" + name + "</name>\n\t\t");
            s.Append("<gender>" + gender + "</gender>\n\t\t");
            s.Append("<description>" + description + "</description>\n\t");
            s.Append("</character>\n\n");

            return s.ToString();
        }

        public static string NoteToXML(string name, string description)
        {
            StringBuilder s = new StringBuilder();

            s.Append("<general_note>\n\t\t");
            s.Append("<name>" + name + "</name>\n\t");
            s.Append("<description>" + description + "</description>\n\t");
            s.Append("</general_note>\n\n");

            return s.ToString();
        }

        public static string EventToXML(string name, string location, string unit_date, string date, string description)
        {
            StringBuilder s = new StringBuilder();

            s.Append("<event>\n\t\t");
            s.Append("<name>" + name + "</name>\n\t\t");
            s.Append("<location>" + location + "</location>\n\t\t");
            s.Append("<unit_date>" + unit_date + "</unit_date>\n\t\t");
            s.Append("<date>" + date + "</date>\n\t\t");
            s.Append("<description>" + description + "</description>\n\t");
            s.Append("</event>\n\n");

            return s.ToString();
        }

        public static string LocationToXML(string name, string type, string subtype, string description)
        {
            StringBuilder s = new StringBuilder();

            s.Append("<location>\n\t\t");
            s.Append("<name>" + name + "</name>\n\t\t");
            s.Append("<type>" + type + "</type>\n\t\t");
            s.Append("<subtype>" + subtype + "</subtype>\n\t\t");
            s.Append("<description>" + description + "</description>\n\t");
            s.Append("</location>\n\n");

            return s.ToString();
        }

    }
}
