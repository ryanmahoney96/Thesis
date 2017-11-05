﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class Media : IEntity
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

        public void Save(string folderPath)
        {
            System.IO.StreamWriter handle = null;
            
            string filePath = folderPath + "\\media_notes.xml";

            handle = new System.IO.StreamWriter(filePath, false);
            handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<media_notes>\n\t");

            handle.Write(ToXMLString());

            handle.Write("</media_notes>");
            
            handle.Close();
        }

        public string GetName()
        {
            return Title;
        }

        public string ToXMLString()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<media_note>");
            s.Append("<title>" + Title + "</title>\n\t\t");
            s.Append("<year>" + Year + "</year>\n\t\t");
            s.Append("<type>" + Type + "</type>\n\t\t");
            s.Append("<genre>" + Genre + "</genre>\n\t\t");
            s.Append("<summary>" + Summary + "</summary>\n\t");
            s.Append("</media_note>");


            return s.ToString();
        }


    }
}