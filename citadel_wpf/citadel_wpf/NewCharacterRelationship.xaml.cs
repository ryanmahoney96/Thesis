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
    /// TODO: return to developing this
    public partial class NewCharacterRelationship : NewEntityWindow
    {

        public NewCharacterRelationship(string fp, FrontPage fpr, params NewEntityWindow[] rw) : base(fp, fpr, rw)
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
                        character_relationships_handle = XMLParserClass.RemoveLastLine(filePath);
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
                //TODO: required text
                //required_text.Foreground = Brushes.Red;
            }
        }

        private void Fill_Character_Boxes()
        {
            character_one_combo.Items.Clear();
            character_two_combo.Items.Clear();

            List<string> characterNames = XMLParserClass.GetAllNames(base.folderPath + "\\character_notes.xml", "character");

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
            NewCharacterWindow ncw = new NewCharacterWindow(folderPath, frontPageReference, this);
            ncw.Show();
            ncw.Topmost = true;
        }

        public override void UpdateReliantWindows()
        {
            Fill_Character_Boxes();
        }
    }
}
