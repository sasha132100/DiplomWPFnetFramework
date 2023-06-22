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
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Microsoft.Win32;
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;
using Path = System.IO.Path;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для SnilsWindow.xaml
    /// </summary>
    public partial class SnilsWindow : System.Windows.Window
    {
        byte[] snilsPhotoBytes = null;
        byte[] coverImage = null;

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
            {
                DocumentMoreInteractionsGrid.Visibility = Visibility.Hidden;
                confirmButtonImage.Margin = new Thickness(0, 0, 10, 0);
                return;
            }
            using (var db = new LocalMyDocsAppDBEntities())
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
            using (var db = new LocalMyDocsAppDBEntities())
            {
                SNILS snils = new SNILS();
                if (SystemContext.isChange == false)
                    snils.Id = SystemContext.NewItem.Id;
                else
                    snils.Id = (from p in db.SNILS where p.Id == SystemContext.Item.Id select p).FirstOrDefault<SNILS>().Id;

                snils.Number = SNILSNumberTextBox.Text.Replace("_"," ");
                snils.FIO = FIOTextBox.Text;
                snils.BirthDate = DateOfBirthDatePicker.SelectedDate;
                snils.RegistrationDate = RegistrationDateDatePicker.SelectedDate;
                snils.BirthPlace = PlaceOfBirthTextBox.Text;
                if (MaleChoiseRadioButton.IsChecked == true)
                    snils.Gender = "M";
                else
                    snils.Gender = "F";
                snils.PhotoPage1 = snilsPhotoBytes;
                snils.UpdateTime = DateTime.Now;
                return snils;
            }
        }

        private string CheckingTheFullness()
        {
            if (SNILSNumberTextBox.Text != "" && FIOTextBox.Text != "" && PlaceOfBirthTextBox.Text != "" &&
                DateOfBirthDatePicker.Text != "" && RegistrationDateDatePicker.Text != "")
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
                item.Title = "Новый снилс " + (db.Item.Where(i => i.Type == "SNILS" && i.UserId == SystemContext.User.Id).Count() + 1);
                item.Type = "SNILS";
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

        private void AddNewSnils()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                try
                {
                    AddNewItem();
                    db.SNILS.Add(CreatingSnilsObject());
                    db.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Данные не должны превышать 250 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ChangeSnils()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                try
                {
                    db.SNILS.AddOrUpdate(CreatingSnilsObject());
                    db.SaveChanges();
                    MessageBox.Show("Данные изменены.", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Данные не должны превышать 250 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
                snilsPhotoBytes = File.ReadAllBytes(fileImage);
                imageName.Source = ByteArrayToImage(snilsPhotoBytes);
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

        private void SnilsPhoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(SnilsPhotoHolder);
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewSnils();
                BackWindowButtonImage_MouseLeftButtonUp(sender, e);
            }
            else
            {
                ChangeSnils();
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
                AddNewSnils();
            }
            else
            {
                ChangeSnils();
            }
        }

        private void MenuItemChangeCover_Click(object sender, RoutedEventArgs e)
        {
            ChangeCoverImage();
        }

        private void MenuItemCreateDocumentScan_Click(object sender, RoutedEventArgs e)
        {
            SNILS snils;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                snils = (from s in db.SNILS where s.Id == SystemContext.Item.Id select s).FirstOrDefault<SNILS>();
            }
            Application wordApp = new Application();

            Document doc = wordApp.Documents.Add();

            Paragraph passportParagraph = doc.Content.Paragraphs.Add();
            passportParagraph.Range.Text = "";
            passportParagraph.Range.Text += $"Номер: {snils.Number}";
            passportParagraph.Range.Text += $"ФИО: {snils.FIO}";
            passportParagraph.Range.Text += $"Пол: {snils.Gender}";
            if (snils.BirthDate != null)
                passportParagraph.Range.Text += $"Дата рождения: {snils.BirthDate.Value.ToString("dd.MM.yyyy")}";
            passportParagraph.Range.Text += $"Место рождения: {snils.BirthPlace}";
            if (snils.RegistrationDate != null)
                passportParagraph.Range.Text += $"Дата регистрации: {snils.RegistrationDate.Value.ToString("dd.MM.yyyy")}";

            if (snils.PhotoPage1 != null)
            {
                doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);

                Range imageRange1 = doc.Content.Paragraphs.Add().Range;
                imageRange1.InsertParagraphAfter();

                string tempImage1Path = Path.GetTempFileName();
                File.WriteAllBytes(tempImage1Path, snils.PhotoPage1);
                InlineShape shape1 = imageRange1.InlineShapes.AddPicture(tempImage1Path);
                File.Delete(tempImage1Path);
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
