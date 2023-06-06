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
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using System.Data.Entity.Migrations;
using System.IO;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для InnWindow.xaml
    /// </summary>
    public partial class InnWindow : Window
    {
        byte[] innPhotoBytes = null;
        byte[] coverImage = null;

        public InnWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                INNOutTextBlock.Text = SystemContext.Item.Title;
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
                var inn = (from p in db.INN where p.Id == SystemContext.Item.Id select p).FirstOrDefault<INN>();
                try
                {
                    INNPhotoHolder.Source = ByteArrayToImage(inn.PhotoPage1);
                    innPhotoBytes = inn.PhotoPage1;
                }
                catch
                {

                }
                INNNumberTextBox.Text = inn.Number;
                FIOTextBox.Text = inn.FIO;
                DateOfBirthDatePicker.SelectedDate = inn.BirthDate;
                INNRegistrationDateDatePicker.SelectedDate = inn.RegistrationDate;
                BirthPlaceTextBox.Text = inn.BirthPlace;
                if (inn.Gender == "M")
                    MaleChoiseRadioButton.IsChecked = true;
                else
                    FemaleChoiseRadioButton.IsChecked = true;
            }

        }

        private INN CreatingInnObject()
        {
            using (var db = new test123Entities1())
            {
                INN inn = new INN();
                if (SystemContext.isChange == false)
                    inn.Id = SystemContext.NewItem.Id;
                else
                    inn.Id = (from p in db.INN where p.Id == SystemContext.Item.Id select p).FirstOrDefault<INN>().Id;

                inn.Number = INNNumberTextBox.Text;
                inn.FIO = FIOTextBox.Text;
                inn.BirthDate = DateOfBirthDatePicker.SelectedDate;
                inn.RegistrationDate = INNRegistrationDateDatePicker.SelectedDate;
                inn.BirthPlace = BirthPlaceTextBox.Text;
                if (MaleChoiseRadioButton.IsChecked == true)
                    inn.Gender = "M";
                else
                    inn.Gender = "F";
                inn.PhotoPage1 = innPhotoBytes;
                return inn;
            }
        }

        private string CheckingTheFullness()
        {
            if (INNNumberTextBox.Text != "" && FIOTextBox.Text != "" && BirthPlaceTextBox.Text != "" &&
                DateOfBirthDatePicker.Text != "" && INNRegistrationDateDatePicker.Text != "")
            {
                if (MaleChoiseRadioButton.IsChecked == true || FemaleChoiseRadioButton.IsChecked == true)
                    return "Добавить";
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы заполнили не все поля, вы уверены, что хотите сохранить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                    return "Не добавить";
                else
                    return "Добавить";
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы заполнили не все поля, вы уверены, что хотите сохранить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                    return "Не добавить";
                else
                    return "Добавить";
            }
        }

        private void AddNewItem()
        {
            using (var db = new test123Entities1())
            {
                Items item = new Items();
                item.Title = "Новый ИНН" + db.Items.Where(i => i.IType == "INN" && i.UserId == SystemContext.User.Id).Count();
                item.IType = "INN";
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

        private void AddNewINN()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new test123Entities1())
            {
                AddNewItem();
                db.INN.Add(CreatingInnObject());
                db.SaveChanges();
            }
        }

        private void ChangeINN()
        {
            using (var db = new test123Entities1())
            {
                db.INN.AddOrUpdate(CreatingInnObject());
                db.SaveChanges();
                MessageBox.Show("Данные изменены");
            }
        }

        private BitmapSource ByteArrayToImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void ImageSetter(Image imageName)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                innPhotoBytes = File.ReadAllBytes(filePath);
                imageName.Source = ByteArrayToImage(innPhotoBytes);
            }
        }

        private void ChangeCoverImage()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                coverImage = File.ReadAllBytes(filePath);
                Items item;
                item = SystemContext.Item;
                using (var db = new test123Entities1())
                {
                    item.IImage = coverImage;
                    db.Items.AddOrUpdate(item);
                    db.SaveChanges();
                }
            }
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

        private void INNPhoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(INNPhotoHolder);
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewINN();
            }
            else
            {
                ChangeINN();
            }
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
            if (SystemContext.isChange == false)
            {
                AddNewINN();
            }
            else
            {
                ChangeINN();
            }
        }

        private void MenuItemChangeCover_Click(object sender, RoutedEventArgs e)
        {
            ChangeCoverImage();
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
