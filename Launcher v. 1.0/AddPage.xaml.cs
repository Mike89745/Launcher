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
using System.IO;
using FileHelpers;

namespace Launcher_v._1._0
{
    /// <summary>
    /// Interakční logika pro AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public static bool Paths;
        public static string FileName;
        public static List<string> SearchPaths = new List<string>();
        public AddPage()
        {
            InitializeComponent();
            UpdateListView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPath(InputBox.Text);
            UpdateListView();
        }
        public void UpdateListView()
        {
            SearchPaths = new List<string>();
            PathList.Items.Clear();
            if (File.Exists("Paths.csv"))
            {
                var engine = new FileHelperAsyncEngine<Paths>();
                using (engine.BeginReadFile("Paths.csv"))
                {
                    foreach (Paths pathss in engine)
                    {
                        AddItemsToListView(pathss.FilePaths);
                    }
                }
            }
            else
            {
                ErrorMsg("Neexistuje Paths.csv");
            }
        }
        public void ErrorMsg(string msg)
        {
            ErrorWindow window = new ErrorWindow(msg);
            window.Show();
        }
        public void AddPath(string Text)
        {

            DataSaver DataSave = new DataSaver("Paths.csv");
            DataSave.DataSave(Text, "Paths.txt");
            

        }
        public void AddItemsToListView(string path)
        {
            SearchPaths.Add(path);

            GridView newGrid = new GridView();
            PathList.View = newGrid;

            newGrid.Columns.Add(new GridViewColumn
            {
                Header = "Nazev",
                DisplayMemberBinding = new Binding("Nazev")
            });

            PathList.Items.Add(new ListItem { Nazev = path });
        }
        public void DeletePath()
        {
            if (PathList.SelectedIndex != -1)
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
                cesty.RemoveAt(PathList.SelectedIndex);
                var engines = new FileHelperEngine<Paths>();

                engines.WriteFile("Paths.csv", cesty);

                ErrorMsg("Cesta byla smazána.");

                SearchPaths = new List<string>();
                PathList.Items.Clear();
                using (engine.BeginReadFile("Paths.csv"))
                {
                    foreach (Paths pathss in engine)
                    {
                        AddItemsToListView(pathss.FilePaths);
                    }
                }
            }
            else
            {
                ErrorMsg("Nebyla vybrána cesta.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeletePath();
        }
    }
}
