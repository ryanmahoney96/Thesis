using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public class EventItem
        {
            public string Name { get; set; }
            public bool IsSelected { get; set; }
        }

        public ObservableCollection<EventItem> EventNames = new ObservableCollection<EventItem>();
        private List<string> events = (from e in XMLParser.EventXDocument.Handle.Root.Elements("event")
                                           orderby e.Element("name").Value
                                           select e.Element("name").Value).ToList();

        public TimelinePromptWindow() : base()
        {

            foreach(var e in events)
            {
                EventItem temp = new EventItem();
                temp.Name = e;
                temp.IsSelected = false;
                EventNames.Add(temp);
            }

            InitializeComponent();

            eventList.ItemsSource = EventNames;
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            var selecteds = (from s in EventNames
                            where s.IsSelected == true
                            select s.Name).ToList();

            TimelineConstruction.CreateTimeline(selecteds);

        }

        override public void Update(XDocumentInformation x = null)
        {
            //
        }
    }
}
