using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Task.Utils
{
    public static class LogTrace
    {
        private static StreamWriter sw = null;
        private static string filename = "log_step.txt";

        private static void Init()
        {
            if (sw == null)
                sw = new StreamWriter(filename, true);
        }

        public static void WriteInLog(string value)
        {
            Init();
            sw.WriteLine(value);
            sw.Flush();
        }

        public static void CloseLogFile()
        {
            sw.Flush();
            sw.Close();
            sw = null;
        }
    }
}
