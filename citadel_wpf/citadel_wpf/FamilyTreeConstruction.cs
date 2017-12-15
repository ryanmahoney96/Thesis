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
        //TODO cleanup with nodes that connect two characters, then branches to each child
        //TODO make a pastel color generator for the color of family trees

        public static string ImmediateFamilyTreeString = "Immediate Family Tree";
        public static string ExtendedFamilyTreeString = "Extended Family Tree";
        public static string FullFamilyTreeString = "Full Family Tree";

        //TODO add binding in prompt
        public static string[] Relationships = { ImmediateFamilyTreeString, ExtendedFamilyTreeString, FullFamilyTreeString };

        private static string fontname = $"fontname=\"Helvetica\"";
        private static string focusShape = $"shape=oval";
        private static string shape = $"shape=rect";
        private static string maleColor = $"\"#deebf7\"";
        private static string femaleColor = $"\"#fdd0a2\"";
        private static string otherColor = $"\"f9f7f9\"";
        private static string isMarried = $"crimson";
        private static string wasMarried = $"lightsalmon4";
        private static string splines = $"splines=true";
        private static string concentrate = $"concentrate=true";
        private static string rankdir = $"rankdir=LR";

        public static void OldImmediateFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ImmediateFamilyTreeString} for {focusCharacter}\" {fontname}; ");
            Dictionary<string, bool> relationships = new Dictionary<string, bool>();
            Dictionary<string, string> characters = new Dictionary<string, string>();

            AddCharacterIfAbsent(focusCharacter, ref characters);

            //Parents
            foreach (var p in GetParentsOf(focusCharacter))
            {
                AddCharacterIfAbsent(p, ref characters);
                AddRelationshipIfAbsent(p, focusCharacter, ref relationships);

                //Siblings
                foreach (var s in GetSiblingsOf(focusCharacter, p))
                {
                    AddCharacterIfAbsent(s, ref characters);
                    AddRelationshipIfAbsent(p, s, ref relationships);
                }
            }

            //Children
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                AddCharacterIfAbsent(c, ref characters);
                AddRelationshipIfAbsent(focusCharacter, c, ref relationships);
            }

            AddCharacterInformation(ref echo, characters, relationships, focusCharacter);

            echo.Append("}");

            SaveEcho(echo, "ImmediateFamilyTree", focusCharacter);
        }

        public static void ImmediateFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ImmediateFamilyTreeString} for {focusCharacter}\" {fontname} {concentrate} {splines} {rankdir}; ");
            Dictionary<XElement, bool> characters = new Dictionary<XElement, bool>();
            HashSet<string> marriages = new HashSet<string>();

            NewAddCharacterIfAbsent(focusCharacter, true, ref characters);

            //Parents
            foreach (var p in GetParentsOf(focusCharacter))
            {
                NewAddCharacterIfAbsent(p, false, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, p);

                //Siblings
                foreach (var s in GetSiblingsOf(focusCharacter, p))
                {
                    NewAddCharacterIfAbsent(s, true, ref characters);
                }
            }

            //Children
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                NewAddCharacterIfAbsent(c, true, ref characters);
            }

            AppendAllMarriages(ref echo, ref characters, ref marriages, focusCharacter);

            NewAddCharacterInformation(ref echo, characters, focusCharacter);

            foreach (var character in characters)
            {
                if (character.Value)
                {
                    NewAddParents(ref echo, character.Key.Element("parents"), character.Key);
                }
            }

            echo.Append("}");

            SaveEcho(echo, "ImmediateFamilyTree", focusCharacter);
        }

        public static void OldExtendedFamilyTree(string focusCharacter)
        {
            //Grand-parents/children + aunts/uncles + cousins + nieces/nephews
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ExtendedFamilyTreeString} for {focusCharacter}\" {fontname} {splines}; ");
            Dictionary<string, bool> relationships = new Dictionary<string, bool>();
            Dictionary<string, string> characters = new Dictionary<string, string>();

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

                    //Aunts and Uncles
                    foreach (var auntOrUncleOfFocusCharacter in GetSiblingsOf(parentOfFocusCharacter, grandparentOfFocusCharacter))
                    {
                        AddCharacterIfAbsent(auntOrUncleOfFocusCharacter, ref characters);
                        AddRelationshipIfAbsent(grandparentOfFocusCharacter, auntOrUncleOfFocusCharacter, ref relationships);

                        //Cousins
                        foreach (var cousinOfFocusCharacter in GetChildrenOf(auntOrUncleOfFocusCharacter))
                        {
                            AddCharacterIfAbsent(cousinOfFocusCharacter, ref characters);
                            AddRelationshipIfAbsent(auntOrUncleOfFocusCharacter, cousinOfFocusCharacter, ref relationships);

                            //Aunts and Uncles by cousin
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

            AddCharacterInformation(ref echo, characters, relationships, focusCharacter);

            echo.Append("}");

            SaveEcho(echo, "ExtendedFamilyTree", focusCharacter);
        }

        public static void ExtendedFamilyTree(string focusCharacter)
        {
            //Grand-parents/children + aunts/uncles + cousins + nieces/nephews
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ExtendedFamilyTreeString} for {focusCharacter}\" {fontname} {concentrate} {splines} {rankdir}; ");
            Dictionary<XElement, bool> characters = new Dictionary<XElement, bool>();
            HashSet<string> marriages = new HashSet<string>();

            NewAddCharacterIfAbsent(focusCharacter, true, ref characters);

            //Parents
            foreach (var p in GetParentsOf(focusCharacter))
            {
                NewAddCharacterIfAbsent(p, true, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, p);

                //GrandParents
                foreach (var g in GetParentsOf(p))
                {
                    NewAddCharacterIfAbsent(g, false, ref characters);
                    AppendAllMarriages(ref echo, ref characters, ref marriages, g);

                    //Aunts and Uncles
                    foreach (var au in GetSiblingsOf(p, g))
                    {
                        NewAddCharacterIfAbsent(au, true, ref characters);
                        AppendAllMarriages(ref echo, ref characters, ref marriages, au);

                        //Cousins
                        foreach (var c in GetChildrenOf(au))
                        {
                            NewAddCharacterIfAbsent(c, true, ref characters);
                        }
                    }
                }

                //Siblings
                foreach (var s in GetSiblingsOf(focusCharacter, p))
                {
                    NewAddCharacterIfAbsent(s, true, ref characters);
                    AppendAllMarriages(ref echo, ref characters, ref marriages, s);

                    //Nieces + Nephews
                    foreach (var nn in GetChildrenOf(s))
                    {
                        NewAddCharacterIfAbsent(nn, true, ref characters);
                    }
                }
            }

            //Children
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                NewAddCharacterIfAbsent(c, true, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, c);

                //GrandChildren
                foreach (var g in GetChildrenOf(c))
                {
                    NewAddCharacterIfAbsent(g, true, ref characters);
                }
            }

            AppendAllMarriages(ref echo, ref characters, ref marriages, focusCharacter);

            NewAddCharacterInformation(ref echo, characters, focusCharacter);

            foreach (var character in characters)
            {
                if (character.Value)
                {
                    NewAddParents(ref echo, character.Key.Element("parents"), character.Key);
                }
            }

            echo.Append("}");

            SaveEcho(echo, "ExtendedFamilyTree", focusCharacter);
        }

        public static void FullFamilyTree(string focusCharacter)
        {
            //All familial relationships
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{FullFamilyTreeString} for {focusCharacter}\" {fontname} {splines} {concentrate} {rankdir}; ");
            Dictionary<XElement, bool> characters = new Dictionary<XElement, bool>();
            HashSet<string> marriages = new HashSet<string>();

            NewAddCharacterIfAbsent(focusCharacter, false, ref characters);

            RecursiveTreeHelper(ref echo, ref characters, ref marriages, focusCharacter);

            NewAddCharacterInformation(ref echo, characters, focusCharacter);

            foreach (var character in characters)
            {
                NewAddParents(ref echo, character.Key.Element("parents"), character.Key);
            }

            echo.Append("}");

            SaveEcho(echo, "FullFamilyTree", focusCharacter);
        }

        public static void RecursiveTreeHelper(ref StringBuilder echo, ref Dictionary<XElement, bool> characters, ref HashSet<string> marriages, string focusCharacter)
        {
            //TODO still incorrect
            foreach (var p in GetParentsOf(focusCharacter))
            {

                NewAddCharacterIfAbsent(p, false, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, p, true);

                var parentRef = (from xmlp in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                where xmlp.Element("name").Value.Equals(p)
                                select xmlp).First();

                if (!characters[parentRef])
                {
                    characters[parentRef] = true;
                    RecursiveTreeHelper(ref echo, ref characters, ref marriages, p);
                }

            }
            foreach (var c in GetChildrenOf(focusCharacter))
            {

                NewAddCharacterIfAbsent(c, false, ref characters);
                AppendAllMarriages(ref echo, ref characters, ref marriages, c, true);

                var childRef = (from xmlc in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                 where xmlc.Element("name").Value.Equals(c)
                                 select xmlc).First();

                if (!characters[childRef])
                {
                    characters[childRef] = true;
                    RecursiveTreeHelper(ref echo, ref characters, ref marriages, c);
                }

            }
        }

        //returns true if the character was successfully added 
        private static bool NewAddCharacterIfAbsent(string focusCharacter, bool addParents, ref Dictionary<XElement, bool> characters)
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

        private static void NewAddCharacterInformation(ref StringBuilder echo, Dictionary<XElement, bool> characters, string focusCharacter)
        {
            foreach (var character in characters)
            {

                echo.Append($"\"{character.Key.Element("name").Value}\" [style = filled, fillcolor={GetGenderColor(character.Key.Element("name").Value)}, {fontname}");
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

        //TODO returns true if the relationship was successfully added 
        private static void NewAddParents(ref StringBuilder echo, XElement parents, XElement focusCharacter)
        {
            List<XElement> parentList = parents.Elements().ToList().OrderBy(p => p.Value).ToList();

            if (parentList.Count > 0)
            {
                StringBuilder parentNode = new StringBuilder("<>");
                foreach (var p in parentList)
                {
                    parentNode.Append(p.Value);
                }

                echo.AppendLine($"\"{parentNode.ToString()}\" [shape=point,width=0.01,height=0.01]; ");

                foreach (var p in parentList)
                {
                    echo.AppendLine($"\"{p.Value}\" -- \"{parentNode.ToString()}\"; ");
                    echo.AppendLine($"\"{parentNode.ToString()}\" -- \"{focusCharacter.Element("name").Value}\"; ");
                }

            }

            //if (!relationships.ContainsKey($"\"{parent}\" -- \"{child}\"; "))
            //{
            //    relationships.Add($"\"{parent}\" -- \"{child}\"; ", true);
            //}
        }

        //returns true if the character was successfully added 
        private static bool AddCharacterIfAbsent(string focusCharacter, ref Dictionary<string, string> characters)
        {
            if (!characters.ContainsKey(focusCharacter))
            {
                XElement tempRoot = XMLParser.CharacterXDocument.Handle.Root;
                characters.Add(focusCharacter, GetGenderColor(focusCharacter));
                return true;
            }
            return false;
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

        private static void AddCharacterInformation(ref StringBuilder echo, Dictionary<string, string> characters, Dictionary<string, bool> relationships, string focusCharacter)
        {
            foreach (var character in characters)
            {
                echo.Append($"\"{character.Key}\" [color={character.Value}, {fontname}");
                if (character.Key.Equals(focusCharacter))
                {
                    echo.Append($", {focusShape}");
                }
                echo.Append($"]; ");
            }
            foreach (var relationship in relationships)
            {
                echo.Append(relationship.Key);
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
            //TODO input XElement
            var result = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                          where c.Element("name").Value.Equals(focusCharacter)
                          select c.Element("gender").Value).First();

            string returnString = $"{maleColor}";

            if (result.Equals("Female"))
            {
                returnString = $"{femaleColor}";
            }
            else if (result.Equals("Other"))
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
                NewAddCharacterIfAbsent(c, addParent, ref characters);
                AppendMarriage(ref echo, ref marriages, c, focusCharacter, isMarried, "diamond");
            }

            //Past Marriages
            foreach (var p in GetPastMarriages(focusCharacter))
            {
                NewAddCharacterIfAbsent(p, addParent, ref characters);
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
                echo.AppendLine($"{{rank=same; \"{c1}\"; \"{c2}\"}}; ");
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

    }
}
