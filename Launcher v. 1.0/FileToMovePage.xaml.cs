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
using System.Windows.Shapes;
using FileHelpers;
using System.IO;
using System.Diagnostics;

namespace Launcher_v._1._0
{
    /// <summary>
    /// Interakční logika pro FileToMovePage.xaml
    /// </summary>
    public partial class FileToMovePage : Page
    {
        public static int ctr;

        public static List<string> Paths = new List<string>();
        public static List<string> SearchPaths = new List<string>();
        public static List<string> FilesNames = new List<string>();

        public static bool state = true;

        public FileToMovePage()
        {
            InitializeComponent();
            Text.Text = "Vyberte cestu k projektům";
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
                        string path = pathss.FilePaths;
                        AddItemToPathList(path);
                    }
                }
            }
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (state)
            {
                if (PathsList.SelectedIndex == -1)
                {
                    ErrorMsg("Nebyla vybrána cesta. ");
                }
                else
                {
                    string path = SearchPaths[PathsList.SelectedIndex];
                    GetFiles(path);
                    PathsList.Items.Clear();
                    var engine = new FileHelperAsyncEngine<Paths>();
                    using (engine.BeginReadFile("Paths.csv"))
                    {
                        foreach (Paths pathss in engine)
                        {
                            if (path != pathss.FilePaths)
                            {
                                AddItemToPathList(pathss.FilePaths);
                            }
                        }
                    }
                    Text.Text = "Vyberte Projekt k přesunutí a cestu";
                    state = false;
                }
            }
            else
            {
                if (PathsList.SelectedIndex != -1 && ProjectsList.SelectedIndex != -1)
                {
                    if (!Directory.Exists(SearchPaths[PathsList.SelectedIndex] + @"\" + FilesNames[ProjectsList.SelectedIndex]))
                    {
                        MoveFiles(Paths[ProjectsList.SelectedIndex], SearchPaths[PathsList.SelectedIndex] + @"\" + FilesNames[ProjectsList.SelectedIndex]);
                        ErrorMsg("Projekt byl přesunut");
                    }
                    else
                    {
                        ErrorMsg("Projekt se stejným názvem již existuje");

                    }
                }
                else
                {
                    if (PathsList.SelectedIndex == -1 && ProjectsList.SelectedIndex == -1)
                    {
                        ErrorMsg("Nebyla vybrána cesta ani projekt.");
                    }
                    else if (ProjectsList.SelectedIndex == -1)
                    {
                        ErrorMsg("Nebyl vybrán Projekt.");
                    }
                    else if (PathsList.SelectedIndex == -1)
                    {
                        ErrorMsg("Nebyla vybrána cesta");
                    }

                }
            }

        }
        private void AddItemToPathList(string fullpath)
        {
            SearchPaths.Add(fullpath);

            GridView newGrid = new GridView();
            PathsList.View = newGrid;

            newGrid.Columns.Add(new GridViewColumn
            {
                Header = "Nazev",
                DisplayMemberBinding = new Binding("Nazev")
            });

            PathsList.Items.Add(new ListItem { Nazev = fullpath });

        }
        public void MoveFiles(string SourcePath, string DestPath)
        {
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestPath));
            }
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(SourcePath, DestPath), true);
            }
        }
        public void GetFiles(string path)
        {
            ProjectsList.Items.Clear();

            if (Directory.Exists(path))
            {

                var dir = new DirectoryInfo(path);

                var dicc = dir.GetDirectories();
                foreach (var item in dicc)
                {
                    AddItemToListView(item.Name, item.FullName);
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
            Paths.Add(fullpath);
            FilesNames.Add(fileName);

            GridView newGrid = new GridView();
            ProjectsList.View = newGrid;

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
            ProjectsList.Items.Add(new ListItem { Nazev = fileName, Informace = GetInfoAt(fileName) });
            ctr++;

        }
        public string GetInfoAt(string fileName)
        {
            Debug.WriteLine(fileName);
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
        public void ErrorMsg(string msg)
        {
            ErrorWindow window = new ErrorWindow(msg);
            window.Show();
        }
        public void DeletePath()
        {
            if (PathsList.SelectedIndex != -1)
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
                cesty.RemoveAt(PathsList.SelectedIndex);
                var engines = new FileHelperEngine<Paths>();

                engines.WriteFile("Paths.csv", cesty);

                ErrorMsg("Cesta byla smazána.");

                SearchPaths = new List<string>();
                PathsList.Items.Clear();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DeletePath();
        }
    }
}
