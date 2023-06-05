using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
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
    /// Логика взаимодействия для CollectionContentPage.xaml
    /// </summary>
    public partial class CollectionContentPage : Page
    {
        Window parentWindow;

        public CollectionContentPage()
        {
            InitializeComponent();
            LoadContent();
            FolderNameTextBlock.Text = SystemContext.Item.Title;
        }


        private void LoadContent()
        {
            DocumentsViewGrid.Children.Clear();
            using (var db = new test123Entities1())
            {
                List<Photo> photoes = null;
                try
                {
                    photoes = (from p in db.Photo
                               where p.CollectionID == SystemContext.Item.Id
                               select p).ToList<Photo>();
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке документов");
                    return;
                }
                foreach (var photo in photoes)
                    AddNewDocument(photo);
            }
        }

        private BitmapSource ByteArrayToImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void AddNewDocument(Photo photo)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)DocumentsViewGrid.Resources["ContentBorderStyle"], Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a8eab")) };
            var mainGrid = new Grid() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            var contextMenu = (ContextMenu)this.FindResource("MyContextMenu");

            ImageBrush imageBrush = new ImageBrush();
            Image image = new Image() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            image.Source = ByteArrayToImage(photo.PPath);

            imageBrush.ImageSource = image.Source;
            imageBrush.Stretch = Stretch.UniformToFill;
            mainGrid.Background = imageBrush;

            borderPanel.Tag = photo;

            borderPanel.MouseLeftButtonUp += ChangeItemButton_Click;
            borderPanel.ContextMenu = contextMenu;

            borderPanel.Child = mainGrid;
            DocumentsViewGrid.Children.Add(borderPanel);
        }

        private void ChangeItemButton_Click(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            using (var db = new test123Entities1())
            {
                SystemContext.Item = (sender as Border).Tag as Items;
                SystemContext.isChange = true;
                var photoWindow = new PhotoWindow();
                photoWindow.ShowDialog();
            }
        }

        private void ChangeTitleNameButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock)
                SystemContext.Item = (sender as TextBlock).Tag as Items;
            else
                SystemContext.Item = (sender as Border).Tag as Items;
            SystemContext.isChangeTitleName = true;
            ChangeItemTitleNameWindow changeItemTitleNameWindow = new ChangeItemTitleNameWindow();
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
            Items item = new Items();
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
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            LoadContent();
        }
    }
}
