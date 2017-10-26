using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class GraphConstruction
    {
        private string folderPath;

        public GraphConstruction(string fp)
        {
            folderPath = fp;

        }

        public void TestGraphviz()
        {

            string echo = "digraph G { Hello->World}";
            string path = folderPath + "\\temp.dot";
            StreamWriter streamWriter = File.CreateText(path);
            streamWriter.Write(echo);
            streamWriter.Close();
            
            //Process process = new Process();
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = @"/C" + $"dot -Tpng {path} -o {folderPath}/outfile.png";
            //process.StartInfo = startInfo;

            Process.Start("cmd.exe", @"/C" + $"dot -Tpng {path} -o {folderPath}/outfile.png");
        }

    }
}
