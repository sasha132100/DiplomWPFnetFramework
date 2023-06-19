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
using System.Globalization;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;
using Microsoft.Win32;
using System.Xml.Linq;
using Path = System.IO.Path;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для PolisWindow.xaml
    /// </summary>
    public partial class PolisWindow : System.Windows.Window
    {
        byte[] polisPhoto1Bytes = null;
        byte[] polisPhoto2Bytes = null;
        byte[] coverImage = null;

        public PolisWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                PolisOutTextBlock.Text = SystemContext.Item.Title;
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
                var polis = (from p in db.Polis where p.Id == SystemContext.Item.Id select p).FirstOrDefault<Polis>();
                try
                {
                    PolisPhoto1Holder.Source = ByteArrayToImage(polis.PhotoPage1);
                    polisPhoto1Bytes = polis.PhotoPage1;
                    PolisPhoto2Holder.Source = ByteArrayToImage(polis.PhotoPage2);
                    polisPhoto2Bytes = polis.PhotoPage2;
                }
                catch
                {

                }
                PolisNumberTextBox.Text = polis.Number;
                FIOTextBox.Text = polis.FIO;
                DateOfBirthDatePicker.SelectedDate = polis.BirthDate;
                ValidUntilTextBox.Text = polis.ValidUntil;
                if (polis.Gender == "M")
                    MaleChoiseRadioButton.IsChecked = true;
                else
                    FemaleChoiseRadioButton.IsChecked = true;
            }

        }

        private Polis CreatingPolisObject()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                Polis polis = new Polis();
                if (SystemContext.isChange == false)
                    polis.Id = SystemContext.NewItem.Id;
                else
                    polis.Id = (from p in db.Polis where p.Id == SystemContext.Item.Id select p).FirstOrDefault<Polis>().Id;

                polis.Number = PolisNumberTextBox.Text;
                polis.FIO = FIOTextBox.Text;
                polis.BirthDate = DateOfBirthDatePicker.SelectedDate;
                polis.ValidUntil = ValidUntilTextBox.Text;
                if (MaleChoiseRadioButton.IsChecked == true)
                    polis.Gender = "M";
                else
                    polis.Gender = "F";
                polis.PhotoPage1 = polisPhoto1Bytes;
                polis.PhotoPage2 = polisPhoto2Bytes;
                return polis;
            }
        }

        private string CheckingTheFullness()
        {
            if (PolisNumberTextBox.Text != "" && FIOTextBox.Text != "" && ValidUntilTextBox.Text != "" &&
                DateOfBirthDatePicker.Text != "")
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
                item.Title = "Новый полис " + (db.Item.Where(i => i.Type == "Polis" && i.UserId == SystemContext.User.Id).Count() + 1);
                item.Type = "Polis";
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

        private void AddNewPolis()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                AddNewItem();
                db.Polis.Add(CreatingPolisObject());
                db.SaveChanges();
            }
        }

        private void ChangePolis()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.Polis.AddOrUpdate(CreatingPolisObject());
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
                    case "PolisPhoto1Holder":
                        polisPhoto1Bytes = File.ReadAllBytes(fileImage);
                        imageName.Source = ByteArrayToImage(polisPhoto1Bytes);
                        break;

                    case "PolisPhoto2Holder":
                        polisPhoto2Bytes = File.ReadAllBytes(fileImage);
                        imageName.Source = ByteArrayToImage(polisPhoto2Bytes);
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

        private void PolisPhoto1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(PolisPhoto1Holder);
        }

        private void PolisPhoto2_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(PolisPhoto2Holder);
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewPolis();
                BackWindowButtonImage_MouseLeftButtonUp(sender, e);
            }
            else
            {
                ChangePolis();
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
                AddNewPolis();
            }
            else
            {
                ChangePolis();
            }
        }

        private void MenuItemChangeCover_Click(object sender, RoutedEventArgs e)
        {
            ChangeCoverImage();
        }

        private void MenuItemCreateDocumentScan_Click(object sender, RoutedEventArgs e)
        {
            Polis polis;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                polis = (from p in db.Polis where p.Id == SystemContext.Item.Id select p).FirstOrDefault<Polis>();
            }

            Application wordApp = new Application();

            Document doc = wordApp.Documents.Add();

            Paragraph passportParagraph = doc.Content.Paragraphs.Add();
            passportParagraph.Range.Text = "";
            passportParagraph.Range.Text += $"Номер: {polis.Number}";
            passportParagraph.Range.Text += $"ФИО: {polis.FIO}";
            passportParagraph.Range.Text += $"Пол: {polis.Gender}";
            passportParagraph.Range.Text += $"Дата рождения: {polis.BirthDate}";
            passportParagraph.Range.Text += $"Годен до: {polis.ValidUntil}";

            doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);

            Range imageRange1 = doc.Content.Paragraphs.Add().Range;
            imageRange1.InsertParagraphAfter();

            string tempImage1Path = Path.GetTempFileName();
            File.WriteAllBytes(tempImage1Path, polis.PhotoPage1);
            InlineShape shape1 = imageRange1.InlineShapes.AddPicture(tempImage1Path);
            File.Delete(tempImage1Path);

            doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);

            Range imageRange2 = doc.Content.Paragraphs.Add().Range;
            imageRange2.InsertParagraphAfter();

            string tempImage2Path = Path.GetTempFileName();
            File.WriteAllBytes(tempImage2Path, polis.PhotoPage2);
            InlineShape shape2 = imageRange2.InlineShapes.AddPicture(tempImage2Path);
            File.Delete(tempImage2Path);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.FileName = $"{SystemContext.Item.Title}.pdf";

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
