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
    public partial class XMLParser
    {

        public static XDocumentInformation CharacterXDocument;
        public static XDocumentInformation LocationXDocument;
        public static XDocumentInformation EventXDocument;
        public static XDocumentInformation NoteXDocument;
        public static XDocumentInformation MediaXDocument;

        public static XDocumentInformation[] XDocuments = {CharacterXDocument, LocationXDocument, EventXDocument, NoteXDocument, MediaXDocument };

        public static string FolderPath;

        public static XMLParser Instance;

        private static Regex validCharacterRegex = new Regex(@"^[a-zA-Z0-9'\!\\\$\%\&\(\)\,\.\;\:\+\=\- ]+$");
        private static Regex yearRegex = new Regex(@"^[0-9]*$");

        public XMLParser(string folderPath)
        {
            FolderPath = folderPath;
            UpdateXDocuments();
            EventOrdering.Initialize();
        }

        private void SetXDocumentContent(string documentName, ref XDocumentInformation XDoc)
        {
            string filePath = FolderPath + $"\\{documentName}.xml";
            XDoc = new XDocumentInformation();
            XDoc.Path = filePath;
            XDoc.Name = documentName;

            if (!File.Exists(filePath))
            {
                XDoc.Handle = new XDocument(new XElement(documentName));
                XDoc.Save();
            }
            else
            {
                XDoc.Handle = XDocument.Load(filePath);
            }
            
        }

        public void UpdateMediaXDocument()
        {
            SetXDocumentContent("media_notes", ref MediaXDocument);
        }

        public void UpdateXDocuments()
        {
            SetXDocumentContent("character_notes", ref CharacterXDocument);
            SetXDocumentContent("location_notes", ref LocationXDocument);
            SetXDocumentContent("event_notes", ref EventXDocument);
            SetXDocumentContent("general_notes", ref NoteXDocument);
        }

        public static void DetachFromAll(EntityWindow e, List<XDocumentInformation> attachments)
        {
            foreach (var x in attachments)
            {
                x.Detach(e);
            }
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

        public static void RemoveEntityFromEventEntities(string entityName)
        {
            //TODO throw new NotImplementedException();
        }

        public static void RemoveEntityFromRelationships(string entityName, XDocumentInformation XDoc)
        {
            //TODO throw new NotImplementedException();
        }

        //is this entity present in the XDocument
        public static bool IsEntityPresent(XDocument handle, string entityName)
        {
            return (from c in handle.Root.Elements()
                    where c.Element("name").Value.ToString().Equals(entityName)
                    select c).Count() > 0 ? true : false;
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
