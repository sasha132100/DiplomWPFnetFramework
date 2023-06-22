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
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using System.IO;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
using DiplomWPFnetFramework.Windows.BufferWindows;
using System.Data.Entity.Migrations;

namespace DiplomWPFnetFramework.Pages.MainInteractionsPages
{
    /// <summary>
    /// Логика взаимодействия для FolderContentPage.xaml
    /// </summary>
    public partial class FolderContentPage : Page
    {
        Window parentWindow;

        public FolderContentPage()
        {
            InitializeComponent();
            LoadContent();
            FolderNameTextBlock.Text = SystemContext.Folder.Title;
        }

        public void LoadContent()
        {
            SystemContext.PageForLoadContent = this;
            DocumentsViewGrid.Children.Clear();
            using (var db = new LocalMyDocsAppDBEntities())
            {
                List<Item> Item = null;
                try
                {
                    if (SystemContext.isFromHiddenFiles)
                    {
                        Item = (from i in db.Item
                                 where i.UserId == SystemContext.User.Id && i.IsHidden == 1 && i.FolderId == SystemContext.Folder.Id
                                 orderby i.Priority descending, i.DateCreation
                                 select i).ToList<Item>();
                    }
                    else
                    {
                        Item = (from i in db.Item
                                 where i.UserId == SystemContext.User.Id && i.IsHidden == 0 && i.FolderId == SystemContext.Folder.Id
                                 orderby i.Priority descending, i.DateCreation
                                 select i).ToList<Item>();
                    }
                    if (!SystemContext.isDocumentNeedToShow)
                    {
                        Item.RemoveAll(d => d.Type == "Passport" || d.Type == "INN" || d.Type == "SNILS" || d.Type == "Polis" || d.Type == "Template");
                    }
                    if (!SystemContext.isCreditCardNeedToShow)
                        Item.RemoveAll(cc => cc.Type == "CreditCard");
                    if (!SystemContext.isCollectionNeedToShow)
                        Item.RemoveAll(c => c.Type == "Collection");
                    if (!SystemContext.isFolderNeedToShow)
                        Item.RemoveAll(f => f.Type == "Folder");
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке документов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                foreach (var item in Item)
                    AddNewDocument(item);
            }
        }

        private BitmapSource ByteArrayToImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void AddPhotoInCollectionView(Photo photo, Grid parentGrid, int marginTop, int marginRight)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)DocumentsViewGrid.Resources["ContentBorderStyle"], Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a8eab")), Width = 100, Height = 170, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, marginTop, marginRight, 0) };
            var mainGrid = new Grid() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };

            ImageBrush imageBrush = new ImageBrush();
            Image image = new Image() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            image.Source = ByteArrayToImage(photo.Image);

