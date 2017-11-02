using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class Location : IEntity
    {
        static List<IEntity> LocationRecords;
        string Name;
        string Type;
        string Subtype;
        string Description;

        public Location(string n, string t, string s, string d)
        {

            Name = n;
            Type = t;
            Subtype = s;
            Description = d;

            if (LocationRecords == null)
            {
                LocationRecords = new List<IEntity>();
            }
        }

        public static bool AddRecord(Location l)
        {
            bool alreadyPresent = LocationRecords.Where(s => s.GetName().Equals(l.GetName())).Count() > 0 ? true : false;
            if (!alreadyPresent)
            {
                LocationRecords.Add(l);
            }

            return !alreadyPresent;
        }
        public static List<IEntity> GetRecords()
        {
            return LocationRecords;
        }

        public string GetName()
        {
            return Name;
        }

        public string ToXMLString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<location>\n\t\t");
            s.Append("<name>" + Name + "</name>\n\t\t");
            s.Append("<type>" + Type + "</type>\n\t\t");
            s.Append("<subtype>" + Subtype + "</subtype>\n\t\t");
            s.Append("<description>" + Description + "</description>\n\t");
            s.Append("</location>\n\n");

            return s.ToString();
        }


    }
}