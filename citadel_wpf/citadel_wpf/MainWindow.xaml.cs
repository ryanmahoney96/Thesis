using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_Folder(object sender, RoutedEventArgs e)
        {

            String myStream = null;
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Select the directory holding your notes";
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (!string.IsNullOrEmpty(myStream = fbd.SelectedPath))
                    {

                        folderName.Text = fbd.SelectedPath;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("reference"))
                    {
                        System.Windows.Forms.MessageBox.Show("Error: The selected directory is invalid.");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                if (folderName.Text.Equals(""))
                {
                    System.Windows.Forms.MessageBox.Show("Please Select or Create a Note Folder.");
                }
            }

        }

        private void Submit_Folder(object sender, RoutedEventArgs e)
        {

            if (!folderName.Text.Equals(""))
            {
                Media_Note_Validation();
            }
        }

        private void Media_Note_Validation()
        {
            string mediaNotes = folderName.Text + "\\media_notes.xml";

            if (!File.Exists(mediaNotes))
            {
                NewMediaWindow nmw = new NewMediaWindow(folderName.Text, null);
                nmw.Topmost = true;
                nmw.Show();
            }
            else
            {
                FrontPage frontPage = new FrontPage(folderName.Text);
                frontPage.Topmost = true;
                frontPage.Show();
            }

            this.Close();
        }

    }
}