            imageBrush.ImageSource = image.Source;
            imageBrush.Stretch = Stretch.UniformToFill;
            mainGrid.Background = imageBrush;
            borderPanel.Child = mainGrid;
            parentGrid.Children.Add(borderPanel);
        }

        private void AddNewDocument(Item item)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)DocumentsViewGrid.Resources["ContentBorderStyle"], Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a8eab")) };
            var mainGrid = new Grid() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            var bottomDarkeningBorder = new Border() { Style = (Style)DocumentsViewGrid.Resources["BottomBorderProperties"] };
            var contextMenu = (ContextMenu)this.FindResource("MyContextMenu");

            ImageBrush imageBrush = new ImageBrush();
            Image image = new Image() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            if (item.Image != null && item.Image.ToString() != "")
                image.Source = ByteArrayToImage(item.Image);
            else
            {
                switch (item.Type)
                {
                    case "Passport":
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/RussianPassportPlug.png"));
                        imageBrush.Stretch = Stretch.Uniform;
                        break;

                    case "INN":
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/RussianINNPlug.jpg"));
                        imageBrush.Stretch = Stretch.Uniform;
                        break;

                    case "SNILS":
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/RussianSNILSPlug.jpg"));
                        imageBrush.Stretch = Stretch.Uniform;
                        break;

                    case "CreditCard":
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/CreditCardPlugImage.png"));
                        imageBrush.Stretch = Stretch.UniformToFill;
                        break;

                    case "Polis":
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/RussianPolisPlug.png"));
                        imageBrush.Stretch = Stretch.UniformToFill;
                        break;

                    case "Collection":
                        List<Photo> photoInCollectionList;
                        int marginTop = 15, marginRight = 15;
                        using (var db = new LocalMyDocsAppDBEntities())
                        {
                            photoInCollectionList = (from p in db.Photo
                                                     where p.CollectionID == item.Id
                                                     select p).ToList<Photo>();
                            if (photoInCollectionList.Count >= 3)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    AddPhotoInCollectionView(photoInCollectionList[i], mainGrid, marginTop, marginRight);
                                    marginTop += 15;
                                    marginRight += 15;
                                }
                            }
                            else if (photoInCollectionList.Count > 0)
                            {
                                for (int i = 0; i < photoInCollectionList.Count; i++)
                                {
                                    AddPhotoInCollectionView(photoInCollectionList[i], mainGrid, marginTop, marginRight);
                                    marginTop += 15;
                                    marginRight += 15;
                                }
                            }
                        }
                        break;

                    default:
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DocumentPlugImage.png"));
                        imageBrush.Stretch = Stretch.UniformToFill;
                        break;
                }
            }
            imageBrush.ImageSource = image.Source;
            mainGrid.Background = imageBrush;

            bottomDarkeningBorder.VerticalAlignment = VerticalAlignment.Bottom;
            TextBlock itemName = new TextBlock() { Text = item.Title, Style = (Style)DocumentsViewGrid.Resources["DocumentTextBlockPropeties"] };

            borderPanel.Tag = item;
            bottomDarkeningBorder.Tag = item;
            itemName.Tag = item;

            borderPanel.MouseLeftButtonUp += ChangeItemButton_Click;
            borderPanel.ContextMenu = contextMenu;
            bottomDarkeningBorder.MouseLeftButtonUp += ChangeTitleNameButton_Click;
            itemName.MouseLeftButtonUp += ChangeTitleNameButton_Click;

            Image LockImage = new Image() { Name = "LockImage", VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(5, 5, 0, 0), Height = 25, Width = 25 };
            if (item.Type == "Folder" || (item.Type == "Passport" && item.Image == null))
                LockImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/LockImageWhite.png"));
            else
                LockImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/LockImage.png"));

            if (item.Priority == 0)
                LockImage.Visibility = Visibility.Hidden;

            mainGrid.Children.Add(bottomDarkeningBorder);
            mainGrid.Children.Add(itemName);
            mainGrid.Children.Add(LockImage);
            borderPanel.Child = mainGrid;
            DocumentsViewGrid.Children.Add(borderPanel);
        }

        private void ChangeItemButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChangeTitleName)
            {
                SystemContext.isChangeTitleName = false;
                return;
            }
            parentWindow = Window.GetWindow(this);
            using (var db = new LocalMyDocsAppDBEntities())
            {
                SystemContext.Item = (sender as Border).Tag as Item;
                SystemContext.isChange = true;
                SystemContext.PageForLoadContent = this;
                switch (SystemContext.Item?.Type)
                {
                    case "Passport":
                        var passportWindow = new PassportWindow();
                        passportWindow.Closed += Window_Closed;
                        passportWindow.ShowDialog();
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    case "INN":
                        var innWindow = new InnWindow();
                        innWindow.Closed += Window_Closed;
                        innWindow.ShowDialog();
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    case "SNILS":
                        var snilsWindow = new SnilsWindow();
                        snilsWindow.Closed += Window_Closed;
                        snilsWindow.ShowDialog();
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    case "Polis":
                        var polisWindow = new PolisWindow();
                        polisWindow.Closed += Window_Closed;
                        polisWindow.ShowDialog();
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    case "Photo":
                        var photoWindow = new PhotoWindow();
                        photoWindow.Closed += Window_Closed;
                        photoWindow.ShowDialog();
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    case "CreditCard":
                        var creditCardWindow = new CreditCardWindow();
                        creditCardWindow.Closed += Window_Closed;
                        creditCardWindow.ShowDialog();
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    case "Collection":
                        SystemContext.isFromFolder = true;
                        Frame openCollectionPageFrame = parentWindow.FindName("openPageFrame") as Frame;
                        CollectionContentPage collectionContentPage = new CollectionContentPage();
                        openCollectionPageFrame.Content = collectionContentPage;
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    case "Template":
                        TemplateDocument templateDocument = db.TemplateDocument.Where(td => td.Id == SystemContext.Item.Id).FirstOrDefault();
                        SystemContext.Template = db.Template.Where(t => t.Id == templateDocument.TemplateId).FirstOrDefault();
                        var UserTemplateWindow = new UserTemplateWindow();
                        UserTemplateWindow.Closed += Window_Closed;
                        UserTemplateWindow.ShowDialog();
                        SystemContext.Item = SystemContext.Folder;
                        break;

                    default:
                        MessageBox.Show("Ошибка при открытии документа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

        private void ChangeTitleNameButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock)
                SystemContext.Item = (sender as TextBlock).Tag as Item;
            else
                SystemContext.Item = (sender as Border).Tag as Item;
            SystemContext.isChangeTitleName = true;
            SystemContext.PageForLoadContent = this;
            ChangeItemTitleNameWindow changeItemTitleNameWindow = new ChangeItemTitleNameWindow();
            changeItemTitleNameWindow.Closed += Window_Closed;
            changeItemTitleNameWindow.ShowDialog();
        }

        private void addNewElementsInFolderButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isFromFolder = true;
            parentWindow = Window.GetWindow(this);
            Frame openDocumentViewingPageFrame = parentWindow.FindName("openPageFrame") as Frame;
            DocumentViewingPage folderContentPage = new DocumentViewingPage();
            openDocumentViewingPageFrame.Content = folderContentPage;

        }

        private void CloseFolderPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            Frame openDocumentViewingPageFrame = parentWindow.FindName("openPageFrame") as Frame;
            DocumentViewingPage folderContentPage = new DocumentViewingPage();
            openDocumentViewingPageFrame.Content = folderContentPage;
        }

        private void MenuItemLock_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            Grid grid = border.Child as Grid;
            Item item = border.Tag as Item;
            Image LockImage = grid.Children.OfType<Image>().FirstOrDefault(li => li.Name == "LockImage");
            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (item.Priority == 1)
                {
                    LockImage.Visibility = Visibility.Hidden;
                    item.Priority = 0;
                }
                else
                {
                    LockImage.Visibility = Visibility.Visible;
                    item.Priority = 1;
                }
                db.Item.AddOrUpdate(item);
                db.SaveChanges();
            }
            LoadContent();
        }

        private void MenuItemHide_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            Item item = border.Tag as Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (SystemContext.isFromHiddenFiles)
                {
                    item.IsHidden = 0;
                }
                else
                {
                    item.IsHidden = 1;
                }
                if (item.FolderId != new Guid())
                {
                    item.FolderId = new Guid();
                    item.IsSelected = 0;
                }
                db.Item.AddOrUpdate(item);
                db.SaveChanges();
            }
            LoadContent();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            Item item = border.Tag as Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            LoadContent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as ContextMenu;
            Border border = contextMenu.PlacementTarget as Border;
            Item item = border.Tag as Item;

            MenuItem LockMenu = contextMenu.Items[0] as MenuItem;
            MenuItem HideMenu = contextMenu.Items[1] as MenuItem;

            if (item.IsHidden == 1)
                HideMenu.Header = "Сделать публичным";
            else
                HideMenu.Header = "Скрыть";
            if (item.Priority == 1)
                LockMenu.Header = "Открепить";
            else
                LockMenu.Header = "Закрепить";
        }
    }
}
