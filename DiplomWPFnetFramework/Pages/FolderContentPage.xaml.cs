using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using DiplomWPFnetFramework.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using System.IO;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;

namespace DiplomWPFnetFramework.Pages
{
    /// <summary>
    /// Логика взаимодействия для FolderContentPage.xaml
    /// </summary>
    public partial class FolderContentPage : Page
    {
        Window parentWindow;
        public FolderContentPage()
        {
            InitializeComponent();
            LoadContent();
            FolderNameTextBlock.Text = SystemContext.Item.Title;
        }

        private void LoadContent()
        {
            using (var db = new test123Entities1())
            {
                List<Items> items = null;
                try
                {
                    items = (from i in db.Items
                             where i.UserId == SystemContext.User.Id && i.FolderId == SystemContext.Item.Id
                             select i).ToList<Items>();
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке документов");
                    return;
                }
                foreach (var item in items)
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
            parentWindow = Window.GetWindow(this);
            using (var db = new test123Entities1())
            {
                SystemContext.Item = (sender as Border).Tag as Items;
                SystemContext.isChange = true;
                switch (SystemContext.Item?.IType)
                {
                    case "Passport":
                        var passportWindow = new PassportWindow();
                        parentWindow.Close();
                        passportWindow.ShowDialog();
                        break;

                    case "INN":
                        var innWindow = new InnWindow();
                        parentWindow.Close();
                        innWindow.ShowDialog();
                        break;

                    case "SNILS":
                        var snilsWindow = new SnilsWindow();
                        parentWindow.Close();
                        snilsWindow.ShowDialog();
                        break;

                    case "Polis":
                        var polisWindow = new PolisWindow();
                        parentWindow.Close();
                        polisWindow.ShowDialog();
                        break;

                    case "Photo":
                        var photoWindow = new PhotoWindow();
                        parentWindow.Close();
                        photoWindow.ShowDialog();
                        break;

                    case "CreditCard":
                        var creditCardWindow = new CreditCardWindow();
                        parentWindow.Close();
                        creditCardWindow.ShowDialog();
                        break;

                    case "Folder":
                        SystemContext.Folder = SystemContext.Item;
                        Frame openFolderPageFrame = parentWindow.FindName("openPageFrame") as Frame;
                        FolderContentPage folderContentPage = new FolderContentPage();
                        openFolderPageFrame.Content = folderContentPage;
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
            //SystemContext.Item = (sender as Border).Tag as Items;
            SystemContext.Item = (sender as TextBlock).Tag as Items;
            SystemContext.isChangeTitleName = true;
            MessageBox.Show("Привет");
        }

        private void addNewElementsInFolderButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isFromFolder = true;
            parentWindow = Window.GetWindow(this);
            Frame openDocumentViewingPageFrame = parentWindow.FindName("openPageFrame") as Frame;
            DocumentViewingPage folderContentPage = new DocumentViewingPage();
            openDocumentViewingPageFrame.Content = folderContentPage;

        }

        private void CloseFolderPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            Frame openDocumentViewingPageFrame = parentWindow.FindName("openPageFrame") as Frame;
            DocumentViewingPage folderContentPage = new DocumentViewingPage();
            openDocumentViewingPageFrame.Content = folderContentPage;
        }
    }
}
