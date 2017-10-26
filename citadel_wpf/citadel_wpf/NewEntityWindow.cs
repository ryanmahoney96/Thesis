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
    /// Interaction logic for NewEntityWindow.xaml
    /// </summary>
    public abstract class NewEntityWindow : Window
    {
        protected string folderPath;
        protected FrontPage frontPageReference;
        protected NewEntityWindow[] reliantWindows;

        public NewEntityWindow(string fp, FrontPage fpr, params NewEntityWindow[] rw)
        {
            folderPath = fp;
            frontPageReference = fpr;
            reliantWindows = rw;
        }
        public NewEntityWindow()
        {
            folderPath = "";
        }

        protected void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //TODO: create save that takes in function, bool list, handle name to take away all the redundant code in the inheritor classes
        protected abstract void Save(object sender, RoutedEventArgs e);

        public abstract void UpdateReliantWindows();

    }
}
