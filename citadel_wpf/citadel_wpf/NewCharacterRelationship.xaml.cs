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
    public partial class NewCharacterRelationship : Window
    {
        string folderPath;

        public NewCharacterRelationship(string fp)
        {
            InitializeComponent();
            folderPath = fp;
        }

        private void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Character(object sender, RoutedEventArgs e)
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
                    character_relationships_handle.Write("<char_two>" + character_two + "</char_two>\n\t\t");
                    character_relationships_handle.Write("<relationship>" + relationship + "</relationship>\n\t");
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
                //required_text.Foreground = Brushes.Red;
            }
        }
    }
}
