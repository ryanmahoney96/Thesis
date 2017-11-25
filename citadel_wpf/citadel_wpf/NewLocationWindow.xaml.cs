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
    public partial class NewLocationWindow : EntityWindow
    {
        public NewLocationWindow(params EntityWindow[] rw) : base(rw)
        {
            InitializeComponent();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            if (XMLParser.IsEntityPresent(XMLParser.LocationXDocument.Handle, name_text.Text))
            {
                System.Windows.Forms.MessageBox.Show("This location already exists, please try again.");
            }
            else
            {
                if (!XMLParser.IsTextValid(name_text.Text) || string.IsNullOrWhiteSpace(type_combobox.Text))
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

                    XMLParser.LocationXDocument.Handle.Root.Add(newLocation);
                    XMLParser.LocationXDocument.Save();

                    UpdateReliantWindows();
                    Close();
                }
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
            else if (selectionString.Equals("Unknown") || selectionString.Equals("Other"))
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

        override public void UpdateReliantWindows()
        {
            FrontPage.FrontPageReference.Update_Locations();

            foreach (EntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }
    }
}
