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
    /// Interaction logic for NewEventRelationship.xaml
    /// </summary>
    public partial class ViewEventRelationships : EntityWindow
    {

        public ViewEventRelationships() : base()
        {
            InitializeComponent();
            //XMLParser.FillComboboxWithNames(XMLParser.EventXDocument.Handle, ref focus_event_combo);
            FillPanelWithRelationships(relationship_stackpanel);

            AttachToXDocument(ref XMLParser.EventXDocument);
        }

        private void FillPanelWithRelationships(StackPanel stackpanel)
        {
            var results = from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                          orderby int.Parse(c.Element("order_key").Value)
                          select new
                          {
                              Name = c.Element("name").Value.ToString(),
                              OrderKey = int.Parse(c.Element("order_key").Value)
                          };

            stackpanel.Children.Clear();
            int orderNumber = 1;

            foreach (var r in results)
            {

                WrapPanel panel = new WrapPanel();
                panel.HorizontalAlignment = HorizontalAlignment.Left;

                TextBlock textblock = new TextBlock();
                textblock.FontSize = 16;
                textblock.Text = (orderNumber++).ToString() + ". ";
                textblock.Text += r.Name;
                textblock.Margin = new Thickness(3);

                panel.Children.Add(textblock);
                stackpanel.Children.Add(panel);
                stackpanel.Children.Add(new Separator());
            }

        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddEvent());
        }

        override public void Update(XDocumentInformation x = null)
        {
            FillPanelWithRelationships(relationship_stackpanel);
        }

        public void AdjustOrdering_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new EventRelationshipPrompt());
        }

        private void TimelineButton_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new TimelinePromptWindow());
        }
    }
}
