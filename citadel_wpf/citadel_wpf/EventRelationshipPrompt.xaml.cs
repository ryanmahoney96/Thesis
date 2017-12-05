using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace citadel_wpf
{
    public partial class EventRelationshipPrompt : EntityWindow
    {
        //TODO: each event relationship is given an index. Index = before index++ on 2^62 char table
       
        //This class COULD be adjusted using the Strategy Pattern
        public const string ComesBefore = "Comes Before";
        public const string ComesAfter = "Comes After";
        public const string SameTime = "Occurs at the Same Time as";

        public string[] Relationships = { ComesBefore, ComesAfter, SameTime };

        string FocusEvent;

        public EventRelationshipPrompt(string fe) : base()
        {
            InitializeComponent();

            FocusEvent = fe;
            focus_event.Text = FocusEvent;

            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_two_combo, FocusEvent);
            FillComboboxWithRelationshipTypes();

            AttachToXDocument(ref XMLParser.EventXDocument);
        }

        private void FillComboboxWithRelationshipTypes()
        {
            relationship_combo.Items.Clear();

            ComboBoxItem cBoxItem;

            foreach (string r in Relationships)
            {
                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = r;
                relationship_combo.Items.Add(cBoxItem);
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            string relationship = relationship_combo.Text;
            string opposite = ComesBefore;
            if (relationship.Equals(ComesBefore))
            {
                opposite = ComesAfter;
            }
            else if (relationship.Equals(SameTime))
            {
                opposite = SameTime;
            }

            if (!XMLParser.IsTextValid(relationship_combo.Text) || !XMLParser.IsTextValid(event_two_combo.Text))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                if (XMLParser.IsRelationshipPresent(XMLParser.EventRelationshipXDocument.Handle, FocusEvent, relationship, event_two_combo.Text)
                || XMLParser.IsRelationshipPresent(XMLParser.EventRelationshipXDocument.Handle, event_two_combo.Text, opposite, FocusEvent))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement newEventRelationship = new XElement("event_relationship",
                    new XElement("entity_one", FocusEvent),
                    new XElement("relationship", relationship),
                    new XElement("entity_two", event_two_combo.Text));

                    XMLParser.EventRelationshipXDocument.Handle.Root.Add(newEventRelationship);

                    newEventRelationship = new XElement("event_relationship",
                    new XElement("entity_one", event_two_combo.Text),
                    new XElement("relationship", opposite),
                    new XElement("entity_two", FocusEvent));

                    XMLParser.EventRelationshipXDocument.Handle.Root.Add(newEventRelationship);

                    XMLParser.EventRelationshipXDocument.Save();

                    if (MessageBox.Show($"Would you like to create another relationship for \"{FocusEvent}?\"", "Create Another Relationship", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        Close();
                    }
                }
            }
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddEvent());
        }

        override public void Update(XDocumentInformation x = null)
        {
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_two_combo, FocusEvent);

        }

    }
}
