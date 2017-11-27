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
    /// <summary>
    /// Interaction logic for NewEventRelationship.xaml
    /// </summary>
    public partial class NewEventRelationship : EntityWindow
    {

        public NewEventRelationship() : base()
        {
            InitializeComponent();
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_one_combo);
        }

        private void event_one_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_two_combo, event_one_combo.Text);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO this query is marked for change when relationship is redone
            string relationship = relationship_combo.Text;
            string opposite = "Comes Before";
            if (relationship.Equals("Comes Before"))
            {
                opposite = "Comes After";
            }

            if (!XMLParser.IsTextValid(event_one_combo.Text) || !XMLParser.IsTextValid(relationship_combo.Text) || !XMLParser.IsTextValid(event_two_combo.Text))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                if (XMLParser.IsRelationshipPresent(XMLParser.EventRelationshipXDocument.Handle, event_one_combo.Text, relationship, event_two_combo.Text)
                || XMLParser.IsRelationshipPresent(XMLParser.EventRelationshipXDocument.Handle, event_two_combo.Text, opposite, event_one_combo.Text))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement newEventRelationship = new XElement("event_relationship",
                    new XElement("entity_one", event_one_combo.Text),
                    new XElement("relationship", relationship),
                    new XElement("entity_two", event_two_combo.Text));

                    XMLParser.EventRelationshipXDocument.Handle.Root.Add(newEventRelationship);

                    newEventRelationship = new XElement("event_relationship",
                    new XElement("entity_one", event_two_combo.Text),
                    new XElement("relationship", opposite),
                    new XElement("entity_two", event_one_combo.Text));

                    XMLParser.EventRelationshipXDocument.Handle.Root.Add(newEventRelationship);

                    XMLParser.EventRelationshipXDocument.Save();

                    UpdateReliantWindows();
                    Close();
                }
            }
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new NewEventWindow(this));      
        }

        override public void UpdateReliantWindows()
        {
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_one_combo);
            event_two_combo.Items.Clear();
        }

    }
}
