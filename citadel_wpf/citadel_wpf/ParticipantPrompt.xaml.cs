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
using System.Xml.Linq;

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for ParticipantPrompt.xaml
    /// </summary>
    public partial class ParticipantPrompt : EntityWindow
    {
        public const string ParticipatedIn = "Participated In";

        string FocusEvent;

        public ParticipantPrompt(string fe) : base()
        {
            InitializeComponent();

            FocusEvent = fe;
            focus_event.Text = FocusEvent;

            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_combo, FocusEvent);
            relationship.Text = ParticipatedIn;

            AttachToXDocument(ref XMLParser.CharacterXDocument);
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            if ( !XMLParser.IsTextValid(character_combo.Text))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                XElement EventRef = (from ev in XMLParser.EventXDocument.Handle.Root.Elements("event")
                                where ev.Element("name").Value.ToString().Equals(FocusEvent)
                                select ev).First();
                bool relationshipPresentAlready = (from ev in EventRef.Element("participants").Elements()
                                         where ev.Value.Equals(character_combo.Text)
                                         select ev).Count() > 0 ? true : false;

                if (relationshipPresentAlready)
                {
                    System.Windows.Forms.MessageBox.Show("This participant already exists, please try again.");
                }
                else
                {
                    XElement newParticipant = new XElement("character_name", character_combo.Text);

                    EventRef.Element("participants").Add(newParticipant);

                    XMLParser.EventXDocument.Save();

                    if (MessageBox.Show($"Would you like to add another participant of \"{FocusEvent}?\"", "Add Another Participant", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        Close();
                    }
                }
            }
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddCharacter());
        }

        override public void Update(XDocumentInformation x = null)
        {
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_combo, FocusEvent);

        }

    }
}
