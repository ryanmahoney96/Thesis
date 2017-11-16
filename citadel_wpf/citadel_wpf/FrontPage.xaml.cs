using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FrontPage : Window
    {
        //TODO: Make "> \ <" in all text unusable + all non alphanumeric characters (" ' " and " - " should be included)
        //TODO: Separate new folder and select folder dialogs
        //TODO: When making a new folder, verify that a media entry does not exist
        //TODO: Organize so event has list of pointers to things before and things after. Use this when adding new relationship to filter out contradictory data
        //TODO: Family Trees
        //TODO: Add location v event graph
        //TODO: Stylize with https://github.com/MahApps/MahApps.Metro

        public static String FolderPath;
        public static FrontPage FrontPageReference;

        public FrontPage(String fp)
        {
            InitializeComponent();
            FolderPath = fp;
            Title += " - " + FolderPath;
            Update_Note_Pages();
            FrontPageReference = this;
        }

        private void New_Note_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewGeneralNote()));
        }
        public void Update_Notes()
        {
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetNoteXDocument()), general_notes_area);
        }

        private void New_Character_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewCharacterWindow()));
        }
        public void Update_Characters()
        {
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetCharacterXDocument()), character_notes_area);
        }

        private void New_Event_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewEventWindow()));
        }
        public void Update_Events()
        {
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetEventXDocument()), event_notes_area);
        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewLocationWindow()));
        }
        public void Update_Locations()
        {
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetLocationXDocument()), location_notes_area);
        }

        private void Character_Relationship_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewCharacterRelationship()));
        }

        private void Event_Relationship_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewEventRelationship()));
        }

        private void Update_Note_Pages()
        {
            Fill_Media_Area(XMLParser.GetMediaInformation(FolderPath + "\\media_notes.xml"));
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetNoteXDocument()), general_notes_area);
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetCharacterXDocument()), character_notes_area);
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetEventXDocument()), event_notes_area);
            Fill_Note_Area(XMLParser.GetAllNotes(XMLParser.GetInstance().GetLocationXDocument()), location_notes_area);
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

        public void Fill_Note_Area(List<Dictionary<string, string>> entityNodes, WrapPanel area)
        {
            area.Children.Clear();
            area.MinHeight = NoteNode.NoteNodeHeight;
            //TODO button notenode

            foreach (Dictionary<string, string> entityNode in entityNodes)
            {
                NoteNode n = new NoteNode();
                n.FillWith(entityNode);
                
                area.Children.Add(n);
                area.MinHeight += NoteNode.NoteNodeHeight / 2;
            }
        }

        private void Save_Media_Information(object sender, RoutedEventArgs e)
        {
            Regex yearRegex = new Regex(@"^[0-9]*$");

            if (titleText.Text.Equals(""))
            {
                System.Windows.Forms.MessageBox.Show("The Title Cannot Be Left Blank");
            }
            else if (!yearRegex.IsMatch(yearText.Text))
            {
                System.Windows.Forms.MessageBox.Show("Invalid Year Entered");
            }
            else
            {
                Media m = new Media(titleText.Text, yearText.Text, type_combobox.Text, genre_combobox.Text, summaryText.Text);
                m.Save(FolderPath);
            }
        }

        private void NewImmediateFamilyTree(object sender, RoutedEventArgs e)
        {
            Action<string> a = FamilyTreeConstruction.ImmediateFamilyTree;
            NewEntityWindow.InitializeModalWindow(this, (new CharacterPromptWindow(a)));
        }

        private void NewExtendedFamilyTree(object sender, RoutedEventArgs e)
        {
            Action<string> a = FamilyTreeConstruction.ExtendedFamilyTree;
            NewEntityWindow.InitializeModalWindow(this, (new CharacterPromptWindow(a)));
        }

        private void NewFullFamilyTree(object sender, RoutedEventArgs e)
        {
            Action<string> a = FamilyTreeConstruction.RecursiveFullFamilyTree;
            NewEntityWindow.InitializeModalWindow(this, (new CharacterPromptWindow(a)));
        }

        private void NewEventMap(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new LocationPromptWindow()));
        }
    }
}
