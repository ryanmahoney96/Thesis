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

        public static void InitializeModalWindow(Window owner, Window child)
        {
            //w.Show();
            //w.Topmost = true;
            child.Owner = owner;
            child.ShowDialog();
            //TODO make these windows strictly modal -> spread to all necessary windows
            child.Topmost = false;
        }

        protected void Cancel_and_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected abstract void Save(object sender, RoutedEventArgs e);

        protected bool SaveEntity(object sender, RoutedEventArgs e, List<String> controlTexts, TextBlock required_text, string docName, string entityName, string entityAsXML)
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

                    if (File.Exists(filePath))
                    {
                        handle = XMLEntityParser.RemoveLastLine(filePath);
                    }
                    else
                    {
                        handle = new StreamWriter(filePath, true);
                        handle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<" + docName + ">\n\t");
                    }
                    
                    //TODO
                    //Verify entity is not already present -> otherwise return false
                    //Add entity and return true

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

            return true;
        }

        public abstract void UpdateReliantWindows();

    }
}
