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
    /// Логика взаимодействия для SnilsWindow.xaml
    /// </summary>
    public partial class SnilsWindow : Window
    {
        byte[] snilsPhotoBytes = null;

        public SnilsWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                SNILSOutTextBlock.Text = SystemContext.Item.Title;
        }

        private void LoadContent()
        {
            if (SystemContext.isChange == false)
                return;
            using (var db = new test123Entities1())
            {
                var snils = (from p in db.SNILS where p.Id == SystemContext.Item.Id select p).FirstOrDefault<SNILS>();
                try
                {
                    SnilsPhotoHolder.Source = ByteArrayToImage(snils.PhotoPage1);
                    snilsPhotoBytes = snils.PhotoPage1;
                }
                catch
                {

                }
                SNILSNumberTextBox.Text = snils.Number;
                FIOTextBox.Text = snils.FIO;
                DateOfBirthDatePicker.SelectedDate = snils.BirthDate;
                RegistrationDateDatePicker.SelectedDate = snils.RegistrationDate;
                PlaceOfBirthTextBox.Text = snils.BirthPlace;
                if (snils.Gender == "M")
                    MaleChoiseRadioButton.IsChecked = true;
                else
                    FemaleChoiseRadioButton.IsChecked = true;
            }

        }

        private SNILS CreatingSnilsObject()
        {
            using (var db = new test123Entities1())
            {
                SNILS snils = new SNILS();
                if (SystemContext.isChange == false)
                    snils.Id = SystemContext.NewItem.Id;
                else
                    snils.Id = (from p in db.SNILS where p.Id == SystemContext.Item.Id select p).FirstOrDefault<SNILS>().Id;

                snils.Number = SNILSNumberTextBox.Text;
                snils.FIO = FIOTextBox.Text;
                snils.BirthDate = DateOfBirthDatePicker.SelectedDate;
                snils.RegistrationDate = RegistrationDateDatePicker.SelectedDate;
                snils.BirthPlace = PlaceOfBirthTextBox.Text;
                if (MaleChoiseRadioButton.IsChecked == true)
                    snils.Gender = "M";
                else
                    snils.Gender = "F";
                snils.PhotoPage1 = snilsPhotoBytes;
                return snils;
            }
        }

        private string CheckingTheFullness()
        {
            if (SNILSNumberTextBox.Text != "" && FIOTextBox.Text != "" && PlaceOfBirthTextBox.Text != "" &&
                DateOfBirthDatePicker.Text != "" && RegistrationDateDatePicker.Text != "")
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
                item.IType = "SNILS";
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

        private string AddNewSnils()
        {
            if (CheckingTheFullness() != "Заполнены")
                return "Не заполнены";
            using (var db = new test123Entities1())
            {
                AddNewItem();
                db.SNILS.Add(CreatingSnilsObject());
                db.SaveChanges();
                return "Добавлен";
            }
        }

        private void ChangeSnils()
        {
            using (var db = new test123Entities1())
            {
                db.SNILS.AddOrUpdate(CreatingSnilsObject());
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
                snilsPhotoBytes = File.ReadAllBytes(filePath);
                imageName.Source = ByteArrayToImage(snilsPhotoBytes);
            }
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                if (AddNewSnils() == "Не заполнены")
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Вы заполнили не все поля, уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.No)
                        return;
                }
            }
            else
            {
                ChangeSnils();
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

        private void SnilsPhoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(SnilsPhotoHolder);
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
