﻿using System;
using System.Collections.Generic;
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

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FrontPage : Window
    {
        private String folderPath;

        public FrontPage(String fp)
        {
            InitializeComponent();
            folderPath = fp;
            Title += " - " + folderPath;
        }

        private void New_Note_Click(object sender, RoutedEventArgs e)
        {
            NewGeneralNote ngn = new NewGeneralNote(folderPath);
            ngn.Show();
            ngn.Topmost = true;
        }

        private void New_Character_Click(object sender, RoutedEventArgs e)
        {
            //testHeader.Header = XMLParserClass.XPathParse(folderPath);
            NewCharacterWindow ncw = new NewCharacterWindow(folderPath);
            ncw.Show();
            ncw.Topmost = true;
        }

        private void New_Event_Click(object sender, RoutedEventArgs e)
        {

        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            NewLocationWindow nlw = new NewLocationWindow(folderPath);
            nlw.Show();
            nlw.Topmost = true;
        }

        private void Media_Note_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //testHeader.Content = XMLParserClass.attemptParse();
            //testHeader.Content = XMLParserClass.attemptSpecificParse();
            testButton.Content = XMLParserClass.XPathParse(folderPath + "\\character_notes.xml", "/characters/character/*");
            testButton.Content = XMLParserClass.LINQParseTest(folderPath + "\\character_notes.xml");
        }

    }
}