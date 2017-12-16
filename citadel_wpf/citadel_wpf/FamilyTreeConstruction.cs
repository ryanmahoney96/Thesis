using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace citadel_wpf
{
    public class FamilyTreeConstruction
    {

        public static string ImmediateFamilyTreeString = "Immediate Family Tree";
        public static string ExtendedFamilyTreeString = "Extended Family Tree";
        public static string FullFamilyTreeString = "Full Family Tree";

        public static string[] Relationships = { ImmediateFamilyTreeString, ExtendedFamilyTreeString, FullFamilyTreeString };

        private static string fontname = $"fontname=\"Helvetica\"";
        private static string focusShape = $"shape=oval";
        private static string shape = $"shape=rect";
        private static string color = $"color=\"#393739\"";
        private static string maleColor = $"\"#dfeefb\"";
        private static string femaleColor = $"\"#fdd0a2\"";
        private static string otherColor = $"\"#e989ff\"";
        private static string isMarried = $"crimson";
        private static string wasMarried = $"lightsalmon4";
        private static string splines = $"splines=true";
        private static string concentrate = $"concentrate=true";
        private static string rankdir = $"";
        // rankdir=LR

        public static void ImmediateFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ImmediateFamilyTreeString} for {focusCharacter}\" {fontname} {concentrate} {splines} {rankdir}; \n");
            Dictionary<XElement, bool> characters = new Dictionary<XElement, bool>();
            HashSet<string> marriages = new HashSet<string>();

            AddCharacterIfAbsent(focusCharacter, true, ref characters);

            //Parents
            foreach (var p in GetParentsOf(focusCharacter))
            {
                AddCharacterIfAbsent(p, false, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, p);

                //Siblings
                foreach (var s in GetSiblingsOf(focusCharacter, p))
                {
                    AddCharacterIfAbsent(s, true, ref characters);
                }
            }

            //Children
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                AddCharacterIfAbsent(c, true, ref characters);
            }

            AppendAllMarriages(ref echo, ref characters, ref marriages, focusCharacter);

            AddCharacterInformation(ref echo, characters, focusCharacter);

            foreach (var character in characters)
            {
                if (character.Value)
                {
                    AddParents(ref echo, character.Key.Element("parents"), character.Key);
                }
            }

            AppendLegend(ref echo);

            echo.Append("}");

            SaveEcho(echo, "ImmediateFamilyTree", focusCharacter);
        }

        public static void ExtendedFamilyTree(string focusCharacter)
        {
            //Grand-parents/children + aunts/uncles + cousins + nieces/nephews
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ExtendedFamilyTreeString} for {focusCharacter}\" {fontname} {concentrate} {splines} {rankdir}; ");
            Dictionary<XElement, bool> characters = new Dictionary<XElement, bool>();
            HashSet<string> marriages = new HashSet<string>();

            AddCharacterIfAbsent(focusCharacter, true, ref characters);

            //Parents
            foreach (var p in GetParentsOf(focusCharacter))
            {
                AddCharacterIfAbsent(p, true, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, p);

                //GrandParents
                foreach (var g in GetParentsOf(p))
                {
                    AddCharacterIfAbsent(g, true, ref characters);
                    AppendAllMarriages(ref echo, ref characters, ref marriages, g);

                    //Aunts and Uncles
                    foreach (var au in GetSiblingsOf(p, g))
                    {
                        AddCharacterIfAbsent(au, true, ref characters);
                        AppendAllMarriages(ref echo, ref characters, ref marriages, au);

                        //Cousins
                        foreach (var c in GetChildrenOf(au))
                        {
                            AddCharacterIfAbsent(c, true, ref characters);
                        }
                    }
                }

                //Siblings
                foreach (var s in GetSiblingsOf(focusCharacter, p))
                {
                    AddCharacterIfAbsent(s, true, ref characters);
                    AppendAllMarriages(ref echo, ref characters, ref marriages, s);

                    //Nieces + Nephews
                    foreach (var nn in GetChildrenOf(s))
                    {
                        AddCharacterIfAbsent(nn, true, ref characters);
                    }
                }
            }

            //Children
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                AddCharacterIfAbsent(c, true, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, c);

                //GrandChildren
                foreach (var g in GetChildrenOf(c))
                {
                    AddCharacterIfAbsent(g, true, ref characters);
                }
            }

            AppendAllMarriages(ref echo, ref characters, ref marriages, focusCharacter);

            AddCharacterInformation(ref echo, characters, focusCharacter);

            foreach (var character in characters)
            {
                if (character.Value)
                {
                    AddParents(ref echo, character.Key.Element("parents"), character.Key);
                }
            }

            AppendLegend(ref echo);

            echo.Append("}");

            SaveEcho(echo, "ExtendedFamilyTree", focusCharacter);
        }

        public static void FullFamilyTree(string focusCharacter)
        {
            //All familial relationships
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{FullFamilyTreeString} for {focusCharacter}\" {fontname} {splines} {concentrate} {rankdir}; ");
            Dictionary<XElement, bool> characters = new Dictionary<XElement, bool>();
            HashSet<string> marriages = new HashSet<string>();

            AddCharacterIfAbsent(focusCharacter, false, ref characters);

            RecursiveTreeHelper(ref echo, ref characters, ref marriages, focusCharacter);

            AddCharacterInformation(ref echo, characters, focusCharacter);

            foreach (var character in characters)
            {
                AddParents(ref echo, character.Key.Element("parents"), character.Key);
            }

            AppendLegend(ref echo);

            echo.Append("}");

            SaveEcho(echo, "FullFamilyTree", focusCharacter);
        }

        public static void RecursiveTreeHelper(ref StringBuilder echo, ref Dictionary<XElement, bool> characters, ref HashSet<string> marriages, string focusCharacter)
        {
            var cRef = (from xmlc in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
             where xmlc.Element("name").Value.Equals(focusCharacter)
             select xmlc).First();

            if (!characters[cRef])
            {
                characters[cRef] = true;
                foreach (var p in GetParentsOf(focusCharacter))
                {

                    AddCharacterIfAbsent(p, false, ref characters);
                    AppendAllMarriages(ref echo, ref characters, ref marriages, p, false);
                    RecursiveTreeHelper(ref echo, ref characters, ref marriages, p);

                }
                foreach (var c in GetChildrenOf(focusCharacter))
                {

                    AddCharacterIfAbsent(c, false, ref characters);
                    AppendAllMarriages(ref echo, ref characters, ref marriages, c, false);
                    RecursiveTreeHelper(ref echo, ref characters, ref marriages, c);

                }
            }
            
        }

        //returns true if the character was successfully added 
        private static bool AddCharacterIfAbsent(string focusCharacter, bool addParents, ref Dictionary<XElement, bool> characters)
        {
            XElement characterRef = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                     where c.Element("name").Value.Equals(focusCharacter)
                                     select c).First();

            if (!characters.ContainsKey(characterRef))
            {
                characters.Add(characterRef, addParents);
                return true;
            }

            return false;
        }

        private static void AddCharacterInformation(ref StringBuilder echo, Dictionary<XElement, bool> characters, string focusCharacter)
        {
            foreach (var character in characters)
            {

                echo.Append($"\"{character.Key.Element("name").Value}\" [style = filled, fillcolor={GetGenderColor(character.Key.Element("name").Value)}, {color}, {fontname}");
                if (character.Key.Element("name").Value.Equals(focusCharacter))
                {
                    echo.Append($", {focusShape}");
                }
                else
                {
                    echo.Append($", {shape}");
                }
                echo.AppendLine($"]; ");

            }
        }

        private static void AddParents(ref StringBuilder echo, XElement parents, XElement focusCharacter)
        {
            List<XElement> parentList = parents.Elements().ToList().OrderBy(p => p.Value).ToList();

            if (parentList.Count > 0)
            {
                StringBuilder parentNode = new StringBuilder("<>");
                foreach (var p in parentList)
                {
                    parentNode.Append(p.Value);
                }

                echo.AppendLine($"\"{parentNode.ToString()}\" [shape=circle, label=\"\", style=filled, fillcolor=greenyellow, width=0.1, height=0.1]; ");

                foreach (var p in parentList)
                {
                    echo.AppendLine($"\"{p.Value}\" -- \"{parentNode.ToString()}\"; ");
                    echo.AppendLine($"\"{parentNode.ToString()}\" -- \"{focusCharacter.Element("name").Value}\"; ");
                }

            }
        }

        private static void SaveEcho(StringBuilder echo, string type, string focusCharacter)
        {
            focusCharacter = focusCharacter.Replace(" ", "_");
            focusCharacter = focusCharacter.Replace("'", "");
            string textPath = XMLParser.FolderPath + $"\\{type}.dot";
            string imagePath = $"{XMLParser.FolderPath}/{focusCharacter}_{type}.svg";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            //Process.Start("cmd.exe", @"/c" + $"dot -Tsvg {textPath} -o {imagePath} & del {textPath} & {imagePath}");

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c" + $"dot -Tsvg {textPath} -o {imagePath} & del {textPath} & {imagePath}";
            process.StartInfo = startInfo;
            process.Start();
        }

        private static string GetGenderColor(string focusCharacter)
        {
            var result = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                          where c.Element("name").Value.Equals(focusCharacter)
                          select c.Element("gender").Value).First();

            string returnString = $"{maleColor}";

            if (result.Equals("Female"))
            {
                returnString = $"{femaleColor}";
            }
            else if (!result.Equals("Male"))
            {
                returnString = $"{otherColor}";
            }

            return returnString;
        }

        private static IEnumerable<string> GetRelationNames(string focusCharacter, string elementType, string childToIgnore = "")
        {
            XElement CurrentCharacterReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                                  where c.Element("name").Value.Equals(focusCharacter)
                                                  select c).First();

            var relations = from p in CurrentCharacterReference.Element(elementType).Elements()
                            where (childToIgnore.Equals("") || ((!childToIgnore.Equals("") && (!p.Value.Equals(childToIgnore)))))
                            select p.Value;

            return relations;

        }

        private static void AppendAllMarriages(ref StringBuilder echo, ref Dictionary<XElement, bool> characters, ref HashSet<string> marriages, string focusCharacter, bool addParent = false)
        {
            //Current Marriages
            foreach (var c in GetCurrentMarriages(focusCharacter))
            {
                AddCharacterIfAbsent(c, addParent, ref characters);
                AppendMarriage(ref echo, ref marriages, c, focusCharacter, isMarried, "diamond");
            }

            //Past Marriages
            foreach (var p in GetPastMarriages(focusCharacter))
            {
                AddCharacterIfAbsent(p, addParent, ref characters);
                AppendMarriage(ref echo, ref marriages, p, focusCharacter, wasMarried, "ocurve");
            }
        }

        private static IEnumerable<string> GetCurrentMarriages(string focusCharacter)
        {
            XElement CurrentCharacterReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                                  where c.Element("name").Value.Equals(focusCharacter)
                                                  select c).First();

            var curr = from c in CurrentCharacterReference.Element("marriages").Elements("ismarriedto")
                       select c.Value;

            return curr;
        }

        private static IEnumerable<string> GetPastMarriages(string focusCharacter)
        {
            XElement CurrentCharacterReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                                  where c.Element("name").Value.Equals(focusCharacter)
                                                  select c).First();

            var past = from p in CurrentCharacterReference.Element("marriages").Elements("wasmarriedto")
                       select p.Value;

            return past;
        }

        private static void AppendMarriage(ref StringBuilder echo, ref HashSet<string> marriages, string c1, string c2, string color, string arrowhead)
        {
            if (c1.CompareTo(c2) < 0)
            {
                string temp = c1;
                c1 = c2;
                c2 = temp;
            }
            if (!marriages.Contains(c1 + c2))
            {
                marriages.Add(c1 + c2);
//                echo.AppendLine($"{{rank=same; \"{c1}\"; \"{c2}\"}}; ");
                echo.AppendLine($"\"{c1}\" -- \"{c2}\" [color={color} dir=both arrowhead={arrowhead} arrowtail={arrowhead}]; ");
            }
            
        }

        private static IEnumerable<string> GetParentsOf(string focusCharacter)
        {
            return GetRelationNames(focusCharacter, "parents");
        }
        private static IEnumerable<string> GetChildrenOf(string focusCharacter)
        {
            return GetRelationNames(focusCharacter, "children");
        }
        private static IEnumerable<string> GetSiblingsOf(string focusCharacter, string parent)
        {
            return GetRelationNames(parent, "children", focusCharacter);
        }

        private static void AppendLegend(ref StringBuilder echo)
        {
            echo.AppendLine($"\"Male\" [style=filled, {shape}, fillcolor={maleColor}, {fontname}];");
            echo.AppendLine($"\"Female\" [style=filled, {shape}, fillcolor={femaleColor}, {fontname}];");
            echo.AppendLine($"\"Other\" [style=filled, {shape}, fillcolor={otherColor}, {fontname}];");
            echo.AppendLine($"\"p1\" [shape=point, label=\"\", style=filled, width=0.01, height=0.01]; ");
            echo.AppendLine($"\"p2\" [shape=point, label=\"\", style=filled, width=0.01, height=0.01]; ");

            echo.AppendLine($"subgraph cluster_0 {{ label=\"Legend\"; rankdir=LR; ");
            echo.AppendLine($"\"Male\" ;");
            echo.AppendLine($"\"Female\" ;");
            echo.AppendLine($"\"Other\" ;");
            //TODO adding these throws off the shape of the trees
            //echo.AppendLine($"\"p1\" -- \"p1\" [label=\"Is Married\" color={isMarried} dir=forward arrowhead=diamond arrowtail=diamond]; ");
            //echo.AppendLine($"\"p2\" -- \"p2\" [label=\"Was Married\" color={wasMarried} dir=forward arrowhead=ocurve arrowtail=ocurve]; ");

            echo.AppendLine("}");
        }

    }
}
