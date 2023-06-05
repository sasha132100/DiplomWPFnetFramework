using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
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
using System.Windows.Shapes;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для PhotoWindow.xaml
    /// </summary>
    public partial class PhotoWindow : Window
    {
        byte[] photoBytes = null;

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
                return;
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
                item.Title = "NewTitle" + db.Items.OrderByDescending(items => items.Id).FirstOrDefault().Id.ToString();
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
                MessageBoxResult messageBoxResult = MessageBox.Show("Фото не добавлено, точно хотите выйти?");
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
                if (SystemContext.isChange)
                    ChangePhoto();
                else
                    AddNewPhoto();
                this.Close();
            }
        }

        private void Photo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(PhotoHolder);
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
