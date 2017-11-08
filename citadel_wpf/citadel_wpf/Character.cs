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
        List<Character> Children;
        List<Character> Parents;

        public Character(string n, string g, string d)
        {
            Name = n;
            Gender = g;
            Description = d;
            Children = new List<Character>();
            Parents = new List<Character>();

            if (CharacterRecords == null)
            {
                CharacterRecords = new List<IEntity>();
            }
        }
        public bool AddChild(Character child)
        {
            bool alreadyPresent = Children.Where(s => s.GetName().Equals(child.GetName())).Count() > 0 ? true : false;
            if (!alreadyPresent)
            {
                Children.Add(child);
            }

            return !alreadyPresent;
        }
        public List<Character> GetChildren()
        {
            //TODO: Add Parents?
            return Children;
        }
        public List<Character> GetParents()
        {
            //TODO: Add Parents?
            return Parents;
        }

        public static bool AddRecord(Character character)
        {
            //TODO verify c_relationship does not already exist as well as just the character
            bool alreadyPresent = CharacterRecords.Where(s => s.GetName().Equals(character.GetName())).Count() > 0 ? true : false;
            if (!alreadyPresent)
            {
                CharacterRecords.Add(character);
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
