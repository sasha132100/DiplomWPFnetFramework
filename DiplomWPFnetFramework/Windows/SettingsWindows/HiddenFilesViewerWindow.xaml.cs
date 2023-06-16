using DiplomWPFnetFramework.Classes;
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

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows
{
    /// <summary>
    /// Логика взаимодействия для HiddenFilesViewerWindow.xaml
    /// </summary>
    public partial class HiddenFilesViewerWindow : Window
    {
        public HiddenFilesViewerWindow()
        {
            InitializeComponent();
            DocumentViewingPage documentViewingPage = new DocumentViewingPage();
            openPageFrame.Content = documentViewingPage;
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.isFromHiddenFiles = false;
            DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            this.Close();
            documentViewingWindow.ShowDialog();
        }

        private void sortImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("SortContextMenu");
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void MenuItemhowOrHideDocuments_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isDocumentNeedToShow = !SystemContext.isDocumentNeedToShow;
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
        }

        private void MenuItemhowOrHideCreditCards_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isCreditCardNeedToShow = !SystemContext.isCreditCardNeedToShow;
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
        }

        private void MenuItemhowOrHideCollections_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isCollectionNeedToShow = !SystemContext.isCollectionNeedToShow;
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
        }

        private void MenuItemhowOrHideFolders_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isFolderNeedToShow = !SystemContext.isFolderNeedToShow;
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
        }

        private void MenuItemhowAll_Click(object sender, RoutedEventArgs e)
        {
            SystemContextService.MakeAllElementsShowable();
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
        }
    }
}
