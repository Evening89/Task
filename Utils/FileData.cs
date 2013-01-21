using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Task.Utils
{
    public class FileData
    {
        public string item { get; set; }

        public void Piece(string line)
        {
            string[] parts = line.Split(';');  //Разделитель в CSV файле
            item = parts[1];
        }

        public static List<FileData> ReadFile(string filename)
        {
            List<FileData> res = new List<FileData>();
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    FileData filedata = new FileData();
                    filedata.Piece(line);
                    res.Add(filedata);
                }
            }

            return res;
        }
    }
}
