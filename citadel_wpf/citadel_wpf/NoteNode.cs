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
        //TODO use this as identifier -> type\name
        private String referenceKey;
        private TextBlock t;

        public static int NoteNodeWidth = 320;
        public static int NoteNodeHeight = 150;

        //TODO: give each notenode a reference to the entity it represents -> must change Fill_Note_area to accept Entity list
        //TODO: Add edit and delete buttons to notenode. Because of reference to entity and oop design you can manipulate the node and update
        //TODO: after reference is implemented, use this design to update instead of the "redraw all" method used now
        //TODO: make base constuctor. Make Button constructor and Text contructor that call it and each fill the node with that content
        //TODO: Make first notenode the "button" notenode for adding things -> take out the menu style used now

        public NoteNode()
        {
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
            b.Width = NoteNodeWidth;
            b.Height = NoteNodeHeight;
            //button.Content = "Edit";
            s.Content = t;
            b.Child = s;
            b.Padding = new Thickness(1, 1, 1, 1);
            b.Margin = new Thickness(5, 5, 5, 5);
            this.Child = b;
        }

        public NoteNode (string type, string name): this()
        {
            referenceKey = type + "\\" + name;
        }

        public string Text { get => t.Text; set => t.Text = value; }
    }
}
