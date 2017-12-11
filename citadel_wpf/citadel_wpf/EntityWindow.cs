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
        public static SolidColorBrush BackgroundColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFbFbFb"));
        //public static SolidColorBrush BackgroundColor = Brushes.WhiteSmoke;

        private List<XDocumentInformation> Attachments = new List<XDocumentInformation>();

        public EntityWindow()
        {
            SetDecorations(this);
            Uri iconUri = new Uri("../../citadel_logo.png", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        ~EntityWindow()
        {
            XMLParser.DetachFromAll(this, Attachments);
        }

        protected void AttachToXDocument(ref XDocumentInformation x)
        {
            Attachments.Add(x);
            x.Attach(this);
        }

        public static void SetDecorations(Control u)
        {
            u.Background = BackgroundColor;
            u.BorderBrush = Brushes.Purple;
            u.BorderThickness = new Thickness(0, 1, 0, 0);
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

        public abstract void Update(XDocumentInformation x = null);

    }
}
