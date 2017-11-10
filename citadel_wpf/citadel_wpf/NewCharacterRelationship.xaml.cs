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
    /// Interaction logic for NewCharacterRelationship.xaml
    /// </summary>
    public partial class NewCharacterRelationship : NewEntityWindow
    {

        //TODO: Add "Other" field for other types of relationships
        //TODO: Add the relationships on startup

        public NewCharacterRelationship() : base()
        {
            InitializeComponent();
            Fill_Character_Boxes();
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO make functional
            //if (IsPresent(XMLEntityParser.GetInstance().GetCharacterXDocument(), name_text.Text))
            //{
            //    System.Windows.Forms.MessageBox.Show("This character already exists, please try again.");
            //}
            //else
            //{
            //    if (!character_one_combo.Text.Equals("") && !relationship_combo.Text.Equals("") && !character_two_combo.Text.Equals(""))
            //    {
            //        required_text.Foreground = Brushes.Red;
            //    }
            //    else
            //    {
            //        XElement newCharacter = new XElement("character",
            //        new XElement("name", name_text.Text),
            //        new XElement("gender", gender_combo_box.Text),
            //        new XElement("description", description_text.Text));

            //        string temp = newCharacter.ToString();

            //        XMLEntityParser.GetInstance().GetCharacterXDocument().Root.Add(newCharacter);
            //        XMLEntityParser.GetInstance().GetCharacterXDocument().Save(FrontPage.FolderPath + "\\character_notes.xml");

            //        UpdateReliantWindows();
            //        Close();
            //    }
            //}
        }

        private void Fill_Character_Boxes()
        {
            character_one_combo.Items.Clear();
            character_two_combo.Items.Clear();

            List<string> characterNames = XMLEntityParser.GetAllNames(XMLEntityParser.GetInstance().GetCharacterXDocument());

            ComboBoxItem cBoxItem;

            foreach (string character in characterNames)
            {
                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = character;
                character_one_combo.Items.Add(cBoxItem);

                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = character;
                character_two_combo.Items.Add(cBoxItem);
            }
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewCharacterWindow(this));
        }

        public override void UpdateReliantWindows()
        {
            Fill_Character_Boxes();
        }
    }
}
