using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class Location : Entity
    {
        static List<Entity> LocationRecords;
        //TODO can these variables be changed into a hash table?
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
                LocationRecords = new List<Entity>();
            }
        }

        public static void AddRecord(Location l)
        {
            //TODO verify note is not already in record
            LocationRecords.Add(l);
        }
        public static List<Entity> GetRecords()
        {
            return LocationRecords;
        }

        public string ToXMLString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<location>\n\t\t");
            s.Append("<name>" + Name + "</name>\n\t\t");
            s.Append("<Type>" + Type + "</Type>\n\t\t");
            s.Append("<subtype>" + Subtype + "</subtype>\n\t\t");
            s.Append("<description>" + Description + "</description>\n\t");
            s.Append("</location>\n\n");

            return s.ToString();
        }


    }
}