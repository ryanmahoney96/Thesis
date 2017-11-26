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
    /// Interaction logic for NewCharacterWindow.xaml
    /// </summary>
    public partial class NewCharacterWindow : EntityWindow, INewEntity
    {
        private bool Editing = false;

        public NewCharacterWindow(params EntityWindow[] rw) : base(rw)
        {
            InitializeComponent();
        }

        public void FillWith(string characterName)
        {
            Editing = true;

            var character = (from c in XMLParser.CharacterXDocument.Handle.Root.Descendants("character")
                             where c.Element("name").Value.Equals(characterName)
                             select new
                             {
                                 Name = c.Element("name").Value,
                                 Gender = c.Element("gender").Value,
                                 Description = c.Element("description").Value
                             }).First();

            name_text.Text = character.Name;
            name_text.IsEnabled = false;

            gender_combo_box.Text = character.Gender;
            description_text.Text = character.Description;

        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO make this method generic in NewEntityWindow?
            if (XMLParser.IsEntityPresent(XMLParser.CharacterXDocument.Handle, name_text.Text) && !Editing)
            {
                System.Windows.Forms.MessageBox.Show("This character already exists, please try again.");
            }
            else
            {
                if (!XMLParser.IsTextValid(name_text.Text))
                {
                    required_text.Foreground = Brushes.Red;
                }
                else
                {

                    if (Editing)
                    {
                        XElement characterReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Descendants("character")
                        where c.Element("name").Value.Equals(name_text.Text)
                        select c).First();

                        characterReference.Element("gender").Value = gender_combo_box.Text;
                        characterReference.Element("description").Value = description_text.Text;
                    }
                    else
                    {
                        XElement newCharacter = new XElement("character",
                            new XElement("name", name_text.Text),
                            new XElement("gender", gender_combo_box.Text),
                            new XElement("description", description_text.Text));

                        string temp = newCharacter.ToString();

                        XMLParser.CharacterXDocument.Handle.Root.Add(newCharacter);
                    }

                    XMLParser.CharacterXDocument.Save();

                    UpdateReliantWindows();
                    Close();
                }
            }
        }

        override public void UpdateReliantWindows()
        {
            FrontPage.FrontPageReference.Update_Characters();

            foreach (EntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }
    }

}
