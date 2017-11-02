using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class Character : IEntity
    {
        static List<IEntity> CharacterRecords;
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
                CharacterRecords = new List<IEntity>();
            }
        }

        public static bool AddRecord(Character c)
        {
            //TODO verify c_relationship does not already exist as well
            bool alreadyPresent = CharacterRecords.Where(s => s.GetName().Equals(c.GetName())).Count() > 0 ? true : false;
            if (!alreadyPresent)
            {
                CharacterRecords.Add(c);
            }

            return !alreadyPresent;
        }
        public static List<IEntity> GetRecords()
        {
            return CharacterRecords;
        }

        public string GetName()
        {
            return Name;
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
