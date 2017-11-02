using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using System.Diagnostics;

namespace Launcher_v._1._0
{
    class DataSaver
    {
        public void DataSave(string path, string FilePath)
        {
            SaveTxt(path, FilePath);
            var Datasave = new FileHelperEngine<Paths>();
            var ScoreToSave = Datasave.ReadFile(FilePath);
            Datasave.AppendToFile("Paths.csv", ScoreToSave);

        }
        public void SaveTxt(string path, string FilePath)
        {
            string text = path;
            System.IO.File.WriteAllText(FilePath, text);
        }

    }
}
