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
    /// Interaction logic for NewCharacterWindow.xaml
    /// </summary>
    public partial class NewWindowFunctions : Window
    {

        public static StreamWriter RemoveLastLine(string filePath)
        {
            //Deletes the last line in the xml file, the closing content tag
            string line = null;
            List<string> deferredLines = new List<string>();
            using (TextReader inputReader = new StreamReader(filePath))
            {
                while ((line = inputReader.ReadLine()) != null)
                {
                    deferredLines.Add(line);
                }
                deferredLines.RemoveAt(deferredLines.Count - 1);
            }

            StreamWriter stream = new StreamWriter(filePath, false);

            for (int i = 0; i < deferredLines.Count; i++)
            {
                stream.Write(deferredLines[i]);
            }

            return stream;
        }
    }
}
