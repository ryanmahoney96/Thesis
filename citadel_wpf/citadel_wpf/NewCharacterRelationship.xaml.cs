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
            Fill_Character_Boxes();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO parent/child split
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
                if (IsRelationshipPresent(XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument(), character_one_combo.Text, relationship, character_two_combo.Text)
                || IsRelationshipPresent(XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument(), character_two_combo.Text, opposite, character_one_combo.Text))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", character_one_combo.Text),
                    new XElement("relationship", relationship),
                    new XElement("entity_two", character_two_combo.Text));

                    XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);

                    newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", character_two_combo.Text),
                    new XElement("relationship", opposite),
                    new XElement("entity_two", character_one_combo.Text));

                    XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);


                    XMLEntityParser.GetInstance().GetCharacterRelationshipXDocument().Save(FrontPage.FolderPath + "\\character_relationship_notes.xml");

                    UpdateReliantWindows();
                    Close();
                }
            }
        }

        //TODO move to XMLEntityParser
        public static bool IsRelationshipPresent(XDocument handle, string entityOne, string relationship, string entityTwo)
        {
            return ((from c in handle.Root.Elements()
                     where c.Element("entity_one").Value.ToString().Equals(entityOne)
                     && c.Element("relationship").Value.ToString().Equals(relationship)
                     && c.Element("entity_two").Value.ToString().Equals(entityTwo)
                     select c).Count() > 0 ? true : false);
        }

        private void Fill_Character_Boxes()
        {
            character_one_combo.Items.Clear();
            character_two_combo.Items.Clear();

            List<string> characterNames = XMLEntityParser.GetAllNames(XMLEntityParser.GetInstance().GetCharacterXDocument());

            ComboBoxItem cBoxItem;

            foreach (string character in characterNames)
            {
                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = character;
                character_one_combo.Items.Add(cBoxItem);

                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = character;
                character_two_combo.Items.Add(cBoxItem);
            }
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewCharacterWindow(this));
        }

        public override void UpdateReliantWindows()
        {
            Fill_Character_Boxes();
        }
    }
}
