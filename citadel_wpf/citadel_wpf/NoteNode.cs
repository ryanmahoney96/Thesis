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
        public static int ButtonHeight = 22;
        public static int ButtonWidth = 40;

        public static int NoteNodeWidth = 350;
        public static int NoteNodeHeight = NameBoxHeight + ContentBoxHeight + ButtonHeight + 25;

        //TODO: give each notenode a reference to the entity it represents
        //TODO: Add edit and delete buttons to notenode. Because of reference to entity you can manipulate the node and update
        //TODO: make base constuctor. Make Button constructor and Text contructor that call it and each fill the node with that content

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

            Button editButton = new Button();
            editButton.Content = "Edit";
            editButton.Click += EditClick;
            editButton.Height = ButtonHeight;
            editButton.Width = ButtonWidth;

            MainPanel.Children.Add(editButton);

            this.Child = borderLine;
        }

        public void EditClick(object sender, RoutedEventArgs e)
        {
            //TODO: Edit button
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

                if (!String.IsNullOrWhiteSpace(entityNode[key]))
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
        //public string ContentText { get => ContentTextBlock.Text; set => ContentTextBlock.Text = value; }

    }
}
