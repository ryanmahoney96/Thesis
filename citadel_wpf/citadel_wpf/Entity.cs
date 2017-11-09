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

            s.Append("<character>");
            s.Append("<name>" + name + "</name>");
            s.Append("<gender>" + gender + "</gender>");
            s.Append("<description>" + description + "</description>");
            s.Append("</character>");

            return s.ToString();
        }

        public static string NoteToXML(string name, string description)
        {
            StringBuilder s = new StringBuilder();

            s.Append("<general_note>");
            s.Append("<name>" + name + "</name>");
            s.Append("<description>" + description + "</description>");
            s.Append("</general_note>");

            return s.ToString();
        }

        public static string EventToXML(string name, string location, string unit_date, string date, string description)
        {
            StringBuilder s = new StringBuilder();

            s.Append("<event>");
            s.Append("<name>" + name + "</name>");
            s.Append("<location>" + location + "</location>");
            s.Append("<unit_date>" + unit_date + "</unit_date>");
            s.Append("<date>" + date + "</date>");
            s.Append("<description>" + description + "</description>");
            s.Append("</event>");

            return s.ToString();
        }

        public static string LocationToXML(string name, string type, string subtype, string description)
        {
            StringBuilder s = new StringBuilder();

            s.Append("<location>");
            s.Append("<name>" + name + "</name>");
            s.Append("<type>" + type + "</type>");
            s.Append("<subtype>" + subtype + "</subtype>");
            s.Append("<description>" + description + "</description>");
            s.Append("</location>");

            return s.ToString();
        }

    }
}
