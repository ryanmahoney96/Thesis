using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public const string IsMarriedTo = "Is Married To";
        public const string WasMarriedTo = "Was Married To";

        public ObservableCollection<string> Relationships = new ObservableCollection<string>();

        string FocusCharacter;

        public CharacterRelationshipPrompt(string fc = "") : base()
        {
            Relationships.Add(IsChildOf);
            Relationships.Add(IsParentOf);
            Relationships.Add(IsMarriedTo);
            Relationships.Add(WasMarriedTo);

            InitializeComponent();

            relationship_combo.ItemsSource = Relationships;
            
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref focus_character_combo);
            focus_character_combo.Text = fc;
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_two_combo, fc);

            AttachToXDocument(ref XMLParser.CharacterXDocument);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            string ancestorTag = "children";
            string descendantTag = "child";
            string relationship = relationship_combo.Text;
            string oppositeAncestorTag = "parents";
            string oppositeDescendantTag = "parent";

            if (!XMLParser.IsTextValid(focus_character_combo.Text) || !XMLParser.IsTextValid(relationship_combo.Text) || !XMLParser.IsTextValid(character_two_combo.Text))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                if (relationship.Equals(IsParentOf))
                {
                    ancestorTag = "parents";
                    descendantTag = "parent";
                    oppositeAncestorTag = "children";
                    oppositeDescendantTag = "child";
                }
                else if (relationship.Equals(WasMarriedTo))
                {
                    ancestorTag = "marriages";
                    descendantTag = "wasmarriedto";
                    oppositeAncestorTag = ancestorTag;
                    oppositeDescendantTag = descendantTag;
                }
                else if (relationship.Equals(IsMarriedTo))
                {
                    ancestorTag = "marriages";
                    descendantTag = "ismarriedto";
                    oppositeAncestorTag = ancestorTag;
                    oppositeDescendantTag = descendantTag;
                }

                if (IsRelationshipPresent(FocusCharacter, oppositeAncestorTag, oppositeDescendantTag, character_two_combo.Text))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement characterOneReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Descendants("character")
                                                      where c.Element("name").Value.Equals(FocusCharacter)
                                                      select c).First();
                    XElement characterTwoReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Descendants("character")
                                                      where c.Element("name").Value.Equals(character_two_combo.Text)
                                                      select c).First();

                    XElement newRelationship = new XElement(oppositeDescendantTag, character_two_combo.Text);

                    characterOneReference.Element(oppositeAncestorTag).Add(newRelationship);

                    newRelationship = new XElement(descendantTag, FocusCharacter);

                    characterTwoReference.Element(ancestorTag).Add(newRelationship);

                    XMLParser.CharacterXDocument.Save();

                    if (MessageBox.Show($"Would you like to create another relationship?", "Create Another Relationship", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        Close();
                    }
                }
            }
        }

        private static bool IsRelationshipPresent(string entityOne, string relationshipAncestorTag, string relationshipDescendantTag, string entityTwo)
        {

            var focusElement = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                 where c.Element("name").Value.Equals(entityOne)
                                select c).First();
            var relationshipsOfType = from r in focusElement.Element(relationshipAncestorTag).Elements()
                                      where r.Value.Equals(entityTwo)
                                      select r;

            return relationshipsOfType.Count() > 0 ? true : false;
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddCharacter());
        }

        override public void Update(XDocumentInformation x = null)
        {
            string fc = focus_character_combo.Text;
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref focus_character_combo);
            focus_character_combo.Text = fc;
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_two_combo, fc);
            relationship_combo.Text = "";
        }

        private void focus_character_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(focus_character_combo.Text) && focus_character_combo.SelectedItem != null)
            {
                FocusCharacter = focus_character_combo.SelectedItem.ToString().Split(':')[1].Substring(1);
                XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_two_combo, FocusCharacter);
            }
        }
    }
}
