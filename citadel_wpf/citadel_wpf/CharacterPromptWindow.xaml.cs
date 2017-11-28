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
    /// Interaction logic for CharacterPromptWindow.xaml
    /// </summary>
    public partial class CharacterPromptWindow : EntityWindow
    {
       
        public CharacterPromptWindow()
        {
            InitializeComponent();

            ComboBoxItem cBoxItem;

            foreach (string r in FamilyTreeConstruction.Relationships)
            {
                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = r;
                treeType.Items.Add(cBoxItem);
            }

            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref characterName);
        }

        override public void UpdateReliantWindows()
        {
            //
        }

        protected override void Save(object sender, RoutedEventArgs e)
        {
            //
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (XMLParser.IsTextValid(treeType.Text) && XMLParser.IsTextValid(characterName.Text))
            {
                if (treeType.Text.Equals(FamilyTreeConstruction.ImmediateFamilyTreeString))
                {
                    FamilyTreeConstruction.ImmediateFamilyTree(characterName.Text);
                }
                else if (treeType.Text.Equals(FamilyTreeConstruction.ExtendedFamilyTreeString))
                {
                    FamilyTreeConstruction.ExtendedFamilyTree(characterName.Text);
                }
                else
                {
                    FamilyTreeConstruction.RecursiveFullFamilyTree(characterName.Text);
                }

                Close();
            }
            else
            {
                requiredText.Foreground = Brushes.Red;
            }
        }
    }
}
