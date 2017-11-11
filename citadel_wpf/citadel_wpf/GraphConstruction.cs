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

            TestImmediateFamilyTree("Adam");
            TestExtendedFamilyTree("Donald");
            TestExtendedFamilyTree("Ryan");
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

        public static bool TestImmediateFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Test Immediate Family Tree for {focusCharacter}\"; ");

            var parents = GetParentsOf(focusCharacter);
            var children = GetChildrenOf(focusCharacter);

            foreach (var c in parents)
            {
                echo.Append($"{c} -- {focusCharacter}; ");
    
                //Siblings
                foreach (var t in GetSiblingsOf(c, focusCharacter))
                {
                    echo.Append($"{c} -- {t}; ");
                }
            }

            foreach (var c in children)
            {
                echo.Append($"{focusCharacter} -- {c}; ");
            }

            echo.Append("}");

            string textPath = FrontPage.FolderPath + "\\testImmediateTree.dot";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/testImmediateTree.png");

            return true;
        }

        public static bool TestExtendedFamilyTree(string focusCharacter)
        {
            //Granparents/children + aunts/uncles + cousins
            //TODO aunts/uncles, cousins
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Test Extended Family Tree for {focusCharacter}\"; ");

            var parents = GetParentsOf(focusCharacter);
            var children = GetChildrenOf(focusCharacter);

            foreach (var c in parents)
            {
                echo.Append($"{c} -- {focusCharacter}; ");

                //GrandParents
                foreach (var q in GetParentsOf(c))
                {
                    echo.Append($"{q} -- {c}; ");
                }

                //Siblings
                foreach (var t in GetSiblingsOf(c, focusCharacter))
                {
                    echo.Append($"{c} -- {t}; ");
                }
            }

            foreach (var c in children)
            {
                echo.Append($"{focusCharacter} -- {c}; ");

                //GrandChildren
                foreach (var q in GetChildrenOf(c))
                {
                    echo.Append($"{c} -- {q}; ");
                }
            }

            echo.Append("}");

            string textPath = FrontPage.FolderPath + "\\testExtendedTree.dot";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/testExtendedTree.png");

            return true;
        }

        //TODO move format to XMLEntityParser
        private static IEnumerable<string> GetGenerationNames(string focusCharacter, string relationshipName, string otherCharacter = "")
        {
            return (from c in XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument().Root.Descendants("character_relationship")
                    where c.Element("entity_one").Value.ToString().Equals(focusCharacter)
                    && c.Element("relationship").Value.ToString().Equals(relationshipName)
                    && (otherCharacter.Equals("") || (!otherCharacter.Equals("") && (!c.Element("entity_two").Value.ToString().Equals(otherCharacter))))
                    select c.Element("entity_two").Value.ToString());
        }

        private static IEnumerable<string> GetParentsOf(string focusCharacter)
        {
            return GetGenerationNames(focusCharacter, "Is the Child of");
        }
        private static IEnumerable<string> GetChildrenOf(string focusCharacter)
        {
            return GetGenerationNames(focusCharacter, "Is the Parent of");
        }
        private static IEnumerable<string> GetSiblingsOf(string focusCharacter, string otherCharacter)
        {
            return GetGenerationNames(focusCharacter, "Is the Parent of", otherCharacter);
        }


    }
}
