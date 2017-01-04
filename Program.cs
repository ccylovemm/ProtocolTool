using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Directory.Exists("Xml"))
            {
                if (Directory.Exists("Out"))
                {
                    DirectoryInfo outFile = new DirectoryInfo("Out");
                    outFile.Delete(true);
                }
                string[] files = Directory.GetFiles("Xml", "*.xml");
                for (int i = 0; i < files.Length; i++)
                {
                    ProtocolExport.Export(files[i]);
                }
                Console.Write("导出结束");
            }
        }
    }
}
