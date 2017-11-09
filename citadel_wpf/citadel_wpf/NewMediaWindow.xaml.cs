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
            Regex yearRegex = new Regex(@"^[0-9]*$");

            if (name_text.Text.Equals(""))
            {
                required_text.Text = "Fill in the Title";
                required_text.Foreground = Brushes.Red;
            }
            else if (!yearRegex.IsMatch(year_text.Text))
            {
                required_text.Text = "Invalid Year";
                required_text.Foreground = Brushes.Red;
            }
            else if (type_combobox.Text.Equals(""))
            {
                required_text.Text = "Select a Media Type";
                required_text.Foreground = Brushes.Red;
            }
            else
            {
                try
                {
                    string name = name_text.Text;
                    string year = year_text.Text;
                    string type = type_combobox.Text;
                    string genre = genre_combobox.Text;
                    string summary = summary_text.Text;
                    string filePath = folderPath + "\\media_notes.xml";

                    
                    media_notes_handle = new StreamWriter(filePath, true);
                    media_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><media_notes>");

                    media_notes_handle.Write("<media_note>");
                    media_notes_handle.Write("<name>" + name + "</name>");
                    media_notes_handle.Write("<year>" + year + "</year>");
                    media_notes_handle.Write("<type>" + type + "</type>");
                    media_notes_handle.Write("<genre>" + genre + "</genre>");
                    media_notes_handle.Write("<summary>" + summary + "</summary>");
                    media_notes_handle.Write("</media_note>");

                    media_notes_handle.Write("</media_notes>");

                    media_notes_handle.Close();

                    FrontPage frontPage = new FrontPage(folderPath);
                    frontPage.Topmost = true;
                    frontPage.Topmost = false;
                    frontPage.Show();

                    Close();
                }
                catch (IOException)
                {
                    System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
                }
                //catch (Exception)
                //{
                //    System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
                //}
                finally
                {
                    if (!media_notes_handle.Equals(null))
                    {
                        media_notes_handle.Close();
                    }

                    Close();
                }
            }
        }

        public override void UpdateReliantWindows() { }
    }
}