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
    class TimelineConstruction
    {

        private static string rankdir = $"rankdir=\"LR\"";
        private static string fontname = $"fontname=\"Helvetica\"";
        private static string overlap = $"overlap=false";
        private static string bgcolor = $"bgcolor=white";
        private static string fontcolor = $"fontcolor=black";
        private static string nodeShape = $"shape=rect";
        private static string centerShape = $"shape=ellipse";
        private static string nodeColor = $"color=navy";
        private static string clusterColor = $"color=black";

        //TODO give timeline a title
        public static void CreateTimeline(List<string> selectedEvents, string title = "")
        {

            StringBuilder echo = new StringBuilder($"digraph s {{ label=\"Timeline {title}\" {fontname} {overlap} {bgcolor} {rankdir} compound=true; ");

            GetEventInformation(ref echo, selectedEvents);

            echo.Append("}");

            SaveEcho(echo, "Timeline", title);


        }

        private static void GetEventInformation(ref StringBuilder echo, List<string> selectedEvents)
        {
            //echo.Append($"\"{focusLocation}\" [{centerShape}, {centerColor}, {fontname}, {fontcolor}]; ");
            SortedDictionary<string, List<XElement>> eventDictionary = new SortedDictionary<string, List<XElement>>();

            foreach (string sel in selectedEvents)
            {
                XElement temp = (from e in XMLParser.EventXDocument.Handle.Root.Elements("event")
                           where e.Element("name").Value.Equals(sel)
                           select e).First();

                if (!eventDictionary.ContainsKey(temp.Element("order_key").Value))
                {
                    eventDictionary[temp.Element("order_key").Value] = new List<XElement>();
                }
                eventDictionary[temp.Element("order_key").Value].Add(temp);

            }

            string lastElement = "";
            int clusterIndex = 0;

            foreach (var currentList in eventDictionary)
            {
                //adding each event as a dot node
                foreach (var currentEvent in currentList.Value)
                {
                    string name = currentEvent.Element("name").Value;
                    echo.Append($"\"{name}\" [{nodeColor}, {nodeShape}, {fontname}, {fontcolor}, label=\"{name}");

                    if (!string.IsNullOrWhiteSpace(currentEvent.Element("unit_date").Value))
                    {
                        echo.Append($"\nSource: {currentEvent.Element("unit_date").Value}");
                    }
                    if (!string.IsNullOrWhiteSpace(currentEvent.Element("date").Value))
                    {
                        echo.Append($"\nDate: {currentEvent.Element("date").Value}");
                    }
                    echo.Append($"\"]; ");
                }

                if (currentList.Value.Count > 1)
                {
                    echo.Append($"subgraph cluster_{++clusterIndex} {{ label=\"\" ");

                    foreach (var currentEvent in currentList.Value)
                    {
                        echo.Append($"\"{currentEvent.Element("name").Value}\"; ");
                    }

                    echo.Append($" {clusterColor};}}; ");

                    if (!string.IsNullOrWhiteSpace(lastElement))
                    {
                        echo.Append($" \"{lastElement}\" -> \"{currentList.Value.First().Element("name").Value}\" [ltail=\"{lastElement}\" lhead=\"cluster_{clusterIndex}\"]; ");
                    }

                }
                else if (!string.IsNullOrWhiteSpace(lastElement))
                {
                    echo.Append($" \"{lastElement}\" -> \"{currentList.Value.First().Element("name").Value}\"; ");
                }


                lastElement = currentList.Value.First().Element("name").Value;
            }


        }

        //TODO make generic
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

            /*& del {textPath}*/
            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {imagePath}  & {imagePath}");
        }
    }
}

