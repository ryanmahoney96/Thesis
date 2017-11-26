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
    /// Interaction logic for NewEntityWindow.xaml
    /// </summary>
    public abstract class EntityWindow : Window
    {
        protected EntityWindow[] reliantWindows;

        public EntityWindow(params EntityWindow[] rw): this()
        {
            reliantWindows = rw;
        }

        public EntityWindow()
        {
            Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#fcfcff"));
            BorderBrush = Brushes.DarkBlue;
            BorderThickness = new Thickness(0, 1, 0, 0);
            Uri iconUri = new Uri("../../citadel_icon.ico", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public static void InitializeModalWindow(Window owner, Window child)
        {
            child.Owner = owner;
            child.Topmost = true;
            child.Topmost = false;
            child.ShowDialog();
        }

        protected void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected abstract void Save(object sender, RoutedEventArgs e);
        public abstract void UpdateReliantWindows();

    }
}
