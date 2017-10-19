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
    /// Interaction logic for NewLocationWindow.xaml
    /// </summary>
    public partial class NewLocationWindow : Window
    {
        string folderPath;

        public NewLocationWindow(string fp)
        {
            InitializeComponent();
            folderPath = fp;
        }

        private void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Location(object sender, RoutedEventArgs e)
        {
            //TODO: make sure location does not already exist
            StreamWriter location_notes_handle = null;

            if (!name_text.Text.Equals("") && !type_combobox.Text.Equals(""))
            {
                try
                {
                    string name = name_text.Text;
                    string type = type_combobox.Text;
                    string subtype = subtype_combobox.Text;
                    string physdesc_notes = physdesc_notes_text.Text;
                    string filePath = folderPath + "\\location_notes.xml";

                    if (File.Exists(filePath))
                    {
                        location_notes_handle = XMLParserClass.RemoveLastLine(filePath);
                    }
                    else
                    {
                        location_notes_handle = new StreamWriter(filePath, true);
                        location_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<locations>\n\t");
                    }

                    location_notes_handle.Write("<location>\n\t\t");
                    location_notes_handle.Write("<name>" + name + "</name>\n\t\t");
                    location_notes_handle.Write("<type>" + type + "</type>\n\t\t");
                    location_notes_handle.Write("<subtype>" + subtype + "</subtype>\n\t\t");
                    location_notes_handle.Write("<physdesc_and_notes>" + physdesc_notes + "</physdesc_and_notes>\n\t");
                    location_notes_handle.Write("</location>\n\n");

                    location_notes_handle.Write("</locations>");

                    location_notes_handle.Close();
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
                    if (!location_notes_handle.Equals(null))
                    {
                        location_notes_handle.Close();
                    }

                    Close();
                }
            }
            else
            {
                required_text.Foreground = Brushes.Red;
            }
        }

        private void updateSubtypes()
        {
            subtype_combobox.IsEnabled = true;

            subtype_combobox.Items.Clear();

            List<string> subtypes = new List<string>();

            string selectionString = type_combobox.SelectedItem.ToString().Split(':')[1].Substring(1);

            if (selectionString.Equals("Settlements"))
            {
                subtypes.Add("Village/Town");
                subtypes.Add("City/County");
                subtypes.Add("Country/Region");
            }
            else if (selectionString.Equals("Nature"))
            {
                subtypes.Add("Mountain/Range");
                subtypes.Add("Lake");
                subtypes.Add("River");
                subtypes.Add("Ocean");
                subtypes.Add("Forest/Jungle");
                subtypes.Add("Desert");
                subtypes.Add("Continent");
            }
            else if (selectionString.Equals("Buildings/Monuments"))
            {
                subtypes.Add("Statue/Obelisk");     
                subtypes.Add("Temple/Monastary");
                subtypes.Add("Fort/Castle");
                subtypes.Add("Dungeon/Grave");
            }
            else if (selectionString.Equals("Unknown"))
            {
                subtype_combobox.IsEnabled = false;
            }

            foreach (string s in subtypes)
            {
                ComboBoxItem cBoxItem = new ComboBoxItem();
                cBoxItem.Content = s;
                subtype_combobox.Items.Add(cBoxItem);
            }
        }

        private void type_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateSubtypes();
        }
    }
}
