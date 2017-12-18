using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace citadel_wpf
{
    class NoteNode : Decorator
    {

        private string EntityName;
        private XDocumentInformation EntityType;
        private TextBlock NameTextBlock;
        private StackPanel ContentStackPanel;
        private StackPanel MainPanel;
        private Border borderLine;

        public static int NameBoxHeight = 24;
        public static int ContentBoxHeight = 225;
        public static int ButtonHeight = 24;
        public static int ButtonWidth = 40;

        public static double NoteNodeWidth = SystemParameters.PrimaryScreenWidth * 0.28;

        public static double NoteNodeHeight = NameBoxHeight + ContentBoxHeight + ButtonHeight + 25;

        public NoteNode(ref XDocumentInformation source)
        {
            EntityType = source;
            borderLine = new Border();
            borderLine.BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
            borderLine.BorderBrush = new SolidColorBrush(Colors.Black);
            //borderLine.CornerRadius = new CornerRadius(2, 2, 2, 2);
            borderLine.Padding = new Thickness(1, 1, 1, 1);
            borderLine.Margin = new Thickness(5, 5, 5, 5);
            borderLine.Width = NoteNodeWidth;
            borderLine.MaxHeight = NoteNodeHeight;

            MainPanel = new StackPanel();
            MainPanel.Background = Brushes.White;
            borderLine.Child = MainPanel;

            NameTextBlock = new TextBlock();
            NameTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            NameTextBlock.FontSize = 16;
            NameTextBlock.Height = NameBoxHeight;
            MainPanel.Children.Add(NameTextBlock);

            MainPanel.Children.Add(new Separator());

            ScrollViewer scrollView = new ScrollViewer();
            scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollView.Padding = new Thickness(5);
            scrollView.Height = ContentBoxHeight;

            ContentStackPanel = new StackPanel();
            //ContentTextBlock.TextWrapping = TextWrapping.Wrap;
            //ContentTextBlock.FontSize = 15;
            scrollView.Content = ContentStackPanel;
            MainPanel.Children.Add(scrollView);

            MainPanel.Children.Add(new Separator());

            WrapPanel wrap = new WrapPanel();
            wrap.HorizontalAlignment = HorizontalAlignment.Right;

            Button editButton = new Button();
            ButtonStylize(editButton);
            editButton.Content = "Edit";
            editButton.Click += EditClick;
            editButton.Height = ButtonHeight;
            editButton.Width = ButtonWidth;
            editButton.Margin = new Thickness(2);
            wrap.Children.Add(editButton);

            Button deleteButton = new Button();
            ButtonStylize(deleteButton);
            deleteButton.Content = "Delete";
            deleteButton.Click += DeleteClick; ;
            deleteButton.Height = ButtonHeight;
            deleteButton.Width = ButtonWidth;
            deleteButton.Margin = new Thickness(2);
            wrap.Children.Add(deleteButton);

            MainPanel.Children.Add(wrap);

            this.Child = borderLine;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete \"{EntityName}?\" (This includes any relationships it has with other entities)", "Delete Entity", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                var entity = (from c in EntityType.Handle.Root.Elements()
                             where c.Element("name").Value.Equals(EntityName)
                             select c).First();
                entity.Remove();
                EntityType.Save();

                if (EntityType.Name.Equals(XMLParser.CharacterXDocument.Name))
                {
                    //Remove from parents, children, ismarried, wasmarried, participants 
                    XMLParser.RemoveCharacterFromRelationships(EntityName);
                }
                else if (EntityType.Name.Equals(XMLParser.LocationXDocument.Name))
                {
                    //remove from event locations
                    XMLParser.RemoveLocationFromEvents(EntityName);
                }

            }
        }

        public void EditClick(object sender, RoutedEventArgs e)
        {
            
            INewEntity newWindow;

            if (EntityType.Name.Equals(XMLParser.CharacterXDocument.Name))
            {
                newWindow = new AddCharacter();
            }
            else if (EntityType.Name.Equals(XMLParser.EventXDocument.Name))
            {
                newWindow = new AddEvent();
            }
            else if (EntityType.Name.Equals(XMLParser.LocationXDocument.Name))
            {
                newWindow = new AddLocation();
            }
            else
            {
                newWindow = new AddGeneralNote();
            }

            newWindow.FillWith(EntityName);
            EntityWindow.InitializeModalWindow(FrontPage.FrontPageReference, (EntityWindow)newWindow);
        }

        public double FillWith(Dictionary<string, string> entityNode)
        {
            foreach (string key in entityNode.Keys)
            {
                string fieldName = entityNode[key];
                    //ToTitleCase(entityNode[key]);

                if (!string.IsNullOrWhiteSpace(entityNode[key]))
                {
                    if (key.Equals("name"))
                    {
                        NameText = fieldName;
                        EntityName = entityNode[key];
                    }
                    else if (!key.Equals("order_key") && !key.Equals("children") && !key.Equals("parents") && !key.Equals("marriages") && !key.Equals("participants"))
                    {
                        string keyContent = entityNode[key];
                        string keyTitle = key;

                        if (key.Equals("unit_date"))
                        {
                            keyTitle = key.Replace('_', ' ');
                        }
                        TextBlock titleText = new TextBlock();
                        titleText.Text = ToTitleCase(keyTitle);
                        titleText.TextWrapping = TextWrapping.Wrap;
                        titleText.FontSize = 16;
                        titleText.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF600060"));
                        ContentStackPanel.Children.Add(titleText);


                        TextBlock contentText = new TextBlock();
                        contentText.Text = "\t" + keyContent;
                        contentText.TextWrapping = TextWrapping.Wrap;
                        contentText.FontSize = 15;
                        contentText.Foreground = Brushes.Black;
                        ContentStackPanel.Children.Add(contentText);

                    }
                }
            }
            //return MainPanel.ExtentHeight;
            //return (NameTextBlock.ActualHeight + ContentTextBlock.ActualHeight + 50) / 2;
            return NoteNodeHeight / 2;
        }

        private string ToTitleCase(string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public string NameText { get => NameTextBlock.Text; set => NameTextBlock.Text = value; }

        public static void ButtonStylize(Button b)
        {
            //b.Background = Brushes.Silver;
            b.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFd0d0d0"));
            b.Foreground = Brushes.Black;
            b.BorderBrush = Brushes.Black;
            b.BorderThickness = new Thickness(0, 0, 0, 1);
        }

    }
}
