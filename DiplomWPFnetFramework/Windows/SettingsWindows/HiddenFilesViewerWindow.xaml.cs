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

        private MenuItem FindMenuItemByName(ContextMenu contextMenu, string menuItemName)
        {
            foreach (var item in contextMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem.Name == menuItemName)
                    return menuItem;
            }
            return null;
        }

        private void SetterSortName(ContextMenu contextMenu)
        {
            if (!SystemContext.isDocumentNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowDocuments").Header = "Показать документы";
            else if (SystemContext.isDocumentNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowDocuments").Header = "Скрыть документы";

            if (!SystemContext.isCreditCardNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCreditCards").Header = "Показать карты";
            else if (SystemContext.isCreditCardNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCreditCards").Header = "Скрыть карты";

            if (!SystemContext.isCollectionNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCollections").Header = "Показать коллекции";
            else if (SystemContext.isCollectionNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCollections").Header = "Скрыть коллекции";

            if (!SystemContext.isFolderNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowFolders").Header = "Показать папки";
            else if (SystemContext.isFolderNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowFolders").Header = "Скрыть папки";
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.isFromHiddenFiles = false;
            SystemContext.isFromFolder = false;
            DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            this.Close();
            documentViewingWindow.ShowDialog();
        }

        private void sortImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("SortContextMenu");
            SetterSortName(contextMenu);
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
