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
        public static List<string> Paths = new List<string>();
        public static List<string> SearchPaths = new List<string>();
        public static int ctr = 0;
        public MainWindow()
        {
            InitializeComponent();
            var engine = new FileHelperAsyncEngine<Paths>();
            using (engine.BeginReadFile("Paths.csv"))
            {
                foreach (Paths pathss in engine)
                {
                    path = pathss.FilePaths;
                    AddItemToPathList(path);
                }
            }
        }
        public void GetFiles(string path)
        {
            FilesView.Items.Clear();
            
            if (Directory.Exists(path))
            {
                NoSelect.Content = " ";
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
                            AddItemToListView(FileName, fullpath);
                        }
                        
                    }

                }
                ctr = 0;
            }
            else
            {
                NoSelect.Content = "Cesta Neexisutje: " + path;
            }
        }
        private void AddItemToListView(string fileName, string fullpath)
        {
            Paths.Add(fullpath);
            GridView newGrid = new GridView();
            FilesView.View = newGrid;

            newGrid.Columns.Add(new GridViewColumn
            {
                Header = "Nazev",
                DisplayMemberBinding = new Binding("Nazev")
            });
            newGrid.Columns.Add(new GridViewColumn
            {
                Header = "Informace",
                DisplayMemberBinding = new Binding("Informace")
            });
            FilesView.Items.Add(new ListItem { Nazev = fileName, Informace = GetInfoAt(fullpath)});
            ctr++;
            
        }
        public string GetInfoAt(string FilePath)
        {
            string information = " ";
            var engine = new FileHelperAsyncEngine<Info>();
            using (engine.BeginReadFile("information.csv"))
            {
                foreach (Info info in engine)
                {
                    if(info.path == FilePath)
                    {
                        information = info.Information;
                    }

                }
            }

            return information;
        }
        private void AddItemToPathList(string fullpath)
        {
            SearchPaths.Add(fullpath);

            GridView newGrid = new GridView();
            ListViewPaths.View = newGrid;

            newGrid.Columns.Add(new GridViewColumn
            {
                Header = "Nazev",
                DisplayMemberBinding = new Binding("Nazev")
            });
           
            ListViewPaths.Items.Add(new ListItem { Nazev = fullpath});

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewPaths.SelectedIndex == -1)

            {
                NoSelect.Content = "Nebylo nic vybráno ";

            }
            else
            {
                GetFiles(SearchPaths[ListViewPaths.SelectedIndex]);

            }
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

            Window1 main = new Window1(true, 0, "xd");
            main.Show();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (FilesView.SelectedIndex == -1)
            {
                NoFile.Content = "Nebylo nic vybráno " ;
            }
            else
            {
                ProccesStart(Paths[FilesView.SelectedIndex]);

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (FilesView.SelectedIndex == -1)
            {
                NoFile.Content = "Nebylo nic vybráno ";

            }
            else
            {
                Window1 main = new Window1(false, FilesView.SelectedIndex, Paths[FilesView.SelectedIndex]);
                main.Show();

            }
           
        }
        public void DeletePath()
        {
            var engine = 
        }
    }
}
