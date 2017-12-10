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
                           && e.Element("participants").HasElements
                           select e);

                IEnumerable<string> results2 = new List<string>();
                foreach(var r in results)
                {
                    results2 = (from c in r.Element("participants").Elements("character_name")
                                select c.Value);
                    break;
                }
                
                stackpanel.Children.Clear();

                foreach (var r in results2)
                {

                    DockPanel panel = new DockPanel();
                    TextBlock textblock = new TextBlock();
                    textblock.Text = r;
                    textblock.Margin = new Thickness(3);
                    DockPanel.SetDock(textblock, Dock.Left);

                    Button deleteButton = new Button();
                    deleteButton.Tag = r;
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
                string n = (string)((Button)sender).Tag;

                var results = (from ev in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                               where ev.Element("name").Value.Equals(focus_event_combo.Text)
                               && !ev.Element("participants").IsEmpty
                               select (from c in ev.Element("participants").Elements("character_name")
                                       where c.Value.Equals(n)
                                       select c)).First();

                foreach (var c in results)
                {
                    c.Remove();
                }

                XMLParser.EventXDocument.Save();
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
            string temp = focus_event_combo.Text;
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref focus_event_combo);
            focus_event_combo.Text = temp;
            relationship_stackpanel.Children.Clear();
            FillPanelWithRelationships(relationship_stackpanel);
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
