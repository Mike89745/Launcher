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
using System.IO;

namespace Launcher_v._1._0
{
    /// <summary>
    /// Interakční logika pro Window2.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static FileToMovePage movePage = new FileToMovePage();
        static ExeStartPage startPage = new ExeStartPage();
        static AddPage page = new AddPage(true, 0, "xd");

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("Paths.csv"))
            {
                ContentPage.Content = page;
                ContentPage.Width = 300;
                ContentPage.Height = 180;
                ErrorMsg("Neexistuje Paths.csv, Přidejte cestu");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentPage.Content = startPage;
            ContentPage.Width = 525;
            ContentPage.Height = 570;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ContentPage.Content = movePage;
            ContentPage.Width = 1000;
            ContentPage.Height = 300;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ContentPage.Content = page;
            ContentPage.Width = 300;
            ContentPage.Height = 180;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        public void ErrorMsg(string msg)
        {
            ErrorWindow window = new ErrorWindow(msg);
            window.Show();
        }

    }
}
