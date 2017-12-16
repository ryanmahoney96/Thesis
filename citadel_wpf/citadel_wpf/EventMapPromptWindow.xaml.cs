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

    public partial class EventMapPromptWindow : EntityWindow
    {

        public EventMapPromptWindow() : base()
        {
            InitializeComponent();
            XMLParser.FillComboboxWithNames(XMLParser.LocationXDocument.Handle, ref locationName);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            if (XMLParser.IsTextValid(eventMapType.Text) && XMLParser.IsTextValid(locationName.Text))
            {
                
                if (eventMapType.Text.Equals("Single Event Map"))
                {
                    EventMapConstruction.BasicMap(locationName.Text);
                }
                else
                {
                    EventMapConstruction.MapWithParticipants(locationName.Text);
                }
                Close();
            }
            else
            {
                requiredText.Foreground = Brushes.Red;
            }
        }
        
    }
}
