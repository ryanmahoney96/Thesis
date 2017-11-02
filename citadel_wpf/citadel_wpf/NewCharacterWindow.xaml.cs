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
    /// Interaction logic for NewCharacterWindow.xaml
    /// </summary>
    public partial class NewCharacterWindow : NewEntityWindow
    {

        public NewCharacterWindow(string fp, FrontPage fpr, params NewEntityWindow[] rw) : base(fp, fpr, rw)
        {
            InitializeComponent();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO double check values before save
            if (Character.AddRecord(new Character(name_text.Text, gender_combo_box.Text, description_text.Text)))
            {
                controlTexts.Add(name_text.Text);
                controlTexts.Add(gender_combo_box.Text);
                SaveEntity(sender, e, controlTexts, required_text, "character_notes", Character.GetRecords());
                UpdateReliantWindows();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("This character already exists, please try again.");
            }


            //StreamWriter character_notes_handle = null;

            //if (!name_text.Text.Equals("") && !gender_combo_box.Text.Equals(""))
            //{
            //    try
            //    {
            //        string name = name_text.Text;
            //        string gender = gender_combo_box.Text;
            //        string description = description_text.Text;
            //        string filePath = folderPath + "\\character_notes.xml";

            //        if (File.Exists(filePath))
            //        {
            //            character_notes_handle = XMLParserClass.RemoveLastLine(filePath);
            //        }
            //        else
            //        {
            //            character_notes_handle = new StreamWriter(filePath, true);
            //            character_notes_handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<characters>\n\t");
            //        }

            //        character_notes_handle.Write("<character>\n\t\t");
            //        character_notes_handle.Write("<name>" + name + "</name>\n\t\t");
            //        character_notes_handle.Write("<gender>" + gender + "</gender>\n\t\t");
            //        character_notes_handle.Write("<description>" + description + "</description>\n\t");
            //        character_notes_handle.Write("</character>\n\n");

            //        character_notes_handle.Write("</characters>");

            //        character_notes_handle.Close();

            //        UpdateReliantWindows();

            //        base.Close();
            //    }
            //    catch (IOException)
            //    {
            //        System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
            //    }
            //    catch (Exception)
            //    {
            //        System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
            //    }
            //    finally
            //    {
            //        if (!character_notes_handle.Equals(null))
            //        {
            //            character_notes_handle.Close();
            //        }

            //        base.Close();
            //    }
            //}
            //else
            //{
            //    required_text.Foreground = Brushes.Red;
            //}
        }

        public override void UpdateReliantWindows()
        {
            frontPageReference.Update_Characters();

            foreach (NewEntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }
    }

}
