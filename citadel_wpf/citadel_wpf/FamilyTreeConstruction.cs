﻿using System;
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
        private static string font = $"fontname=\"Helvetica\"";

        //public FamilyTreeConstruction() { }        

        public static void ImmediateFamilyTree(string focusCharacter)
        {
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Immediate Family Tree for {focusCharacter}\" {font}; ");
            Dictionary<string, bool> relationships = new Dictionary<string, bool>();
            Dictionary<string, string> characters = new Dictionary<string, string>();

            AddCharacterIfAbsent(focusCharacter, ref characters);

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

            foreach (var c in GetChildrenOf(focusCharacter))
            {
                AddCharacterIfAbsent(c, ref characters);
                AddRelationshipIfAbsent(focusCharacter, c, ref relationships);
            }

            AddCharacterInformation(ref echo, characters, relationships);

            echo.Append("}");

            SaveEcho(echo, "ImmediateFamilyTree", focusCharacter);
        }

        public static void ExtendedFamilyTree(string focusCharacter)
        {
            //Grandparents/children + aunts/uncles + cousins + nieces/nephews
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Extended Family Tree for {focusCharacter}\" {font}; ");
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

            AddCharacterInformation(ref echo, characters, relationships);

            echo.Append("}");

            SaveEcho(echo, "ExtendedFamilyTree", focusCharacter);
        }

        //TODO Make a function to clean up relationships -> currently VERY cluttered; If two characters ar emarried and have the same kids, sprout from a marriage node
        public static void RecursiveFullFamilyTree(string focusCharacter)
        {
            //All familial relationships
            StringBuilder echo = new StringBuilder($"graph s {{ label=\"Recursive Full Family Tree for {focusCharacter}\" {font}; ");
            Dictionary<string, bool> relationships = new Dictionary<string, bool>();
            Dictionary<string, string> characters = new Dictionary<string, string>();

            AddCharacterIfAbsent(focusCharacter, ref characters);

            RecursiveTreeHelper(focusCharacter, ref characters, ref relationships);
            //TODO catch the stack being blown and draw up to this point

            AddCharacterInformation(ref echo, characters, relationships);

            echo.Append("}");

            SaveEcho(echo, "FullFamilyTree", focusCharacter);
        }

        public static void RecursiveTreeHelper (string focusCharacter, ref Dictionary<string, string> characters, ref Dictionary<string, bool> relationships)
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
        private static bool AddCharacterIfAbsent(string focusCharacter, ref Dictionary<string, string> characters)
        {
            if (!characters.ContainsKey(focusCharacter))
            {
                characters.Add(focusCharacter, GetGenderColor(focusCharacter));
                return true;
            }
            return false;
        }

        private static void AddCharacterInformation(ref StringBuilder echo, Dictionary<string, string> characters, Dictionary<string, bool> relationships)
        {
            foreach (var character in characters)
            {
                echo.Append($"\"{character.Key}\" [color={character.Value}, {font}]; ");
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
            string textPath = FrontPage.FolderPath + $"\\{type}.dot";
            string imagePath = $"{FrontPage.FolderPath}/{focusCharacter}_{type}.png";
            StreamWriter streamwriter = File.CreateText(textPath);
            streamwriter.Write(echo);
            streamwriter.Close();

            Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textPath} -o {imagePath} & del {textPath} & {imagePath}");
        }

        //TODO move format to XMLEntityParser?
        private static IEnumerable<string> GetGenerationNames(string focusCharacter, string relationshipName, string childToIgnore = "")
        {
            //TODO make more effecient by reducing the number of times the first line has to be called -> set descendant before the group of calls to this function is made
            return (from c in XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Descendants("character_relationship")
                    where c.Element("entity_one").Value.ToString().Equals(focusCharacter)
                    && c.Element("relationship").Value.ToString().Equals(relationshipName)
                    && (childToIgnore.Equals("") || (!childToIgnore.Equals("") && (!c.Element("entity_two").Value.ToString().Equals(childToIgnore))))
                    select c.Element("entity_two").Value.ToString());
        }

        private static string GetGenderColor(string focusCharacter)
        {
            //TODO make more effecient by reducing the number of times the first line has to be called -> set descendant before the group of calls to this function is made
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
