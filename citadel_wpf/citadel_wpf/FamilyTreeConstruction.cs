using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class FamilyTreeConstruction
    {

        public FamilyTreeConstruction() { }        

        public static void TestGraphviz()
        {
            //Procedure: construct the proper dot information, output to a file, open cmd to parse that file
            //string echo = "digraph G { Hello->World }";
            //echo = "graph s { label=\"Test\"; A -- B; B -- C; subgraph cluster01 { label =\"\" D -- E } }";
            //string textPath = FrontPage.FolderPath + "\\citadel.dot";
            //StreamWriter streamWriter = File.CreateText(textPath);
            //streamWriter.Write(echo);
            //streamWriter.Close();

            //Process process = new Process();
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = @"/C" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/outfile.png";
            //process.StartInfo = startInfo;

            //TODO take output of cmd and save file + return in this program
            //use output to open picture file?
            //Process.Start("cmd.exe", @"/C" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/outfile.png & del {textPath}");
            //Process.Start("cmd.exe", @"/C" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/outfile.png").WaitForExit();
            //Process.Start("cmd.exe", @"/C" + $"del {textPath}");

            //TestCharacterRelationship("Ryan", "Matt");

            ImmediateFamilyTree("Caitlyn");
            ExtendedFamilyTree("Ryan");
            RecursiveFullFamilyTree("Adam");
        }

        public static void CharacterRelationship(string c1, string c2)
        {
            string echo = $"graph s {{ label=\"Character Relationship\"; {c1} -- {c2}; }}";
            string textpath = FrontPage.FolderPath + "\\Relationship.dot";
            StreamWriter streamwriter = File.CreateText(textpath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textpath} -o {FrontPage.FolderPath}/testRelationship.png  & del {textpath}");
        }

        public static bool ImmediateFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Immediate Family Tree for {focusCharacter}\"; {focusCharacter}; ");

            foreach (var c in GetParentsOf(focusCharacter))
            {
                echo.Append($"{c} -- {focusCharacter}; ");
    
                //Siblings
                foreach (var t in GetSiblingsOf(focusCharacter, c))
                {
                    echo.Append($"{c} -- {t}; ");
                }
            }

            foreach (var c in GetChildrenOf(focusCharacter))
            {
                echo.Append($"{focusCharacter} -- {c}; ");
            }

            echo.Append("}");

            string textPath = FrontPage.FolderPath + "\\ImmediateTree.dot";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/ImmediateTree.png");

            return true;
        }

        public static bool ExtendedFamilyTree(string focusCharacter)
        {
            //Grandparents/children + aunts/uncles + cousins + nieces/nephews + great aunts/uncles
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Extended Family Tree for {focusCharacter}\"; ");
            Dictionary<string, bool> relationships = new Dictionary<string, bool>();
            Dictionary<string, bool> characters = new Dictionary<string, bool>();

            AddCharacterIfAbsent(focusCharacter, ref characters);

            //Parents
            foreach (var parentOfFocusCharacter in GetParentsOf(focusCharacter))
            {
                AddCharacterIfAbsent(parentOfFocusCharacter, ref characters);
                AddRelationshipIfAbsent(parentOfFocusCharacter, focusCharacter, ref relationships);

                //GrandParents
                foreach (var grandparentOfFocusCharacter in GetParentsOf(parentOfFocusCharacter))
                {
                    AddCharacterIfAbsent(grandparentOfFocusCharacter, ref characters);
                    AddRelationshipIfAbsent(grandparentOfFocusCharacter, parentOfFocusCharacter, ref relationships);

                    //Aunts and Uncles (By "Blood")
                    foreach (var auntOrUncleOfFocusCharacter in GetSiblingsOf(parentOfFocusCharacter, grandparentOfFocusCharacter))
                    {
                        AddCharacterIfAbsent(auntOrUncleOfFocusCharacter, ref characters);
                        AddRelationshipIfAbsent(grandparentOfFocusCharacter, auntOrUncleOfFocusCharacter, ref relationships);

                        //Cousins
                        foreach (var cousinOfFocusCharacter in GetChildrenOf(auntOrUncleOfFocusCharacter))
                        {
                            AddCharacterIfAbsent(cousinOfFocusCharacter, ref characters);
                            AddRelationshipIfAbsent(auntOrUncleOfFocusCharacter, cousinOfFocusCharacter, ref relationships);

                            //Non-"Blood" Aunts and Uncles
                            foreach (var parentOfCousin in GetParentsOf(cousinOfFocusCharacter))
                            {
                                AddCharacterIfAbsent(parentOfCousin, ref characters);
                                AddRelationshipIfAbsent(parentOfCousin, cousinOfFocusCharacter, ref relationships);
                            }
                        }
                    }
                }

                //Siblings
                foreach (var siblingOfFocusCharacter in GetSiblingsOf(focusCharacter, parentOfFocusCharacter))
                {
                    AddCharacterIfAbsent(siblingOfFocusCharacter, ref characters);
                    AddRelationshipIfAbsent(parentOfFocusCharacter, siblingOfFocusCharacter, ref relationships);

                    //Nieces + Nephews
                    foreach (var nieceOrNephewOfFocusCharacter in GetChildrenOf(siblingOfFocusCharacter))
                    {
                        AddCharacterIfAbsent(nieceOrNephewOfFocusCharacter, ref characters);
                        AddRelationshipIfAbsent(siblingOfFocusCharacter, nieceOrNephewOfFocusCharacter, ref relationships);
                    }
                }
            }

            //Children
            foreach (var childOfFocusCharacter in GetChildrenOf(focusCharacter))
            {
                AddCharacterIfAbsent(childOfFocusCharacter, ref characters);
                AddRelationshipIfAbsent(focusCharacter, childOfFocusCharacter, ref relationships);

                //GrandChildren
                foreach (var grandchildOfFocusCharacter in GetChildrenOf(childOfFocusCharacter))
                {
                    AddCharacterIfAbsent(grandchildOfFocusCharacter, ref characters);
                    AddRelationshipIfAbsent(childOfFocusCharacter, grandchildOfFocusCharacter, ref relationships);
                }
            }

            foreach(var character in characters)
            {
                echo.Append($"\"{character.Key}\" [color={GetCharacterGender(character.Key)}]; ");
            }

            foreach (var relationship in relationships)
            {
                echo.Append(relationship.Key);
            }

            echo.Append("}");

            string textPath = FrontPage.FolderPath + "\\ExtendedTree.dot";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/ExtendedTree.png");

            return true;
        }

        //returns true if the relationship was successfully added 
        private static bool AddRelationshipIfAbsent(string parent, string child, ref Dictionary<string, bool> relationships)
        {
            if (!relationships.ContainsKey($"\"{parent}\" -- \"{child}\"; "))
            {
                relationships.Add($"\"{parent}\" -- \"{child}\"; ", true);
                return true;
            }
            return false;
        }

        //returns true if the character was successfully added 
        private static bool AddCharacterIfAbsent(string focusCharacter, ref Dictionary<string, bool> characters)
        {
            if (!characters.ContainsKey(focusCharacter))
            {
                characters.Add(focusCharacter, true);
                return true;
            }
            return false;
        }

        //TODO Make a function to clean up relationships -> currently VERY cluttered
        public static bool RecursiveFullFamilyTree(string focusCharacter)
        {
            //All familial relationships
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Recursive Full Family Tree for {focusCharacter}\"; ");
            Dictionary<string, bool> relationships = new Dictionary<string, bool>();
            Dictionary<string, bool> characters = new Dictionary<string, bool>();

            AddCharacterIfAbsent(focusCharacter, ref characters);

            RecursiveTreeHelper(focusCharacter, ref characters, ref relationships);

            foreach (var character in characters)
            {
                echo.Append($"\"{character.Key}\" [color={GetCharacterGender(character.Key)}]; ");
            }
            //TODO catch the stack being blown and draw up to this point
            foreach (var relationship in relationships)
            {
                echo.Append(relationship.Key);
            }

            echo.Append("}");

            string textPath = FrontPage.FolderPath + "\\RecursiveFullFamilyTree.dot";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {FrontPage.FolderPath}/RecursiveFullFamilyTree.png");

            return true;
        }

        public static void RecursiveTreeHelper (string focusCharacter, ref Dictionary<string, bool> characters, ref Dictionary<string, bool> connections)
        {
            foreach (var p in GetParentsOf(focusCharacter))
            {
                if (!connections.ContainsKey($"\"{p}\" -- \"{focusCharacter}\"; "))
                {
                    AddCharacterIfAbsent(p, ref characters);
                    connections.Add($"\"{p}\" -- \"{focusCharacter}\"; ", true);
                    RecursiveTreeHelper(p, ref characters, ref connections);
                }
            }
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                if (!connections.ContainsKey($"\"{focusCharacter}\" -- \"{c}\"; "))
                {
                    AddCharacterIfAbsent(c, ref characters);
                    connections.Add($"\"{focusCharacter}\" -- \"{c}\"; ", true);
                    RecursiveTreeHelper(c, ref characters, ref connections);
                }
            }
        }

        //TODO move format to XMLEntityParser
        private static IEnumerable<string> GetGenerationNames(string focusCharacter, string relationshipName, string childToIgnore = "")
        {
            //TODO make more iffecient by reducing the number of times the first line has to be called -> set descendant before the group of calls to this function is made
            return (from c in XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Descendants("character_relationship")
                    where c.Element("entity_one").Value.ToString().Equals(focusCharacter)
                    && c.Element("relationship").Value.ToString().Equals(relationshipName)
                    && (childToIgnore.Equals("") || (!childToIgnore.Equals("") && (!c.Element("entity_two").Value.ToString().Equals(childToIgnore))))
                    select c.Element("entity_two").Value.ToString());
        }

        private static string GetCharacterGender(string focusCharacter)
        {
            //TODO make more iffecient by reducing the number of times the first line has to be called -> set descendant before the group of calls to this function is made
            return (from c in XMLParser.GetInstance().GetCharacterXDocument().Root.Descendants("character")
                    where c.Element("name").Value.ToString().Equals(focusCharacter)
                    select c.Element("gender").Value.ToString()).First().Equals("Male") ? "Blue" : "Red";
        }

        private static IEnumerable<string> GetParentsOf(string focusCharacter)
        {
            return GetGenerationNames(focusCharacter, "Is the Child of");
        }
        private static IEnumerable<string> GetChildrenOf(string focusCharacter)
        {
            return GetGenerationNames(focusCharacter, "Is the Parent of");
        }
        private static IEnumerable<string> GetSiblingsOf(string focusCharacter, string parent)
        {
            return GetGenerationNames(parent, "Is the Parent of", focusCharacter);
        }


    }
}
