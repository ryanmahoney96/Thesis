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
        private static string participantStyle = $"style=filled, fillcolor=\"#ffc6d1\" shape=oval";
        private static string eventStyle = $"style=filled, fillcolor=\"#fff750\" shape=rect";

        //How many characters were at a certain event
        public static void CreateMap(string focusEvent)
        {
            StringBuilder echo = new StringBuilder($"digraph s {{ label=\"Participant Map for {focusEvent}\" {fontname} {overlap}; ");

            AddCharacterInformation(ref echo, focusEvent);

            echo.Append("}");

            SaveEcho(echo, "ParticipantMap", focusEvent);
        }

        private static void AddCharacterInformation(ref StringBuilder echo, string focusEvent)
        {
            echo.Append($"\"{focusEvent}\" [{eventStyle}, {fontname}]; ");

            foreach (string c in GetCharactersAtEvent(focusEvent))
            {
                echo.AppendLine($"\"{c}\" [{participantStyle}, {fontname}," +
                    $" label=\"{c}\"]; \"{c}\" -> \"{focusEvent}\"; ");
            }
        }

        private static IEnumerable<string> GetCharactersAtEvent(string focusEvent)
        {
            return (from e in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                  where e.Element("name").Value.Equals(focusEvent)
                                  && !e.Element("participants").IsEmpty
                                  select (from c in e.Element("participants").Elements("character_name")
                                          select c.Value)).First();
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
            string imagePath = $"{XMLParser.FolderPath}/{focusEvent}{type}.svg";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            //twopi or neato
            //Process.Start("cmd.exe", @"/c" + $"twopi -Tsvg {textPath} -o {imagePath} & del {textPath} & {imagePath}");

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c" + $"twopi -Tsvg {textPath} -o {imagePath} & del {textPath} & {imagePath}";
            process.StartInfo = startInfo;
            process.Start();
        }

        private static void AppendLegend(ref StringBuilder echo, bool participantsOn = false)
        {
            echo.AppendLine($"\"Event\" [{eventStyle}, {fontname}];");
                echo.AppendLine($"\"Participant\" [{participantStyle}, {fontname}];");
            

            echo.AppendLine($"subgraph {{ label=\"Legend\"; overlap=false; ");

            echo.AppendLine($"\"Event\" -- \"Participant\" ;");
            
            echo.AppendLine("}");
        }
    }
}
