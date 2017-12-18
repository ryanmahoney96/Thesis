using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        private static string fontcolor = $"fontcolor=black";
        private static string participantStyle = $"style=filled, fillcolor=\"#ffc6d1\" shape=oval";
        private static string eventStyle = $"style=filled, fillcolor=\"#fff790\" shape=rect";
        private static string locationStyle = $"style=filled, fillcolor=\"#d9ffd9\" shape=rect";
        private static int participantCounter = 1;

        //How many events take place at a certain location
        public static void BasicMap(string focusLocation)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Event Map for {focusLocation}\" {fontname} {overlap}; ");

            AddEventInformation(ref echo, focusLocation);

            AppendLegend(ref echo);

            echo.Append("}");

            SaveEcho(echo, "EventMap", focusLocation);
        }

        //Every location and the events that take place at them
        public static void MapWithParticipants(string focusLocation)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Event Map with Participants for {focusLocation}\" {fontname} {overlap}; ");

            AddEventInformation(ref echo, focusLocation, true);

            AppendLegend(ref echo, true);

            echo.Append("}");

            SaveEcho(echo, "EventMapWithParticipants", focusLocation);
        }

        private static void AddEventInformation(ref StringBuilder echo, string focusLocation, bool getParticipants = false)
        {
            echo.AppendLine($"\"{focusLocation}\" [{locationStyle}, {fontname}, {fontcolor}]; ");

            foreach (EventInfo e in GetEventsAtLocation(focusLocation))
            {
                echo.Append($"\"{e.Name}\" [{eventStyle}, {fontname}, {fontcolor}, label=\"{e.Name}");
                if (!string.IsNullOrWhiteSpace(e.Unit_date))
                {
                    echo.Append($"\nSource: {e.Unit_date}");
                }
                if (!string.IsNullOrWhiteSpace(e.Date))
                {
                    echo.Append($"\nDate: {e.Date}");
                }
                echo.AppendLine($"\"]; ");
                echo.AppendLine($"\"{e.Name}\" -- \"{focusLocation}\"; ");

                if (getParticipants)
                {
                    foreach(var p in GetParticipantsAtEvent(e.Name))
                    {
                        if (!string.IsNullOrWhiteSpace(p.Value))
                        {
                            string participantNodeName = $"part{participantCounter++}";
                            echo.AppendLine($"\"{participantNodeName}\" [label=\"{p.Value}\" {participantStyle} {fontname}]; ");

                            echo.AppendLine($"\"{participantNodeName}\" -- \"{e.Name}\"; ");
                        }
                    }
                }

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

        private static IEnumerable<XElement> GetParticipantsAtEvent(string focusEvent)
        {
            return (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                    where c.Element("name").Value.Equals(focusEvent)
                    select c.Element("participants").Elements()).First();
        }

        //if focusLocation left null, it is a full event map
        private static void SaveEcho(StringBuilder echo, string type, string focusLocation)
        {
            if (!string.IsNullOrWhiteSpace(focusLocation))
            {
                focusLocation = focusLocation.Replace(" ", "_");
                focusLocation = focusLocation.Replace("'", "");
                focusLocation += "_";
            }

            string textPath = XMLParser.FolderPath + $"\\{type}.dot";
            string imagePath = $"{XMLParser.FolderPath}/{focusLocation}{type}.svg";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            //Process.Start("cmd.exe", @"/c" + $"neato -Tsvg {textPath} -o {imagePath}  & {imagePath}");

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c" + $"neato -Tsvg \"{textPath}\" -o \"{imagePath}\" & del \"{textPath}\" & \"{imagePath}\"";
            process.StartInfo = startInfo;
            process.Start();
        }

        private static void AppendLegend(ref StringBuilder echo, bool participantsOn = false)
        {
            echo.AppendLine($"\"Location\" [{locationStyle}, {fontname}];");
            echo.AppendLine($"\"Event\" [{eventStyle}, {fontname}];");
            if (participantsOn)
            {
                echo.AppendLine($"\"Participant\" [{participantStyle}, {fontname}];");
            }

            echo.AppendLine($"subgraph {{ label=\"Legend\"; overlap=false; ");
            echo.AppendLine($"\"Location\" -- \"Event\";");
            if (participantsOn)
            {
                echo.AppendLine($"\"Event\" -- \"Participant\" ;");
            }
            echo.AppendLine("}");
        }
    }
}
