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

namespace DiplomWPFnetFramework.Pages.MainInteractionsPages
{
    /// <summary>
    /// Логика взаимодействия для CollectionContentPage.xaml
    /// </summary>
    public partial class CollectionContentPage : Page
    {
        Window parentWindow;
        byte[] photoBytes = null;

        public CollectionContentPage()
        {
            InitializeComponent();
            LoadContent();
            CollectionNameTextBlock.Text = SystemContext.Item.Title;
        }


        private void LoadContent()
        {
            DocumentsViewGrid.Children.Clear();
            using (var db = new LocalMyDocsAppDBEntities())
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
            image.Source = ByteArrayToImage(photo.Image);

            imageBrush.ImageSource = image.Source;
            imageBrush.Stretch = Stretch.UniformToFill;
            mainGrid.Background = imageBrush;

            borderPanel.Tag = photo;

            borderPanel.MouseLeftButtonUp += ChangeItemButton_Click;
            borderPanel.ContextMenu = contextMenu;

            borderPanel.Child = mainGrid;
            DocumentsViewGrid.Children.Add(borderPanel);
        }

        private string ImageSetter()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileImage = openFileDialog.FileName;
                photoBytes = File.ReadAllBytes(fileImage);
                return "Успешно";
            }
            else
            {
                return "Отмена";
            }
        }

        private Photo CreatingPhotoObject()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                Photo photo = new Photo();
                photo.Id = Guid.NewGuid();
                photo.Image = photoBytes;
                photo.CollectionID = SystemContext.Item.Id;
                return photo;
            }
        }

        private string AddNewPhoto()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.Photo.Add(CreatingPhotoObject());
                db.SaveChanges();
                LoadContent();
                return "Добавлен";
            }
        }

        private void ChangeItemButton_Click(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            using (var db = new LocalMyDocsAppDBEntities())
            {
                SystemContext.Photo = (sender as Border).Tag as Photo;
                SystemContext.isChange = true;
                var photoWindow = new PhotoWindow();
                photoWindow.ShowDialog();
            }
        }

        private void addNewElementsInFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImageSetter() == "Успешно")
                AddNewPhoto();
            else
                return;
        }

        private void CloseFolderPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            Frame openDocumentViewingPageFrame = parentWindow.FindName("openPageFrame") as Frame;
            if (SystemContext.isFromFolder)
            {
                FolderContentPage folderContentPage = new FolderContentPage();
                openDocumentViewingPageFrame.Content = folderContentPage;
                SystemContext.isFromFolder = false;
            }
            else
            {
                DocumentViewingPage documentViewingPage = new DocumentViewingPage();
                openDocumentViewingPageFrame.Content = documentViewingPage;
            }
        }

        private void MenuItemLock_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Item;
            Item item = new Item();
            item = SystemContext.Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                item.Priority = 1;
                db.Item.AddOrUpdate(item);
                db.SaveChanges();
            }
        }

        private void MenuItemHide_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Item = border.Tag as Item;
            Item item = new Item();
            item = SystemContext.Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                item.IsHidden = 1;
                db.Item.AddOrUpdate(item);
                db.SaveChanges();
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Border border = (Border)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            SystemContext.Photo = border.Tag as Photo;
            Photo photo;
            photo = SystemContext.Photo;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.Entry(photo).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            LoadContent();
        }
    }
}
