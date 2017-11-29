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
    public partial class AddLocation : EntityWindow, INewEntity
    {
        private bool Editing = false;

        public AddLocation() : base()
        {
            InitializeComponent();
        }

        public void FillWith(string locationName)
        {
            Editing = true;

            var location = (from c in XMLParser.LocationXDocument.Handle.Root.Descendants("location")
                             where c.Element("name").Value.Equals(locationName)
                             select new
                             {
                                 Name = c.Element("name").Value,
                                 Type = c.Element("type").Value,
                                 Subtype = c.Element("subtype").Value,
                                 Description = c.Element("description").Value
                             }).First();

            name_text.Text = location.Name;
            name_text.IsEnabled = false;

            type_combobox.Text = location.Type;
            subtype_combobox.Text = location.Subtype;
            description_text.Text = location.Description;

        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            if (XMLParser.IsEntityPresent(XMLParser.LocationXDocument.Handle, name_text.Text) && !Editing)
            {
                System.Windows.Forms.MessageBox.Show("This location already exists, please try again.");
            }
            else
            {
                if (!XMLParser.IsTextValid(name_text.Text) || !XMLParser.IsTextValid(type_combobox.Text))
                {
                    required_text.Foreground = Brushes.Red;
                }
                else
                {
                    if (Editing)
                    {
                        XElement locationReference = (from c in XMLParser.LocationXDocument.Handle.Root.Descendants("location")
                                                       where c.Element("name").Value.Equals(name_text.Text)
                                                       select c).First();

                        locationReference.Element("type").Value = type_combobox.Text;
                        locationReference.Element("subtype").Value = subtype_combobox.Text;
                        locationReference.Element("description").Value = description_text.Text;
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
                    }

                    XMLParser.LocationXDocument.Save();

                    Update();
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
                subtypes.Add("Neighborhood \\ Hamlet");
                subtypes.Add("Village \\ Town");
                subtypes.Add("City \\ County");
                subtypes.Add("Country \\ Region");
                subtypes.Add("Other");
            }
            else if (selectionString.Equals("Nature"))
            {
                subtypes.Add("Mountain \\ Range");
                subtypes.Add("Lake");
                subtypes.Add("River");
                subtypes.Add("Ocean");
                subtypes.Add("Field \\ Grassland");
                subtypes.Add("Forest \\ Jungle");
                subtypes.Add("Desert");
                subtypes.Add("Continent");
                subtypes.Add("Planet \\ Interplanetary Body");
                subtypes.Add("Other");
            }
            else if (selectionString.Equals("Buildings \\ Monuments \\ Establishments"))
            {
                subtypes.Add("House \\ Home");
                subtypes.Add("Inn \\ Hotel");
                subtypes.Add("Store Front \\ Shop");
                subtypes.Add("Tower \\ Skyscraper");
                subtypes.Add("Statue \\ Obelisk");     
                subtypes.Add("Temple \\ Monastary \\ Religious Building");
                subtypes.Add("College \\ University \\ School");
                subtypes.Add("Fort \\ Castle");
                subtypes.Add("Dungeon \\ Grave \\ Crypt");
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

        override public void Update(XDocumentInformation x = null)
        {

        }
    }
}
