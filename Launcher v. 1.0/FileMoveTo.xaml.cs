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
    /// Interakční logika pro FileMoveTo.xaml
    /// </summary>
    public partial class FileMoveTo : Window
    {
        public static int ctr;

        public static List<string> Paths = new List<string>();
        public static List<string> SearchPaths = new List<string>();
        public static List<string> FilesNames = new List<string>();

        public static bool state = true;

        public FileMoveTo()
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
                        string path = pathss.FilePaths;
                        AddItemToPathList(path);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            state = true;
            Window2 main = new Window2();
            this.Close();
            main.Show();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (state)
            {
                if (PathsList.SelectedIndex == -1)

                {
                    TextLabel.Content = "Nebyla vybrána cesta. ";

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
                    Butt.Content = "Vyberte Projekt k přesunutí";
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
                        TextLabel.Content = "Projekt byl přesunut";
                    }
                    else
                    {
                        TextLabel.Content = "Projekt se stejným názvem již existuje";
                    }
                }
                else
                {
 
                    if (ProjectsList.SelectedIndex == -1)
                    {
                        TextLabel.Content = "Nebyl vybrán Projekt.";
                    }
                    if (PathsList.SelectedIndex == -1)
                    {
                        TextLabel.Content = "Nebyla vybrána cesta.";
                    }
                    if (PathsList.SelectedIndex == -1 && ProjectsList.SelectedIndex == -1)
                    {
                        TextLabel.Content = "Nebyla vybrána cesta ani projekt.";
                    }
                }
            }

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
                NoSelect.Content = " ";
                var dir = new DirectoryInfo(path);

                var dicc = dir.GetDirectories();
                foreach (var item in dicc)
                {
                     AddItemToListView(item.Name,item.FullName); 
                }
                ctr = 0;
            }
            else
            {
                TextLabel.Content = "Cesta Neexisutje: " + path;
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
    }
}
