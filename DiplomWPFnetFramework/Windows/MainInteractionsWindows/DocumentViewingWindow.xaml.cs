using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
using System.IO;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewingWindow.xaml
    /// </summary>
    public partial class DocumentViewingWindow : Window
    {
        public DocumentViewingWindow()
        {
            InitializeComponent();
            CheckIsGuest();
            LoadContent();
        }

        private void CheckIsGuest()
        {
            if (SystemContext.isGuest)
            {
                EmailOutTextBlock.Text = "Гость";
            }
            else
            {
                EmailOutTextBlock.Text = SystemContext.User.ULogin;
            }
        }

        private void LoadContent()
        {
            using (var db = new test123Entities1())
            {
                List<Items> items = null;
                try
                {
                    items = (from i in db.Items 
                             where i.UserId == SystemContext.User.Id
                             select i).ToList<Items>();
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке документов");
                    return;
                }
                foreach(var item in items)
                    AddNewDocument(item);
            }
        }

        private BitmapSource ByteArrayToImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void AddNewDocument(Items item)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)DocumentsViewGrid.Resources["ContentBorderStyle"], Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a8eab")) };
            var mainGrid = new Grid() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            var bottomDarkeningBorder = new Border() { Style = (Style)DocumentsViewGrid.Resources["BottomBorderProperties"] };

            ImageBrush imageBrush = new ImageBrush();
            Image image = new Image() { Resources = (ResourceDictionary)DocumentsViewGrid.Resources["CornerRadiusSetter"] };
            if (item.IImage != null && item.IImage.ToString() != "")
                image.Source = ByteArrayToImage(item.IImage);
            else
            {
                switch (item.IType) 
                {
                    case "CreditCard":
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/CreditCardPlugImage.png"));
                        break;

                    case "Folder":
                        break;

                    case "Collection":
                        break;

                    default:
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DocumentPlugImage.png"));
                        break;
                }
            }
            imageBrush.ImageSource = image.Source;
            imageBrush.Stretch = Stretch.UniformToFill;
            mainGrid.Background = imageBrush;

            bottomDarkeningBorder.VerticalAlignment = VerticalAlignment.Bottom;
            TextBlock itemName = new TextBlock() { Text = item.Title, Style = (Style)DocumentsViewGrid.Resources["DocumentTextBlockPropeties"] };

            borderPanel.Tag = item;
            bottomDarkeningBorder.Tag = item;
            itemName.Tag = item;

            borderPanel.MouseLeftButtonUp += ChangeItemButton_Click;
            bottomDarkeningBorder.MouseLeftButtonUp += ChangeTitleNameButton_Click;

            mainGrid.Children.Add(bottomDarkeningBorder);
            mainGrid.Children.Add(itemName);
            borderPanel.Child = mainGrid;
            DocumentsViewGrid.Children.Add(borderPanel);
        }

        private void ChangeItemButton_Click(object sender, MouseButtonEventArgs e)
        {
            using (var db = new test123Entities1())
            {
                SystemContext.Item = (sender as Border).Tag as Items;
                SystemContext.isChange = true;
                switch (SystemContext.Item?.IType) 
                {
                    case "Passport":
                        var passportWindow = new PassportWindow();
                        this.Close();
                        passportWindow.ShowDialog();
                        break;

                    case "INN":
                        var innWindow = new InnWindow();
                        this.Close();
                        innWindow.ShowDialog();
                        break;

                    case "SNILS":
                        var snilsWindow = new SnilsWindow();
                        this.Close();
                        snilsWindow.ShowDialog();
                        break;

                    case "Polis":
                        var polisWindow = new PolisWindow();
                        this.Close();
                        polisWindow.ShowDialog();
                        break;

                    case "Photo":
                        var photoWindow = new PhotoWindow();
                        this.Close(); 
                        photoWindow.ShowDialog();
                        break;

                    case "CreditCard":
                        var creditCardWindow = new CreditCardWindow();
                        this.Close();
                        creditCardWindow.ShowDialog();
                        break;

                    case "Folder":
                        MessageBox.Show("Открытие папки пока не реализовано");
                        break;
                    
                    case "Collection":
                        MessageBox.Show("Открытие коллекции пока не реализовано");
                        break;

                    default:
                        MessageBox.Show("Ошибка при открытии документа");
                        break;
                }
            } 
        }

        private void ChangeTitleNameButton_Click(object sender, MouseButtonEventArgs e)
        {
            SystemContext.Item = (sender as Border).Tag as Items;
        }

        private void OpenSettingPageButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoginWindow mainWindow = new LoginWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        private void sortImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Window1 window1 = new Window1();
            this.Close();
            window1.ShowDialog();
        }

        private void addNewDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            this.Close();
            window1.ShowDialog();
        }
    }
}
