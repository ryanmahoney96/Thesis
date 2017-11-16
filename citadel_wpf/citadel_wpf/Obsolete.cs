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

        //protected bool SaveEntity(XDocument handle, string docName, string entityName, string entityAsXML)
        //{
        //    StreamWriter fileHandle = null;
        //    string docURI = FrontPage.FolderPath + $"\\{docName}.xml";

        //    try
        //    {

        //        if (IsPresent(handle, entityName))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            if (File.Exists(docURI))
        //            {
        //                fileHandle = XMLEntityParser.RemoveEndTag(docURI);
        //            }
        //            else
        //            {
        //                fileHandle = new StreamWriter(docURI, true);
        //                fileHandle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><" + docName + ">");
        //            }

        //            fileHandle.Write(entityAsXML);

        //            fileHandle.Write("</" + docName + ">");

        //            fileHandle.Close();

        //            //UpdateReliantWindows();

        //            //Close();
        //            XMLEntityParser.GetInstance().UpdateXDocuments();

        //            return true;
        //        }


        //    }
        //    catch (IOException)
        //    {
        //        System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
        //    }
        //    //catch (Exception)
        //    //{
        //    //    System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
        //    //}
        //    finally
        //    {
        //        if (fileHandle != null)
        //        {
        //            fileHandle.Close();
        //        }

        //        base.Close();
        //    }


        //    return false;

        //    //else
        //    //{
        //    //    required_text.Foreground = Brushes.Red;
        //    //}
        //}

        public static void TestCharacterRelationship(string c1, string c2)
        {
            string echo = $"graph s {{ label=\"Character Relationship\"; {c1} -- {c2}; }}";
            string textpath = FrontPage.FolderPath + "\\Relationship.dot";
            StreamWriter streamwriter = File.CreateText(textpath);
            streamwriter.Write(echo);
            streamwriter.Close();

            //Process.Start("cmd.exe", @"/c" + $"dot -Tpng {textpath} -o {FrontPage.FolderPath}/testRelationship.png  & del {textpath}");
        }

        public void Fill_Note_Area(List<List<string>> entityNodes, WrapPanel area)
        {
            area.Children.Clear();
            area.MinHeight = NoteNode.NoteNodeHeight;

            foreach (List<string> l in entityNodes)
            {
                NoteNode n = new NoteNode();

                foreach (string s in l)
                {
                    string[] parts = s.Split('\\');
                    //parts[0] = ToTitleCase(parts[0]);

                    if (!String.IsNullOrWhiteSpace(parts[1]))
                    {
                        StringBuilder t = new StringBuilder();
                        t.Append(parts[0]);
                        t.Append(":\n    ");
                        t.Append(parts[1]);
                        t.Append("\n");
                        n.DescriptionText += t.ToString();
                    }
                }
                area.Children.Add(n);
                area.MinHeight += NoteNode.NoteNodeHeight / 2;
            }
        }
    }
}
