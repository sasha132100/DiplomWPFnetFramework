using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Application = Microsoft.Office.Interop.Word.Application;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для PassportWindow.xaml
    /// </summary>
    public partial class PassportWindow : System.Windows.Window
    {
        byte[] ownersPhotoBytes = null;
        byte[] passportPhoto1Bytes = null;
        byte[] passportPhoto2Bytes = null;
        byte[] coverImage = null;

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
            {
                DocumentMoreInteractionsGrid.Visibility = Visibility.Hidden;
                confirmButtonImage.Margin = new Thickness(0, 0, 10, 0);
                return;
            }
            using (var db = new LocalMyDocsAppDBEntities())
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
            using (var db = new LocalMyDocsAppDBEntities())
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
            using (var db = new LocalMyDocsAppDBEntities())
            {
                Item item = new Item();
                item.Id = Guid.NewGuid();
                item.Title = "Новый паспорт " + (db.Item.Where(i => i.Type == "Passport" && i.UserId == SystemContext.User.Id).Count() + 1);
                item.Type = "Passport";
                item.Priority = 0;
                item.IsHidden = 0;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.FolderId = Guid.Empty;
                item.UserId = SystemContext.User.Id;
                db.Item.Add(item);
                db.SaveChanges();
                SystemContext.NewItem = item;
            }
        }

        private void AddNewPassport()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                AddNewItem();
                db.Passport.Add(CreatingPassportObject());
                db.SaveChanges();
            }
        }

        private void ChangePassport()
        {
            using (var db = new LocalMyDocsAppDBEntities())
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
                string fileImage = openFileDialog.FileName;
                switch (imageName.Name)
                {
                    case "OwnersPhotoHolder":
                        ownersPhotoBytes = File.ReadAllBytes(fileImage);
                        imageName.Source = ByteArrayToImage(ownersPhotoBytes);
                        break;

                    case "PassportPhoto1Holder":
                        passportPhoto1Bytes = File.ReadAllBytes(fileImage);
                        imageName.Source = ByteArrayToImage(passportPhoto1Bytes);
                        break;

                    case "PassportPhoto2Holder":
                        passportPhoto2Bytes = File.ReadAllBytes(fileImage);
                        imageName.Source = ByteArrayToImage(passportPhoto2Bytes);
                        break;

                    default:
                        break;
                }
            }
        }

        private void ChangeCoverImage()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileImage = openFileDialog.FileName;
                coverImage = File.ReadAllBytes(fileImage);
                Item item;
                item = SystemContext.Item;
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    item.Image = coverImage;
                    db.Item.AddOrUpdate(item);
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

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewPassport();
                BackWindowButtonImage_MouseLeftButtonUp(sender, e);
            }
            else
            {
                ChangePassport();
            }
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("DocumentMoreInteractionsContextMenu");
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void MenuItemave_Click(object sender, RoutedEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewPassport();
            }
            else
            {
                ChangePassport();
            }
        }

        private void MenuItemChangeCover_Click(object sender, RoutedEventArgs e)
        {
            ChangeCoverImage();
        }

        private void MenuItemCreateDocumentScan_Click(object sender, RoutedEventArgs e)
        {
            Passport passport;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                passport = (from p in db.Passport where p.Id == SystemContext.Item.Id select p).FirstOrDefault<Passport>();
            }
            Application wordApp = new Application();

            Document doc = wordApp.Documents.Add();

            Paragraph serialNumberParagraph = doc.Content.Paragraphs.Add();
            serialNumberParagraph.Range.Text = $"Serial Number: {passport.SerialNumber}";

            Paragraph fioParagraph = doc.Content.Paragraphs.Add();
            fioParagraph.Range.Text = $"FIO: {passport.FIO}";

            Paragraph genderParagraph = doc.Content.Paragraphs.Add();
            genderParagraph.Range.Text = $"Gender: {passport.Gender}";

            Paragraph birthDateParagraph = doc.Content.Paragraphs.Add();
            birthDateParagraph.Range.Text = $"Birth Date: {passport.BirthDate}";

            Paragraph birthPlaceParagraph = doc.Content.Paragraphs.Add();
            birthPlaceParagraph.Range.Text = $"Birth Place: {passport.BirthPlace}";

            Paragraph residencePlaceParagraph = doc.Content.Paragraphs.Add();
            residencePlaceParagraph.Range.Text = $"Residence Place: {passport.ResidencePlace}";

            Paragraph byWhomParagraph = doc.Content.Paragraphs.Add();
            byWhomParagraph.Range.Text = $"By Whom: {passport.ByWhom}";

            Paragraph divisionCodeParagraph = doc.Content.Paragraphs.Add();
            divisionCodeParagraph.Range.Text = $"Division Code: {passport.DivisionCode}";

            Paragraph giveDateParagraph = doc.Content.Paragraphs.Add();
            giveDateParagraph.Range.Text = $"Give Date: {passport.GiveDate}";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.FileName = "CreditCardDetails.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                string outputPath = saveFileDialog.FileName;

                doc.ExportAsFixedFormat(outputPath, WdExportFormat.wdExportFormatPDF);
            }
            else
            {
                return;
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Item item;
            item = SystemContext.Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
