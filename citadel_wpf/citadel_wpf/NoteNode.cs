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
        private XDocument EntityType;
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

        public NoteNode(XDocument source)
        {
            EntityType = source;
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
            //TODO Actually Edit 
            NewCharacterWindow ncw = new NewCharacterWindow(null);
            ncw.FillWith("Ryan");
            EntityWindow.InitializeModalWindow(null, ncw);

            //ContentText = "Test";
            //NewEntityWindow newWindow;

            //if (EntityType.Equals("character"))
            //{
            //    //new window = character window -> open modally
            //}
            //else if (EntityType.Equals("event"))
            //{

            //}
            //else if (EntityType.Equals("location"))
            //{

            //}
            //else
            //{

            //}

        }

        public double FillWith(Dictionary<string, string> entityNode)
        {
            foreach (string key in entityNode.Keys)
            {
                string name = ToTitleCase(entityNode[key]);

                if (!string.IsNullOrWhiteSpace(entityNode[key]))
                {
                    if (key.Equals("name"))
                    {
                        NameText = name;
                    }
                    else
                    {
                        //StringBuilder t = new StringBuilder();
                        //if (!string.IsNullOrWhiteSpace(ContentText))
                        //{
                        //    t.Append("\n");
                        //}
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

                        //t.Append(ToTitleCase(key));
                        //t.Append(":\n    ");
                        //t.Append(entityNode[key]);
                        //ContentText += t.ToString();
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
