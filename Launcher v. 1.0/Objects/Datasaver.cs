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
        private string fileToSaveTo;
        public string FileToSaveTo { get => fileToSaveTo; set => fileToSaveTo = value; }
        public DataSaver(string FileToSaveTo)
        {
            this.FileToSaveTo = FileToSaveTo;
        }
        public void DataSave(string Text, string FilePath)
        {
            SaveTxt(Text, FilePath);
            var Datasave = new FileHelperEngine<Paths>();
            var ScoreToSave = Datasave.ReadFile(FilePath);
            Datasave.AppendToFile(FileToSaveTo, ScoreToSave);
        }
        public void DataSaveInfo(string info,string path, string FilePath)
        {
            SaveTxtInfo(path,info, FilePath);

            var Datasave = new FileHelperEngine<Info>();
            var ScoreToSave = Datasave.ReadFile(FilePath);
            Datasave.AppendToFile(FileToSaveTo, ScoreToSave);
        }
        public void SaveTxtInfo(string path, string info,string FilePath)
        {
            string text = path + ";" + info;
            System.IO.File.WriteAllText(FilePath, text);
        }
        public void SaveTxt(string path, string FilePath)
        {
            string text = path;
            System.IO.File.WriteAllText(FilePath, text);
        }

    }
}
