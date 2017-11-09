using System;
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

    }
}
