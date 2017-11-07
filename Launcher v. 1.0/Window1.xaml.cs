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

namespace Launcher_v._1._0
{
    /// <summary>
    /// Interakční logika pro Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public static bool paths;
        public static string FullPath;
        public Window1(bool paths,int Id,string fullpath)
        {
            InitializeComponent();
            FullPath = fullpath;
            if (paths)
            {
                InputBox.Text = "Cesta";
                Butter.Content = "Přidat cestu";
            }
            else
            {
                InputBox.Text = "info";
                Butter.Content = "Přidat informace";
                FullPath = fullpath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPath(InputBox.Text);
            this.Close();
        }
        public void AddPath(string Text)
        {
            if (paths)
            {
                DataSaver DataSave = new DataSaver("Paths.csv");
                DataSave.DataSave(Text, "Paths.txt");
            }
            else
            {
                DataSaver DataSave = new DataSaver("information.csv");
                DataSave.DataSaveInfo(Text, FullPath, "information.txt");
            }

        }
    }
}
