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
    /// Interaction logic for NewLocationWindow.xaml
    /// </summary>
    public partial class NewLocationWindow : NewEntityWindow
    {
        public NewLocationWindow(params NewEntityWindow[] rw) : base(rw)
        {
            InitializeComponent();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            if (XMLEntityParser.IsPresent(XMLEntityParser.GetInstance().GetLocationXDocument(), name_text.Text))
            {
                System.Windows.Forms.MessageBox.Show("This location already exists, please try again.");
            }
            else
            {
                if (name_text.Text.Equals("") || type_combobox.Text.Equals(""))
                {
                    required_text.Foreground = Brushes.Red;
                }
                else
                {

                    XElement newLocation = new XElement("location",
                    new XElement("name", name_text.Text),
                    new XElement("type", type_combobox.Text),
                    new XElement("subtype", subtype_combobox.Text),
                    new XElement("description", description_text.Text));

                    string temp = newLocation.ToString();

                    XMLEntityParser.GetInstance().GetLocationXDocument().Root.Add(newLocation);
                    XMLEntityParser.GetInstance().GetLocationXDocument().Save(FrontPage.FolderPath + "\\location_notes.xml");

                    UpdateReliantWindows();
                    Close();
                }
            }

            //StreamWriter location_notes_handle = null;

            //if (!name_text.Text.Equals("") && !type_combobox.Text.Equals(""))
            //{
            //    try
            //    {
            //        string name = name_text.Text;
            //        string type = type_combobox.Text;
            //        string subtype = subtype_combobox.Text;
            //        string description = description_text.Text;
            //        string filePath = folderPath + "\\location_notes.xml";

            //        if (File.Exists(filePath))
            //        {
            //            location_notes_handle = XMLParserClass.RemoveLastLine(filePath);
            //        }
            //        else
            //        {
            //            location_notes_handle = new StreamWriter(filePath, true);
            //            location_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<locations>\n\t");
            //        }

            //        location_notes_handle.Write("<location>\n\t\t");
            //        location_notes_handle.Write("<name>" + name + "</name>\n\t\t");
            //        location_notes_handle.Write("<type>" + type + "</type>\n\t\t");
            //        location_notes_handle.Write("<subtype>" + subtype + "</subtype>\n\t\t");
            //        location_notes_handle.Write("<description>" + description + "</description>\n\t");
            //        location_notes_handle.Write("</location>\n\n");

            //        location_notes_handle.Write("</locations>");

            //        location_notes_handle.Close();

            //        UpdateReliantWindows();

            //        Close();
            //    }
            //    catch (IOException)
            //    {
            //        System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
            //    }
            //    catch (Exception)
            //    {
            //        System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
            //    }
            //    finally
            //    {
            //        if (!location_notes_handle.Equals(null))
            //        {
            //            location_notes_handle.Close();
            //        }

            //        Close();
            //    }
            //}
            //else
            //{
            //    required_text.Foreground = Brushes.Red;
            //}
        }

        private void updateSubtypes()
        {
            subtype_combobox.IsEnabled = true;

            subtype_combobox.Items.Clear();

            List<string> subtypes = new List<string>();

            string selectionString = type_combobox.SelectedItem.ToString().Split(':')[1].Substring(1);

            if (selectionString.Equals("Settlements"))
            {
                subtypes.Add("Neighborhood/Hamlet");
                subtypes.Add("Village/Town");
                subtypes.Add("City/County");
                subtypes.Add("Country/Region");
                subtypes.Add("Other");
            }
            else if (selectionString.Equals("Nature"))
            {
                subtypes.Add("Mountain/Range");
                subtypes.Add("Lake");
                subtypes.Add("River");
                subtypes.Add("Ocean");
                subtypes.Add("Field/Grassland");
                subtypes.Add("Forest/Jungle");
                subtypes.Add("Desert");
                subtypes.Add("Continent");
                subtypes.Add("Planet/Interplanetary Body");
                subtypes.Add("Other");
            }
            else if (selectionString.Equals("Buildings/Monuments/Establishments"))
            {
                subtypes.Add("House/Home");
                subtypes.Add("Inn/Hotel");
                subtypes.Add("Store Front/Shop");
                subtypes.Add("Tower/Skyscraper");
                subtypes.Add("Statue/Obelisk");     
                subtypes.Add("Temple/Monastary/Religious Building");
                subtypes.Add("College/University/School");
                subtypes.Add("Fort/Castle");
                subtypes.Add("Dungeon/Grave/Crypt");
                subtypes.Add("Other");
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

        public override void UpdateReliantWindows()
        {
            FrontPage.FrontPageReference.Update_Locations();

            foreach (NewEntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }
    }
}
