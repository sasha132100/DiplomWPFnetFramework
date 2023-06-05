using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Pages.BufferPages;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
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
    /// Логика взаимодействия для addNewDocumentBufferWindow.xaml
    /// </summary>
    public partial class addNewDocumentBufferWindow : Window
    {
        public addNewDocumentBufferWindow()
        {
            InitializeComponent();
            if (SystemContext.isFromHiddenFiles)
            {
                AddNewHiddenFolderPage addNewHiddenFolderPage = new AddNewHiddenFolderPage();
                ChooseTemplatesTypePageFrame.Content = addNewHiddenFolderPage;
                DocumentMoreInteractionsImage.Visibility = Visibility.Hidden;
            }
            else
            {
                SystemTemplatesShowPage systemTemplatesShowPage = new SystemTemplatesShowPage();
                ChooseTemplatesTypePageFrame.Content = systemTemplatesShowPage;
            }
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("TemplateTypeContextMenu");
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void SystemTemplatesShow_Click(object sender, RoutedEventArgs e)
        {
            SystemTemplatesShowPage systemTemplatesShowPage = new SystemTemplatesShowPage();
            ChooseTemplatesTypePageFrame.Content = systemTemplatesShowPage;
        }

        private void MyTemplatesShow_Click(object sender, RoutedEventArgs e)
        {
            MyTemplatesShowPage myTemplatesShowPage = new MyTemplatesShowPage();
            ChooseTemplatesTypePageFrame.Content = myTemplatesShowPage;
        }

        private void DownloadTemplatesShow_Click(object sender, RoutedEventArgs e)
        {
            DownloadTemplatesShowPage downloadTemplatesShowPage = new DownloadTemplatesShowPage();
            ChooseTemplatesTypePageFrame.Content = downloadTemplatesShowPage;
        }
    }
}
