using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class EventNote : Entity
    {
        static List<Entity> EventRecords;
        //TODO can these variables be changed into a hash table?
        string Name;
        string Location;
        string Unit_Date;
        string Date;
        string Description;
        
        public EventNote(string n, string l, string u, string d, string desc)
        {

            Name = n;
            Location = l;
            Unit_Date = u;
            Date = d;
            Description = desc;

            if (EventRecords == null)
            {
                EventRecords = new List<Entity>();
            }
        }

        public static void AddRecord(EventNote e)
        {
            //TODO verify note is not already in record
            EventRecords.Add(e);
        }
        public static List<Entity> GetRecords()
        {
            return EventRecords;
        }

        public string ToXMLString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<event>\n\t\t");
            s.Append("<name>" + Name + "</name>\n\t\t");
            s.Append("<location>" + Location + "</location>\n\t\t");
            s.Append("<unit_date>" + Unit_Date + "</unit_date>\n\t\t");
            s.Append("<date>" + Date + "</date>\n\t\t");
            s.Append("<description>" + Description + "</description>\n\t");
            s.Append("</event>\n\n");

            return s.ToString();
        }


    }
}