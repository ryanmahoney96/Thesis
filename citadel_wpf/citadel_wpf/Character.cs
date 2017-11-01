using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class Character : Entity
    {
        static List<Entity> CharacterRecords;
        string Name;
        string Gender;
        string Description;

        public Character(string n, string g, string d)
        {
            Name = n;
            Gender = g;
            Description = d;

            if (CharacterRecords == null)
            {
                CharacterRecords = new List<Entity>();
            }
        }

        public static void AddRecord(Character c)
        {
            //TODO verify character is not already in record
            CharacterRecords.Add(c);
        }
        public static List<Entity> GetRecords()
        {
            return CharacterRecords;
        }

        public string ToXMLString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<character>\n\t\t");
            s.Append("<name>" + Name + "</name>\n\t\t");
            s.Append("<gender>" + Gender + "</gender>\n\t\t");
            s.Append("<description>" + Description + "</description>\n\t");
            s.Append("</character>\n\n");

            return s.ToString();
        }


    }
}
