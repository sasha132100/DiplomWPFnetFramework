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
            FolderNameTextBlock.Text = SystemContext.Item.Title;
        }

        public void LoadContent()
        {
            SystemContext.PageForLoadContent = this;
            DocumentsViewGrid.Children.Clear();
            using (var db = new test123Entities1())
            {
                List<Items> items = null;
                try
                {
                    if (SystemContext.isFromHiddenFiles)
                    {
                        items = (from i in db.Items
                                 where i.UserId == SystemContext.User.Id && i.IsHidden == 1 && i.FolderId == SystemContext.Folder.Id
                                 orderby i.IPriority descending, i.DateCreation
                                 select i).ToList<Items>();
                    }
                    else
                    {
                        items = (from i in db.Items
                                 where i.UserId == SystemContext.User.Id && i.IsHidden == 0 && i.FolderId == SystemContext.Folder.Id
                                 orderby i.IPriority descending, i.DateCreation
                                 select i).ToList<Items>();
                    }
                    if (!SystemContext.isDocumentNeedToShow)
                    {
                        items.RemoveAll(d => d.IType == "Passport" || d.IType == "INN" || d.IType == "SNILS" || d.IType == "Polis");
                    }
                    if (!SystemContext.isCreditCardNeedToShow)
                        items.RemoveAll(cc => cc.IType == "CreditCard");
                    if (!SystemContext.isCollectionNeedToShow)
                        items.RemoveAll(c => c.IType == "Collection");
                    if (!SystemContext.isFolderNeedToShow)
                        items.RemoveAll(f => f.IType == "Folder");
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
            var mainGrid = new Grid() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
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
            bottomDarkeningBorder.Tag = item;
            itemName.Tag = item;

            borderPanel.MouseLeftButtonUp += ChangeItemButton_Click;
            borderPanel.ContextMenu = contextMenu;
            bottomDarkeningBorder.MouseLeftButtonUp += ChangeTitleNameButton_Click;
            itemName.MouseLeftButtonUp += ChangeTitleNameButton_Click;

            Image LockImage = new Image() { Name = "LockImage", VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(5, 5, 0, 0), Height = 25, Width = 25 };
            LockImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/LockImage.png"));
            mainGrid.Children.Add(LockImage);
            if (item.IPriority == 0)
                LockImage.Visibility = Visibility.Hidden;

            mainGrid.Children.Add(bottomDarkeningBorder);
            mainGrid.Children.Add(itemName);
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
            using (var db = new test123Entities1())
            {
                SystemContext.Item = (sender as Border).Tag as Items;
                SystemContext.isChange = true;
                SystemContext.PageForLoadContent = this;
                switch (SystemContext.Item?.IType)
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
                        MessageBox.Show("Открытие коллекции пока не реализовано");
                        break;

                    default:
                        MessageBox.Show("Ошибка при открытии документа");
                        break;
                }
            }
        }

        private void ChangeTitleNameButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock)
                SystemContext.Item = (sender as TextBlock).Tag as Items;
            else
                SystemContext.Item = (sender as Border).Tag as Items;
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
            SystemContext.Item = border.Tag as Items;
            Items item;
            item = SystemContext.Item;
            Image LockImage = grid.Children.OfType<Image>().FirstOrDefault(li => li.Name == "LockImage");
            using (var db = new test123Entities1())
            {
                if (item.IPriority == 1)
                {
                    LockImage.Visibility = Visibility.Hidden;
                    item.IPriority = 0;
                }
                else
                {
                    LockImage.Visibility = Visibility.Visible;
                    item.IPriority = 1;
                }
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
            }
            LoadContent();
        }

        private void MenuItemHide_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Items;
            Items item = new Items();
            item = SystemContext.Item;
            using (var db = new test123Entities1())
            {
                if (SystemContext.isFromHiddenFiles)
                {
                    item.IsHidden = 0;
                }
                else
                {
                    item.IsHidden = 1;
                }
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
            }
            LoadContent();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Items;
            Items item = new Items();
            item = SystemContext.Item;
            using (var db = new test123Entities1())
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
    }
}
