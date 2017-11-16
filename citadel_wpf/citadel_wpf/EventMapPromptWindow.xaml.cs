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
    /// <summary>
    /// Interaction logic for CharacterPromptWindow.xaml
    /// </summary>
    public partial class LocationPromptWindow : Window
    {
        //TODO Tooltips
        public LocationPromptWindow()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(eventMapType.Text) && (locationName.IsEnabled && !String.IsNullOrWhiteSpace(locationName.Text)
                ||
                !String.IsNullOrWhiteSpace(eventMapType.Text) && locationName.IsEnabled == false))
            {
                
                if (eventMapType.Text.Equals("Single Event Map"))
                {
                    EventMapConstruction.SingleLocation(locationName.Text);
                }
                else
                {
                    EventMapConstruction.AllLocations();
                }
                Close();
            }
            else
            {
                requiredText.Foreground = Brushes.Red;
            }
        }

        private void map_type_changed(object sender, SelectionChangedEventArgs e)
        {
            locationName.IsEnabled = !locationName.IsEnabled;

            if (locationName.IsEnabled)
            {
                XMLParser.FillBoxWithNames(XMLParser.GetInstance().GetLocationXDocument(), ref locationName);
            }
        }
    }
}
