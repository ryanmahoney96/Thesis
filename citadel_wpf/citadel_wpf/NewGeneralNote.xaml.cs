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
    /// Interaction logic for NewGeneralNote.xaml
    /// </summary>
    public partial class NewGeneralNote : NewEntityWindow
    {
        public NewGeneralNote(string fp, FrontPage fpr) : base(fp, fpr)
        {
            InitializeComponent();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            StreamWriter general_notes_handle = null;

            if (!note_text.Text.Equals(""))
            {
                try
                {
                    string note = note_text.Text;
                    string filePath = folderPath + "\\general_notes.xml";

                    if (File.Exists(filePath))
                    {
                        general_notes_handle = XMLParserClass.RemoveLastLine(filePath);
                    }
                    else
                    {
                        general_notes_handle = new StreamWriter(filePath, true);
                        general_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<general_notes>\n\t");
                    }

                    general_notes_handle.Write("<note>\n\t<content>" + note + "\n\t</content></note>\n");

                    general_notes_handle.Write("</general_notes>");

                    general_notes_handle.Close();

                    frontPageReference.Update_Notes();

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
                    if (!general_notes_handle.Equals(null))
                    {
                        general_notes_handle.Close();
                    }

                    Close();
                }
            }
            else
            {
                required_text.Foreground = Brushes.Red;
            }
        }
    }
}