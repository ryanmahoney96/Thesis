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
        private String EntityType;
        private String EntityName;
        private TextBlock t;

        public static int NoteNodeWidth = 320;
        public static int NoteNodeHeight = 260;

        //TODO: give each notenode a reference to the entity it represents
        //TODO: Add edit and delete buttons to notenode. Because of reference to entity you can manipulate the node and update
        //TODO: after reference is implemented, use this design to update instead of the "redraw all" method used now
        //TODO: make base constuctor. Make Button constructor and Text contructor that call it and each fill the node with that content
        //TODO: Make first notenode the "button" notenode for adding things -> take out the menu style used now

        public NoteNode()
        {
            Border b = new Border();
            b.BorderThickness = new Thickness(1, 1, 1, 1);
            b.BorderBrush = new SolidColorBrush(Colors.DarkBlue);
            b.CornerRadius = new CornerRadius(2, 2, 2, 2);
            b.Padding = new Thickness(1, 1, 1, 1);
            b.Margin = new Thickness(5, 5, 5, 5);
            b.Width = NoteNodeWidth;
            b.Height = NoteNodeHeight;

            StackPanel stackPanel = new StackPanel();
            b.Child = stackPanel;

            Button button = new Button();
            button.Content = "Edit";
            button.Click += EditClick;

            ScrollViewer s = new ScrollViewer();
            stackPanel.Children.Add(s);
            stackPanel.Children.Add(button);

            t = new TextBlock();
            Text = "";
            t.TextWrapping = TextWrapping.Wrap;
            t.FontSize = 15;            
            s.Content = t;
            
            this.Child = b;
        }

        public NoteNode (string type, string name): this()
        {
            EntityType = type;
            EntityName = name;
        }

        public void EditClick(object sender, RoutedEventArgs e)
        {
            //TODO: Edit button
            Text = "Test";
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

        public void FillWith(Dictionary<string, string> entityNode)
        {
            foreach (string key in entityNode.Keys)
            {
                string name = ToTitleCase(key);

                if (!String.IsNullOrWhiteSpace(entityNode[key]))
                {
                    StringBuilder t = new StringBuilder();
                    t.Append(name);
                    t.Append(":\n    ");
                    t.Append(entityNode[key]);
                    t.Append("\n");
                    Text += t.ToString();
                }
            }
        }

        private string ToTitleCase(string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public string Text { get => t.Text; set => t.Text = value; }
    }
}
