using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
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
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;
using Microsoft.Win32;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для CreditCardWindow.xaml
    /// </summary>
    public partial class CreditCardWindow : System.Windows.Window
    {
        byte[] creditCardImageBytes = null;
        byte[] coverImage = null;

        public CreditCardWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                CreditCardOutTextBlock.Text = SystemContext.Item.Title;
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
                var creditCard = (from p in db.CreditCard where p.Id == SystemContext.Item.Id select p).FirstOrDefault<CreditCard>();
                CardNumberTextBox.Text = creditCard.Number;
                FIOTextBox.Text = creditCard.FIO;
                MonthAndYearTextBox.Text = creditCard.ExpiryDate;
                CVVCodeTextBox.Text = creditCard.CVV.ToString();
                CreditCardPhotoHolder.Source = ByteArrayToImage(creditCard.PhotoPage1);
                creditCardImageBytes = creditCard.PhotoPage1;
            }

        }

        private CreditCard CreatingCreditCardObject()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                CreditCard creditCard = new CreditCard();
                if (SystemContext.isChange == false)
                    creditCard.Id = SystemContext.NewItem.Id;
                else
                    creditCard.Id = (from p in db.CreditCard where p.Id == SystemContext.Item.Id select p).FirstOrDefault<CreditCard>().Id;

                creditCard.Number = CardNumberTextBox.Text;
                creditCard.FIO = FIOTextBox.Text;
                creditCard.ExpiryDate = MonthAndYearTextBox.Text;
                creditCard.CVV = Convert.ToInt32(CVVCodeTextBox.Text);
                creditCard.PhotoPage1 = creditCardImageBytes;
                return creditCard;
            }
        }

        private string CheckingTheFullness()
        {
            if (CardNumberTextBox.Text != "" && FIOTextBox.Text != "" && MonthAndYearTextBox.Text != "" &&
                CVVCodeTextBox.Text != "")
                return "Добавить";
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
                item.Title = "Новая карта " + (db.Item.Where(i => i.Type == "CreditCard" && i.UserId == SystemContext.User.Id).Count() + 1);
                item.Type = "CreditCard";
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

        private void AddNewCreditCard()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                AddNewItem();
                db.CreditCard.Add(CreatingCreditCardObject());
                db.SaveChanges();
            }
        }

        private void ChangeCreditCard()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.CreditCard.AddOrUpdate(CreatingCreditCardObject());
                db.SaveChanges();
                MessageBox.Show("Данные изменены");
            }
        }

        private void ImageSetter(Image imageName)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileImage = openFileDialog.FileName;
                creditCardImageBytes = File.ReadAllBytes(fileImage);
                imageName.Source = ByteArrayToImage(creditCardImageBytes);
            }
        }

        private BitmapSource ByteArrayToImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
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

        private void PhotoPageOpen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainDataBorder.Visibility = Visibility.Hidden;
            CreditCardPhotoOpenTextBlock.Visibility = Visibility.Hidden;
            CreditCardDataOpenTextBlock.Visibility = Visibility.Visible;
            PhotoBorder.Visibility = Visibility.Visible;
        }

        private void CreditCardDataOpen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainDataBorder.Visibility = Visibility.Visible;
            CreditCardPhotoOpenTextBlock.Visibility = Visibility.Visible;
            CreditCardDataOpenTextBlock.Visibility = Visibility.Hidden;
            PhotoBorder.Visibility = Visibility.Hidden;
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("DocumentMoreInteractionsContextMenu");
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (SystemContext.isChange == false)
            {
                AddNewCreditCard();
                BackWindowButtonImage_MouseLeftButtonUp(sender, e);
            }
            else
            {
                ChangeCreditCard();
            }
        }

        private void CreditCardPhotoHolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(CreditCardPhotoHolder);
        }

        private void MenuItemave_Click(object sender, RoutedEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewCreditCard();
            }
            else
            {
                ChangeCreditCard();
            }
        }

        private void MenuItemChangeCover_Click(object sender, RoutedEventArgs e)
        {
            ChangeCoverImage();
        }

        private void MenuItemCreateDocumentScan_Click(object sender, RoutedEventArgs e)
        {
            CreditCard creditCard;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                creditCard = (from p in db.CreditCard where p.Id == SystemContext.Item.Id select p).FirstOrDefault<CreditCard>();
            }
            Application wordApp = new Application();

            Document doc = wordApp.Documents.Add();

            Paragraph numberParagraph = doc.Content.Paragraphs.Add();
            numberParagraph.Range.Text = $"Number: {creditCard.Number}";

            Paragraph fioParagraph = doc.Content.Paragraphs.Add();
            fioParagraph.Range.Text = $"FIO: {creditCard.FIO}";

            Paragraph expiryDateParagraph = doc.Content.Paragraphs.Add();
            expiryDateParagraph.Range.Text = $"Expiry Date: {creditCard.ExpiryDate}";

            Paragraph cvvParagraph = doc.Content.Paragraphs.Add();
            cvvParagraph.Range.Text = $"CVV: {creditCard.CVV}";

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
