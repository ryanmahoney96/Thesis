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

        public NewEntityWindow(string fp, FrontPage fpr)
        {
            folderPath = fp;
            frontPageReference = fpr;
        }
        public NewEntityWindow()
        {
            folderPath = "";
        }

        protected void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //TODO: create save that takes in function pointer to take away all the redundant code in the inheritor classes
        protected abstract void Save(object sender, RoutedEventArgs e);

    }
}
