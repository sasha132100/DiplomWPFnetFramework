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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Data;
using Application = Microsoft.Office.Interop.Word.Application;
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;
using Path = System.IO.Path;

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

                passport.SerialNumber = SerialAndNumberTextBox.Text.Replace("_", " ");
                passport.DivisionCode = DivisionCodeTextBox.Text.Replace("_", " ");
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
                passport.UpdateTime = DateTime.Now;
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
                item.UpdateTime = DateTime.Now;
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
                MessageBox.Show("Данные изменены.", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
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
                Item item = SystemContext.Item;
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    item.Image = coverImage;
                    item.UpdateTime = DateTime.Now;
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

            Paragraph passportParagraph = doc.Content.Paragraphs.Add();
            passportParagraph.Range.Text = "";
            passportParagraph.Range.Text += $"Серия и номер: {passport.SerialNumber}";
            passportParagraph.Range.Text += $"ФИО: {passport.FIO}";
            passportParagraph.Range.Text += $"Пол: {passport.Gender}";
            if (passport.BirthDate != null)
                passportParagraph.Range.Text += $"Дата рождения: {passport.BirthDate.Value.ToString("dd.MM.yyyy")}";
            passportParagraph.Range.Text += $"Место рождения: {passport.BirthPlace}";
            passportParagraph.Range.Text += $"Место жительства: {passport.ResidencePlace}";
            passportParagraph.Range.Text += $"Кем выдан: {passport.ByWhom}";
            passportParagraph.Range.Text += $"Код подразделения: {passport.DivisionCode}";
            if (passport.GiveDate != null)
                passportParagraph.Range.Text += $"Дата выдачи: {passport.GiveDate.Value.ToString("dd.MM.yyyy")}";

            if (passport.FacePhoto != null)
            {
                string tempFaceImagePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFaceImagePath, passport.FacePhoto);
                InlineShape shape = doc.InlineShapes.AddPicture(tempFaceImagePath);
                shape.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoFalse;
                shape.Width = 180;
                shape.Height = 220;
                File.Delete(tempFaceImagePath);
            }

            if (passport.PhotoPage1 != null)
            {
                doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);

                Range imageRange1 = doc.Content.Paragraphs.Add().Range;
                imageRange1.InsertParagraphAfter();

                string tempImage1Path = Path.GetTempFileName();
                File.WriteAllBytes(tempImage1Path, passport.PhotoPage1);
                InlineShape shape1 = imageRange1.InlineShapes.AddPicture(tempImage1Path);
                File.Delete(tempImage1Path);
            }

            if (passport.PhotoPage2 != null)
            {
                doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);

                Range imageRange2 = doc.Content.Paragraphs.Add().Range;
                imageRange2.InsertParagraphAfter();

                string tempImage2Path = Path.GetTempFileName();
                File.WriteAllBytes(tempImage2Path, passport.PhotoPage2);
                InlineShape shape2 = imageRange2.InlineShapes.AddPicture(tempImage2Path);
                File.Delete(tempImage2Path);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.FileName = $"{SystemContext.Item.Title}.pdf";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string outputPath = saveFileDialog.FileName;

                    doc.ExportAsFixedFormat(outputPath, WdExportFormat.wdExportFormatPDF);
                }
                catch
                {
                    MessageBox.Show("Закойте документ для обновления!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
}
