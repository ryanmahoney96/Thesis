using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class GeneralNote : IEntity
    {
        static List<IEntity> NoteRecords;
        string Description;

        public GeneralNote(string d)
        {

            Description = d;

            if (NoteRecords == null)
            {
                NoteRecords = new List<IEntity>();
            }
        }

        public static bool AddRecord(GeneralNote g)
        {
            bool alreadyPresent = NoteRecords.Where(s => s.GetName().Equals(g.GetName())).Count() > 0 ? true : false;
            if (!alreadyPresent)
            {
                NoteRecords.Add(g);
            }

            return !alreadyPresent;
        }
        public static List<IEntity> GetRecords()
        {
            return NoteRecords;
        }

        public string GetName()
        {
            return Description;
        }

        public string ToXMLString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<general_note>\n\t\t");
            s.Append("<description>" + Description + "</description>\n\t");
            s.Append("</general_note>\n\n");

            return s.ToString();
        }


    }
}

