using System;
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
        private static XDocument characterXDocument = null;
        private static XDocument characterRelationshipXDocument = null;
        private static XDocument locationXDocument = null;
        private static XDocument eventXDocument = null;
        private static XDocument eventRelationshipXDocument = null;
        private static XDocument noteXDocument = null;

        private static XMLParser instance = null;
        //TODO get rid of, or move here
        private static string FolderPath;

        private static Regex validCharacterRegex = new Regex(@"^[a-zA-Z0-9'\- ]+$");

        private XMLParser(string folderPath)
        {
            FolderPath = folderPath;
            UpdateXDocuments();
        }

        private void SetXDocumentContent(string folderName, string documentName, ref XDocument handle)
        {
            string filePath = folderName + $"\\{documentName}.xml";
            if (!File.Exists(filePath))
            {
                StreamWriter s = File.CreateText(folderName + $"\\{documentName}.xml");
                s.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                s.WriteLine($"<{documentName}>");
                s.WriteLine($"");
                s.WriteLine($"</{documentName}>");
                s.Close();
            }

            handle = XDocument.Load(filePath);
        }
        public void UpdateXDocuments()
        {
            SetXDocumentContent(FolderPath, "character_notes", ref characterXDocument);
            SetXDocumentContent(FolderPath, "character_relationship_notes", ref characterRelationshipXDocument);
            SetXDocumentContent(FolderPath, "location_notes", ref locationXDocument);
            SetXDocumentContent(FolderPath, "event_notes", ref eventXDocument);
            SetXDocumentContent(FolderPath, "event_relationship_notes", ref eventRelationshipXDocument);
            SetXDocumentContent(FolderPath, "general_notes", ref noteXDocument);
        }

        public static XMLParser GetInstance(string folderPath = "")
        {
            if (instance == null)
            {
                instance = new XMLParser(folderPath);
            }

            return instance;
        }

        public ref XDocument GetCharacterXDocument()
        {
            return ref characterXDocument;
        }
        public ref XDocument GetCharacterRelationshipXDocument()
        {
            return ref characterRelationshipXDocument;
        }
        public ref XDocument GetLocationXDocument()
        {
            return ref locationXDocument;
        }
        public ref XDocument GetEventXDocument()
        {
            return ref eventXDocument;
        }
        public ref XDocument GetEventRelationshipXDocument()
        {
            return ref eventRelationshipXDocument;
        }
        public ref XDocument GetNoteXDocument()
        {
            return ref noteXDocument;
        }

        public static List<Dictionary<string, string>> GetAllNotes(XDocument handle)
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

        public static List<string> GetAllNames(XDocument handle)
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

        public static Hashtable GetMediaInformation (string fullFilePath)
        {
            Hashtable returnTable = new Hashtable();

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
        public static bool IsPresent(XDocument handle, string entityName)
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

        public static void FillBoxWithNames(XDocument handle, ref ComboBox combo, string nameToSkip = "")
        {
            combo.Items.Clear();

            List<string> names = XMLParser.GetAllNames(handle);

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
    }
}
