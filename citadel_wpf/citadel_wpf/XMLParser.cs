﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace citadel_wpf
{
    class XMLParser
    {
        public struct XDocumentPair
        {
            public XDocument Handle;
            public string Path;
            public void Save()
            {
                Handle.Save(Path);
                Handle = XDocument.Load(Path);
            }
        }

        public static XDocumentPair CharacterXDocument;
        public static XDocumentPair CharacterRelationshipXDocument;
        public static XDocumentPair LocationXDocument;
        public static XDocumentPair EventXDocument;
        public static XDocumentPair EventRelationshipXDocument;
        public static XDocumentPair NoteXDocument;
        public static XDocumentPair MediaXDocument;

        public static string FolderPath;

        public static XMLParser Instance;

        private static Regex validCharacterRegex = new Regex(@"^[a-zA-Z0-9'\- ]+$");
        private static Regex yearRegex = new Regex(@"^[0-9]*$");

        public XMLParser(string folderPath)
        {
            FolderPath = folderPath;
            UpdateXDocuments();
        }

        private void SetXDocumentContent(string documentName, ref XDocumentPair handle)
        {
            string filePath = FolderPath + $"\\{documentName}.xml";
            if (!File.Exists(filePath))
            {
                StreamWriter s = File.CreateText(filePath);
                s.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                s.WriteLine($"<{documentName}>");
                s.WriteLine($"");
                s.WriteLine($"</{documentName}>");
                s.Close();
            }

            handle.Handle = XDocument.Load(filePath);
            handle.Path = filePath;
        }
        public void UpdateXDocuments()
        {
            SetXDocumentContent("character_notes", ref CharacterXDocument);
            SetXDocumentContent("character_relationship_notes", ref CharacterRelationshipXDocument);
            SetXDocumentContent("location_notes", ref LocationXDocument);
            SetXDocumentContent("event_notes", ref EventXDocument);
            SetXDocumentContent("event_relationship_notes", ref EventRelationshipXDocument);
            SetXDocumentContent("general_notes", ref NoteXDocument);
            SetXDocumentContent("media_notes", ref MediaXDocument);
        }

        public static List<Dictionary<string, string>> GetAllEntities(XDocument handle)
        {
            List<Dictionary<string, string>> entityInformation = new List<Dictionary<string, string>>();
            
            //for each entity
            foreach (var entity in handle.Root.Elements().ToList())
            {
                Dictionary<string, string> information = new Dictionary<string, string>();
                //for each entity's elements
                foreach (var t in entity.Elements())
                {
                    information.Add(t.Name.ToString(), t.Value);
                }
                entityInformation.Add(information);
            }

            //Order by the entity name
            entityInformation = entityInformation.OrderBy(o => o["name"]).ToList();

            return entityInformation;
        }

        public static List<string> GetAllEntityNames(XDocument handle)
        {
            List<string> returnList = new List<string>();
            
            var query = from c in handle.Root.Elements()
                        select new
                        {
                            Name = c.Element("name").Value
                        };

            foreach (var entry in query)
            {
                returnList.Add(entry.Name);
            }

            returnList.Sort();

            return returnList;
        }

        public static Hashtable GetMediaInformation ()
        {
            Hashtable returnTable = new Hashtable();
            string fullFilePath = XMLParser.FolderPath + "\\media_notes.xml";

            if (File.Exists(fullFilePath))
            {

                var xml = XDocument.Load(fullFilePath);

                var query = from c in xml.Root.Descendants("media_note")
                            select new
                            {
                                Title = c.Element("name").Value,
                                Year = c.Element("year").Value,
                                Type = c.Element("type").Value,
                                Genre = c.Element("genre").Value,
                                Summary = c.Element("summary").Value
                            };

                foreach (var t in query)
                {
                    returnTable.Add("Name", t.Title);
                    returnTable.Add("Year", t.Year);
                    returnTable.Add("Type", t.Type);
                    returnTable.Add("Genre", t.Genre);
                    returnTable.Add("Summary", t.Summary);
                }

            }

            return returnTable;
        }

        //is this entity present in the XDocument
        public static bool IsEntityPresent(XDocument handle, string entityName)
        {
            return (from c in handle.Root.Elements()
                    where c.Element("name").Value.ToString().Equals(entityName)
                    select c).Count() > 0 ? true : false;
        }

        //is this relationship present in the XDocument
        public static bool IsRelationshipPresent(XDocument handle, string entityOne, string relationship, string entityTwo)
        {
            return ((from c in handle.Root.Elements()
                     where c.Element("entity_one").Value.ToString().Equals(entityOne)
                     && c.Element("relationship").Value.ToString().Equals(relationship)
                     && c.Element("entity_two").Value.ToString().Equals(entityTwo)
                     select c).Count() > 0 ? true : false);
        }

        public static void FillComboboxWithNames(XDocument handle, ref ComboBox combo, string nameToSkip = "")
        {
            combo.Items.Clear();

            List<string> names = XMLParser.GetAllEntityNames(handle);

            ComboBoxItem cBoxItem;

            foreach (string newEntity in names)
            {
                if (!newEntity.Equals(nameToSkip))
                {
                    cBoxItem = new ComboBoxItem();
                    cBoxItem.Content = newEntity;
                    combo.Items.Add(cBoxItem);
                }
            }
        }

        public static bool IsTextValid(string text)
        {
            return !(string.IsNullOrWhiteSpace(text) || !validCharacterRegex.IsMatch(text));
        }
        public static bool IsYearValid(string year)
        {
            return validCharacterRegex.IsMatch(year);
        }
    }
}
