using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Pages.SettingsPages;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
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

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для UserTemplateWindow.xaml
    /// </summary>
    public partial class UserTemplateWindow : Window
    {
        public UserTemplateWindow()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
