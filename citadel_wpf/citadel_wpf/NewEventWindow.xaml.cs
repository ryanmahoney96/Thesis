
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

        public NewEventWindow(string fp, FrontPage fpr, params NewEntityWindow[] rw) : base(fp, fpr, rw)
        {
            InitializeComponent();
            Initialize_Locations();
        }

        private void Initialize_Locations()
        {

            location_combo_box.Items.Clear();

            List<string> locationNames = XMLEntityParser.GetAllNames(base.folderPath + "\\location_notes.xml", "location");

            foreach (string location in locationNames)
            {
                ComboBoxItem cBoxItem = new ComboBoxItem();
                cBoxItem.Content = location;
                location_combo_box.Items.Add(cBoxItem);
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO: check texts
            if (SaveEntity(sender, e, controlTexts, required_text, "event_notes", name_text.Text, Entity.EventToXML(name_text.Text, location_combo_box.Text, event_unit_date_number.Text, event_date_number.Text, description_text.Text)))
            {
                UpdateReliantWindows();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("This event already exists, please try again.");
            }

    /*StreamWriter event_notes_handle = null;

    if (!name_text.Text.Equals("") && !notes_text.Text.Equals(""))
    {
        try
        {
            string name = name_text.Text;
            string location = location_combo_box.Text;
            string unit_date = event_unit_date_number.Text;
            string date = event_date_number.Text;
            string description = notes_text.Text;
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
            event_notes_handle.Write("<description>" + description + "</description>\n\t");
            event_notes_handle.Write("</event>\n\n");

            event_notes_handle.Write("</events>");

            event_notes_handle.Close();

            UpdateReliantWindows();

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
    */
}

        public override void UpdateReliantWindows()
        {
            frontPageReference.Update_Events();
            Initialize_Locations();

            foreach (NewEntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }

        private void Add_New_Location(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewLocationWindow(folderPath, frontPageReference, this));
        }
    }
}

