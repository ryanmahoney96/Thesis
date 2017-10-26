﻿
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
    /// Interaction logic for NewCharacterWindow.xaml
    /// </summary>
    public partial class NewEventWindow : NewEntityWindow
    {

        public NewEventWindow(string fp, FrontPage fpr) : base(fp, fpr)
        {
            InitializeComponent();
            Initialize_locations();
        }

        private void Initialize_locations()
        {
            //TODO: add a new location and update
            List<string> locationNames = XMLParserClass.GetAllLocationNames(base.folderPath + "\\location_notes.xml");

            foreach (string location in locationNames)
            {
                ComboBoxItem cBoxItem = new ComboBoxItem();
                cBoxItem.Content = location;
                location_combo_box.Items.Add(cBoxItem);
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO: make sure character does not already exist
            StreamWriter event_notes_handle = null;

            if (!name_text.Text.Equals("") && !notes_text.Text.Equals(""))
            {
                try
                {
                    string name = name_text.Text;
                    string location = location_combo_box.Text;
                    string unit_date = event_unit_date_number.Text;
                    string date = event_date_number.Text;
                    string notes = notes_text.Text;
                    string filePath = base.folderPath + "\\event_notes.xml";

                    if (File.Exists(filePath))
                    {
                        event_notes_handle = XMLParserClass.RemoveLastLine(filePath);
                    }
                    else
                    {
                        event_notes_handle = new StreamWriter(filePath, true);
                        event_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<events>\n\t");
                    }

                    event_notes_handle.Write("<event>\n\t\t");
                    event_notes_handle.Write("<name>" + name + "</name>\n\t\t");
                    event_notes_handle.Write("<location>" + location + "</location>\n\t\t");
                    event_notes_handle.Write("<unit_date>" + unit_date + "</unit_date>\n\t\t");
                    event_notes_handle.Write("<date>" + date + "</date>\n\t\t");
                    event_notes_handle.Write("<notes>" + notes + "</notes>\n\t");
                    event_notes_handle.Write("</event>\n\n");

                    event_notes_handle.Write("</events>");

                    event_notes_handle.Close();

                    frontPageReference.Update_Events();

                    base.Close();
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
                    if (!event_notes_handle.Equals(null))
                    {
                        event_notes_handle.Close();
                    }

                    base.Close();
                }
            }
            else
            {
                required_text.Foreground = Brushes.Red;
            }
        }

    }
}

