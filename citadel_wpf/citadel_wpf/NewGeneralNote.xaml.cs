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
using System.Xml.Linq;

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for NewGeneralNote.xaml
    /// </summary>
    public partial class NewGeneralNote : EntityWindow
    {
        public NewGeneralNote(params EntityWindow[] rw) : base(rw)
        {
            InitializeComponent();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            if (XMLParser.IsEntityPresent(XMLParser.NoteXDocument.Handle, name_text.Text))
            {
                System.Windows.Forms.MessageBox.Show("This note already exists, please try again.");
            }
            else
            {
                if (!XMLParser.IsTextValid(name_text.Text) || string.IsNullOrWhiteSpace(description_text.Text))
                {
                    required_text.Foreground = Brushes.Red;
                }
                else
                {
                    XElement newNote = new XElement("general_note",
                    new XElement("name", name_text.Text),
                    new XElement("description", description_text.Text));

                    string temp = newNote.ToString();

                    XMLParser.NoteXDocument.Handle.Root.Add(newNote);
                    XMLParser.NoteXDocument.Save();

                    UpdateReliantWindows();
                    Close();
                }
            }

        }

        override public void UpdateReliantWindows()
        {
            FrontPage.FrontPageReference.Update_Notes();

            foreach (EntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }
    }
}