using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace citadel_wpf
{
    class XMLEntityParser
    {
        //TODO add relationship handles
        private static XDocument characterXDocument = null;
        private static XDocument characterRelationshipXDocument = null;
        private static XDocument locationXDocument = null;
        private static XDocument eventXDocument = null;
        private static XDocument eventRelationshipXDocument = null;
        private static XDocument noteXDocument = null;

        private static XMLEntityParser instance = null;
        private static string FolderPath;

        private XMLEntityParser(string folderPath)
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

        public static XMLEntityParser GetInstance(string folderPath = "")
        {
            if (instance == null)
            {
                instance = new XMLEntityParser(folderPath);
            }

            return instance;
        }

        public ref XDocument GetCharacterXDocument()
        {
            return ref characterXDocument;
        }
        public ref XDocument GetLocationXDocument()
        {
            return ref locationXDocument;
        }
        public ref XDocument GetEventXDocument()
        {
            return ref eventXDocument;
        }
        public ref XDocument GetNoteXDocument()
        {
            return ref noteXDocument;
        }

        public static StreamWriter RemoveEndTag(string filePath)
        {
            //Deletes the last line in the xml file, the closing content tag
            string line = null;
            string finalLine = null;
            List<string> deferredLines = new List<string>();
            using (TextReader inputReader = new StreamReader(filePath))
            {
                while ((line = inputReader.ReadLine()) != null)
                {
                    deferredLines.Add(line);
                }
                finalLine = deferredLines[deferredLines.Count - 1];
                deferredLines.RemoveAt(deferredLines.Count - 1);

                while(!finalLine[finalLine.Count() - 1].Equals('/'))
                {
                    finalLine = finalLine.Remove(finalLine.Count() - 1, 1);
                }
                finalLine = finalLine.Remove(finalLine.Count() - 2, 2);
            }

            StreamWriter stream = new StreamWriter(filePath, false);

            for (int i = 0; i < deferredLines.Count; i++)
            {
                stream.Write(deferredLines[i]);
            }
            stream.Write(finalLine);

            return stream;
        }

        public List<List<string>> GetAllCharacterNotes()
        {
            List<List<string>> returnList = new List<List<string>>();
            
            // Query the data and write out a subset of contacts
            var character = from c in GetCharacterXDocument().Root.Descendants("character")
                            //where ((string)c.Element("name")).Equals("Ygritte")
                        select new
                        {
                            Name = c.Element("name").Value,
                            Gender = c.Element("gender").Value,
                            Description = c.Element("description").Value
                        };

            foreach (var characterEntry in character)
            {

                List<string> temp = new List<string>();

                temp.Add("Name\\" + characterEntry.Name);
                temp.Add("Gender\\" + characterEntry.Gender);
                temp.Add("Description\\" + characterEntry.Description);
                returnList.Add(temp);
            }

            //TODO: after all characters added, add all relationships
                
            return returnList;
        }

        //TODO this
        public static List<List<string>> GetAllCharacterRelationships(string fullFilePath)
        {
            List<List<string>> returnList = new List<List<string>>();

            if (File.Exists(fullFilePath))
            {
                var xml = XDocument.Load(fullFilePath);

                var query = from c in xml.Root.Descendants("character")
                                //where ((string)c.Element("name")).Equals("Ygritte")
                            select new
                            {
                                Name = c.Element("name").Value,
                                Gender = c.Element("gender").Value,
                                Description = c.Element("description").Value
                            };

                foreach (var characterEntry in query)
                {

                    List<string> temp = new List<string>();

                    temp.Add("Name\\" + characterEntry.Name);
                    temp.Add("Gender\\" + characterEntry.Gender);
                    temp.Add("Description\\" + characterEntry.Description);
                    returnList.Add(temp);
                }
            }
            return returnList;
        }

        public List<List<string>> GetAllGeneralNotes()
        {
            List<List<string>> returnList = new List<List<string>>();
            
            var query = from c in GetNoteXDocument().Root.Descendants("general_note")
                        select new {
                            Name = c.Element("name").Value,
                            Description = c.Element("description").Value
                        };

            foreach (var noteEntry in query)
            {

                List<string> temp = new List<string>();
                temp.Add("Name\\" + noteEntry.Name);
                temp.Add("Note\\" + noteEntry.Description);
                returnList.Add(temp);
            }
            
            return returnList;
        }

        public List<List<string>> GetAllLocationNotes()
        {
            List<List<string>> returnList = new List<List<string>>();
            
            var query = from c in GetLocationXDocument().Root.Descendants("location")
                        select new
                        {
                            Name = c.Element("name").Value,
                            Type = c.Element("type").Value,
                            Subtype = c.Element("subtype").Value,
                            Description = c.Element("description").Value,
                        };

            foreach (var locationEntry in query)
            {

                List<string> temp = new List<string>();
                temp.Add("Name\\" + locationEntry.Name);
                temp.Add("Type\\" + locationEntry.Type);
                temp.Add("Subtype\\" + locationEntry.Subtype);
                temp.Add("Description\\" + locationEntry.Description);
                returnList.Add(temp);
            }
            
            return returnList;
        }

        public List<List<string>> GetAllEventNotes()
        {
            List<List<string>> returnList = new List<List<string>>();
            
            var query = from c in GetEventXDocument().Root.Descendants("event")
                        select new
                        {
                            Name = c.Element("name").Value,
                            Location = c.Element("location").Value,
                            Unit_Date = c.Element("unit_date").Value,
                            Date = c.Element("date").Value,
                            Notes = c.Element("description").Value
                        };

            foreach (var eventEntry in query)
            {

                List<string> temp = new List<string>();
                temp.Add("Name\\" + eventEntry.Name);
                temp.Add("Location\\" + eventEntry.Location);
                temp.Add("Unit Date\\" + eventEntry.Unit_Date);
                temp.Add("Date\\" + eventEntry.Date);
                temp.Add("Description\\" + eventEntry.Notes);
                returnList.Add(temp);
            }
            
            return returnList;
        }

        public static List<List<string>> GetAllNotes(string fullFilePath, string rootName)
        {
            List<List<string>> returnList = new List<List<string>>();

            //TODO xml exception
            if (File.Exists(fullFilePath))
            {
                var xml = XDocument.Load(fullFilePath);

                //select all entities in the file, along with their elements and attributes
                var query = from c in xml.Root.Descendants(rootName) select c;
                
                //for each of these, add the attributes to a temporary list
                foreach (var q in xml.Root.Descendants(rootName).ToList())
                {
                    List<string> temp = new List<string>();
                    foreach (var t in q.Elements())
                    {
                        temp.Add(t.Name + "\\" + t.Value);
                        //temp.Add(t.Value);
                    }
                    returnList.Add(temp);
                }
            }
            return returnList;
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

        public static bool IsPresent(XDocument handle, string entityName)
        {
            return (from c in handle.Root.Elements()
                    where c.Element("name").Value.ToString().Equals(entityName)
                    select c).Count() > 0 ? true : false;
        }
    }
}
