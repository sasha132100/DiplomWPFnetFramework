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

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для InnWindow.xaml
    /// </summary>
    public partial class InnWindow : Window
    {
        byte[] innPhotoBytes = null;

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
                return;
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
                    inn.Id = (from p in db.SNILS where p.Id == SystemContext.Item.Id select p).FirstOrDefault<SNILS>().Id;

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
                    return "Заполнены";
                return "Не заполнены";
            }
            else
                return "Не заполнены";
        }

        private void AddNewItem()
        {
            using (var db = new test123Entities1())
            {
                Items item = new Items();
                item.Title = "NewTitle" + db.Items.OrderByDescending(items => items.Id).FirstOrDefault().Id.ToString();
                item.IType = "Photo";
                item.IPriority = 0;
                item.IsHidden = 0;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.UserId = SystemContext.User.Id;
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
                SystemContext.NewItem = (from i in db.Items where i.DateCreation == item.DateCreation select i).FirstOrDefault<Items>();
            }
        }

        private string AddNewINN()
        {
            if (CheckingTheFullness() != "Заполнены")
                return "Не заполнены";
            using (var db = new test123Entities1())
            {
                AddNewItem();
                db.INN.Add(CreatingInnObject());
                db.SaveChanges();
                return "Добавлен";
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

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                if (AddNewINN() == "Не заполнены")
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Вы заполнили не все поля, уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.No)
                        return;
                }
            }
            else
            {
                ChangeINN();
            }
            DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            this.Close();
            documentViewingWindow.ShowDialog();
        }

        private void INNPhoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(INNPhotoHolder);
        }
    }
}
