using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NewMediaWindow.xaml
    /// </summary>
    public partial class NewMediaWindow : EntityWindow
    {

        public NewMediaWindow(): base()
        {
            InitializeComponent();
            XMLParser.Instance.UpdateMediaXDocument();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            if (!XMLParser.IsTextValid(name_text.Text))
            {
                required_text.Text = "Fill in the Title";
                required_text.Foreground = Brushes.Red;
            }
            else if (!XMLParser.IsYearValid(year_text.Text))
            {
                required_text.Text = "Invalid Year";
                required_text.Foreground = Brushes.Red;
            }
            else if (string.IsNullOrWhiteSpace(type_combobox.Text))
            {
                required_text.Text = "Select a Media Type";
                required_text.Foreground = Brushes.Red;
            }
            else
            {
                try
                {
                    XElement newMedia = new XElement("media_note",
                            new XElement("name", PrepareText(name_text.Text)),
                            new XElement("year", year_text.Text),
                            new XElement("type", PrepareText(type_combobox.Text)),
                            new XElement("genre", PrepareText(genre_combobox.Text)),
                            new XElement("summary", PrepareText(summary_text.Text)));

                    XMLParser.MediaXDocument.Handle.Root.Add(newMedia);
                    XMLParser.MediaXDocument.Save();

                    InitializeModalWindow(this, new FrontPage());

                    Close();
                }
                catch (IOException)
                {
                    System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
                }
            }
        }

        override public void Update(XDocumentInformation x = null) { }
    }
}