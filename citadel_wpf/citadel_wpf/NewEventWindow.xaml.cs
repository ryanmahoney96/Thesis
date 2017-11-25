
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

    public partial class NewEventWindow : EntityWindow
    {

        public NewEventWindow(params EntityWindow[] rw) : base(rw)
        {
            InitializeComponent();
            Initialize_Locations();
        }

        private void Initialize_Locations()
        {

            location_combo_box.Items.Clear();

            XMLParser.FillComboboxWithNames(XMLParser.LocationXDocument.Handle, ref location_combo_box);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            if (XMLParser.IsEntityPresent(XMLParser.EventXDocument.Handle, name_text.Text))
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
                    XElement newEvent = new XElement("event",
                    new XElement("name", name_text.Text),
                    new XElement("location", location_combo_box.Text),
                    new XElement("unit_date", event_unit_date_number.Text),
                    new XElement("date", event_date_number.Text),
                    new XElement("description", description_text.Text));

                    string temp = newEvent.ToString();

                    XMLParser.EventXDocument.Handle.Root.Add(newEvent);
                    XMLParser.EventXDocument.Save();

                    UpdateReliantWindows();
                    Close();
                }
            }
            
        }

        override public void UpdateReliantWindows()
        {
            FrontPage.FrontPageReference.Update_Events();
            Initialize_Locations();

            foreach (EntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }

        private void Add_New_Location(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new NewLocationWindow(this));
        }
    }
}

