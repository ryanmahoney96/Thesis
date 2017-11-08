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

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for NewCharacterRelationship.xaml
    /// </summary>
    public partial class NewCharacterRelationship : NewEntityWindow
    {

        //TODO: Add "Other" field for other types of relationships
        //TODO: Add the relationships on startup

        public NewCharacterRelationship(string fp, FrontPage fpr) : base(fp, fpr)
        {
            InitializeComponent();
            Fill_Character_Boxes();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO: make sure relationship does not already exist
            StreamWriter character_relationships_handle = null;

            if (!character_one_combo.Text.Equals("") && !relationship_combo.Text.Equals("") && !character_two_combo.Text.Equals(""))
            {
                try
                {
                    string character_one = character_one_combo.Text;
                    string character_two = character_two_combo.Text;
                    string relationship = relationship_combo.Text;
                    string filePath = folderPath + "\\character_relationships.xml";

                    if (File.Exists(filePath))
                    {
                        character_relationships_handle = XMLEntityParser.RemoveLastLine(filePath);
                    }
                    else
                    {
                        character_relationships_handle = new StreamWriter(filePath, true);
                        character_relationships_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<character_relationships>\n\t");
                    }

                    character_relationships_handle.Write("<character_relationship>\n\t\t");
                    character_relationships_handle.Write("<char_one>" + character_one + "</char_one>\n\t\t");
                    character_relationships_handle.Write("<relationship>" + relationship + "</relationship>\n\t");
                    character_relationships_handle.Write("<char_two>" + character_two + "</char_two>\n\t\t");
                    character_relationships_handle.Write("</character_relationship>\n\n");

                    character_relationships_handle.Write("</character_relationships>");

                    character_relationships_handle.Close();

                    //TODO make more permanent by making it similar to the add_record 
                    Add_Relationship(character_one, relationship, character_two);

                    Close();
                }
                catch (IOException)
                {
                    System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
                }
                finally
                {
                    if (!character_relationships_handle.Equals(null))
                    {
                        character_relationships_handle.Close();
                    }

                    Close();
                }
            }
            else
            {
                required_text.Foreground = Brushes.Red;
            }
        }

        private void Fill_Character_Boxes()
        {
            character_one_combo.Items.Clear();
            character_two_combo.Items.Clear();

            List<string> characterNames = XMLEntityParser.GetAllNames(base.folderPath + "\\character_notes.xml", "character");

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

        private void Add_Relationship(string character_one, string relationship, string character_two)
        {
            var c1ref = from c in Character.GetRecords() where c.GetName().Equals(character_one) select c;
            var c2ref = from c in Character.GetRecords() where c.GetName().Equals(character_two) select c;

            //TODO if non-familial relationships are added, be conscious of this function
            if (relationship.Contains("Parent"))
            {
                //character one is the parent
                foreach(Character c in c1ref)
                {
                    foreach(Character t in c2ref)
                    {
                        c.AddChild(t);
                    }
                }
            }
            else if (relationship.Contains("Child"))
            {
                //character two is the parent
                foreach (Character c in c1ref)
                {
                    foreach (Character t in c2ref)
                    {
                        t.AddChild(c);
                    }
                }
            }
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewCharacterWindow(folderPath, frontPageReference, this));
        }

        public override void UpdateReliantWindows()
        {
            Fill_Character_Boxes();
        }
    }
}
