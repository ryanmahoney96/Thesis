using System;
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

        //TODO: Object Orient
        public FrontPage(String fp)
        {
            InitializeComponent();
            folderPath = fp;
            Title += " - " + folderPath;
            Update_Note_Pages();
        }

        private void New_Note_Click(object sender, RoutedEventArgs e)
        {
            NewGeneralNote ngn = new NewGeneralNote(folderPath);
            ngn.Show();
            ngn.Topmost = true;
        }

        private void New_Character_Click(object sender, RoutedEventArgs e)
        {
            NewCharacterWindow ncw = new NewCharacterWindow(folderPath);
            ncw.Show();
            ncw.Topmost = true;
        }

        private void New_Event_Click(object sender, RoutedEventArgs e)
        {
            NewEventWindow new_event_window = new NewEventWindow(folderPath);
            new_event_window.Show();
            new_event_window.Topmost = true;
        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            NewLocationWindow nlw = new NewLocationWindow(folderPath);
            nlw.Show();
            nlw.Topmost = true;
        }

        private void Media_Note_Click(object sender, RoutedEventArgs e)
        {

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
            //TODO: take away file name and add these in notes functions
            Fill_Note_Area(XMLParserClass.GetAllCharacterNotes(folderPath + "\\character_notes.xml"), character_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllGeneralNotes(folderPath + "\\general_notes.xml"), general_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllLocationNotes(folderPath + "\\location_notes.xml"), location_notes_area);
            Fill_Note_Area(XMLParserClass.GetAllEventNotes(folderPath + "\\event_notes.xml"), event_notes_area);
        }

        //TODO move this functionality over so that compilation into a "note" is done in "GetAll____Notes"
        private void Fill_Note_Area(List<List<string>> entityNodes, WrapPanel area)
        {
            foreach (List<string> l in entityNodes)
            {
                //TODO: Generic Item --> UIElement?
                Border b = new Border();
                b.BorderThickness = new Thickness(1, 1, 1, 1);
                b.BorderBrush = new SolidColorBrush(Colors.Black);
                TextBlock t = new TextBlock();
                t.TextWrapping = TextWrapping.Wrap;
                t.FontSize = 15;
                b.Width = 300;
                b.Height = 150;
                b.Child = t;
                b.Padding = new Thickness(1, 1, 1, 1);
                b.Margin = new Thickness(5, 5, 5, 5);

                foreach (string s in l)
                {
                    //TODO: add type to string and delimit by ^ to add headings
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        t.Text += s + "\n";
                    }
                }
                area.Children.Add(b);
            }
        }

    }
}
