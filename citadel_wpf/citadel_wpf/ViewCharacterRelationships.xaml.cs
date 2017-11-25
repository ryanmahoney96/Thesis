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
    public partial class ViewCharacterRelationships : NewEntityWindow
    {

        //TODO: Add an "ADD" button so that you don't have to keep going back and forth between windows for one character
        //TODO: If the above is done, make it so any individual relationship that already exists is skipped/marked instead
        //TODO: Add the relationships on startup

        public ViewCharacterRelationships() : base()
        {
            InitializeComponent();
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref focus_character_combo);
            FillPanelWithRelationships(relationship_stackpanel);
        }

        private void focus_character_changed(object sender, SelectionChangedEventArgs e)
        {
            //XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref character_two_combo, character_one_combo.Text);
            //Refill stackpanel
            FillPanelWithRelationships(relationship_stackpanel);
        }

        private void FillPanelWithRelationships(StackPanel stackpanel)
        {
            if (focus_character_combo.SelectedItem != null && !string.IsNullOrWhiteSpace(focus_character_combo.SelectedItem.ToString()))
            {
                string fc = focus_character_combo.SelectedItem.ToString().Split(':')[1].Substring(1);
                var results = from c in XMLParser.CharacterRelationshipXDocument.Handle.Root.Descendants("character_relationship")
                              where c.Element("entity_one").Value.ToString().Equals(fc)
                              select new
                              {
                                  Relationship = c.Element("relationship").Value.ToString(),
                                  Entity_Two = c.Element("entity_two").Value.ToString(),
                              };

                stackpanel.Children.Clear();

                foreach (var r in results)
                {
                    WrapPanel panel = new WrapPanel();
                    TextBlock textblock = new TextBlock();
                    textblock.Text = fc + " " + r.Relationship + " " + r.Entity_Two;
                    textblock.Margin = new Thickness(3);
                    Button deleteButton = new Button();
                    deleteButton.Click += DeleteButton_Click;
                    deleteButton.Content = "Delete";
                    deleteButton.Margin = new Thickness(3);
                    deleteButton.Width = 60;
                    panel.Children.Add(textblock);
                    panel.Children.Add(deleteButton);
                    stackpanel.Children.Add(panel);
                    stackpanel.Children.Add(new Separator());
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this relationship?", "Delete Relationship", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //TODO functionality
                //Delete
            }
        }

        override protected void Save(object sender, RoutedEventArgs e)
        {
            //TODO is married to
            //TODO was married to
            //string relationship = relationship_combo.Text;
            //string opposite = "Is the Child of";
            //if (relationship.Equals("Is the Child of"))
            //{
            //    opposite = "Is the Parent of";
            //}

            //if (character_one_combo.Text.Equals("") || relationship_combo.Text.Equals("") || character_two_combo.Text.Equals(""))
            //{
            //    required_text.Foreground = Brushes.Red;
            //}
            
            //else
            //{
            //    if (XMLParser.IsRelationshipPresent(XMLParser.CharacterRelationshipXDocument.Handle, character_one_combo.Text, relationship, character_two_combo.Text)
            //    || XMLParser.IsRelationshipPresent(XMLParser.CharacterRelationshipXDocument.Handle, character_two_combo.Text, opposite, character_one_combo.Text))
            //    {
            //        System.Windows.Forms.MessageBox.Show("This relationship already exists, please try again.");
            //    }
            //    else
            //    {
            //        XElement newCharacterRelationship = new XElement("character_relationship",
            //        new XElement("entity_one", character_one_combo.Text),
            //        new XElement("relationship", relationship),
            //        new XElement("entity_two", character_two_combo.Text));

            //        XMLParser.CharacterRelationshipXDocument.Handle.Root.Add(newCharacterRelationship);

            //        newCharacterRelationship = new XElement("character_relationship",
            //        new XElement("entity_one", character_two_combo.Text),
            //        new XElement("relationship", opposite),
            //        new XElement("entity_two", character_one_combo.Text));

            //        XMLParser.CharacterRelationshipXDocument.Handle.Root.Add(newCharacterRelationship);


            //        XMLParser.CharacterRelationshipXDocument.Handle.Save(XMLParser.FolderPath + "\\character_relationship_notes.xml");

            //        UpdateReliantWindows();
            //        Close();
            //    }
            //}
        }

        private void Add_Character(object sender, RoutedEventArgs e)
        {
            NewEntityWindow.InitializeModalWindow(this, new NewCharacterWindow(this));
        }

        public override void UpdateReliantWindows()
        {
            XMLParser.FillComboboxWithNames(XMLParser.CharacterXDocument.Handle, ref focus_character_combo);
            focus_character_combo.Text = "";
            relationship_stackpanel.Children.Clear();
            //Get all relationships
        }

        private void add_relationship_button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(focus_character_combo.Text))
            {
                NewEntityWindow.InitializeModalWindow(this, new RelationshipPrompt(focus_character_combo.Text, this));
            }
        }
    }
}