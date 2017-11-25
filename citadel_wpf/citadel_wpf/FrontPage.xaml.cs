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
using System.Xml.Linq; 

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FrontPage : Window
    {
        //TODO: Make "> \ <" in all text unusable + all non alphanumeric characters (" ' " and " - " should be included)
        //TODO: When making a new folder, verify that a media entry does not exist
        //TODO: Organize so event has list of pointers to things before and things after. Use this when adding new relationship to filter out contradictory data
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
            SizeChanged += FrontPage_SizeChanged;
            Loaded += FrontPage_Loaded;
        }

        private void FrontPage_Loaded(object sender, EventArgs e)
        {
            FrontPage_SizeChanged(null, null);
        }

        private void FrontPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            string selectedName = ((TabItem)MainTabControl.SelectedItem).Header as string;
            double referenceWidth = 780;
            if (selectedName.Equals("General Notes"))
            {
                referenceWidth = note_scrollview.ActualWidth;
            }
            else if (selectedName.Equals("Characters"))
            {
                referenceWidth = character_scrollview.ActualWidth;
            }
            else if (selectedName.Equals("Events"))
            {
                referenceWidth = event_scrollview.ActualWidth;
            }
            else if (selectedName.Equals("Locations"))
            {
                referenceWidth = location_scrollview.ActualWidth;
            }
            SetWidths(ref general_notes_area, referenceWidth);
            SetWidths(ref character_notes_area, referenceWidth);
            SetWidths(ref event_notes_area, referenceWidth);
            SetWidths(ref location_notes_area, referenceWidth);
            MiddleColumn.Width = new GridLength(Width - 300);
            MiddleRow.Height = new GridLength(Height - 300);
        }

        private void SetWidths(ref WrapPanel panel, double referenceWidth)
        {
            SetMinWidth(ref panel);
            SetMaxWidth(ref panel, referenceWidth);
        }

        private void SetMinWidth(ref WrapPanel panel)
        {
            panel.MinWidth = (NoteNode.NoteNodeWidth);
        }
        private void SetMaxWidth(ref WrapPanel panel, double referenceWidth)
        {
            if (location_scrollview.ActualWidth > 0)
            {
                panel.MaxWidth = (referenceWidth / NoteNode.NoteNodeWidth) * (NoteNode.NoteNodeWidth);
            }
            else
            {
                panel.MaxWidth = Width;
            }
        }

        private void New_Note_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewGeneralNote()));
        }
        public void Update_Notes()
        {
            Fill_Note_Area(XMLParser.GetInstance().GetNoteXDocument(), general_notes_area);
        }

        private void New_Character_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewCharacterWindow()));
        }
        public void Update_Characters()
        {
            Fill_Note_Area(XMLParser.GetInstance().GetCharacterXDocument(), character_notes_area);
        }

        private void New_Event_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewEventWindow()));
        }
        public void Update_Events()
        {
            Fill_Note_Area(XMLParser.GetInstance().GetEventXDocument(), event_notes_area);
        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewLocationWindow()));
        }
        public void Update_Locations()
        {
            Fill_Note_Area(XMLParser.GetInstance().GetLocationXDocument(), location_notes_area);
        }

        private void Character_Relationship_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new ViewCharacterRelationships()));
        }

        private void Event_Relationship_Click(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, (new NewEventRelationship()));
        }

        private void Update_Note_Pages()
        {
            Fill_Media_Area(XMLParser.GetMediaInformation(FolderPath + "\\media_notes.xml"));
            Fill_Note_Area(XMLParser.GetInstance().GetNoteXDocument(), general_notes_area);
            Fill_Note_Area(XMLParser.GetInstance().GetCharacterXDocument(), character_notes_area);
            Fill_Note_Area(XMLParser.GetInstance().GetEventXDocument(), event_notes_area);
            Fill_Note_Area(XMLParser.GetInstance().GetLocationXDocument(), location_notes_area);
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

        public void Fill_Note_Area(XDocument type, WrapPanel area)
        {
            List<Dictionary<string, string>> entityNodes = XMLParser.GetAllNotes(type);
            area.Children.Clear();
            area.MinHeight = NoteNode.NoteNodeHeight;
            //TODO button notenode

            foreach (Dictionary<string, string> entityNode in entityNodes)
            {
                NoteNode n = new NoteNode(type);
                //TODO adjust this
                area.MinHeight += Convert.ToInt32(n.FillWith(entityNode)) ;
                
                area.Children.Add(n);
                //area.MinHeight += NoteNode.NoteNodeHeight / 2;
            }
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to go back? Any unsaved data will be lost.", "Return to Main Menu", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MainWindow m = new MainWindow();
                m.Show();
                Close();
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
