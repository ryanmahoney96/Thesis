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

        //TODO: Add "Other" field for other types of relationships
        //TODO: Add the relationships on startup

        public NewCharacterRelationship() : base()
        {
            InitializeComponent();
            XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref character_one_combo);
        }

        private void character_one_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref character_two_combo, character_one_combo.Text);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO is married to
            //TODO was married to
            string relationship = relationship_combo.Text;
            string opposite = "Is the Child of";
            if (relationship.Equals("Is the Child of"))
            {
                opposite = "Is the Parent of";
            }

            if (character_one_combo.Text.Equals("") || relationship_combo.Text.Equals("") || character_two_combo.Text.Equals(""))
            {
                required_text.Foreground = Brushes.Red;
            }
            
            else
            {
                if (XMLParser.IsRelationshipPresent(XMLParser.GetInstance().GetCharacterRelationshipXDocument(), character_one_combo.Text, relationship, character_two_combo.Text)
                || XMLParser.IsRelationshipPresent(XMLParser.GetInstance().GetCharacterRelationshipXDocument(), character_two_combo.Text, opposite, character_one_combo.Text))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", character_one_combo.Text),
                    new XElement("relationship", relationship),
                    new XElement("entity_two", character_two_combo.Text));

                    XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);

                    newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", character_two_combo.Text),
                    new XElement("relationship", opposite),
                    new XElement("entity_two", character_one_combo.Text));

                    XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);


                    XMLParser.GetInstance().GetCharacterRelationshipXDocument().Save(FrontPage.FolderPath + "\\character_relationship_notes.xml");

                    UpdateReliantWindows();
                    Close();
                }
            }
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewCharacterWindow(this));
        }

        public override void UpdateReliantWindows()
        {
            XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref character_one_combo);
            character_two_combo.Items.Clear();
        }

    }
}
