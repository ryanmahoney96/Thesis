﻿using System;
using System.Collections;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FrontPage : Window
    {
        //TODO: Alphabetize entities in their files (fairly difficult)? or in the lists (much easier)?
        //TODO: Make \ unusable 
        //TODO: Add relationships to both event/character/location/etc entities of a pair (rather than separate files) to reduce search/organization time?
        //TODO Later: stylize with https://github.com/MahApps/MahApps.Metro

        private String folderPath;

        public FrontPage(String fp)
        {
            InitializeComponent();
            folderPath = fp;
            Title += " - " + folderPath;
            Update_Note_Pages(true);
        }

        private void New_Note_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewGeneralNote(folderPath, this));
        }
        public void Update_Notes()
        {
            Fill_Note_Area(XMLParserClass.GetAllGeneralNotes(folderPath + "\\general_notes.xml"), general_notes_area);
        }

        private void New_Character_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewCharacterWindow(folderPath, this));
        }
        public void Update_Characters()
        {
            Fill_Note_Area(XMLParserClass.GetAllCharacterNotes(folderPath + "\\character_notes.xml"), character_notes_area);
        }

        private void New_Event_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewEventWindow(folderPath, this));
        }
        public void Update_Events()
        {
            Fill_Note_Area(XMLParserClass.GetAllEventNotes(folderPath + "\\event_notes.xml"), event_notes_area);
        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewLocationWindow(folderPath, this));
        }
        public void Update_Locations()
        {
            Fill_Note_Area(XMLParserClass.GetAllLocationNotes(folderPath + "\\location_notes.xml"), location_notes_area);
        }

        private void Character_Relationship_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewCharacterRelationship(folderPath, this));
        }

        private void Event_Relationship_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewEventRelationship(folderPath, this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //testHeader.Content = XMLParserClass.attemptParse();
            //testHeader.Content = XMLParserClass.attemptSpecificParse();
            //testButton.Content = XMLParserClass.XPathParse(folderPath + "\\character_notes.xml", "/characters/character/*");
            //testButton.Content += XMLParserClass.LINQParseTest(folderPath + "\\character_notes.xml");
            //Fill_Note_Pages();
        }

        private void Update_Note_Pages(bool initialize = false)
        {
            //Fill_Note_Area(XMLParserClass.GetAllCharacterNotes(folderPath + "\\character_notes.xml"), character_notes_area);
            //Fill_Note_Area(XMLParserClass.GetAllGeneralNotes(folderPath + "\\general_notes.xml"), general_notes_area);
            //Fill_Note_Area(XMLParserClass.GetAllLocationNotes(folderPath + "\\location_notes.xml"), location_notes_area);
            //Fill_Note_Area(XMLParserClass.GetAllEventNotes(folderPath + "\\event_notes.xml"), event_notes_area);

            Fill_Media_Area(XMLParserClass.GetMediaInformation(folderPath + "\\media_notes.xml"));
            Fill_Note_Area(XMLParserClass.GetAllGeneralNotes(folderPath + "\\general_notes.xml", initialize), general_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllCharacterNotes(folderPath + "\\character_notes.xml", initialize), character_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllEventNotes(folderPath + "\\event_notes.xml", initialize), event_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllLocationNotes(folderPath + "\\location_notes.xml", initialize), location_notes_area);
        }

        private void Fill_Media_Area(Hashtable information)
        {
            if (!information.Equals(null))
            {
                titleText.Text = information["Name"].ToString();
                yearText.Text = information["Year"].ToString();
                type_combobox.Text = information["Type"].ToString();
                genre_combobox.Text = information["Genre"].ToString();
                summaryText.Text = information["Summary"].ToString();
            }
        }

        public void Fill_Note_Area(List<List<string>> entityNodes, WrapPanel area)
        {
            area.Children.Clear();
            area.MinHeight = 200;

            foreach (List<string> l in entityNodes)
            {
                NoteNode n = new NoteNode();

                foreach (string s in l)
                {
                    string[] parts = s.Split('\\');
                    parts[0] = ToTitleCase(parts[0]);

                    if (!String.IsNullOrWhiteSpace(parts[1]))
                    {
                        StringBuilder t = new StringBuilder();
                        t.Append(parts[0]);
                        t.Append(": ");
                        t.Append(parts[1]);
                        t.Append("\n");
                        n.Text += t.ToString();
                    }
                }
                area.Children.Add(n);
                area.MinHeight += 75;
            }
        }

        private string ToTitleCase(string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        private void initWindow (Window w)
        {
            w.Show();
            w.Topmost = true;
        }

        private void Save_Media_Information(object sender, RoutedEventArgs e)
        {
            //TODO: Verify all text boxes are filled
            Media m = new Media(titleText.Text, yearText.Text, type_combobox.Text, genre_combobox.Text, summaryText.Text);
            m.Save(folderPath);

            GraphConstruction g = new GraphConstruction(folderPath);
            g.TestGraphviz();
        }
    }
}
