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
        public class EventItem : CheckBox
        {
            public string LabelName { get; set; }
            //public bool IsChecked { get; set; }
        }

        public ObservableCollection<EventItem> EventNames = new ObservableCollection<EventItem>();
        private List<string> events = (from e in XMLParser.EventXDocument.Handle.Root.Elements("event")
                                           orderby e.Element("name").Value
                                           select e.Element("name").Value).ToList();

        public TimelinePromptWindow() : base()
        {
            InitializeComponent();

            foreach (var e in events)
            {
                EventItem temp = new EventItem();
                temp.LabelName = e;
                temp.IsChecked = false;
                EventNames.Add(temp);
            }

            eventList.ItemsSource = EventNames;
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            List<string> selecteds = (from s in EventNames
                                      where s.IsChecked == true
                                      select s.LabelName).ToList();

            if (!XMLParser.IsTextValid(titleText.Text))
            {
                System.Windows.Forms.MessageBox.Show("The Title is Invalid");
            }
            else if (selecteds.Count < 2)
            {
                System.Windows.Forms.MessageBox.Show("You Must Select at Least Two Events for a Timeline");
            }
            else
            {
                TimelineConstruction.CreateTimeline(selecteds, PrepareText(titleText.Text));
            }

        }

        private void SelectAll(object sender, RoutedEventArgs e)
        {

            foreach (CheckBox c in eventList.Items)
            {
                c.IsChecked = true;
            }

        }
    }
}
