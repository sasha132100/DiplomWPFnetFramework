﻿using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для PhotoWindow.xaml
    /// </summary>
    public partial class PhotoWindow : Window
    {
        byte[] photoBytes = null;
        byte[] coverImage = null;

        public PhotoWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                PhotoOutTextBlock.Text = SystemContext.Item.Title;
        }

        private void LoadContent()
        {
            if (SystemContext.isChange == false)
            {
                DocumentMoreInteractionsGrid.Visibility = Visibility.Hidden;
                confirmButtonImage.Margin = new Thickness(0, 0, 10, 0);
                return;
            }
            using (var db = new test123Entities1())
            {
                var photo = SystemContext.Photo;
                try
                {
                    PhotoHolder.Source = ByteArrayToImage(photo.PPath);
                    photoBytes = photo.PPath;
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }
            }

        }

        private void ImageSetter(Image imageName)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                photoBytes = File.ReadAllBytes(filePath);
                imageName.Source = ByteArrayToImage(photoBytes);
            }
        }

        private BitmapSource ByteArrayToImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private Photo CreatingPhotoObject()
        {
            using (var db = new test123Entities1())
            {
                Photo photo = new Photo();
                if (SystemContext.isChange == false)
                    photo.CollectionID = SystemContext.NewItem.Id;
                else
                    photo = SystemContext.Photo;
                photo.PPath = photoBytes;
                return photo;
            }
        }

        private void AddNewItem()
        {
            using (var db = new test123Entities1())
            {
                Items item = new Items();
                item.Title = "Новая коллекция" + db.Items.Where(i => i.IType == "Collection" && i.UserId == SystemContext.User.Id).Count();
                item.IType = "Collection";
                item.IPriority = 0;
                item.IsHidden = 0;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.UserId = SystemContext.User.Id;
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
                SystemContext.NewItem = item;
            }
        }

        private string AddNewPhoto()
        {
            using (var db = new test123Entities1())
            {
                AddNewItem();
                db.Photo.Add(CreatingPhotoObject());
                db.SaveChanges();
                return "Добавлен";
            }
        }

        private void ChangePhoto()
        {
            using (var db = new test123Entities1())
            {
                db.Photo.AddOrUpdate(CreatingPhotoObject());
                db.SaveChanges();
                MessageBox.Show("Данные изменены");
            }
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PhotoHolder.Source == null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Фото не добавлено, точно хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                    return;
                else if (messageBoxResult == MessageBoxResult.Yes && SystemContext.isChange)
                {
                    using (var db = new test123Entities1())
                    {
                        var photo = SystemContext.Photo;
                        db.Entry(photo).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }
                }
                else if (messageBoxResult == MessageBoxResult.Yes && SystemContext.isChange == false)
                {
                    this.Close();
                }
            }
            else
            {
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
                this.Close();
            }
        }

        private void Photo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(PhotoHolder);
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange)
                ChangePhoto();
            else
                AddNewPhoto();
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
            this.Close();
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("DocumentMoreInteractionsContextMenu");
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            if (SystemContext.isChange)
                ChangePhoto();
            else
                AddNewPhoto();
        }

        private void MenuItemCreateDocumentScan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Items item;
            item = SystemContext.Item;
            using (var db = new test123Entities1())
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
