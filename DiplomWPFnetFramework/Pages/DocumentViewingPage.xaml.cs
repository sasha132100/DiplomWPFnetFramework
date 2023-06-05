using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows;
using DiplomWPFnetFramework.Windows.BufferWindows;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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

        private void LoadContent()
        {
            using (var db = new test123Entities1())
            {
                DocumentsViewGrid.Children.Clear();
                List<Items> items = null;
                try
                {
                    if (SystemContext.isFromFolder)
                    {
                        items = (from i in db.Items
                                 where i.UserId == SystemContext.User.Id && i.IType != "Folder" && i.IType != "Collection"
                                 orderby i.Title
                                 select i).ToList<Items>();
                    }
                    else
                    {
                        items = (from i in db.Items
                                 where i.UserId == SystemContext.User.Id && i.FolderId == null
                                 orderby i.Title
                                 select i).ToList<Items>();
                    }
                    if (SystemContext.isFromFolder)
                    {
                        addNewImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/CheckMarkImage.png"));
                        addNewImage.Stretch = Stretch.Uniform;
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке документов");
                    return;
                }
                foreach (var item in items)
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

        private void AddNewDocument(Items item)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)DocumentsViewGrid.Resources["ContentBorderStyle"], Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a8eab")) };
            var mainGrid = new Grid() { Name = "mainGrid", Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            var bottomDarkeningBorder = new Border() { Style = (Style)DocumentsViewGrid.Resources["BottomBorderProperties"] };
            var contextMenu = (ContextMenu)this.FindResource("MyContextMenu");

            ImageBrush imageBrush = new ImageBrush();
            Image image = new Image() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            if (item.IImage != null && item.IImage.ToString() != "")
                image.Source = ByteArrayToImage(item.IImage);
            else
            {
                switch (item.IType)
                {
                    case "CreditCard":
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/CreditCardPlugImage.png"));
                        break;

                    case "Folder":
                        break;

                    case "Collection":
                        break;

                    default:
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DocumentPlugImage.png"));
                        break;
                }
            }
            imageBrush.ImageSource = image.Source;
            imageBrush.Stretch = Stretch.UniformToFill;
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

            if (SystemContext.isFromFolder)
            {
                Image unselectedImage = new Image() { Name = "unselectedImage", VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 5, 5, 0), Height = 25, Width = 25 };
                Image selectedImage = new Image() { Name = "selectedImage", VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 5, 5, 0), Height = 25, Width = 25 };
                unselectedImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/unselected_circle.png"));
                selectedImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/selected_circle.png"));
                mainGrid.Children.Add(unselectedImage);
                mainGrid.Children.Add(selectedImage);
                if (item.IsSelected == 1)
                    unselectedImage.Visibility = Visibility.Hidden;
                else
                    selectedImage.Visibility = Visibility.Hidden;
            }

            mainGrid.Children.Add(bottomDarkeningBorder);
            mainGrid.Children.Add(itemName);
            borderPanel.Child = mainGrid;
            DocumentsViewGrid.Children.Add(borderPanel);
        }

        private void AddInFolder(Items item)
        {
            using (var db = new test123Entities1())
            {
                if (item.FolderId == null && item.IsSelected == 1)
                {
                    item.FolderId = SystemContext.Folder.Id;
                    db.Items.AddOrUpdate(item);
                }
                else if (item.FolderId != null && item.IsSelected == 0)
                {
                    item.FolderId = null;
                    db.Items.AddOrUpdate(item);
                }
                db.SaveChanges();
            }
        }

        private void ChangeItemButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChangeTitleName)
                return;
            parentWindow = Window.GetWindow(this);
            using (var db = new test123Entities1())
            {
                
                if (SystemContext.isFromFolder)
                {
                    SystemContext.SelectedItem = (sender as Border).Tag as Items;
                    Items item;
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

                    db.Items.AddOrUpdate(item);
                    db.SaveChanges();
                    return;
                }
                else
                {
                    SystemContext.Item = (sender as Border).Tag as Items;
                    SystemContext.isChange = true;
                    switch (SystemContext.Item?.IType)
                    {
                        case "Passport":
                            var passportWindow = new PassportWindow();
                            passportWindow.ShowDialog();
                            break;

                        case "INN":
                            var innWindow = new InnWindow();
                            innWindow.ShowDialog();
                            break;

                        case "SNILS":
                            var snilsWindow = new SnilsWindow();
                            snilsWindow.ShowDialog();
                            break;

                        case "Polis":
                            var polisWindow = new PolisWindow();
                            polisWindow.ShowDialog();
                            break;

                        case "Photo":
                            var photoWindow = new PhotoWindow();
                            photoWindow.ShowDialog();
                            break;

                        case "CreditCard":
                            var creditCardWindow = new CreditCardWindow();
                            creditCardWindow.ShowDialog();
                            break;

                        case "Folder":
                            SystemContext.Folder = SystemContext.Item;
                            Frame openFolderPageFrame = parentWindow.FindName("openPageFrame") as Frame;
                            FolderContentPage folderContentPage = new FolderContentPage();
                            openFolderPageFrame.Content = folderContentPage;
                            break;

                        case "Collection":
                            MessageBox.Show("Открытие коллекции пока не реализовано");
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
                SystemContext.Item = (sender as TextBlock).Tag as Items;
            else
                SystemContext.Item = (sender as Border).Tag as Items;
            SystemContext.isChangeTitleName = true;
            ChangeItemTitleNameWindow changeItemTitleNameWindow = new ChangeItemTitleNameWindow();
            changeItemTitleNameWindow.ShowDialog();
        }

        private void addNewDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemContext.isFromFolder)
            {
                using (var db = new test123Entities1())
                {
                    List<Items> items = null;
                    try
                    {
                        items = (from i in db.Items
                                 where i.UserId == SystemContext.User.Id
                                 select i).ToList<Items>();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при иницианлизации выбранных документов");
                        return;
                    }
                    foreach (var item in items)
                        AddInFolder(item);
                }
                parentWindow = Window.GetWindow(this);
                Frame openFolderPageFrame = parentWindow.FindName("openPageFrame") as Frame;
                FolderContentPage folderContentPage = new FolderContentPage();
                openFolderPageFrame.Content = folderContentPage;
                SystemContext.isFromFolder = false;
                return;
            }
            parentWindow = Window.GetWindow(this);
            Window1 window1 = new Window1();
            window1.ShowDialog();
        }

        private void MenuItemLock_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Items;
            Items item = new Items();
            item = SystemContext.Item;
            using (var db = new test123Entities1())
            {
                item.IPriority = 1;
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
            }
        }

        private void MenuItemHide_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Items;
            Items item;
            item = SystemContext.Item;
            using (var db = new test123Entities1())
            {
                item.IsHidden = 1;
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Items;
            Items item = new Items();
            item = SystemContext.Item;
            using (var db = new test123Entities1())
            {
                if (item.IType == "Folder")
                {
                    List<Items> itemsInFolder = (from i in db.Items
                                                 where i.UserId == SystemContext.User.Id && i.FolderId == item.Id
                                                 select i).ToList<Items>();
                    foreach(var itemInFodler in itemsInFolder)
                    {
                        db.Entry(itemInFodler).State = System.Data.Entity.EntityState.Deleted;
                    }
                }
                else if (item.IType == "Collection")
                {

                }
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            LoadContent();
        }
    }
}
