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
        private string EntityType;
        private TextBlock NameTextBlock;
        private StackPanel ContentStackPanel;
        private StackPanel MainPanel;
        private Border borderLine;

        public static int NameBoxHeight = 24;
        public static int ContentBoxHeight = 175;
        public static int ButtonHeight = 24;
        public static int ButtonWidth = 40;

        public static int NoteNodeWidth = 350;
        public static int NoteNodeHeight = NameBoxHeight + ContentBoxHeight + ButtonHeight + 25;

        public NoteNode(string sourceName)
        {
            EntityType = sourceName;
            borderLine = new Border();
            borderLine.BorderThickness = new Thickness(1, 1, 1, 1);
            borderLine.BorderBrush = new SolidColorBrush(Colors.DarkBlue);
            borderLine.CornerRadius = new CornerRadius(2, 2, 2, 2);
            borderLine.Padding = new Thickness(1, 1, 1, 1);
            borderLine.Margin = new Thickness(5, 5, 5, 5);
            borderLine.Width = NoteNodeWidth;
            borderLine.MaxHeight = NoteNodeHeight;

            MainPanel = new StackPanel();
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
            editButton.Content = "Edit";
            editButton.Click += EditClick;
            editButton.Height = ButtonHeight;
            editButton.Width = ButtonWidth;
            editButton.Margin = new Thickness(2);
            wrap.Children.Add(editButton);

            Button deleteButton = new Button();
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
            if (MessageBox.Show($"Are you sure you want to delete \"{NameText}?\"", "Delete Entity", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //TODO Delete it
            }
        }

        public void EditClick(object sender, RoutedEventArgs e)
        {
            
            INewEntity newWindow;

            if (EntityType.Equals(XMLParser.CharacterXDocument.Name))
            {
                newWindow = new NewCharacterWindow();
            }
            else if (EntityType.Equals(XMLParser.EventXDocument.Name))
            {
                newWindow = new NewEventWindow();
            }
            else if (EntityType.Equals(XMLParser.LocationXDocument.Name))
            {
                newWindow = new NewLocationWindow();
            }
            else
            {
                newWindow = new NewGeneralNote();
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
                    else
                    {

                        TextBlock titleText = new TextBlock();
                        titleText.Text = ToTitleCase(key);
                        titleText.TextWrapping = TextWrapping.Wrap;
                        titleText.FontSize = 16;
                        titleText.Foreground = Brushes.DarkBlue;
                        ContentStackPanel.Children.Add(titleText);

                        TextBlock contentText = new TextBlock();
                        contentText.Text = "\t" + entityNode[key];
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

    }
}
