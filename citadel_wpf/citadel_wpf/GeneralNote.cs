using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class GeneralNote : Entity
    {
        static List<Entity> NoteRecords;
        string Description;

        public GeneralNote(string d)
        {

            Description = d;

            if (NoteRecords == null)
            {
                NoteRecords = new List<Entity>();
            }
        }

        public static void AddRecord(GeneralNote g)
        {
            //TODO verify note is not already in record
            NoteRecords.Add(g);
        }
        public static List<Entity> GetRecords()
        {
            return NoteRecords;
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

