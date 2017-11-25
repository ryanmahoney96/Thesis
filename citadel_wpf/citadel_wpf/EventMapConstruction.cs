using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class EventMapConstruction
    {
        struct EventInfo
        {
            public string Name;
            public string Unit_date;
            public string Date;
        }

        private static string fontname = $"fontname=\"Helvetica\"";
        private static string overlap = $"overlap=false";
        private static string bgcolor = $"bgcolor=white";
        private static string fontcolor = $"fontcolor=black";
        private static string nodeShape = $"shape=rect";
        private static string centerShape = $"shape=ellipse";
        private static string nodeColor = $"color=navy";
        private static string centerColor = $"color=orangered";


        //How many events take place at a certain location
        public static void SingleLocation(string focusLocation)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Single Event Map for {focusLocation}\" {fontname} {overlap} {bgcolor}; ");

            AddEventInformation(ref echo, focusLocation);

            echo.Append("}");

            SaveEcho(echo, "SingleEventMap", focusLocation);
        }

        //Every location and the events that take place at them
        public static void AllLocations()
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Full Event Map\" {fontname} {overlap} {bgcolor}; ");

            foreach (var l in XMLParser.GetAllEntityNames(XMLParser.LocationXDocument.Handle))
            {
                AddEventInformation(ref echo, l);
            }

            echo.Append("}");

            SaveEcho(echo, "FullEventMap");
        }

        private static void AddEventInformation(ref StringBuilder echo, string focusLocation)
        {
            echo.Append($"\"{focusLocation}\" [{centerShape}, {centerColor}, {fontname}, {fontcolor}]; ");

            foreach (EventInfo e in GetEventsAtLocation(focusLocation))
            {
                echo.Append($"\"{e.Name}\" [{nodeColor}, {nodeShape}, {fontname}, {fontcolor}, label=\"{e.Name}");
                if (!string.IsNullOrWhiteSpace(e.Unit_date))
                {
                    echo.Append($"\n{e.Unit_date}");
                }
                if (!string.IsNullOrWhiteSpace(e.Unit_date))
                {
                    echo.Append($"\n{e.Date}");
                }
                echo.Append($"\"]; ");
                echo.Append($"\"{e.Name}\" -- \"{focusLocation}\"; ");
            }
        }

        private static IEnumerable<EventInfo> GetEventsAtLocation(string focusLocation)
        {
            return (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                    where c.Element("location").Value.ToString().Equals(focusLocation)
                    select new EventInfo
                    {
                        Name = c.Element("name").Value.ToString(),
                        Unit_date = c.Element("unit_date").Value.ToString(),
                        Date = c.Element("date").Value.ToString()
                    });
        }

        //if focusLocation left null, it is a full event map
        private static void SaveEcho(StringBuilder echo, string type, string focusLocation = "")
        {
            if (!string.IsNullOrWhiteSpace(focusLocation))
            {
                focusLocation = focusLocation.Replace(" ", "_");
                focusLocation = focusLocation.Replace("'", "");
                focusLocation += "_";
            }

            string textPath = XMLParser.FolderPath + $"\\{type}.dot";
            string imagePath = $"{XMLParser.FolderPath}/{focusLocation}{type}.png";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            //twopi or neato
            Process.Start("cmd.exe", @"/c" + $"twopi -Tpng {textPath} -o {imagePath} & del {textPath} & {imagePath}");
        }
    }
}
