using System;
using System.Collections.Generic;
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

    public partial class TimelinePromptWindow : EntityWindow
    {

        public TimelinePromptWindow() : base()
        {
            InitializeComponent();
            FillEventList();
        }

        private void FillEventList()
        {
            var allEvents = from e in XMLParser.EventXDocument.Handle.Root.Elements("event")
                            orderby e.Element("name").Value
                            select e.Element("name").Value;

            foreach(var e in allEvents)
            {
                DockPanel d = new DockPanel();

                CheckBox c = new CheckBox();
                DockPanel.SetDock(c, Dock.Left);
                d.Children.Add(c);

                TextBlock t = new TextBlock();
                t.Text = e;
                DockPanel.SetDock(t, Dock.Left);
                d.Children.Add(t);

                eventList.Children.Add(d);
            }

        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //if (XMLParser.IsTextValid(timelineType.Text) && XMLParser.IsTextValid(event_name_combo.Text))
            //{

            //    if (timelineType.Text.Equals("Short Term Timeline"))
            //    {
            //        //TODO EventMapConstruction.SingleLocation(eventName.Text);
            //    }
            //    else
            //    {
            //        //TODO EventMapConstruction.AllLocations();
            //    }
            //    Close();
            //}
            //else
            //{
            //    requiredText.Foreground = Brushes.Red;
            //}
        }

        override public void Update(XDocumentInformation x = null)
        {
            //
        }
    }
}
