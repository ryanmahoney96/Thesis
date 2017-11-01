using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class Media : Entity
    {
        string Title;
        string Year;
        string Type;
        string Genre;
        string Summary;

        public Media(string t, string y, string type, string g, string s)
        {

            Title = t;
            Year = y;
            Type = type;
            Genre = g;
            Summary = s;

        }

        public string ToXMLString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<media_information>\n\t\t");
            s.Append("<title>" + Title + "</title>\n\t\t");
            s.Append("<year>" + Year + "</year>\n\t\t");
            s.Append("<type>" + Type + "</type>\n\t\t");
            s.Append("<genre>" + Genre + "</genre>\n\t\t");
            s.Append("<summary>" + Summary + "</summary>\n\t");
            s.Append("</media_information>\n\n");

            return s.ToString();
        }


    }
}
