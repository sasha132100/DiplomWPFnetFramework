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

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для PolisWindow.xaml
    /// </summary>
    public partial class PolisWindow : Window
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
            using (var db = new test123Entities1())
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
            using (var db = new test123Entities1())
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
            using (var db = new test123Entities1())
            {
                Items item = new Items();
                item.Title = "Новый полис" + db.Items.Where(i => i.IType == "Polis" && i.UserId == SystemContext.User.Id).Count();
                item.IType = "Polis";
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

        private void AddNewPolis()
        {
            if (CheckingTheFullness() != "Добавить")
                return;
            using (var db = new test123Entities1())
            {
                AddNewItem();
                db.Polis.Add(CreatingPolisObject());
                db.SaveChanges();
            }
        }

        private void ChangePolis()
        {
            using (var db = new test123Entities1())
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
                string filePath = openFileDialog.FileName;
                switch (imageName.Name)
                {
                    case "PolisPhoto1Holder":
                        polisPhoto1Bytes = File.ReadAllBytes(filePath);
                        imageName.Source = ByteArrayToImage(polisPhoto1Bytes);
                        break;

                    case "PolisPhoto2Holder":
                        polisPhoto2Bytes = File.ReadAllBytes(filePath);
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

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
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
