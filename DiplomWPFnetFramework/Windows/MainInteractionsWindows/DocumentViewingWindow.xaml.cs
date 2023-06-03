using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
using System.IO;
using DiplomWPFnetFramework.Pages;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewingWindow.xaml
    /// </summary>
    public partial class DocumentViewingWindow : Window
    {
        public DocumentViewingWindow()
        {
            InitializeComponent();
            CheckIsGuest();
        }

        private void CheckIsGuest()
        {
            EmailOutTextBlock.Text = SystemContext.User.ULogin;
            DocumentViewingPage documentViewingPage = new DocumentViewingPage();
            openPageFrame.Content = documentViewingPage;
        }

        private void OpenSettingPageButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoginWindow mainWindow = new LoginWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        private void sortImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Window1 window1 = new Window1();
            this.Close();
            window1.ShowDialog();
        }
    }
}
