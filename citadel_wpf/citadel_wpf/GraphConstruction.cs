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
        }
        //    TestCharacterRelationship(folderPath, Character.GetRecords(), null);


        //    TestFamilyTree(folderPath, (Character)Character.GetRecords()[0], Character.GetRecords());
        //}

        //public static void TestCharacterRelationship(string folderPath, List<IEntity> characters, Character focusCharacter)
        //{
        //    string echo = $"graph s {{ label=\"Test Relationship\"; {characters[0].GetName()} -- {characters[1].GetName()}; }}";
        //    string textPath = folderPath + "\\testRelationship.dot";
        //    StreamWriter streamWriter = File.CreateText(textPath);
        //    streamWriter.Write(echo);
        //    streamWriter.Close();

        //    Process.Start("cmd.exe", @"/C" + $"dot -Tpng {textPath} -o {folderPath}/testRelationship.png  & del {textPath}");
        //}

        //public static bool TestFamilyTree(string folderPath, Character focusCharacter, List<IEntity> characters)
        //{
        //    StringBuilder echo = new StringBuilder($"graph s {{ label=\"Test Tree\";");
        //    List<Character> relevantCharacters = new List<Character>();
        //    relevantCharacters.Add(focusCharacter);

        //    focusCharacter.GetName();

        //    foreach(Character c in characters)
        //    {
        //        bool present = c.GetChildren().Where(s => s.GetName().Equals(focusCharacter.GetName())).Count() > 0 ? true : false;
        //        c.GetName();
        //        if (present)
        //        {
        //            relevantCharacters.Add(c);
        //        }
        //    }

        //    foreach(Character c in relevantCharacters)
        //    {
        //        foreach(var t in c.GetChildren())
        //        {
        //            echo.Append($"{c.GetName()} -- {t.GetName()}; ");
        //        }
        //    }

        //    echo.Append("}}");

        //    string textPath = folderPath + "\\testTree.dot";
        //    StreamWriter streamWriter = File.CreateText(textPath);
        //    streamWriter.Write(echo);
        //    streamWriter.Close();

        //    Process.Start("cmd.exe", @"/C" + $"dot -Tpng {textPath} -o {folderPath}/testTree.png");

        //    return true;
        //}

    }
}
