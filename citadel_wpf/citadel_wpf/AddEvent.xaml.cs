
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

    public partial class AddEvent : EntityWindow, INewEntity
    {

        private bool Editing = false;

        public AddEvent() : base()
        {
            InitializeComponent();
            Initialize_Locations();

            AttachToXDocument(ref XMLParser.LocationXDocument);
        }

        private void Initialize_Locations()
        {
            //Get all location names
            location_combo_box.Items.Clear();

            XMLParser.FillComboboxWithNames(XMLParser.LocationXDocument.Handle, ref location_combo_box);
        }

        public void FillWith(string eventName)
        {
            Editing = true;

            var eventRef = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                             where c.Element("name").Value.Equals(eventName)
                             select new
                             {
                                 Name = c.Element("name").Value,
                                 Location = c.Element("location").Value,
                                 UnitDate = c.Element("unit_date").Value,
                                 Date = c.Element("date").Value,
                                 Description = c.Element("description").Value
                             }).First();

            name_text.Text = eventRef.Name;
            name_text.IsEnabled = false;

            location_combo_box.Text = eventRef.Location;
            event_unit_date_number.Text = eventRef.UnitDate;
            event_date_number.Text = eventRef.Date;
            description_text.Text = eventRef.Description;

        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            if (XMLParser.IsEntityPresent(XMLParser.EventXDocument.Handle, name_text.Text) && !Editing)
            {
                System.Windows.Forms.MessageBox.Show("This event already exists, please try again.");
            }
            else
            {
                if (!XMLParser.IsTextValid(name_text.Text) || !XMLParser.IsTextValid(description_text.Text))
                {
                    required_text.Foreground = Brushes.Red;
                }
                else
                {
                    if (Editing)
                    {
                        XElement eventReference = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                                   where c.Element("name").Value.Equals(name_text.Text)
                                                   select c).First();

                        eventReference.Element("location").Value = PrepareText(location_combo_box.Text);
                        eventReference.Element("unit_date").Value = PrepareText(event_unit_date_number.Text);
                        eventReference.Element("date").Value = PrepareText(event_date_number.Text);
                        eventReference.Element("description").Value = PrepareText(description_text.Text);
                    }
                    else
                    {

                        XElement newEvent = new XElement("event",
                        new XElement("name", PrepareText(name_text.Text)),
                        new XElement("location", PrepareText(location_combo_box.Text)),
                        new XElement("unit_date", PrepareText(event_unit_date_number.Text)),
                        new XElement("date", PrepareText(event_date_number.Text)),
                        new XElement("description", PrepareText(description_text.Text)),
                        new XElement("participants"),
                        new XElement("order_key", EventOrdering.GetNewOrderKey()));

                        string temp = newEvent.ToString();

                        XMLParser.EventXDocument.Handle.Root.Add(newEvent);
                    }

                    XMLParser.EventXDocument.Save();

                    Update();
                    Close();
                }
            }
            
        }

        override public void Update(XDocumentInformation x = null)
        {
            Initialize_Locations();
        }

        private void Add_New_Location(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddLocation());
        }
    }
}

