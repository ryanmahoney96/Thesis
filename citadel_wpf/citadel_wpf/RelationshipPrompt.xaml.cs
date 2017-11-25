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
    public partial class RelationshipPrompt : NewEntityWindow
    {
        string FocusCharacter;

        public RelationshipPrompt(string fc, params NewEntityWindow[] rw) : base(rw)
        {
            InitializeComponent();

            FocusCharacter = fc;
            focus_character.Text = FocusCharacter;

            XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref character_two_combo, FocusCharacter);
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

            if (relationship_combo.Text.Equals("") || character_two_combo.Text.Equals(""))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                if (XMLParser.IsRelationshipPresent(XMLParser.GetInstance().GetCharacterRelationshipXDocument(), FocusCharacter, relationship, character_two_combo.Text)
                || XMLParser.IsRelationshipPresent(XMLParser.GetInstance().GetCharacterRelationshipXDocument(), character_two_combo.Text, opposite, FocusCharacter))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", FocusCharacter),
                    new XElement("relationship", relationship),
                    new XElement("entity_two", character_two_combo.Text));

                    XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);

                    newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", character_two_combo.Text),
                    new XElement("relationship", opposite),
                    new XElement("entity_two", FocusCharacter));

                    XMLParser.GetInstance().GetCharacterRelationshipXDocument().Root.Add(newCharacterRelationship);


                    XMLParser.GetInstance().GetCharacterRelationshipXDocument().Save(FrontPage.FolderPath + "\\character_relationship_notes.xml");

                    UpdateReliantWindows();
                    if (MessageBox.Show($"Would you like to create another relationship for \"{FocusCharacter}?\"", "Create Another Relationship", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        Close();
                    }
                }
            }
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewCharacterWindow(this));
            XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetCharacterXDocument(), ref character_two_combo, FocusCharacter);
        }

        public override void UpdateReliantWindows()
        {

            foreach (NewEntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }

    }
}
