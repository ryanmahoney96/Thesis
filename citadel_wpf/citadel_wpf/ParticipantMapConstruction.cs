using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class ParticipantMapConstruction
    {
        private static string fontname = $"fontname=\"Helvetica\"";
        private static string overlap = $"overlap=false";
        private static string bgcolor = $"bgcolor=white";
        private static string fontcolor = $"fontcolor=black";
        private static string nodeShape = $"shape=rect";
        private static string centerShape = $"shape=ellipse";
        private static string nodeColor = $"color=navy";
        private static string centerColor = $"color=orangered";

        //How many characters were at a certain event
        public static void CreateMap(string focusEvent)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Participant Map for {focusEvent}\" {fontname} {overlap} {bgcolor}; ");

            AddCharacterInformation(ref echo, focusEvent);

            echo.Append("}");

            SaveEcho(echo, "ParticipantMap", focusEvent);
        }

        private static void AddCharacterInformation(ref StringBuilder echo, string focusEvent)
        {
            echo.Append($"\"{focusEvent}\" [{centerShape}, {centerColor}, {fontname}, {fontcolor}]; ");

            foreach (string c in GetCharactersAtEvent(focusEvent))
            {
                echo.Append($"\"{c}\" [{nodeColor}, {nodeShape}, {fontname}, {fontcolor}," +
                    $" label=\"{c}\"]; \"{c}\" -- \"{focusEvent}\"; ");
            }
        }

        private static IEnumerable<string> GetCharactersAtEvent(string focusEvent)
        {
            return (from c in XMLParser.ParticipantXDocument.Handle.Root.Descendants("character_participation")
                    where c.Element("entity_two").Value.ToString().Equals(focusEvent)
                    select c.Element("entity_one").Value.ToString());
        }

        //if focusLocation left null, it is a full event map
        private static void SaveEcho(StringBuilder echo, string type, string focusEvent = "")
        {
            if (!string.IsNullOrWhiteSpace(focusEvent))
            {
                focusEvent = focusEvent.Replace(" ", "_");
                focusEvent = focusEvent.Replace("'", "");
                focusEvent += "_";
            }

            string textPath = XMLParser.FolderPath + $"\\{type}.dot";
            string imagePath = $"{XMLParser.FolderPath}/{focusEvent}{type}.png";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            //twopi or neato
            Process.Start("cmd.exe", @"/c" + $"twopi -Tpng {textPath} -o {imagePath} & del {textPath} & {imagePath}");
        }
    }
}
