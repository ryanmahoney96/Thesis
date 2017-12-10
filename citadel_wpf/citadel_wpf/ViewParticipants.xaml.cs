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
    public partial class ViewParticipants : EntityWindow
    {

        public ViewParticipants() : base()
        {
            InitializeComponent();
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref focus_event_combo);
            FillPanelWithRelationships(relationship_stackpanel);

            AttachToXDocument(ref XMLParser.EventXDocument);
            AttachToXDocument(ref XMLParser.ParticipantXDocument);
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
                string focusEvent = focus_event_combo.SelectedItem.ToString().Split(':')[1].Substring(1);

                var results = (from e in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                           where e.Element("name").Value.Equals(focusEvent)
                           && !e.Element("participants").IsEmpty
                           select (from c in e.Element("participants").Elements("character_name")
                                   select c.Value)).First();
                              
                stackpanel.Children.Clear();

                foreach (var r in results)
                {

                    DockPanel panel = new DockPanel();
                    TextBlock textblock = new TextBlock();
                    //TODO fix this for deletion
                    NodeInformation n = new NodeInformation(r, "", "");
                    //textblock.Text = n.ToString();
                    textblock.Text = r;
                    textblock.Margin = new Thickness(3);
                    DockPanel.SetDock(textblock, Dock.Left);

                    Button deleteButton = new Button();
                    deleteButton.Tag = n;
                    deleteButton.Click += DeleteButton_Click;
                    deleteButton.Content = "Delete";
                    deleteButton.Margin = new Thickness(3);
                    deleteButton.Width = 60;
                    DockPanel.SetDock(deleteButton, Dock.Right);

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

                var relationship = from c in XMLParser.ParticipantXDocument.Handle.Root.Elements()
                                   where c.Element("entity_one").Value.Equals(n.EntityOne)
                                   && c.Element("relationship").Value.Equals(n.Relationship)
                                   && c.Element("entity_two").Value.Equals(n.EntityTwo)
                                   select c;

                foreach (var r in relationship)
                {
                    r.Remove();
                }


                XMLParser.ParticipantXDocument.Save();
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddEvent());
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
                EntityWindow.InitializeModalWindow(this, new ParticipantPrompt(focus_event_combo.Text));
            }
        }

        private void Map_Event(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(focus_event_combo.Text))
            {
                ParticipantMapConstruction.CreateMap(focus_event_combo.Text);
            }
        }
    }
}
