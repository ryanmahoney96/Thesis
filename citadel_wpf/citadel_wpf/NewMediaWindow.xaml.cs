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

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for NewMediaWindow.xaml
    /// </summary>
    public partial class NewMediaWindow : NewEntityWindow
    {

        public NewMediaWindow(string fp, FrontPage fpr): base(fp, fpr)
        {
            InitializeComponent();
        }

        //TODO: use generic save method? Requires a media entity
        override protected void Save(object sender, RoutedEventArgs e)
        {
            StreamWriter media_notes_handle = null;

            if (!name_text.Text.Equals("") && !type_combobox.Text.Equals(""))
            {
                try
                {
                    string name = name_text.Text;
                    //TODO: verify they are an int?
                    string year = year_text.Text;
                    string type = type_combobox.Text;
                    string genre = genre_combobox.Text;
                    string summary = summary_text.Text;
                    string filePath = folderPath + "\\media_notes.xml";

                    
                    media_notes_handle = new StreamWriter(filePath, true);
                    media_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<media_notes>\n\t");

                    media_notes_handle.Write("<media_note>");
                    media_notes_handle.Write("<name>" + name + "</name>\n\t");
                    media_notes_handle.Write("<year>" + year + "</year>\n\t");
                    media_notes_handle.Write("<type>" + type + "</type>\n\t");
                    media_notes_handle.Write("<genre>" + genre + "</genre>\n\t");
                    media_notes_handle.Write("<summary>" + summary + "</summary>\n\t");
                    media_notes_handle.Write("</media_note>");

                    media_notes_handle.Write("</media_notes>");

                    media_notes_handle.Close();

                    FrontPage frontPage = new FrontPage(folderPath);
                    frontPage.Topmost = true;
                    frontPage.Show();

                    Close();
                }
                catch (IOException)
                {
                    System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
                }
                finally
                {
                    if (!media_notes_handle.Equals(null))
                    {
                        media_notes_handle.Close();
                    }

                    Close();
                }
            }
            else
            {
                required_text.Foreground = Brushes.Red;
            }
        }

        public override void UpdateReliantWindows() { }
    }
}