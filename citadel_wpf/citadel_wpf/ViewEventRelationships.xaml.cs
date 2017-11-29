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
    public partial class ViewEventRelationships : EntityWindow
    {
        
        public ViewEventRelationships() : base()
        {
            InitializeComponent();
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref focus_event_combo);
            FillPanelWithRelationships(relationship_stackpanel);

            AttachToXDocument(ref XMLParser.EventXDocument);
            AttachToXDocument(ref XMLParser.EventRelationshipXDocument);
        }

        private void FocusEventChanged(object sender, SelectionChangedEventArgs e)
        {
            //XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_two_combo, event_one_combo.Text);
            //Refill stackpanel
            FillPanelWithRelationships(relationship_stackpanel);
        }

        private void FillPanelWithRelationships(StackPanel stackpanel)
        {
            if (focus_event_combo.SelectedItem != null && !string.IsNullOrWhiteSpace(focus_event_combo.SelectedItem.ToString()))
            {
                string fc = focus_event_combo.SelectedItem.ToString().Split(':')[1].Substring(1);

                var results = from c in XMLParser.EventRelationshipXDocument.Handle.Root.Descendants("event_relationship")
                              where c.Element("entity_one").Value.ToString().Equals(fc)
                              select new
                              {
                                  Relationship = c.Element("relationship").Value.ToString(),
                                  Entity_Two = c.Element("entity_two").Value.ToString(),
                              };

                stackpanel.Children.Clear();

                foreach (var r in results)
                {

                    WrapPanel panel = new WrapPanel();
                    TextBlock textblock = new TextBlock();
                    NodeInformation n = new NodeInformation(fc, r.Relationship, r.Entity_Two);
                    textblock.Text = n.ToString();
                    textblock.Margin = new Thickness(3);
                    Button deleteButton = new Button();
                    deleteButton.Tag = n;
                    deleteButton.Click += DeleteButton_Click;
                    deleteButton.Content = "Delete";
                    deleteButton.Margin = new Thickness(3);
                    deleteButton.Width = 60;
                    panel.Children.Add(textblock);
                    panel.Children.Add(deleteButton);
                    stackpanel.Children.Add(panel);
                    stackpanel.Children.Add(new Separator());
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this relationship?", "Delete Relationship", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NodeInformation n = (NodeInformation)((Button)sender).Tag;

                var relationship = from c in XMLParser.EventRelationshipXDocument.Handle.Root.Elements()
                                   where c.Element("entity_one").Value.Equals(n.EntityOne)
                                   && c.Element("relationship").Value.Equals(n.Relationship)
                                   && c.Element("entity_two").Value.Equals(n.EntityTwo)
                                   select c;

                foreach (var r in relationship)
                {
                    r.Remove();
                }


                XMLParser.EventRelationshipXDocument.Save();
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new NewEventWindow());
        }

        override public void Update(XDocumentInformation x = null)
        {
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref focus_event_combo);
            focus_event_combo.Text = "";
            relationship_stackpanel.Children.Clear();
            //FillPanelWithRelationships(relationship_stackpanel);
        }

        private void AddRelationship_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(focus_event_combo.Text))
            {
                EntityWindow.InitializeModalWindow(this, new EventRelationshipPrompt(focus_event_combo.Text));
            }
        }
    }
}
