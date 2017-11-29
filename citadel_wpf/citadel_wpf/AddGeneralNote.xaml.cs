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

    public partial class AddGeneralNote : EntityWindow, INewEntity
    {
        private bool Editing = false;

        public AddGeneralNote() : base()
        {
            InitializeComponent();
        }

        public void FillWith(string noteName)
        {
            Editing = true;

            var note = (from c in XMLParser.NoteXDocument.Handle.Root.Descendants("general_note")
                             where c.Element("name").Value.Equals(noteName)
                             select new
                             {
                                 Name = c.Element("name").Value,
                                 Description = c.Element("description").Value
                             }).First();

            name_text.Text = note.Name;
            name_text.IsEnabled = false;

            description_text.Text = note.Description;

        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            if (XMLParser.IsEntityPresent(XMLParser.NoteXDocument.Handle, name_text.Text) && !Editing)
            {
                System.Windows.Forms.MessageBox.Show("This note already exists, please try again.");
            }
            else
            {
                if (!XMLParser.IsTextValid(name_text.Text) || !XMLParser.IsTextValid(description_text.Text))
                {
                    required_text.Foreground = Brushes.Red;
                }
                else
                {
                    if (Editing)
                    {
                        XElement noteReference = (from c in XMLParser.NoteXDocument.Handle.Root.Descendants("general_note")
                                                       where c.Element("name").Value.Equals(name_text.Text)
                                                       select c).First();

                        noteReference.Element("description").Value = description_text.Text;
                    }
                    else
                    {
                        XElement newNote = new XElement("general_note",
                        new XElement("name", name_text.Text),
                        new XElement("description", description_text.Text));

                        string temp = newNote.ToString();

                        XMLParser.NoteXDocument.Handle.Root.Add(newNote);
                    }

                    XMLParser.NoteXDocument.Save();

                    Update();
                    Close();
                }
            }

        }

        override public void Update(XDocumentInformation x = null)
        {

        }
    }
}