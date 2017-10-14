﻿using System;
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
    /// Interaction logic for NewCharacterWindow.xaml
    /// </summary>
    public partial class NewCharacterWindow : Window
    {
        string folderPath;

        public NewCharacterWindow(string fp)
        {
            InitializeComponent();
            folderPath = fp;
        }

        private void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Character(object sender, RoutedEventArgs e)
        {
            //TODO: make sure character does not already exist
            StreamWriter character_notes_handle = null;

            if (!name_text.Text.Equals("") && !gender_combo_box.Text.Equals(""))
            {
                try
                {
                    string name = name_text.Text;
                    string gender = gender_combo_box.Text;
                    string physical_description = physical_description_text.Text;
                    string filePath = folderPath + "\\character_notes.xml";

                    if (File.Exists(folderPath + "\\character_notes.xml"))
                    {
                        character_notes_handle = NewWindowFunctions.RemoveLastLine(filePath);
                    }
                    else
                    {
                        character_notes_handle = new StreamWriter(filePath, true);
                        character_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<characters>\n\t");
                    }


                    //TODO: check to see if file already exists

                    character_notes_handle.Write("<character>\n\t\t");
                    character_notes_handle.Write("<name>" + name_text.Text + "</name>\n\t\t");
                    character_notes_handle.Write("<gender>" + gender_combo_box.Text + "</gender>\n\t\t");
                    character_notes_handle.Write("<physical_description>" + physical_description_text.Text + "</physical_description>\n\t");
                    character_notes_handle.Write("</character>\n\n");

                    character_notes_handle.Write("</characters>");

                    character_notes_handle.Close();
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
                    if (!character_notes_handle.Equals(null))
                    {
                        character_notes_handle.Close();
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
