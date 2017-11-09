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
        private static XDocument character_handle = null;
        private static XDocument character_relationship_handle = null;
        private static XDocument location_handle = null;
        private static XDocument event_handle = null;
        private static XDocument event_relationship_handle = null;
        private static XDocument note_handle = null;

        private static XMLEntityParser instance = null;
        private static string FolderPath;

        private XMLEntityParser(string folderPath)
        {
            FolderPath = folderPath;
            UpdateHandles();
        }

        private void SetHandle(string folderName, string documentName, ref XDocument handle)
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
        public void UpdateHandles()
        {
            SetHandle(FolderPath, "character_notes", ref character_handle);
            SetHandle(FolderPath, "character_relationship_notes", ref character_relationship_handle);
            SetHandle(FolderPath, "location_notes", ref location_handle);
            SetHandle(FolderPath, "event_notes", ref event_handle);
            SetHandle(FolderPath, "event_relationship_notes", ref event_relationship_handle);
            SetHandle(FolderPath, "general_notes", ref note_handle);
        }

        public static XMLEntityParser GetInstance(string folderPath = "")
        {
            if (instance == null)
            {
                instance = new XMLEntityParser(folderPath);
            }

            return instance;
        }

        public XDocument GetCharacterHandle()
        {
            return character_handle;
        }
        public XDocument GetLocationHandle()
        {
            return location_handle;
        }
        public XDocument GetEventHandle()
        {
            return event_handle;
        }
        public XDocument GetNoteHandle()
        {
            return note_handle;
        }


        //Obsolete
        public static string attemptParse()
        {
            StringBuilder output = new StringBuilder();

            String xmlString =
                    @"<?xml version='1.0'?>
        <!-- This is a sample XML document -->
        <Items>
          <Item>test with a child element <more/> stuff</Item>
        </Items>";
            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(output, ws))
                {

                    // Parse the file and display each of the nodes.
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                writer.WriteStartElement(reader.Name);
                                break;
                            case XmlNodeType.Text:
                                writer.WriteString(reader.Value);
                                break;
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.ProcessingInstruction:
                                writer.WriteProcessingInstruction(reader.Name, reader.Value);
                                break;
                            case XmlNodeType.Comment:
                                writer.WriteComment(reader.Value);
                                break;
                            case XmlNodeType.EndElement:
                                writer.WriteFullEndElement();
                                break;
                        }
                    }

                }
            }
            return output.ToString();
        }

        //Obsolete
        public static string attemptSpecificParse()
        {
            StringBuilder output = new StringBuilder();

            String xmlString =
                            @"<bookstore>
                    <book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>
                        <title>The Autobiography of Benjamin Franklin</title>
                        <author>
                            <first-name>Benjamin</first-name>
                            <last-name>Franklin</last-name>
                        </author>
                        <price>8.99</price>
                    </book>
            <book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>
                        <title>The Autobiography of Benjamin Franklin</title>
                        <author>
                            <first-name>Benjamin</first-name>
                            <last-name>Franklin</last-name>
                        </author>
                        <price>8.99</price>
                    </book>
                </bookstore>";

            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                reader.ReadToFollowing("book");
                reader.MoveToFirstAttribute();
                string genre = reader.Value;
                output.AppendLine("The genre value: " + genre);

                reader.ReadToFollowing("title");
                output.AppendLine("Content of the title element: " + reader.ReadElementContentAsString());
            }

            return output.ToString();
        }

        //Obsolete
        public static string XPathParseExample(string folderName)
        {
            XPathNavigator nav;
            XPathDocument docNav;
            XPathNodeIterator NodeIter;
            String strExpression;

            docNav = new XPathDocument(folderName + @"/TestInput.xml");

            nav = docNav.CreateNavigator();

            strExpression = "/cars/supercars[@company = 'Lamborgini']/carname";

            NodeIter = nav.Select(strExpression);

            string temp = "";

            while (NodeIter.MoveNext())
            {
                temp += "Lamborgini: " + NodeIter.Current.Value + "\n";
            };

            return temp;
        }

        //Obsolete
        public static string XPathParse(string fullFilePath, string XMLExpression)
        {
            XPathNavigator nav;
            XPathDocument docNav;
            XPathNodeIterator NodeIter;
            string temp = "";

            //docNav = new XPathDocument(folderName + @"/TestInput.xml");
            if (File.Exists(fullFilePath))
            {
                docNav = new XPathDocument(fullFilePath);

                nav = docNav.CreateNavigator();

                //strExpression = "/cars/supercars[@company = 'Lamborgini']/carname";

                NodeIter = nav.Select(XMLExpression);

                while (NodeIter.MoveNext())
                {
                    temp += "Example: " + NodeIter.Current.Value + "\n";
                };
                
            }

            return temp;
        }

        //Obsolete
        public static string LINQParseTest(string fullFilePath)
        {
            string temp = "";

            //docNav = new XPathDocument(folderName + @"/TestInput.xml");
            if (File.Exists(fullFilePath))
            {
                var xml = XDocument.Load(fullFilePath);

                // Query the data and write out a subset of contacts
                var query = from c in xml.Root.Descendants("character")
                            //where ((string)c.Element("name")).Equals("Ygritte")
                            select c.Element("name").Value + " " +
                                   c.Element("gender").Value;

                //select new
                //{
                //    Id = (string)x.Attribute("id"),
                //    Lang = (string)x.Attribute("lang"),
                //    Name = x.Descendants("configitem").Select(y => y.Attribute("name").Value).FirstOrDefault(y => y == "working_status")
                //};

                foreach (string name in query)
                {
                    temp += "Character's Full Name and gender: " + name + "\n";
                }
            }

            return temp;
        }

        //TODO use this to revive XML only model
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
            var character = from c in GetCharacterHandle().Root.Descendants("character")
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
            
            var query = from c in GetNoteHandle().Root.Descendants("general_note")
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
            
            var query = from c in GetLocationHandle().Root.Descendants("location")
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
            
            var query = from c in GetEventHandle().Root.Descendants("event")
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

        public static List<string> GetAllNames(string fullFilePath, string rootName)
        {
            List<string> returnList = new List<string>();

            //TODO xml exception
            if (File.Exists(fullFilePath))
            {
                var xml = XDocument.Load(fullFilePath);

                // Query the data and write out a subset of contacts
                var query = from c in xml.Root.Descendants(rootName)
                            select new
                            {
                                Name = c.Element("name").Value
                            };

                foreach (var entry in query)
                {
                    returnList.Add(entry.Name);
                }
            }
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
    }
}
