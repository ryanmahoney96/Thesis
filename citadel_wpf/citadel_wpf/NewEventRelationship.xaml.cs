using System;
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
    /// Interaction logic for NewEventRelationship.xaml
    /// </summary>
    public partial class NewEventRelationship : NewEntityWindow
    {

        public NewEventRelationship(string fp, FrontPage fpr) : base(fp, fpr)
        {
            InitializeComponent();
            Fill_Event_Boxes();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO: make sure relationship does not already exist
            StreamWriter event_relationships_handle = null;

            if (!event_one_combo.Text.Equals("") && !relationship_combo.Text.Equals("") && !event_two_combo.Text.Equals(""))
            {
                try
                {
                    string event_one = event_one_combo.Text;
                    string event_two = event_two_combo.Text;
                    string relationship = relationship_combo.Text;
                    string filePath = folderPath + "\\event_relationships.xml";

                    if (File.Exists(filePath))
                    {
                        event_relationships_handle = XMLParserClass.RemoveLastLine(filePath);
                    }
                    else
                    {
                        event_relationships_handle = new StreamWriter(filePath, true);
                        event_relationships_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<event_relationships>\n\t");
                    }

                    event_relationships_handle.Write("<event_relationship>\n\t\t");
                    event_relationships_handle.Write("<char_one>" + event_one + "</char_one>\n\t\t");
                    event_relationships_handle.Write("<relationship>" + relationship + "</relationship>\n\t");
                    event_relationships_handle.Write("<char_two>" + event_two + "</char_two>\n\t\t");
                    event_relationships_handle.Write("</event_relationship>\n\n");

                    event_relationships_handle.Write("</event_relationships>");

                    event_relationships_handle.Close();
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
                    if (!event_relationships_handle.Equals(null))
                    {
                        event_relationships_handle.Close();
                    }

                    Close();
                }
            }
            else
            {
                required_text.Foreground = Brushes.Red;
            }
        }

        private void Fill_Event_Boxes()
        {
            event_one_combo.Items.Clear();
            event_two_combo.Items.Clear();

            List<string> eventNames = XMLParserClass.GetAllNames(base.folderPath + "\\event_notes.xml", "event");

            ComboBoxItem cBoxItem;

            foreach (string newEvent in eventNames)
            {
                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = newEvent;
                event_one_combo.Items.Add(cBoxItem);

                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = newEvent;
                event_two_combo.Items.Add(cBoxItem);
            }
        }

        private void Add_Event(object sender, RoutedEventArgs e)
        {
            NewEventWindow ncw = new NewEventWindow(folderPath, frontPageReference, this);
            ncw.Show();
            ncw.Topmost = true;            
        }

        public override void UpdateReliantWindows()
        {
            Fill_Event_Boxes();
        }
    }
}
