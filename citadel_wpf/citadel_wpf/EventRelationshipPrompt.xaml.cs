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
    public partial class EventRelationshipPrompt : EntityWindow
    {

        public EventRelationshipPrompt() : base()
        {
            InitializeComponent();

            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref focusCombo);

            AttachToXDocument(ref XMLParser.EventXDocument);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            string focusEvent = focusCombo.Text;
            string beforeEvent = event_one_combo.Text;
            string afterEvent = event_two_combo.Text;
            int eventOneOrderKey = 0;
            int eventTwoOrderKey = 0;

            XElement focusEventOrderKey = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                            where c.Element("name").Value.Equals(focusEvent)
                                            select c.Element("order_key")).First();

            bool beforeEventValid = XMLParser.IsTextValid(beforeEvent);
            bool afterEventValid = XMLParser.IsTextValid(afterEvent);
            bool SaveAndClose = true;

            if (beforeEventValid)
            {
                eventOneOrderKey = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                    where c.Element("name").Value.Equals(beforeEvent)
                                    select int.Parse(c.Element("order_key").Value)).First();
            }
            if (afterEventValid)
            {
                eventTwoOrderKey = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                    where c.Element("name").Value.Equals(afterEvent)
                                    select int.Parse(c.Element("order_key").Value)).First();
            }

            if (beforeEventValid || afterEventValid)
            {
                if (beforeEventValid && !afterEventValid)
                {
                    if (int.Parse(focusEventOrderKey.Value) <= eventOneOrderKey)
                    {
                        focusEventOrderKey.Value = EventOrdering.GetKeyAfter(eventOneOrderKey).ToString();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("The event already lies in the order specified");
                    }
                }
                else if (!beforeEventValid && afterEventValid)
                {
                    if (int.Parse(focusEventOrderKey.Value) >= eventTwoOrderKey)
                    {
                        focusEventOrderKey.Value = EventOrdering.GetKeyBefore(eventTwoOrderKey).ToString();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("The event already lies in the order specified");
                    }
                }

                else if (beforeEventValid && afterEventValid)
                {
                    if (eventOneOrderKey > eventTwoOrderKey)
                    {
                        //invalid order
                        System.Windows.Forms.MessageBox.Show("The first event must come before the second event");
                        SaveAndClose = false;
                    }
                    else if (eventOneOrderKey == eventTwoOrderKey)
                    {
                        focusEventOrderKey.Value = eventOneOrderKey.ToString();
                    }
                    else
                    {
                        focusEventOrderKey.Value = EventOrdering.GetKeyBetween(eventOneOrderKey, eventTwoOrderKey).ToString();
                    }
                }
                if (SaveAndClose)
                {
                    XMLParser.EventXDocument.Save();
                    Close();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one relationship.");
            }
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddEvent());
        }

        override public void Update(XDocumentInformation x = null)
        {
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref focusCombo);
            event_one_combo.Items.Clear();
            event_two_combo.Items.Clear();
        }

        private void focusCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (focusCombo.SelectedItem != null && XMLParser.IsTextValid(focusCombo.SelectedItem.ToString()))
            {
                string fcs = focusCombo.SelectedItem.ToString().Split(':')[1].Substring(1);

                XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_one_combo, fcs);
                XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_two_combo, fcs);

            }
        }
    }
}
