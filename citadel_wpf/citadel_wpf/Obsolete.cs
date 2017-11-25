using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace citadel_wpf
{
    class Obsolete
    {

        //Obsolete
        //public static string attemptParse()
        //{
        //    StringBuilder output = new StringBuilder();

        //    String xmlString =
        //            @"<?xml version='1.0'?>
        //<!-- This is a sample XML document -->
        //<Items>
        //  <Item>test with a child element <more/> stuff</Item>
        //</Items>";
        //    // Create an XmlReader
        //    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
        //    {
        //        XmlWriterSettings ws = new XmlWriterSettings();
        //        ws.Indent = true;
        //        using (XmlWriter writer = XmlWriter.Create(output, ws))
        //        {

        //            // Parse the file and display each of the nodes.
        //            while (reader.Read())
        //            {
        //                switch (reader.NodeType)
        //                {
        //                    case XmlNodeType.Element:
        //                        writer.WriteStartElement(reader.Name);
        //                        break;
        //                    case XmlNodeType.Text:
        //                        writer.WriteString(reader.Value);
        //                        break;
        //                    case XmlNodeType.XmlDeclaration:
        //                    case XmlNodeType.ProcessingInstruction:
        //                        writer.WriteProcessingInstruction(reader.Name, reader.Value);
        //                        break;
        //                    case XmlNodeType.Comment:
        //                        writer.WriteComment(reader.Value);
        //                        break;
        //                    case XmlNodeType.EndElement:
        //                        writer.WriteFullEndElement();
        //                        break;
        //                }
        //            }

        //        }
        //    }
        //    return output.ToString();
        //}

        ////Obsolete
        //public static string attemptSpecificParse()
        //{
        //    StringBuilder output = new StringBuilder();

        //    String xmlString =
        //                    @"<bookstore>
        //            <book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>
        //                <title>The Autobiography of Benjamin Franklin</title>
        //                <author>
        //                    <first-name>Benjamin</first-name>
        //                    <last-name>Franklin</last-name>
        //                </author>
        //                <price>8.99</price>
        //            </book>
        //    <book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>
        //                <title>The Autobiography of Benjamin Franklin</title>
        //                <author>
        //                    <first-name>Benjamin</first-name>
        //                    <last-name>Franklin</last-name>
        //                </author>
        //                <price>8.99</price>
        //            </book>
        //        </bookstore>";

        //    // Create an XmlReader
        //    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
        //    {
        //        reader.ReadToFollowing("book");
        //        reader.MoveToFirstAttribute();
        //        string genre = reader.Value;
        //        output.AppendLine("The genre value: " + genre);

        //        reader.ReadToFollowing("title");
        //        output.AppendLine("Content of the title element: " + reader.ReadElementContentAsString());
        //    }

        //    return output.ToString();
        //}

        ////Obsolete
        //public static string XPathParseExample(string folderName)
        //{
        //    XPathNavigator nav;
        //    XPathDocument docNav;
        //    XPathNodeIterator NodeIter;
        //    String strExpression;

        //    docNav = new XPathDocument(folderName + @"/TestInput.xml");

        //    nav = docNav.CreateNavigator();

        //    strExpression = "/cars/supercars[@company = 'Lamborgini']/carname";

        //    NodeIter = nav.Select(strExpression);

        //    string temp = "";

        //    while (NodeIter.MoveNext())
        //    {
        //        temp += "Lamborgini: " + NodeIter.Current.Value + "\n";
        //    };

        //    return temp;
        //}

        ////Obsolete
        //public static string XPathParse(string fullFilePath, string XMLExpression)
        //{
        //    XPathNavigator nav;
        //    XPathDocument docNav;
        //    XPathNodeIterator NodeIter;
        //    string temp = "";

        //    //docNav = new XPathDocument(folderName + @"/TestInput.xml");
        //    if (File.Exists(fullFilePath))
        //    {
        //        docNav = new XPathDocument(fullFilePath);

        //        nav = docNav.CreateNavigator();

        //        //strExpression = "/cars/supercars[@company = 'Lamborgini']/carname";

        //        NodeIter = nav.Select(XMLExpression);

        //        while (NodeIter.MoveNext())
        //        {
        //            temp += "Example: " + NodeIter.Current.Value + "\n";
        //        };

        //    }

        //    return temp;
        //}

        ////Obsolete
        //public static string LINQParseTest(string fullFilePath)
        //{
        //    string temp = "";

        //    //docNav = new XPathDocument(folderName + @"/TestInput.xml");
        //    if (File.Exists(fullFilePath))
        //    {
        //        var xml = XDocument.Load(fullFilePath);

        //        // Query the data and write out a subset of contacts
        //        var query = from c in xml.Root.Descendants("character")
        //                        //where ((string)c.Element("name")).Equals("Ygritte")
        //                    select c.Element("name").Value + " " +
        //                           c.Element("gender").Value;

        //        //select new
        //        //{
        //        //    Id = (string)x.Attribute("id"),
        //        //    Lang = (string)x.Attribute("lang"),
        //        //    Name = x.Descendants("configitem").Select(y => y.Attribute("name").Value).FirstOrDefault(y => y == "working_status")
        //        //};

        //        foreach (string name in query)
        //        {
        //            temp += "Character's Full Name and gender: " + name + "\n";
        //        }
        //    }

        //    return temp;
        //}

        ////protected bool SaveEntity(XDocument handle, string docName, string entityName, string entityAsXML)
        ////{
        ////    StreamWriter fileHandle = null;
        ////    string docURI = XMLParser.FolderPath + $"\\{docName}.xml";

        ////    try
        ////    {

        ////        if (IsEntityPresent(handle, entityName))
        ////        {
        ////            return false;
        ////        }
        ////        else
        ////        {
        ////            if (File.Exists(docURI))
        ////            {
        ////                fileHandle = XMLEntityParser.RemoveEndTag(docURI);
        ////            }
        ////            else
        ////            {
        ////                fileHandle = new StreamWriter(docURI, true);
        ////                fileHandle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><" + docName + ">");
        ////            }

        ////            fileHandle.Write(entityAsXML);

        ////            fileHandle.Write("</" + docName + ">");

        ////            fileHandle.Close();

        ////            //UpdateReliantWindows();

        ////            //Close();
        ////            XMLEntityParser.GetInstance().UpdateXDocuments();

        ////            return true;
        ////        }


        ////    }
        ////    catch (IOException)
        ////    {
        ////        System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
        ////    }
        ////    //catch (Exception)
        ////    //{
        ////    //    System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
        ////    //}
        ////    finally
        ////    {
        ////        if (fileHandle != null)
        ////        {
        ////            fileHandle.Close();
        ////        }

        ////        base.Close();
        ////    }


        ////    return false;

        ////    //else
        ////    //{
        ////    //    required_text.Foreground = Brushes.Red;
        ////    //}
        ////}

        //public static void TestCharacterRelationship(string c1, string c2)
        //{
        //    string echo = $"graph s {{ label=\"Character Relationship\"; {c1} -- {c2}; }}";
        //    string textpath = XMLParser.FolderPath + "\\Relationship.dot";
        //    StreamWriter streamwriter = File.CreateText(textpath);
        //    streamwriter.Write(echo);
        //    streamwriter.Close();

        //    //Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textpath} -o {XMLParser.FolderPath}/testRelationship.png  & del {textpath}");
        //}

        //public void Fill_Note_Area(List<List<string>> entityNodes, WrapPanel area)
        //{
        //    area.Children.Clear();
        //    area.MinHeight = NoteNode.NoteNodeHeight;

        //    foreach (List<string> l in entityNodes)
        //    {
        //        NoteNode n = new NoteNode(null);

        //        foreach (string s in l)
        //        {
        //            string[] parts = s.Split('\\');
        //            //parts[0] = ToTitleCase(parts[0]);

        //            if (!String.IsNullOrWhiteSpace(parts[1]))
        //            {
        //                StringBuilder t = new StringBuilder();
        //                t.Append(parts[0]);
        //                t.Append(":\n    ");
        //                t.Append(parts[1]);
        //                t.Append("\n");
        //                n.DescriptionText += t.ToString();
        //            }
        //        }
        //        area.Children.Add(n);
        //        area.MinHeight += NoteNode.NoteNodeHeight / 2;
        //    }
        //}

        //public static StreamWriter RemoveEndTag(string filePath)
        //{
        //    //Deletes the last line in the xml file, the closing content tag
        //    string line = null;
        //    string finalLine = null;
        //    List<string> deferredLines = new List<string>();
        //    using (TextReader inputReader = new StreamReader(filePath))
        //    {
        //        while ((line = inputReader.ReadLine()) != null)
        //        {
        //            deferredLines.Add(line);
        //        }
        //        finalLine = deferredLines[deferredLines.Count - 1];
        //        deferredLines.RemoveAt(deferredLines.Count - 1);

        //        while (!finalLine[finalLine.Count() - 1].Equals('/'))
        //        {
        //            finalLine = finalLine.Remove(finalLine.Count() - 1, 1);
        //        }
        //        finalLine = finalLine.Remove(finalLine.Count() - 2, 2);
        //    }

        //    StreamWriter stream = new StreamWriter(filePath, false);

        //    for (int i = 0; i < deferredLines.Count; i++)
        //    {
        //        stream.Write(deferredLines[i]);
        //    }
        //    stream.Write(finalLine);

        //    return stream;
        //}

        //public List<List<string>> GetAllCharacterNotes()
        //{
        //    List<List<string>> returnList = new List<List<string>>();

        //    // Query the data and write out a subset of contacts
        //    var character = from c in GetCharacterXDocument.Handle.Root.Descendants("character")
        //                        //where ((string)c.Element("name")).Equals("Ygritte")
        //                    select new
        //                    {
        //                        Name = c.Element("name").Value,
        //                        Gender = c.Element("gender").Value,
        //                        Description = c.Element("description").Value
        //                    };

        //    foreach (var characterEntry in character)
        //    {

        //        List<string> temp = new List<string>();

        //        temp.Add("Name\\" + characterEntry.Name);
        //        temp.Add("Gender\\" + characterEntry.Gender);
        //        temp.Add("Description\\" + characterEntry.Description);
        //        returnList.Add(temp);
        //    }

        //    return returnList;
        //}

        //public static List<List<string>> GetAllCharacterRelationships(string fullFilePath)
        //{
        //    List<List<string>> returnList = new List<List<string>>();

        //    //if (File.Exists(fullFilePath))
        //    //{
        //    //    var xml = XDocument.Load(fullFilePath);

        //    //    var query = from c in xml.Root.Descendants("character")
        //    //                    //where ((string)c.Element("name")).Equals("Ygritte")
        //    //                select new
        //    //                {
        //    //                    Name = c.Element("name").Value,
        //    //                    Gender = c.Element("gender").Value,
        //    //                    Description = c.Element("description").Value
        //    //                };

        //    //    foreach (var characterEntry in query)
        //    //    {

        //    //        List<string> temp = new List<string>();

        //    //        temp.Add("Name\\" + characterEntry.Name);
        //    //        temp.Add("Gender\\" + characterEntry.Gender);
        //    //        temp.Add("Description\\" + characterEntry.Description);
        //    //        returnList.Add(temp);
        //    //    }
        //    //}
        //    return returnList;
        //}

        //public List<List<string>> GetAllGeneralNotes()
        //{
        //    List<List<string>> returnList = new List<List<string>>();

        //    var query = from c in GetNoteXDocument.Handle.Root.Descendants("general_note")
        //                select new
        //                {
        //                    Name = c.Element("name").Value,
        //                    Description = c.Element("description").Value
        //                };

        //    foreach (var noteEntry in query)
        //    {

        //        List<string> temp = new List<string>();
        //        temp.Add("Name\\" + noteEntry.Name);
        //        temp.Add("Note\\" + noteEntry.Description);
        //        returnList.Add(temp);
        //    }

        //    return returnList;
        //}

        //public List<List<string>> GetAllLocationNotes()
        //{
        //    List<List<string>> returnList = new List<List<string>>();

        //    var query = from c in GetLocationXDocument.Handle.Root.Descendants("location")
        //                select new
        //                {
        //                    Name = c.Element("name").Value,
        //                    Type = c.Element("type").Value,
        //                    Subtype = c.Element("subtype").Value,
        //                    Description = c.Element("description").Value,
        //                };

        //    foreach (var locationEntry in query)
        //    {

        //        List<string> temp = new List<string>();
        //        temp.Add("Name\\" + locationEntry.Name);
        //        temp.Add("Type\\" + locationEntry.Type);
        //        temp.Add("Subtype\\" + locationEntry.Subtype);
        //        temp.Add("Description\\" + locationEntry.Description);
        //        returnList.Add(temp);
        //    }

        //    return returnList;
        //}

        //public List<List<string>> GetAllEventNotes()
        //{
        //    List<List<string>> returnList = new List<List<string>>();

        //    var query = from c in GetEventXDocument.Handle.Root.Descendants("event")
        //                select new
        //                {
        //                    Name = c.Element("name").Value,
        //                    Location = c.Element("location").Value,
        //                    Unit_Date = c.Element("unit_date").Value,
        //                    Date = c.Element("date").Value,
        //                    Notes = c.Element("description").Value
        //                };

        //    foreach (var eventEntry in query)
        //    {

        //        List<string> temp = new List<string>();
        //        temp.Add("Name\\" + eventEntry.Name);
        //        temp.Add("Location\\" + eventEntry.Location);
        //        temp.Add("Unit Date\\" + eventEntry.Unit_Date);
        //        temp.Add("Date\\" + eventEntry.Date);
        //        temp.Add("Description\\" + eventEntry.Notes);
        //        returnList.Add(temp);
        //    }

        //    return returnList;
        //}
    }
}
