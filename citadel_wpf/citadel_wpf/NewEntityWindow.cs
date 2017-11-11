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
    public abstract class NewEntityWindow : Window
    {
        protected NewEntityWindow[] reliantWindows;
        protected List<String> controlTexts;

        public NewEntityWindow(params NewEntityWindow[] rw)
        {
            reliantWindows = rw;
            controlTexts = new List<String>();
        }
        public NewEntityWindow()
        {
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
        //public abstract void FillWithData(string name);

    }
}
