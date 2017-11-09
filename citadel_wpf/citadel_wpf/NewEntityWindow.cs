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
using System.Xml.Linq;

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

        protected bool SaveEntity(object sender, RoutedEventArgs e, XDocument handle, string docName, string entityName, string entityAsXML)
        {
            StreamWriter fileHandle = null;
            string docURI = folderPath + $"\\{docName}.xml";

            try
            {
                
                bool present = (from c in handle.Root.Elements()
                                where c.Element("name").Value.ToString().Equals(entityName)
                                select c).Count() > 0 ? true : false;

                if (present)
                {
                    return false;
                }
                else
                {
                    if (File.Exists(docURI))
                    {
                        fileHandle = XMLEntityParser.RemoveEndTag(docURI);
                    }
                    else
                    {
                        fileHandle = new StreamWriter(docURI, true);
                        fileHandle.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><" + docName + ">");
                    }

                    fileHandle.Write(entityAsXML);


                    fileHandle.Write("</" + docName + ">");

                    fileHandle.Close();

                    //UpdateReliantWindows();

                    Close();
                    XMLEntityParser.GetInstance().UpdateHandles();

                    return true;
                }


            }
            catch (IOException)
            {
                System.Windows.Forms.MessageBox.Show("An IO Error Occurred. Please Try Again.");
            }
            //catch (Exception)
            //{
            //    System.Windows.Forms.MessageBox.Show("An Unexpected Error Occurred.");
            //}
            finally
            {
                if (fileHandle != null)
                {
                    fileHandle.Close();
                }

                base.Close();
            }
            

            return false;

            //else
            //{
            //    required_text.Foreground = Brushes.Red;
            //}
        }

        public abstract void UpdateReliantWindows();

    }
}
