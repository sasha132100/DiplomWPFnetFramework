using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using System.Data.Entity.Migrations;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для PassportWindow.xaml
    /// </summary>
    public partial class PassportWindow : Window
    {
        byte[] ownersPhotoBytes = null;
        byte[] passportPhoto1Bytes = null;
        byte[] passportPhoto2Bytes = null;

        public PassportWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                PassportNameOutTextBlock.Text = SystemContext.Item.Title;
        }

        private void LoadContent()
        {
            if (SystemContext.isChange == false)
                return;
            using (var db = new test123Entities1())
            {
                var passport = (from p in db.Passport where p.Id == SystemContext.Item.Id select p).FirstOrDefault<Passport>();
                try
                {
                    OwnersPhotoHolder.Source = ByteArrayToImage(passport.FacePhoto);
                    ownersPhotoBytes = passport.FacePhoto;
                    PassportPhoto1Holder.Source = ByteArrayToImage(passport.PhotoPage1);
                    passportPhoto1Bytes = passport.PhotoPage1;
                    PassportPhoto2Holder.Source = ByteArrayToImage(passport.PhotoPage2);
                    passportPhoto2Bytes = passport.PhotoPage2;
                }
                catch
                {
                    
                }
                SerialAndNumberTextBox.Text = passport.SerialNumber;
                DivisionCodeTextBox.Text = passport.DivisionCode;
                DateOfIssueDatePicker.SelectedDate = passport.GiveDate;
                IssuedByWhomTextBox.Text = passport.ByWhom;
                FIOTextBox.Text = passport.FIO;
                DateOfBirthDatePicker.SelectedDate = passport.BirthDate;
                if (passport.Gender == "M")
                    MaleChoiseRadioButton.IsChecked = true;
                else
                    FemaleChoiseRadioButton.IsChecked = true;
                PlaceOfBirthTextBox.Text = passport.BirthPlace;
                PlaceOfResidenceTextBox.Text = passport.ResidencePlace;
            }

        }

        private Passport CreatingPassportObject()
        {
            using (var db = new test123Entities1())
            {   
                Passport passport = new Passport();
                if (SystemContext.isChange == false)
                    passport.Id = SystemContext.NewItem.Id;
                else
                    passport.Id = (from p in db.Passport where p.Id == SystemContext.Item.Id select p).FirstOrDefault<Passport>().Id;

                passport.SerialNumber = SerialAndNumberTextBox.Text;
                passport.DivisionCode = DivisionCodeTextBox.Text;
                passport.GiveDate = DateOfIssueDatePicker.SelectedDate;
                passport.ByWhom = IssuedByWhomTextBox.Text;
                passport.FIO = FIOTextBox.Text;
                passport.BirthDate = DateOfBirthDatePicker.SelectedDate;
                if (MaleChoiseRadioButton.IsChecked == true)
                    passport.Gender = "M";
                else
                    passport.Gender = "F";
                passport.BirthPlace = PlaceOfBirthTextBox.Text;
                passport.ResidencePlace = PlaceOfResidenceTextBox.Text;
                passport.FacePhoto = ownersPhotoBytes;
                passport.PhotoPage1 = passportPhoto1Bytes;
                passport.PhotoPage2 = passportPhoto2Bytes;
                return passport;
            }
        }

        private string CheckingTheFullness()
        {
            if (SerialAndNumberTextBox.Text != "" && DivisionCodeTextBox.Text != "" && DateOfIssueDatePicker.Text != "" &&
                IssuedByWhomTextBox.Text != "" && FIOTextBox.Text != "" && DateOfBirthDatePicker.Text != "" &&
                PlaceOfBirthTextBox.Text != "" && PlaceOfResidenceTextBox.Text != "")
            {
                if (MaleChoiseRadioButton.IsChecked == true || FemaleChoiseRadioButton.IsChecked == true)
                    return "Добавить";
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы заполнили не все поля, уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                    return "Не добавить";
                else
                    return "Добавить";
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы заполнили не все поля, уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
                item.Title = "NewTitle" + db.Items.OrderByDescending(items => items.Id).FirstOrDefault().Id.ToString();
                item.IType = "Passport";
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

        private void AddNewPassport()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new test123Entities1())
            {
                AddNewItem();
                db.Passport.Add(CreatingPassportObject());
                db.SaveChanges();
            }
        }

        private void ChangePassport()
        {
            using (var db = new test123Entities1())
            {
                db.Passport.AddOrUpdate(CreatingPassportObject());
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
                switch (imageName.Name)
                {
                    case "OwnersPhotoHolder":
                        ownersPhotoBytes = File.ReadAllBytes(filePath);
                        imageName.Source = ByteArrayToImage(ownersPhotoBytes);
                        break;

                    case "PassportPhoto1Holder":
                        passportPhoto1Bytes = File.ReadAllBytes(filePath);
                        imageName.Source = ByteArrayToImage(passportPhoto1Bytes);
                        break;

                    case "PassportPhoto2Holder":
                        passportPhoto2Bytes = File.ReadAllBytes(filePath);
                        imageName.Source = ByteArrayToImage(passportPhoto2Bytes);
                        break;

                    default:
                        break;
                }
            }
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewPassport();
            }
            else
            {
                ChangePassport();
            }
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

        private void PassportNameOutTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void OwnersPhoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(OwnersPhotoHolder);
        }

        private void PassportPhoto1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(PassportPhoto1Holder);
        }

        private void PassportPhoto2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(PassportPhoto2Holder);
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
