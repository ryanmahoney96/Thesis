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

    public partial class FrontPage : EntityWindow
    {
        //TODO: When making a new folder, verify that a media entry does not exist
        //TODO: Organize so event has list of pointers to things before and things after. Use this when adding new relationship to filter out contradictory data
        //TODO: Stylize with https://github.com/MahApps/MahApps.Metro

        public static FrontPage FrontPageReference;
        private string XMLName;

        public FrontPage() : base()
        {
            InitializeComponent();
            XMLParser.Instance.UpdateMediaXDocument();
            base.Title += " - " + XMLParser.FolderPath;
            Update_Note_Pages();
            FrontPageReference = this;
            base.SizeChanged += FrontPage_SizeChanged;
            base.Loaded += FrontPage_Loaded;
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
            MiddleColumn.Width = new GridLength(base.Width - 300);
            MiddleRow.Height = new GridLength(base.Height - 300);
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
                panel.MaxWidth = base.Width;
            }
        }

        private void New_Note_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, (new NewGeneralNote()));
        }
        public void Update_Notes()
        {
            Fill_Note_Area(XMLParser.NoteXDocument, general_notes_area);
        }

        private void New_Character_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, (new NewCharacterWindow()));
        }
        public void Update_Characters()
        {
            Fill_Note_Area(XMLParser.CharacterXDocument, character_notes_area);
        }

        private void New_Event_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, (new NewEventWindow()));
        }
        public void Update_Events()
        {
            Fill_Note_Area(XMLParser.EventXDocument, event_notes_area);
        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, (new NewLocationWindow()));
        }
        public void Update_Locations()
        {
            Fill_Note_Area(XMLParser.LocationXDocument, location_notes_area);
        }

        private void Character_Relationship_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, (new ViewCharacterRelationships()));
        }

        private void Event_Relationship_Click(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, (new NewEventRelationship()));
        }

        private void Update_Note_Pages()
        {
            Fill_Media_Area(XMLParser.GetMediaInformation());
            Fill_Note_Area(XMLParser.NoteXDocument, general_notes_area);
            Fill_Note_Area(XMLParser.CharacterXDocument, character_notes_area);
            Fill_Note_Area(XMLParser.EventXDocument, event_notes_area);
            Fill_Note_Area(XMLParser.LocationXDocument, location_notes_area);
        }

        private void Fill_Media_Area(Hashtable information)
        {
            if (!information.Equals(null))
            {
                XMLName = information["Name"].ToString();
                titleText.Text = information["Name"].ToString();
                yearText.Text = information["Year"].ToString();
                type_combobox.Text = information["Type"].ToString();
                genre_combobox.Text = information["Genre"].ToString();
                summaryText.Text = information["Summary"].ToString();
            }
        }

        public void Fill_Note_Area(XMLParser.XDocumentPair type, WrapPanel area)
        {
            List<Dictionary<string, string>> entityNodes = XMLParser.GetAllEntities(type.Handle);
            area.Children.Clear();
            area.MinHeight = NoteNode.NoteNodeHeight;
            //TODO button notenode

            foreach (Dictionary<string, string> entityNode in entityNodes)
            {
                NoteNode n = new NoteNode(type.Name);
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
                base.Close();
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            if (!XMLParser.IsTextValid(titleText.Text))
            {
                System.Windows.Forms.MessageBox.Show("The Title Is Invalid");
            }
            else if (string.IsNullOrWhiteSpace(type_combobox.Text))
            {
                System.Windows.Forms.MessageBox.Show("Please Select a Media Type");
            }
            else if (!XMLParser.IsYearValid(yearText.Text))
            {
                System.Windows.Forms.MessageBox.Show("Invalid Year Entered");
            }
            else
            {
                XElement mediaReference = (from c in XMLParser.MediaXDocument.Handle.Root.Descendants("media_note")
                                               where c.Element("name").Value.Equals(XMLName)
                                               select c).First();

                XMLName = mediaReference.Element("name").Value;

                mediaReference.Element("name").Value = titleText.Text;
                mediaReference.Element("year").Value = yearText.Text;
                mediaReference.Element("type").Value = type_combobox.Text;
                mediaReference.Element("genre").Value = genre_combobox.Text;
                mediaReference.Element("summary").Value = summaryText.Text;

                XMLParser.MediaXDocument.Save();

                MessageBox.Show("The Media was Successfully Saved", "Success", MessageBoxButton.OK);
            }
        }

        override public void UpdateReliantWindows()
        {
            //
        }

        private void NewImmediateFamilyTree(object sender, RoutedEventArgs e)
        {
            Action<string> a = FamilyTreeConstruction.ImmediateFamilyTree;
            EntityWindow.InitializeModalWindow(this, (new CharacterPromptWindow(a)));
        }

        private void NewExtendedFamilyTree(object sender, RoutedEventArgs e)
        {
            Action<string> a = FamilyTreeConstruction.ExtendedFamilyTree;
            EntityWindow.InitializeModalWindow(this, (new CharacterPromptWindow(a)));
        }

        private void NewFullFamilyTree(object sender, RoutedEventArgs e)
        {
            Action<string> a = FamilyTreeConstruction.RecursiveFullFamilyTree;
            EntityWindow.InitializeModalWindow(this, (new CharacterPromptWindow(a)));
        }

        private void NewEventMap(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, (new LocationPromptWindow()));
        }
    }
}
