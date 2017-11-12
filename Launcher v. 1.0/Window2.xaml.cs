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
    /// Interakční logika pro Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExeStartPage startPage = new ExeStartPage();
            ContentPage.Content = startPage;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FileToMovePage movePage = new FileToMovePage();
            ContentPage.Content = movePage;
            ContentPage.MaxHeight = 500;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            AddPage page = new AddPage(true, 0, "xd");
            ContentPage.Content = page;
            ContentPage.Width = 300;
            ContentPage.Height = 180;
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

      
    }
}
