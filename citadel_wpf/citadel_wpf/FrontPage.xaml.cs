using System;
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
        private String folderPath;

        //TODO: add location to event window
        //TODO: Note Areas do not wrap: change to grid with adapting height? Change wrappanel reference to scrollviewer reference?

        public FrontPage(String fp)
        {
            InitializeComponent();
            folderPath = fp;
            Title += " - " + folderPath;
            Update_Note_Pages();
        }

        private void New_Note_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewGeneralNote(folderPath, this));
        }
        public void Update_Notes()
        {
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\general_notes.xml", "note"), general_notes_area);
        }

        private void New_Character_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewCharacterWindow(folderPath, this));
        }
        public void Update_Characters()
        {
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\character_notes.xml", "character"), character_notes_area);
        }

        private void New_Event_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewEventWindow(folderPath, this));
        }
        public void Update_Events()
        {
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\event_notes.xml", "event"), event_notes_area);
        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewLocationWindow(folderPath, this));
        }
        public void Update_Locations()
        {
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\location_notes.xml", "location"), location_notes_area);
        }

        private void Character_Relationship_Click(object sender, RoutedEventArgs e)
        {
            initWindow(new NewCharacterRelationship(folderPath, this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //testHeader.Content = XMLParserClass.attemptParse();
            //testHeader.Content = XMLParserClass.attemptSpecificParse();
            //testButton.Content = XMLParserClass.XPathParse(folderPath + "\\character_notes.xml", "/characters/character/*");
            //testButton.Content += XMLParserClass.LINQParseTest(folderPath + "\\character_notes.xml");
            //Fill_Note_Pages();
        }

        private void Update_Note_Pages()
        {
            //Fill_Note_Area(XMLParserClass.GetAllCharacterNotes(folderPath + "\\character_notes.xml"), character_notes_area);
            //Fill_Note_Area(XMLParserClass.GetAllGeneralNotes(folderPath + "\\general_notes.xml"), general_notes_area);
            //Fill_Note_Area(XMLParserClass.GetAllLocationNotes(folderPath + "\\location_notes.xml"), location_notes_area);
            //Fill_Note_Area(XMLParserClass.GetAllEventNotes(folderPath + "\\event_notes.xml"), event_notes_area);

            Fill_Media_Area(XMLParserClass.GetMediaInformation(folderPath + "\\media_notes.xml"));
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\general_notes.xml", "note"), general_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\character_notes.xml", "character"), character_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\event_notes.xml", "event"), event_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllNotes(folderPath + "\\location_notes.xml", "location"), location_notes_area);
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

            foreach (List<string> l in entityNodes)
            {
                NoteNode n = new NoteNode();

                foreach (string s in l)
                {
                    //TODO: use delimiter concept to add headings and labels
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        n.Text += s + "\n";
                    }
                }
                area.Children.Add(n);
            }
        }

        public void Fill_Note_Area(List<List<string>> entityNodes, ScrollViewer area)
        {
            area.Content = "";

            foreach (List<string> l in entityNodes)
            {
                NoteNode n = new NoteNode();

                foreach (string s in l)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        n.Text += s + "\n";
                    }
                }
                //TODO: +=
                area.Content = n;
            }
        }

        private void initWindow (Window w)
        {
            w.Show();
            w.Topmost = true;
        }

    }
}
