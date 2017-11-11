using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class GraphConstruction
    {
        private string folderPath;

        public GraphConstruction(string fp)
        {
            folderPath = fp;

        }

        public static void TestGraphviz(string folderPath)
        {
            //Procedure: construct the proper dot information, output to a file, open cmd to parse that file
            string echo = "digraph G { Hello->World }";
            echo = "graph s { label=\"Test\"; A -- B; B -- C; subgraph cluster01 { label =\"\" D -- E } }";
            string textPath = folderPath + "\\citadel.dot";
            StreamWriter streamWriter = File.CreateText(textPath);
            streamWriter.Write(echo);
            streamWriter.Close();

            //Process process = new Process();
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = @"/C" + $"dot -Tpng {textPath} -o {folderPath}/outfile.png";
            //process.StartInfo = startInfo;

            //TODO take output of cmd and save file + return in this program
            //use output to open picture file?
            Process.Start("cmd.exe", @"/C" + $"dot -Tpng {textPath} -o {folderPath}/outfile.png & del {textPath}");
            //Process.Start("cmd.exe", @"/C" + $"dot -Tpng {textPath} -o {folderPath}/outfile.png").WaitForExit();
            //Process.Start("cmd.exe", @"/C" + $"del {textPath}");

            TestCharacterRelationship("Ryan", "Matt");


            TestFamilyTree("Adam");
        }

        public static void TestCharacterRelationship(string c1, string c2)
        {
            string echo = $"graph s {{ label=\"test relationship\"; {c1} -- {c2}; }}";
            string textpath = FrontPage.FolderPath + "\\testRelationship.dot";
            StreamWriter streamwriter = File.CreateText(textpath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textpath} -o {FrontPage.FolderPath}/testRelationship.png  & del {textpath}");
        }

        public static bool TestFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"test tree\"; ");

            var parents = from c in XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument().Root.Descendants("character_relationship")
                          where c.Element("entity_one").Value.ToString().Equals(focusCharacter)
                          && c.Element("relationship").Value.ToString().Equals("Is the Child of")
                          select c.Element("entity_two");

            var children = from c in XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument().Root.Descendants("character_relationship")
                          where c.Element("entity_one").Value.ToString().Equals(focusCharacter)
                          && c.Element("relationship").Value.ToString().Equals("Is the Parent of")
                          select c.Element("entity_two");

            foreach(var c in parents)
            {
                echo.Append($"{c.Value.ToString()} -- {focusCharacter}; ");
            }

            foreach (var c in children)
            {
                echo.Append($"{focusCharacter} -- {c.Value.ToString()}; ");
            }

            //< ComboBoxItem Content = "Is the Parent of" />
            // < ComboBoxItem Content = "Is the Child of" />

            echo.Append("}");

            string textPath = FrontPage.FolderPath + "\\testTree.dot";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/testTree.png");

            return true;
        }

    }
}
