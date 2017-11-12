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

namespace Launcher_v._1._0
{
    /// <summary>
    /// Interakční logika pro AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public static bool Paths;
        public static string FileName;
        public AddPage(bool paths, int Id, string fileName)
        {
            InitializeComponent();
            FileName = fileName;
            Paths = paths;
            if (Paths)
            {
                InputBox.Text = "Cesta";
                Butter.Content = "Přidat cestu";
            }
            else
            {
                InputBox.Text = "info";
                Butter.Content = "Přidat informace";
                FileName = fileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPath(InputBox.Text);
        }
        public void AddPath(string Text)
        {
            if (Paths)
            {
                DataSaver DataSave = new DataSaver("Paths.csv");
                DataSave.DataSave(Text, "Paths.txt");
            }
            else
            {
                DataSaver DataSave = new DataSaver("information.csv");
                DataSave.DataSaveInfo(Text, FileName, "information.txt");
            }

        }
    }
}
