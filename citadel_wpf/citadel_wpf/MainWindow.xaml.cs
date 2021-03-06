﻿using System;
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
using System.Diagnostics;

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : EntityWindow
    {

        public MainWindow(): base()
        {
            Process thisProc = Process.GetCurrentProcess();
            // Check how many total processes have the same name as the current one
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                // If there is more than one, than it is already running.
                System.Windows.MessageBox.Show("Citadel is already running in another process. Please Try Again.");
                this.Close();
            }
            else
            {
                InitializeComponent();
            }
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
                if (XMLParser.IsTextValid(folderName.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Please Select or Create a Note Folder.");
                }
            }

        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(folderName.Text))
            {
                string mediaNotes = folderName.Text + "\\Citadel_XML\\media_notes.xml";

                XMLParser.Instance = new XMLParser(folderName.Text);

                if (!File.Exists(mediaNotes))
                {
                    NewMediaWindow nmw = new NewMediaWindow();
                    nmw.Topmost = true;
                    nmw.Topmost = false;
                    nmw.Show();
                }
                else
                {
                    FrontPage frontPage = new FrontPage();
                    frontPage.Topmost = true;
                    frontPage.Topmost = false;
                    frontPage.Show();
                }

                this.Close();
            }
        }

    }
}
