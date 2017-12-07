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
        //TODO only need one. inc or dec
       
        public const string ComesBefore = "Comes Before";
        public const string ComesAfter = "Comes After";
        public const string SameTime = "Occurs at the Same Time as";

        public string[] Relationships = { ComesBefore, ComesAfter, SameTime };

        string FocusEvent;

        public EventRelationshipPrompt(string fe) : base()
        {
            InitializeComponent();

            FocusEvent = fe;
            beforeText.Text += " '" + FocusEvent + "'";
            afterText.Text += " '" + FocusEvent + "'";

            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_one_combo, FocusEvent);
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_two_combo, FocusEvent);

            AttachToXDocument(ref XMLParser.EventXDocument);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            string eventOne = event_one_combo.Text;
            string eventTwo = event_two_combo.Text;

            if (!XMLParser.IsTextValid(event_one_combo.Text) || !XMLParser.IsTextValid(event_two_combo.Text))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                string eventOneOrderKey = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                          where c.Element("name").Value.Equals(event_one_combo.Text)
                                          select c.Element("order_key").Value).First();

                string eventTwoOrderKey = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                           where c.Element("name").Value.Equals(event_one_combo.Text)
                                           select c.Element("order_key").Value).First();
                //TODO necessary?
                if (eventOneOrderKey.CompareTo(eventTwoOrderKey) > 0)
                {
                    //invalid order
                }
                else
                {
                    XElement focusEventReference = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                                    where c.Element("name").Value.Equals(FocusEvent)
                                                    select c).First();

                    //TODO evaluate the 'middle' of the two keys
                    focusEventReference.Element("order_key").Value = "";

                    XMLParser.EventXDocument.Save();
                }

            }
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddEvent());
        }

        override public void Update(XDocumentInformation x = null)
        {
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_one_combo, FocusEvent);
            XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref event_two_combo, FocusEvent);
        }

    }
}
