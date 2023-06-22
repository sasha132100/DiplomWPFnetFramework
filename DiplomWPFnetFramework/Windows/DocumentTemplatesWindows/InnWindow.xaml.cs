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
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для InnWindow.xaml
    /// </summary>
    public partial class InnWindow : System.Windows.Window
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
            using (var db = new LocalMyDocsAppDBEntities())
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
            using (var db = new LocalMyDocsAppDBEntities())
            {
                INN inn = new INN();
                if (SystemContext.isChange == false)
                    inn.Id = SystemContext.NewItem.Id;
                else
                    inn.Id = (from p in db.INN where p.Id == SystemContext.Item.Id select p).FirstOrDefault<INN>().Id;

                inn.Number = INNNumberTextBox.Text.Replace("_", " ");
                inn.FIO = FIOTextBox.Text;
                inn.BirthDate = DateOfBirthDatePicker.SelectedDate;
                inn.RegistrationDate = INNRegistrationDateDatePicker.SelectedDate;
                inn.BirthPlace = BirthPlaceTextBox.Text;
                if (MaleChoiseRadioButton.IsChecked == true)
                    inn.Gender = "M";
                else
                    inn.Gender = "F";
                inn.PhotoPage1 = innPhotoBytes;
                inn.UpdateTime = DateTime.Now;
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
            using (var db = new LocalMyDocsAppDBEntities())
            {
                Item item = new Item();
                item.Id = Guid.NewGuid();
                item.Title = "Новый ИНН " + (db.Item.Where(i => i.Type == "INN" && i.UserId == SystemContext.User.Id).Count() + 1);
                item.Type = "INN";
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

        private void AddNewINN()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                try
                {
                    AddNewItem();
                    db.INN.Add(CreatingInnObject());
                    db.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Данные не должны превышать 250 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ChangeINN()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                try
                {
                    db.INN.AddOrUpdate(CreatingInnObject());
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
                innPhotoBytes = File.ReadAllBytes(fileImage);
                imageName.Source = ByteArrayToImage(innPhotoBytes);
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

        private void INNPhoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(INNPhotoHolder);
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewINN();
                BackWindowButtonImage_MouseLeftButtonUp(sender, e);
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

        private void MenuItemave_Click(object sender, RoutedEventArgs e)
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
            INN inn;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                inn = (from i in db.INN where i.Id == SystemContext.Item.Id select i).FirstOrDefault<INN>();
            }
            Application wordApp = new Application();

            Document doc = wordApp.Documents.Add();

            Paragraph innParagraph = doc.Content.Paragraphs.Add();
            innParagraph.Range.Text = "";
            innParagraph.Range.Text += $"Номер: {inn.Number}";
            innParagraph.Range.Text += $"ФИО: {inn.FIO}";
            innParagraph.Range.Text += $"Пол: {inn.Gender}";
            if (inn.BirthDate != null)
                innParagraph.Range.Text += $"Дата рождения: {inn.BirthDate.Value.ToString("dd.MM.yyyy")}";
            innParagraph.Range.Text += $"Место рождения: {inn.BirthPlace}";
            if (inn.RegistrationDate != null)
                innParagraph.Range.Text += $"Дата регистрации: {inn.RegistrationDate.Value.ToString("dd.MM.yyyy")}";
            
            if (inn.PhotoPage1 != null)
            {
                doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);

                Range imageRange1 = doc.Content.Paragraphs.Add().Range;
                imageRange1.InsertParagraphAfter();

                string tempImage1Path = Path.GetTempFileName();
                File.WriteAllBytes(tempImage1Path, inn.PhotoPage1);
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
