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
    public partial class BaseRelationshipPrompt : EntityWindow
    {
        string FocusEntity;
        string EntityName;
        string[] Relationships;
        XMLParser.XDocumentInformation EntityType;

        public BaseRelationshipPrompt(string fe, string en, string[] r, ref XMLParser.XDocumentInformation x, params EntityWindow[] rw) : base(rw)
        {
            InitializeComponent();

            EntityType = x;

            FocusEntity = fe;
            focus_entity.Text = FocusEntity;

            EntityName = en;

            Relationships = r;

            focus_entity.Text = FocusEntity;
            FocusTitle.Text = "Select the " + EntityName + " and Its Relationship";
            SecondEntity.Text += EntityName;
            AddButton.Content += EntityName;

            XMLParser.FillComboboxWithNames(EntityType.Handle, ref entity_two_combo, FocusEntity);
            FillComboboxWithRelationshipTypes(ref relationship_combo);
        }

        protected void FillComboboxWithRelationshipTypes(ref ComboBox c)
        {
            c.Items.Clear();

            ComboBoxItem cBoxItem;

            foreach (string r in Relationships)
            {
                cBoxItem = new ComboBoxItem();
                cBoxItem.Content = r;
                relationship_combo.Items.Add(cBoxItem);
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {

            string relationship = relationship_combo.Text;
            string opposite = Relationships[0];
            if (relationship.Equals(Relationships[0]))
            {
                opposite = Relationships[1];
            }
            else if (relationship.Equals(Relationships[2]))
            {
                opposite = Relationships[2];
            }

            if (!XMLParser.IsTextValid(relationship_combo.Text) || !XMLParser.IsTextValid(entity_two_combo.Text))
            {
                required_text.Foreground = Brushes.Red;
            }

            else
            {
                if (XMLParser.IsRelationshipPresent(EntityType.Handle, FocusEntity, relationship, entity_two_combo.Text)
                || XMLParser.IsRelationshipPresent(EntityType.Handle, entity_two_combo.Text, opposite, FocusEntity))
                {
                    System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
                }
                else
                {
                    XElement newEntityRelationship = new XElement("event_relationship",
                    new XElement("entity_one", FocusEntity),
                    new XElement("relationship", relationship),
                    new XElement("entity_two", entity_two_combo.Text));

                    EntityType.Handle.Root.Add(newEntityRelationship);

                    newEntityRelationship = new XElement("event_relationship",
                    new XElement("entity_one", entity_two_combo.Text),
                    new XElement("relationship", opposite),
                    new XElement("entity_two", FocusEntity));

                    EntityType.Handle.Root.Add(newEntityRelationship);

                    EntityType.Save();

                    UpdateReliantWindows();
                    if (MessageBox.Show($"Would you like to create another relationship for \"{FocusEntity}?\"", "Create Another Relationship", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        Close();
                    }
                }
            }
        }

        private void Add_Entity(object sender, RoutedEventArgs e)
        {
            EntityWindow temp;
            if (EntityType.Name.Equals(XMLParser.CharacterXDocument.Name))
            {
                temp = new NewCharacterWindow(this);
            }
            else
            {
                temp = new NewEventWindow(this);
            }

            EntityWindow.InitializeModalWindow(this, temp);
            XMLParser.FillComboboxWithNames(EntityType.Handle, ref entity_two_combo, FocusEntity);
        }

        override public void UpdateReliantWindows()
        {

            foreach (EntityWindow w in reliantWindows)
            {
                w.UpdateReliantWindows();
            }
        }

    }
}
