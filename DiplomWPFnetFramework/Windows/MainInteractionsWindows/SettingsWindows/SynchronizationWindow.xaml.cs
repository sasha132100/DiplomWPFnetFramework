using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
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

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows
{
    /// <summary>
    /// Логика взаимодействия для SynchronizationWindow.xaml
    /// </summary>
    public partial class SynchronizationWindow : Window
    {
        public SynchronizationWindow()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DownloadFromServerButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void UploadOnServerButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
