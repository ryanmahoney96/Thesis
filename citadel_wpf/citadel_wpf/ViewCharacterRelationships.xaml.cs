﻿using System;
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
    /// Interaction logic for NewCharacterRelationship.xaml
    /// </summary>
    public partial class ViewCharacterRelationships : EntityWindow
    {
        List<CheckBox> allCheckboxes = new List<CheckBox>();
        XElement CurrentCharacterReference = null;

        public ViewCharacterRelationships() : base()
        {
            InitializeComponent();
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref focus_character_combo);

            AttachToXDocument(ref XMLParser.CharacterXDocument);
        }

        private void FocusCharacterChanged(object sender, SelectionChangedEventArgs e)
        {
            //Refill stackpanel
            FillPanelsWithRelationships();
        }

        private void FillPanelsWithRelationships()
        {
            if (focus_character_combo.SelectedItem != null && !string.IsNullOrWhiteSpace(focus_character_combo.SelectedItem.ToString()))
            {
                allCheckboxes.Clear();
                CurrentCharacterReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Elements("character")
                                   where c.Element("name").Value.Equals(focus_character_combo.SelectedItem.ToString().Split(':')[1].Substring(1))
                                   select c).First();

                FillParentPanel();
                FillChildrenPanel();
                FillPreviousPanel();
                FillCurrentPanel();
            }
        }

        private void FillParentPanel()
        {
            parents_stackpanel.Children.Clear();

            var parents = from p in CurrentCharacterReference.Element("parents").Elements()
                          select p;

            FillPanelWithEntities(parents_stackpanel, parents);
        }

        private void FillChildrenPanel()
        {
            children_stackpanel.Children.Clear();

            var children = from c in CurrentCharacterReference.Element("children").Elements()
                          select c;

            FillPanelWithEntities(children_stackpanel, children);
        }

        private void FillPreviousPanel()
        {
            previous_marriages_stackpanel.Children.Clear();

            var prev = from p in CurrentCharacterReference.Element("marriages").Elements("wasmarriedto")
                          select p;

            FillPanelWithEntities(previous_marriages_stackpanel, prev);
        }

        private void FillCurrentPanel()
        {
            current_marriages_stackpanel.Children.Clear();

            var curr = from c in CurrentCharacterReference.Element("marriages").Elements("ismarriedto")
                          select c;

            FillPanelWithEntities(current_marriages_stackpanel, curr);
        }

        private void FillPanelWithEntities(StackPanel panel, IEnumerable<XElement> elements)
        {
            foreach (var r in elements)
            {
                DockPanel s = new DockPanel();

                CheckBox c = new CheckBox();
                c.Tag = r;
                allCheckboxes.Add(c);
                DockPanel.SetDock(c, Dock.Left);
                s.Children.Add(c);

                TextBlock textblock = new TextBlock();
                DockPanel.SetDock(textblock, Dock.Left);
                s.Children.Add(textblock);
                textblock.Text = r.Value;
                textblock.Margin = new Thickness(1);

                panel.Children.Add(s);
                panel.Children.Add(new Separator());
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int numChecked = 0;
            foreach(CheckBox c in allCheckboxes)
            {
                if (c.IsChecked == true)
                {
                    numChecked++;
                }
            }

            if (numChecked > 0) {

                if (MessageBox.Show("Are you sure you want to delete these relationships?", "Delete Relationships", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    foreach(CheckBox c in allCheckboxes)
                    {
                        if (c.IsChecked == true)
                        {
                            XElement x = (XElement)c.Tag;

                            string entityName = x.Value;
                            string entityType = x.Name.ToString();
                            x.Remove();
                            RemoveParallelRelationship(entityType, entityName);
                        }
                    }

                    XMLParser.CharacterXDocument.Save();
                    FillPanelsWithRelationships();
                }
            }
        }

        private void RemoveParallelRelationship(string relationship, string newSourceCharacter)
        {
            string thisCharacterName = CurrentCharacterReference.Element("name").Value;

            string ancestorTag = "parents";
            string descendantTag = "parent";

            if (relationship.Equals("parent"))
            {
                ancestorTag = "children";
                descendantTag = "child";
            }
            else if (relationship.Equals("wasmarriedto"))
            {
                ancestorTag = "marriages";
                descendantTag = "wasmarriedto";
            }
            else if (relationship.Equals("ismarriedto"))
            {
                ancestorTag = "marriages";
                descendantTag = "ismarriedto";
            }

            XElement otherCharacterReference = (from c in XMLParser.CharacterXDocument.Handle.Root.Descendants("character")
                                              where c.Element("name").Value.Equals(newSourceCharacter)
                                              select c).First();

            XElement otherRelationshipReference = (from r in otherCharacterReference.Element(ancestorTag).Elements(descendantTag)
                                                  where r.Value.Equals(thisCharacterName)
                                                  select r).First();

            otherRelationshipReference.Remove();

        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            EntityWindow.InitializeModalWindow(this, new AddCharacter());
        }

        override public void Update(XDocumentInformation x = null)
        {
            string temp = focus_character_combo.Text;
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref focus_character_combo);
            focus_character_combo.Text = temp;
            parents_stackpanel.Children.Clear();
            children_stackpanel.Children.Clear();
            previous_marriages_stackpanel.Children.Clear();
            current_marriages_stackpanel.Children.Clear();
            FillPanelsWithRelationships();
        }

        private void AddRelationship_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(focus_character_combo.Text))
            {
                EntityWindow.InitializeModalWindow(this, new CharacterRelationshipPrompt(focus_character_combo.Text));
            }
        }
    }
}
