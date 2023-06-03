using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using DiplomWPFnetFramework.Windows;
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

namespace DiplomWPFnetFramework.Pages
{
    /// <summary>
    /// Логика взаимодействия для FolderContentPage.xaml
    /// </summary>
    public partial class FolderContentPage : Page
    {
        public FolderContentPage()
        {
            InitializeComponent();
        }

        private void OpenSettingPageButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoginWindow mainWindow = new LoginWindow();
            //this.Close();
            mainWindow.ShowDialog();
        }

        private void sortImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Window1 window1 = new Window1();
            //this.Close();
            window1.ShowDialog();
        }
    }
}
