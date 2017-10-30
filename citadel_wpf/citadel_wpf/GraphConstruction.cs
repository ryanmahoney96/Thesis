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
            //Procedure: construct the proper dot information, output to a file, open cmd to parse that file
            string echo = "digraph G { Hello->World}";
            echo = "graph s { label=\"Test\"; A -- B; B -- C; subgraph cluster01 { label =\"\" D -- E } }";
            string path = folderPath + "\\citadel.dot";
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
