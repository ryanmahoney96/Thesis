using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace citadel_wpf
{
    class NoteNode : Decorator
    {
        private TextBlock t;

        public NoteNode()
        {
            //TODO: add edit button to bottom corner
            Border b = new Border();
            //Button button = new Button();
            b.BorderThickness = new Thickness(1, 1, 1, 1);
            b.BorderBrush = new SolidColorBrush(Colors.DarkBlue);
            b.CornerRadius = new CornerRadius(2, 2, 2, 2);
            ScrollViewer s = new ScrollViewer();
            t = new TextBlock();
            Text = "";
            t.TextWrapping = TextWrapping.Wrap;
            t.FontSize = 15;
            b.Width = 320;
            b.Height = 150;
            //button.Content = "Edit";
            s.Content = t;
            b.Child = s;
            b.Padding = new Thickness(1, 1, 1, 1);
            b.Margin = new Thickness(5, 5, 5, 5);
            this.Child = b;
        }

        public string Text { get => t.Text; set => t.Text = value; }
    }
}
