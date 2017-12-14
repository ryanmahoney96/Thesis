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
        private static string focusShape = $"shape=box";
        //TODO adjust these strings
        private static string maleColor = $"navy";
        private static string femaleColor = $"orangered2";
        private static string otherColor = $"mediumvioletred";
        private static string marriage = $"crimson";
        private static string splines = $"splines=ortho";

        public static void OldImmediateFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ImmediateFamilyTreeString} for {focusCharacter}\" {fontname} {splines}; ");
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
            //TODO make concentrate variable, {splines}
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{ImmediateFamilyTreeString} for {focusCharacter}\" {fontname} concentrate=true; ");
            List<XElement> characters = new List<XElement>();

            NewAddCharacterIfAbsent(focusCharacter, ref characters);

            //Parents
            foreach (var p in GetParentsOf(focusCharacter))
            {
                NewAddCharacterIfAbsent(p, ref characters);
                //AddRelationshipIfAbsent(p, focusCharacter, ref relationships);

                //Siblings
                foreach (var s in GetSiblingsOf(focusCharacter, p))
                {
                    NewAddCharacterIfAbsent(s, ref characters);
                    //AddRelationshipIfAbsent(p, s, ref relationships);
                }
            }

            //Children
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                NewAddCharacterIfAbsent(c, ref characters);
                //AddRelationshipIfAbsent(focusCharacter, c, ref relationships);
            }

            NewAddCharacterInformation(ref echo, characters, focusCharacter);

            echo.Append("}");

            SaveEcho(echo, "ImmediateFamilyTree", focusCharacter);
        }

        public static void ExtendedFamilyTree(string focusCharacter)
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

        //TODO Make a function to clean up relationships -> currently VERY cluttered; If two characters have the same kids, connect
        public static void FullFamilyTree(string focusCharacter)
        {
            //All familial relationships
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"{FullFamilyTreeString} for {focusCharacter}\" {fontname} {splines}; ");
            Dictionary<string, bool> relationships = new Dictionary<string, bool>();
            Dictionary<string, string> characters = new Dictionary<string, string>();

            AddCharacterIfAbsent(focusCharacter, ref characters);

            RecursiveTreeHelper(focusCharacter, ref characters, ref relationships);

            AddCharacterInformation(ref echo, characters, relationships, focusCharacter);

            echo.Append("}");

            SaveEcho(echo, "FullFamilyTree", focusCharacter);
        }

        public static void RecursiveTreeHelper(string focusCharacter, ref Dictionary<string, string> characters, ref Dictionary<string, bool> relationships)
        {

            foreach (var p in GetParentsOf(focusCharacter))
            {
                if (AddRelationshipIfAbsent(p, focusCharacter, ref relationships))
                {
                    AddCharacterIfAbsent(p, ref characters);
                    RecursiveTreeHelper(p, ref characters, ref relationships);
                }
            }
            foreach (var c in GetChildrenOf(focusCharacter))
            {
                if (AddRelationshipIfAbsent(focusCharacter, c, ref relationships))
                {
                    AddCharacterIfAbsent(c, ref characters);
                    RecursiveTreeHelper(c, ref characters, ref relationships);
                }
            }
        }

        //returns true if the character was successfully added 
        private static bool NewAddCharacterIfAbsent(string focusCharacter, ref List<XElement> characters)
        {
            XElement characterRef = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                     where c.Element("name").Value.Equals(focusCharacter)
                                     select c).First();

            if (!characters.Contains(characterRef))
            {
                characters.Add(characterRef);
                return true;
            }
            return false;
        }

        private static void NewAddCharacterInformation(ref StringBuilder echo, List<XElement> characters, string focusCharacter)
        {
            var characterReferences = from c in characters
                                      select c;

            foreach (var character in characterReferences)
            {

                echo.Append($"\"{character.Element("name").Value}\" [color={GetGenderColor(character.Element("name").Value)}, {fontname}");
                if (character.Element("name").Value.Equals(focusCharacter))
                {
                    echo.Append($", {focusShape}");
                }
                echo.Append($"]; ");

                //TODO get parent info
            }
            foreach (var character in characterReferences)
            {
                NewAddParents(ref echo, character.Element("parents"), character);
            }

        }

        //TODO returns true if the relationship was successfully added 
        private static void NewAddParents(ref StringBuilder echo, XElement parents, XElement focusCharacter)
        {
            List<XElement> parentList = parents.Elements().ToList().OrderBy(p => p.Value).ToList();

            if (parentList.Count > 0)
            {
                StringBuilder parentNode = new StringBuilder("");
                foreach (var p in parentList)
                {
                    parentNode.Append(p.Value);
                }

                echo.Append($"\"{parentNode.ToString()}\" [shape=point,width=0.01,height=0.01]; ");

                foreach (var p in parentList)
                {
                    echo.Append($"\"{p.Value}\" -- \"{parentNode.ToString()}\"");
                    echo.Append($"\"{parentNode.ToString()}\" -- \"{focusCharacter.Element("name").Value}\"; ");
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

            Process.Start("cmd.exe", @"/c" + $"dot -Tsvg {textPath} -o {imagePath} & del {textPath} & {imagePath}");
        }

        //TODO edit this
        private static IEnumerable<string> GetMarriages(string focusCharacter, XElement rootPlaceholder)
        {
            return (from c in rootPlaceholder.Descendants("character_relationship")
                    where c.Element("entity_one").Value.ToString().Equals(focusCharacter)
                    && c.Element("relationship").Value.ToString().Equals(CharacterRelationshipPrompt.IsMarriedTo)
                    select c.Element("entity_two").Value.ToString());
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
            else if (result.Equals("Other"))
            {
                returnString = $"{otherColor}";
            }

            return returnString;
        }

        private static IEnumerable<string> GetGenerationNames(string focusCharacter, string elementType, string childToIgnore = "")
        {
            XElement CurrentCharacterReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                                  where c.Element("name").Value.Equals(focusCharacter)
                                                  select c).First();

            var relations = from p in CurrentCharacterReference.Element(elementType).Elements()
                            where (childToIgnore.Equals("") || ((!childToIgnore.Equals("") && (!p.Value.Equals(childToIgnore)))))
                            select p.Value;

            return relations;

        }

        private static IEnumerable<string> GetParentsOf(string focusCharacter)
        {
            return GetGenerationNames(focusCharacter, "parents");
        }
        private static IEnumerable<string> GetChildrenOf(string focusCharacter)
        {
            return GetGenerationNames(focusCharacter, "children");
        }
        private static IEnumerable<string> GetSiblingsOf(string focusCharacter, string parent)
        {
            return GetGenerationNames(parent, "children", focusCharacter);
        }

        /*Look for parents, connect child to midnode(parent1name, parent2name), midnode to parents

            One parent= child --> parent

            One Child = straight line?*/

    }
}
