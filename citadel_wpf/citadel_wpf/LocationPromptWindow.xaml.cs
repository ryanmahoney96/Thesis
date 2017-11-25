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

    public partial class LocationPromptWindow : NewEntityWindow
    {
        //TODO Tooltips
        public LocationPromptWindow(params NewEntityWindow[] rw) : base(rw)
        {
            InitializeComponent();
        }

        override protected void Save(object sender, RoutedEventArgs e)
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

        public override void UpdateReliantWindows()
        {
            //
        }

        private void map_type_changed(object sender, SelectionChangedEventArgs e)
        {
            if (eventMapType.SelectedItem != null && XMLParser.IsTextValid(eventMapType.SelectedItem.ToString()))
            {
                string emt = eventMapType.SelectedItem.ToString().Split(':')[1].Substring(1);

                if (emt.Equals("Single Event Map"))
                {
                    locationName.IsEnabled = true;
                    XMLParser.FillComboboxWithNames(XMLParser.LocationXDocument.Handle, ref locationName);
                }
                else
                {
                    locationName.IsEnabled = false;
                }
            }

        }
    }
}
