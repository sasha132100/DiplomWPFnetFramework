using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.BufferWindows;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace DiplomWPFnetFramework.Pages.MainInteractionsPages
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewingPage.xaml
    /// </summary>
    public partial class DocumentViewingPage : Page
    {
        Window parentWindow = null;

        public DocumentViewingPage()
        {
            InitializeComponent();
            LoadContent();
        }

        public void LoadContent()
        {
            SystemContext.PageForLoadContent = this;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                DocumentsViewGrid.Children.Clear();
                List<Item> Item = null;
                try
                {
                    if (SystemContext.isFromFolder)
                    {
                        if (SystemContext.isFromHiddenFiles)
                        {
                            Item = (from i in db.Item
                                    where i.UserId == SystemContext.User.Id && i.Type != "Folder" && (i.FolderId == Guid.Empty || i.FolderId == SystemContext.Item.Id) && i.IsHidden == 1
                                    orderby i.Priority descending, i.DateCreation
                                    select i).ToList<Item>();
                            addNewImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/CheckMarkImage.png"));
                            addNewImage.Stretch = Stretch.Uniform;
                        }
                        else
                        {
                            Item = (from i in db.Item
                                    where i.UserId == SystemContext.User.Id && i.Type != "Folder" && (i.FolderId == Guid.Empty || i.FolderId == SystemContext.Item.Id) && i.IsHidden == 0
                                    orderby i.Priority descending, i.DateCreation
                                    select i).ToList<Item>();
                            addNewImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/CheckMarkImage.png"));
                            addNewImage.Stretch = Stretch.Uniform;
                        }
                        
                    }
                    else if (SystemContext.isFromHiddenFiles)
                    {
                        Item = (from i in db.Item
                                where i.UserId == SystemContext.User.Id && i.FolderId == Guid.Empty && i.IsHidden == 1
                                orderby i.Priority descending, i.DateCreation
                                select i).ToList<Item>();
                        if (!SystemContext.isDocumentNeedToShow)
                            Item.RemoveAll(d => d.Type == "Passport" || d.Type == "INN" || d.Type == "SNILS" || d.Type == "Polis");
                        if (!SystemContext.isCreditCardNeedToShow)
                            Item.RemoveAll(cc => cc.Type == "CreditCard");
                        if (!SystemContext.isCollectionNeedToShow)
                            Item.RemoveAll(c => c.Type == "Collection");
                        if (!SystemContext.isFolderNeedToShow)
                            Item.RemoveAll(f => f.Type == "Folder");
                    }
                    else
                    {
                        Item = (from i in db.Item
                                 where i.UserId == SystemContext.User.Id && i.FolderId == Guid.Empty && i.IsHidden == 0
                                 orderby i.Priority descending, i.DateCreation
                                 select i).ToList<Item>();
                        if (!SystemContext.isDocumentNeedToShow)
                            Item.RemoveAll(d => d.Type == "Passport" || d.Type == "INN" || d.Type == "SNILS" || d.Type == "Polis");
                        if (!SystemContext.isCreditCardNeedToShow)
                            Item.RemoveAll(cc => cc.Type == "CreditCard");
                        if (!SystemContext.isCollectionNeedToShow)
                            Item.RemoveAll(c => c.Type == "Collection");
                        if (!SystemContext.isFolderNeedToShow)
                            Item.RemoveAll(f => f.Type == "Folder");
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке документов");
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

        private void AddItemInFolderView(Item item, WrapPanel parentWrapPanel)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)DocumentsViewGrid.Resources["ContentBorderStyleMini"], Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a8eab")) };
            var mainGrid = new Grid() { Name = "mainGrid", Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetterMini"] };
            var bottomDarkeningBorder = new Border() { Style = (Style)DocumentsViewGrid.Resources["BottomBorderPropertiesMini"] };
            var blurEffect = new BlurEffect() { Radius = 5 };
            borderPanel.Effect = blurEffect;

            ImageBrush imageBrush = new ImageBrush();
            Image image = new Image() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetterMini"] };
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
            TextBlock itemName = new TextBlock() { Text = item.Title, Style = (Style)DocumentsViewGrid.Resources["DocumentTextBlockPropetiesMini"] };

            mainGrid.Children.Add(bottomDarkeningBorder);
            mainGrid.Children.Add(itemName);
            borderPanel.Child = mainGrid;
            parentWrapPanel.Children.Add(borderPanel);
        }

        private void AddNewDocument(Item item)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)DocumentsViewGrid.Resources["ContentBorderStyle"], Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a8eab")) };
            var mainGrid = new Grid() { Name = "mainGrid", Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            var bottomDarkeningBorder = new Border() { Style = (Style)DocumentsViewGrid.Resources["BottomBorderProperties"] };
            var contextMenu = (ContextMenu)this.FindResource("MyContextMenu");
            var wrapPanel = new WrapPanel();

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

                    case "Folder":
                        List<Item> inFolderList;
                        using (var db = new LocalMyDocsAppDBEntities())
                        {
                            inFolderList = (from i in db.Item
                                            where i.UserId == SystemContext.User.Id && item.Id == i.FolderId
                                            orderby i.Priority descending, i.DateCreation
                                            select i).ToList<Item>();
                            if (inFolderList.Count >= 4)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    AddItemInFolderView(inFolderList[i], wrapPanel);
                                }
                            }
                            else if (inFolderList.Count > 0)
                            {
                                for (int i = 0; i < inFolderList.Count; i++)
                                {
                                    AddItemInFolderView(inFolderList[i], wrapPanel);
                                }
                            }
                        }
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
            contextMenu.Tag = item;
            bottomDarkeningBorder.Tag = item;
            itemName.Tag = item;

            borderPanel.MouseLeftButtonUp += ChangeItemButton_Click;
            if (SystemContext.isFromFolder == false)
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

            if (SystemContext.isFromFolder)
            {
                Image unselectedImage = new Image() { Name = "unselectedImage", VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 5, 5, 0), Height = 25, Width = 25 };
                Image selectedImage = new Image() { Name = "selectedImage", VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 5, 5, 0), Height = 25, Width = 25 };
                if ((item.Image == null || item.Image.ToString() == "") && item.Type == "Passport")
                {
                    unselectedImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/unselected_circleType2.png"));
                    selectedImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/selected_circleType2.png"));
                    unselectedImage.Width = 20;
                    unselectedImage.Height= 20;
                }
                else
                {
                    unselectedImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/unselected_circle.png"));
                    selectedImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/selected_circle.png"));
                }
                mainGrid.Children.Add(unselectedImage);
                mainGrid.Children.Add(selectedImage);
                if (item.IsSelected == 1)
                    unselectedImage.Visibility = Visibility.Hidden;
                else
                    selectedImage.Visibility = Visibility.Hidden;
            }

            if (item.Type == "Folder")
                mainGrid.Children.Add(wrapPanel);
            mainGrid.Children.Add(bottomDarkeningBorder);
            mainGrid.Children.Add(itemName);
            mainGrid.Children.Add(LockImage);
            borderPanel.Child = mainGrid;
            DocumentsViewGrid.Children.Add(borderPanel);
        }

        private void AddInFolder(Item item)
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (item.FolderId == Guid.Empty && item.IsSelected == 1)
                {
                    item.FolderId = SystemContext.Folder.Id;
                    db.Item.AddOrUpdate(item);
                }
                else if (item.FolderId != Guid.Empty && item.IsSelected == 0)
                {
                    item.FolderId = Guid.Empty;
                    db.Item.AddOrUpdate(item);
                }
                db.SaveChanges();
            }
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
                
                if (SystemContext.isFromFolder)
                {
                    SystemContext.SelectedItem = (sender as Border).Tag as Item;
                    Item item;
                    item = SystemContext.SelectedItem;
                    Grid grid = (sender as Border).Child as Grid;
                    Image selectedImage = grid.Children.OfType<Image>().FirstOrDefault(si => si.Name == "selectedImage");
                    Image unselectedImage = grid.Children.OfType<Image>().FirstOrDefault(si => si.Name == "unselectedImage");

                    if (item.IsSelected == 1)
                    {
                        selectedImage.Visibility = Visibility.Hidden;
                        unselectedImage.Visibility = Visibility.Visible;
                        item.IsSelected = 0;
                    }
                    else
                    {
                        selectedImage.Visibility = Visibility.Visible;
                        unselectedImage.Visibility = Visibility.Hidden;
                        item.IsSelected = 1;
                    }

                    db.Item.AddOrUpdate(item);
                    db.SaveChanges();
                    return;
                }
                else
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
                            break;

                        case "INN":
                            var innWindow = new InnWindow();
                            innWindow.Closed += Window_Closed;
                            innWindow.ShowDialog();
                            break;

                        case "SNILS":
                            var snilsWindow = new SnilsWindow();
                            snilsWindow.Closed += Window_Closed;
                            snilsWindow.ShowDialog();
                            break;

                        case "Polis":
                            var polisWindow = new PolisWindow();
                            polisWindow.Closed += Window_Closed;
                            polisWindow.ShowDialog();
                            break;

                        case "Photo":
                            var photoWindow = new PhotoWindow();
                            photoWindow.Closed += Window_Closed;
                            photoWindow.ShowDialog();
                            break;

                        case "CreditCard":
                            var creditCardWindow = new CreditCardWindow();
                            creditCardWindow.Closed += Window_Closed;  
                            creditCardWindow.ShowDialog();
                            break;

                        case "Folder":
                            SystemContext.Folder = SystemContext.Item;
                            Frame openFolderPageFrame = parentWindow.FindName("openPageFrame") as Frame;
                            FolderContentPage folderContentPage = new FolderContentPage();
                            openFolderPageFrame.Content = folderContentPage;
                            break;

                        case "Collection":
                            Frame openCollectionPageFrame = parentWindow.FindName("openPageFrame") as Frame;
                            CollectionContentPage collectionContentPage = new CollectionContentPage();
                            openCollectionPageFrame.Content = collectionContentPage;
                            break;

                        default:
                            MessageBox.Show("Ошибка при открытии документа");
                            break;
                    }
                }
                SystemContext.isFromFolder = false;
            }
        }

        private void ChangeTitleNameButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isFromFolder)
                return;
            if (sender is TextBlock)
                SystemContext.Item = (sender as TextBlock).Tag as Item;
            else
                SystemContext.Item = (sender as Border).Tag as Item;
            SystemContext.isChangeTitleName = true;
            ChangeItemTitleNameWindow changeItemTitleNameWindow = new ChangeItemTitleNameWindow();
            changeItemTitleNameWindow.Closed += Window_Closed;
            changeItemTitleNameWindow.ShowDialog();
        }

        private void addNewDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemContext.isFromFolder)
            {
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    List<Item> Item = null;
                    try
                    {
                        Item = (from i in db.Item
                                where i.UserId == SystemContext.User.Id
                                select i).ToList<Item>();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при иницианлизации выбранных документов");
                        return;
                    }
                    foreach (var item in Item)
                        AddInFolder(item);
                }
                parentWindow = Window.GetWindow(this);
                Frame openFolderPageFrame = parentWindow.FindName("openPageFrame") as Frame;
                FolderContentPage folderContentPage = new FolderContentPage();
                openFolderPageFrame.Content = folderContentPage;
                SystemContext.isFromFolder = false;
                return;
            }
            SystemContext.PageForLoadContent = this;
            addNewDocumentBufferWindow addNewDocumentBufferWindow = new addNewDocumentBufferWindow();
            addNewDocumentBufferWindow.ShowDialog();
        }

        private void MenuItemLock_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            Grid grid = border.Child as Grid;
            SystemContext.Item = border.Tag as Item;
            Item item;
            item = SystemContext.Item;
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
            SystemContext.Item = border.Tag as Item;
            Item item;
            item = SystemContext.Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (SystemContext.isFromHiddenFiles)
                {
                    if (item.Type == "Folder")
                    {
                        List<Item> ItemInFolder = (from i in db.Item
                                                     where i.UserId == SystemContext.User.Id && i.FolderId == item.Id
                                                     select i).ToList<Item>();
                        foreach (var itemInFodler in ItemInFolder)
                        {
                            itemInFodler.IsHidden = 0;
                        }
                    }
                    item.IsHidden = 0;
                }
                else
                {
                    if (item.Type == "Folder")
                    {
                        List<Item> ItemInFolder = (from i in db.Item
                                                     where i.UserId == SystemContext.User.Id && i.FolderId == item.Id
                                                     select i).ToList<Item>();
                        foreach (var itemInFodler in ItemInFolder)
                        {
                            itemInFodler.IsHidden = 1;
                        }
                    }
                    item.IsHidden = 1;
                }
                db.Item.AddOrUpdate(item);
                db.SaveChanges();
            }
            LoadContent();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Item;
            Item item = new Item();
            item = SystemContext.Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (item.Type == "Folder")
                {
                    List<Item> ItemInFolder = (from i in db.Item
                                                 where i.UserId == SystemContext.User.Id && i.FolderId == item.Id
                                                 select i).ToList<Item>();
                    foreach(var itemInFodler in ItemInFolder)
                    {
                        db.Entry(itemInFodler).State = System.Data.Entity.EntityState.Deleted;
                    }
                }
                else if (item.Type == "Collection")
                {
                    List<Photo> photoesInCollecion = (from p in db.Photo
                                                      where p.CollectionID == SystemContext.Item.Id
                                                      select p).ToList<Photo>();
                    foreach (var photoInCollecion in photoesInCollecion)
                    {
                        db.Entry(photoInCollecion).State = System.Data.Entity.EntityState.Deleted;
                    }
                }
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
