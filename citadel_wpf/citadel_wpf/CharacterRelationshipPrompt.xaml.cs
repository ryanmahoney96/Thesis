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
    public partial class CharacterRelationshipPrompt : EntityWindow
    {
        public const string IsChildOf = "Is the Child of";
        public const string IsParentOf = "Is the Parent of";
        public const string Married = "Married";

        public string[] Relationships = {IsChildOf, IsParentOf, Married};

        string FocusCharacter;

        public CharacterRelationshipPrompt(string fc, params EntityWindow[] rw) : base(rw)
        {
            InitializeComponent();

            FocusCharacter = fc;
            focus_character.Text = FocusCharacter;

            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_two_combo, FocusCharacter);
            FillComboboxWithRelationshipTypes();
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
            //TODO is married to
            //TODO this query is marked for change when relationship is redone
            string relationship = relationship_combo.Text;
            string opposite = IsChildOf;
            if (relationship.Equals(IsChildOf))
            {
                opposite = IsParentOf;
            }
            else if (relationship.Equals(Married))
            {
                opposite = Married;
            }

            if (!XMLParser.IsTextValid(relationship_combo.Text) || !XMLParser.IsTextValid(character_two_combo.Text))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                if (XMLParser.IsRelationshipPresent(XMLParser.CharacterRelationshipXDocument.Handle, FocusCharacter, relationship, character_two_combo.Text)
                || XMLParser.IsRelationshipPresent(XMLParser.CharacterRelationshipXDocument.Handle, character_two_combo.Text, opposite, FocusCharacter))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", FocusCharacter),
                    new XElement("relationship", relationship),
                    new XElement("entity_two", character_two_combo.Text));

                    XMLParser.CharacterRelationshipXDocument.Handle.Root.Add(newCharacterRelationship);

                    newCharacterRelationship = new XElement("character_relationship",
                    new XElement("entity_one", character_two_combo.Text),
                    new XElement("relationship", opposite),
                    new XElement("entity_two", FocusCharacter));

                    XMLParser.CharacterRelationshipXDocument.Handle.Root.Add(newCharacterRelationship);

                    XMLParser.CharacterRelationshipXDocument.Save();

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
            EntityWindow.InitializeModalWindow(this, new NewCharacterWindow(this));
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_two_combo, FocusCharacter);
        }

        override public void UpdateReliantWindows()
        {

            foreach (EntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }

    }
}
