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
    /// Interaction logic for NewCharacterRelationship.xaml
    /// </summary>
    public partial class NewCharacterRelationship : NewEntityWindow
    {

        //TODO: Add an "ADD" button so that you don't have to keep going back and forth between windows for one character
        //TODO: If the above is done, make it so any individual relationship that already exists is skipped/marked instead
        //TODO: Add the relationships on startup

        public NewCharacterRelationship() : base()
        {
            InitializeComponent();
            XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref focus_character_combo);
            FillPanelWithRelationships(relationship_stackpanel);
        }

        private void focus_character_changed(object sender, SelectionChangedEventArgs e)
        {
            //XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref character_two_combo, character_one_combo.Text);
            //Refill stackpanel
            FillPanelWithRelationships(relationship_stackpanel);
        }

        private void FillPanelWithRelationships(StackPanel s)
        {
            if (focus_character_combo.SelectedItem != null && !string.IsNullOrWhiteSpace(focus_character_combo.SelectedItem.ToString()))
            {
                string fc = focus_character_combo.SelectedItem.ToString().Split(':')[1].Substring(1);
                var results = from c in XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Descendants("character_relationship")
                              where c.Element("entity_one").Value.ToString().Equals(fc)
                              select new
                              {
                                  Relationship = c.Element("relationship").Value.ToString(),
                                  Entity_Two = c.Element("entity_two").Value.ToString(),
                              };

                s.Children.Clear();

                foreach (var r in results)
                {
                    WrapPanel p = new WrapPanel();
                    TextBlock t = new TextBlock();
                    t.Text = fc + " " + r.Relationship + " " + r.Entity_Two;
                    t.Margin = new Thickness(3);
                    //TODO functionality
                    Button b = new Button();
                    b.Content = "Delete";
                    b.Margin = new Thickness(3);
                    b.Width = 60;
                    p.Children.Add(t);
                    p.Children.Add(b);
                    s.Children.Add(p);
                    s.Children.Add(new Separator());
                }
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO is married to
            //TODO was married to
            //string relationship = relationship_combo.Text;
            //string opposite = "Is the Child of";
            //if (relationship.Equals("Is the Child of"))
            //{
            //    opposite = "Is the Parent of";
            //}

            //if (character_one_combo.Text.Equals("") || relationship_combo.Text.Equals("") || character_two_combo.Text.Equals(""))
            //{
            //    required_text.Foreground = Brushes.Red;
            //}
            
            //else
            //{
            //    if (XMLParser.IsRelationshipPresent(XMLParser.GetInstance().GetCharacterRelationshipXDocument(), character_one_combo.Text, relationship, character_two_combo.Text)
            //    || XMLParser.IsRelationshipPresent(XMLParser.GetInstance().GetCharacterRelationshipXDocument(), character_two_combo.Text, opposite, character_one_combo.Text))
            //    {
            //        System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
            //    }
            //    else
            //    {
            //        XElement newCharacterRelationship = new XElement("character_relationship",
            //        new XElement("entity_one", character_one_combo.Text),
            //        new XElement("relationship", relationship),
            //        new XElement("entity_two", character_two_combo.Text));

            //        XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);

            //        newCharacterRelationship = new XElement("character_relationship",
            //        new XElement("entity_one", character_two_combo.Text),
            //        new XElement("relationship", opposite),
            //        new XElement("entity_two", character_one_combo.Text));

            //        XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);


            //        XMLParser.GetInstance().GetCharacterRelationshipXDocument().Save(FrontPage.FolderPath + "\\character_relationship_notes.xml");

            //        UpdateReliantWindows();
            //        Close();
            //    }
            //}
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewCharacterWindow(this));
        }

        public override void UpdateReliantWindows()
        {
            XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref focus_character_combo);
            //Get all relationships
        }

        private void add_relationship_button_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new RelationshipPrompt(focus_character_combo.Text, this));
        }
    }
}
