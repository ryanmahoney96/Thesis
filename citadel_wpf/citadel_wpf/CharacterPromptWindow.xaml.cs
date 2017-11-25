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
    public partial class CharacterPromptWindow : NewEntityWindow
    {
        private Action<string> action;

        public CharacterPromptWindow(Action<string> a)
        {
            InitializeComponent();
            action = a;
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref characterName);
        }

        public override void UpdateReliantWindows()
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
            if (!String.IsNullOrWhiteSpace(characterName.Text))
            {
                action(characterName.Text);
                Close();
            }
            else
            {
                requiredText.Foreground = Brushes.Red;
            }
        }
    }
}
