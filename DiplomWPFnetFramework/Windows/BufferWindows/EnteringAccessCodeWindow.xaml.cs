using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows;
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

namespace DiplomWPFnetFramework.Windows.BufferWindows
{
    /// <summary>
    /// Логика взаимодействия для EnteringAccessCodeWindow.xaml
    /// </summary>
    public partial class EnteringAccessCodeWindow : Window
    {
        public EnteringAccessCodeWindow()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.FromWhichWindowIsCalled == "DocumentViewingWindow")
            {
                if (CodeCheckTextBox.Password == SystemContext.User.PinCode)
                {
                    HiddenFilesViewerWindow hiddenFilesViewerWindow = new HiddenFilesViewerWindow();
                    this.Owner.Close();
                    this.Close();
                    hiddenFilesViewerWindow.ShowDialog();
                }
                else
                    MessageBox.Show("Код введен неправильно!");
            }
            else
            {
                if (CodeCheckTextBox.Password == SystemContext.User.PinCode)
                {
                    DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                    this.Owner.Close();
                    this.Close();
                    documentViewingWindow.ShowDialog();
                }
                else
                    MessageBox.Show("Код введен неправильно!");
            }
        }
    }
}
