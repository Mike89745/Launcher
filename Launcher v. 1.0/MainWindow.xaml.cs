using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileHelpers;
using System.IO;
using System.Diagnostics;

namespace Launcher_v._1._0
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string username;
        public string path;
        public string fullpath;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void GetFiles()
        {
            FilesView.Items.Clear();
            var engine = new FileHelperAsyncEngine<Paths>();
            using (engine.BeginReadFile("Paths.csv"))
            {
                foreach (Paths pathss in engine)
                {
                    path = pathss.FilePaths;

                    if (Directory.Exists(path))
                    {
                        var dir = new DirectoryInfo(path);

                        var dicc = dir.GetDirectories();
                        foreach (var item in dicc)
                        {
                            dir = new DirectoryInfo(item.FullName);
                            FileInfo[] Files = dir.GetFiles();

                            var Exes = Files
                            .Where(items => items.Extension == ".sln")
                            .Select(items => items).ToList();

                            if (Exes.Any())
                            {
                                string FileName = System.IO.Path.GetFileNameWithoutExtension(item.FullName + @"\" + Exes[0]);
                                fullpath = item.FullName + @"\" + FileName + @"\bin\Debug\" + FileName + ".exe";

                                if (File.Exists(fullpath))
                                {
                                    AddItemToListView(fullpath, FileName);
                                }
                                else
                                {
                                    NoFile.Content = "Neexistuje: " + path;
                                }
                            }

                        }
                    }
                    else
                    {
                        NoFile.Content = "Neexistuje: " + path;
                    }
                }
            }
        }
        private void AddItemToListView(string fullpath, string fileName)
        {
            Button newBtn = new Button();
            newBtn.Content = "Spustit";
            newBtn.Click += Btn_click;
            newBtn.Tag = fullpath;

            GridView newGrid = new GridView();
            FilesView.View = newGrid;

            newGrid.Columns.Add(new GridViewColumn
            {
                Header = "Nazev",
                DisplayMemberBinding = new Binding("Nazev")
            });
            FilesView.Items.Add(new ListItem { Nazev = fileName });
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetFiles();
        }
        private void Btn_click(object sender, RoutedEventArgs e)
        {
            var path = ((Button)sender).Tag;
            ProccesStart(path.ToString());
        }
        public void ProccesStart(string path)
        {
            if (File.Exists(path.ToString()))
            {
                Process.Start(path.ToString());
            }
            else
            {
                NoFile.Content = "Neexistuje: " + path;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 main = new Window1();
            main.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int index = FilesView.SelectedIndex;
            Content.Nazev = FilesView.Items.GetItemAt(index) as ListView;
            Debug.WriteLine(listView);
        }
    }
}
