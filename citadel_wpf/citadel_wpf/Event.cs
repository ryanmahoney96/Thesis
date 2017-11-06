using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class EventNote : IEntity
    {
        static List<IEntity> EventRecords;
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
                EventRecords = new List<IEntity>();
            }
        }

        public static bool AddRecord(EventNote e)
        {
            //TODO make this generic for each entity? requires entity to be abstract class. Must include "records" list and Entity as parameters
            bool alreadyPresent = EventRecords.Where(s => s.GetName().Equals(e.GetName())).Count() > 0 ? true : false;
            if (!alreadyPresent)
            {
                EventRecords.Add(e);
            }

            return !alreadyPresent;
        }
        public static List<IEntity> GetRecords()
        {
            return EventRecords;
        }

        public string GetName()
        {
            return Name;
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