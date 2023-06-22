using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
using DiplomWPFnetFramework.Windows.BufferWindows;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows;
using System;
using System.Linq;
using System.Data.Entity.Migrations;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewingWindow.xaml
    /// </summary>
    public partial class DocumentViewingWindow : Window
    {
        byte[] avatarsPhotoBytes = null;

        public DocumentViewingWindow()
        {
            InitializeComponent();
            CheckIsGuest();
        }

        private void CheckIsGuest()
        {
            LoginOutTextBlock.Text = SystemContext.User.Login;
            DocumentViewingPage documentViewingPage = new DocumentViewingPage();
            openPageFrame.Content = documentViewingPage;
            if (SystemContext.isGuest)
            {
                AccountSettingsTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6D6F80"));
                SynchronizationTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6D6F80"));
                MyTemplatesTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6D6F80"));
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
                avatarsPhotoBytes = File.ReadAllBytes(fileImage);
                imageName.Source = ByteArrayToImage(avatarsPhotoBytes);
                AvatarPhotoImage.Width = 150;
                AvatarPhotoImage.Height = 150;
                AvatarPhotoImage.Stretch = Stretch.Fill;
                User user = new User();
                user = SystemContext.User;
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    user.Photo = avatarsPhotoBytes;
                    db.User.AddOrUpdate(user);
                    db.SaveChanges();
                }
            }
        }

        private MenuItem FindMenuItemByName(ContextMenu contextMenu, string menuItemName)
        {
            foreach (var item in contextMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem.Name == menuItemName)
                    return menuItem;
            }
            return null;
        }

        private void SetterSortName(ContextMenu contextMenu)
        {
            if (!SystemContext.isDocumentNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowDocuments").Header = "Показать документы";
            else if (SystemContext.isDocumentNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowDocuments").Header = "Скрыть документы";

            if (!SystemContext.isCreditCardNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCreditCards").Header = "Показать карты";
            else if (SystemContext.isCreditCardNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCreditCards").Header = "Скрыть карты";

            if (!SystemContext.isCollectionNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCollections").Header = "Показать коллекции";
            else if (SystemContext.isCollectionNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowCollections").Header = "Скрыть коллекции";

            if (!SystemContext.isFolderNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowFolders").Header = "Показать папки";
            else if (SystemContext.isFolderNeedToShow)
                FindMenuItemByName(contextMenu, "HideShowFolders").Header = "Скрыть папки";
        }

        private void OpenSettingPageButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SettingsGrid.Width == 0)
            {
                UserEmailTextBlock.Text = "";
                if (SystemContext.isGuest)
                {
                    AvatarPhotoImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/AvatarPhotoImage.png"));
                    UserEmailTextBlock.Text = "Гость";
                }
                else if (SystemContext.User.Photo == null)
                {
                    AvatarPhotoImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/UserAvatarPlug.png"));
                    AvatarPhotoImage.Width = 120;
                    AvatarPhotoImage.Height = 120;
                    AvatarPhotoImage.Stretch = Stretch.Uniform;
                    UserEmailTextBlock.Text += SystemContext.User.Email;
                }
                else
                {
                    AvatarPhotoImage.Source = ByteArrayToImage(SystemContext.User.Photo);
                    UserEmailTextBlock.Text += SystemContext.User.Email;
                }
                SettingsGrid.Width = 450;
            }
            else
                SettingsGrid.Width = 0;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CheckIsGuest();
        }

        private void sortImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isFromFolder)
                return;
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("SortContextMenu");
            SetterSortName(contextMenu);
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void ChangeAccountTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (!SystemContext.isGuest)
                {
                    LastLogginedUser lastLogginedUser = (from llu in db.LastLogginedUser select llu).FirstOrDefault();
                    lastLogginedUser.Email = null;
                    lastLogginedUser.Login = null;
                    db.LastLogginedUser.AddOrUpdate(lastLogginedUser);
                    db.SaveChanges();
                    SystemContext.isSystemStart = false;
                }
            }
            LoginWindow loginWindow = new LoginWindow();
            this.Close();
            loginWindow.ShowDialog();
        }

        private void AvatarPhotoBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isGuest)
            {
                MessageBox.Show("Для доступа к данной функции необходимо зарегистрироваться!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ImageSetter(AvatarPhotoImage);
        }

        private void HiddenFilesButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.FromWhichWindowIsCalled = "DocumentViewingWindow";
            if (SystemContext.User.AccessCode == null)
            {
                MessageBox.Show("Назначте код доступа в меню настроек -> Безопасность", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                EnteringAccessCodeWindow enteringAccessCodeWindow = new EnteringAccessCodeWindow();
                enteringAccessCodeWindow.Owner = this;
                enteringAccessCodeWindow.ShowDialog();
            }
        }

        private void MyTemplatesButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isGuest)
            {
                MessageBox.Show("Для доступа к данной функции необходимо зарегистрироваться!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SystemContext.WindowType = "MyTemplates";
            SettingsAndPatternWindow settingsAndPatternWindow = new SettingsAndPatternWindow();
            this.Close();
            settingsAndPatternWindow.ShowDialog();
        }

        private void SynchronizationButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isGuest)
            {
                MessageBox.Show("Для доступа к данной функции необходимо зарегистрироваться!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SynchronizationWindow synchronizationWindow = new SynchronizationWindow();
            synchronizationWindow.Closed += Window_Closed;
            synchronizationWindow.ShowDialog();
        }

        private void AccountSettingsButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isGuest)
            {
                MessageBox.Show("Для доступа к данной функции необходимо зарегистрироваться!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AccountSettingsWindow accountSettingsWindow = new AccountSettingsWindow();
            accountSettingsWindow.Closed += Window_Closed;
            accountSettingsWindow.ShowDialog();
        }

        private void SettingsButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.WindowType = "Settings";
            SettingsAndPatternWindow settingsAndPatternWindow = new SettingsAndPatternWindow();
            this.Close();
            settingsAndPatternWindow.ShowDialog();
        }

        private void MenuItemhowOrHideDocuments_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isDocumentNeedToShow = !SystemContext.isDocumentNeedToShow;
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
        }

        private void MenuItemhowOrHideCreditCards_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isCreditCardNeedToShow = !SystemContext.isCreditCardNeedToShow;
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
        }

        private void MenuItemhowOrHideCollections_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isCollectionNeedToShow = !SystemContext.isCollectionNeedToShow;
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
        }

        private void MenuItemhowOrHideFolders_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isFolderNeedToShow = !SystemContext.isFolderNeedToShow;
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
        }

        private void MenuItemhowAll_Click(object sender, RoutedEventArgs e)
        {
            SystemContextService.MakeAllElementsShowable();
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
        }
    }
}
