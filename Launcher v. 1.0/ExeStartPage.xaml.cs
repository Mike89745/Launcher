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
    /// Interakční logika pro ExeStartPage.xaml
    /// </summary>
    public partial class ExeStartPage : Page
    {
        public string path;
        public string fullpath;

        public static List<string> Paths = new List<string>();
        public static List<string> SearchPaths = new List<string>();
        public static List<string> FilesNames = new List<string>();

        public static int ctr = 0;
        public ExeStartPage()
        {
            InitializeComponent();

            Paths = new List<string>();
            SearchPaths = new List<string>();
            FilesNames = new List<string>();

            if (File.Exists("Paths.csv"))
            {
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
            else
            {
                ErrorMsg("Neexistuje Paths.csv");
            }

        }
        public void GetFiles(string path)
        {
            FilesView.Items.Clear();

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
                            AddItemToListView(item.Name, fullpath);
                        }

                    }

                }
                ctr = 0;
            }
            else
            {
                ErrorMsg("Cesta Neexisutje: " + path);


            }
        }
        private void AddItemToListView(string fileName, string fullpath)
        {
            FilesNames.Add(fileName);
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
            FilesView.Items.Add(new ListItem { Nazev = fileName, Informace = GetInfoAt(fileName) });
            ctr++;

        }
        public string GetInfoAt(string fileName)
        {
            string information = " ";
            var engine = new FileHelperAsyncEngine<Info>();
            using (engine.BeginReadFile("information.csv"))
            {
                foreach (Info info in engine)
                {
                    if (info.path == fileName)
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

            ListViewPaths.Items.Add(new ListItem { Nazev = fullpath });

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewPaths.SelectedIndex == -1)

            {
                ErrorMsg("Nebylo nic vybráno.");


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
                ErrorMsg("Neexistuje: " + path);

            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (FilesView.SelectedIndex == -1)
            {
                ErrorMsg("Nebylo nic vybráno ");
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
                ErrorMsg("Nebylo nic vybráno ");

            }
            else
            {
                DataSaver DataSave = new DataSaver("information.csv");
                DataSave.DataSaveInfo(InputBox.Text, FilesNames[FilesView.SelectedIndex], "information.txt");
            }

        }

        public void DeletePath()
        {
            if (ListViewPaths.SelectedIndex != -1)
            {
                List<Paths> cesty = new List<Paths>();
                var engine = new FileHelperAsyncEngine<Paths>();
                using (engine.BeginReadFile("Paths.csv"))
                {
                    foreach (Paths paths in engine)
                    {
                        cesty.Add(paths);
                    }
                }
                cesty.RemoveAt(ListViewPaths.SelectedIndex);
                var engines = new FileHelperEngine<Paths>();

                engines.WriteFile("Paths.csv", cesty);

                ErrorMsg("Cesta byla smazána.");

                SearchPaths = new List<string>();
                ListViewPaths.Items.Clear();
                using (engine.BeginReadFile("Paths.csv"))
                {
                    foreach (Paths pathss in engine)
                    {
                        AddItemToPathList(pathss.FilePaths);
                    }
                }
            }
            else
            {
                ErrorMsg("Nebyla vybrána cesta.");
            }

        }
        public void ErrorMsg(string msg)
        {
            ErrorWindow window = new ErrorWindow(msg);
            window.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DeletePath();
        }
    }
}
