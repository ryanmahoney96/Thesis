using System;
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

namespace citadel_wpf
{
    /// <summary>
    /// Interaction logic for NewEntityWindow.xaml
    /// </summary>
    public abstract class NewEntityWindow : Window
    {
        protected string folderPath;
        protected FrontPage frontPageReference;
        protected NewEntityWindow[] reliantWindows;
        protected List<String> controlTexts;

        public NewEntityWindow(string fp, FrontPage fpr, params NewEntityWindow[] rw)
        {
            folderPath = fp;
            frontPageReference = fpr;
            reliantWindows = rw;
            controlTexts = new List<String>();
        }
        public NewEntityWindow()
        {
            folderPath = "";
        }

        protected void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected abstract void Save(object sender, RoutedEventArgs e);

        protected void SaveEntity(object sender, RoutedEventArgs e, List<String> controlTexts, TextBlock required_text, string docName, List<IEntity> entities)
        {

            StreamWriter handle = null;
            bool valid = true;

            //TODO do this before the call -> get rid of control texts
            foreach (String s in controlTexts)
            {
                if (s.Equals(""))
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                try
                {

                    string filePath = folderPath + "\\" + docName + ".xml";

                    //if (File.Exists(filePath))
                    //{
                    //    handle = XMLParserClass.RemoveLastLine(filePath);
                    //}
                    //else
                    //{
                    //    handle = new StreamWriter(filePath, true);
                    //}

                    handle = new StreamWriter(filePath, false);
                    handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<" + docName + ">\n\t");


                    foreach (IEntity entity in entities)
                    {
                        handle.Write(entity.ToXMLString());
                    }

                    handle.Write("</" + docName + ">");

                    handle.Close();

                    //UpdateReliantWindows();

                    Close();
                }
                catch (IOException)
                {
                    System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
                }
                finally
                {
                    if (!handle.Equals(null))
                    {
                        handle.Close();
                    }

                    base.Close();
                }
            }
            else
            {
                required_text.Foreground = Brushes.Red;
            }
        }

        public abstract void UpdateReliantWindows();

    }
}
