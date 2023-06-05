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
using DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows;
using System.IO;
using DiplomWPFnetFramework.Pages;
using System.Data.Entity.Migrations;
using DiplomWPFnetFramework.Windows.BufferWindows;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;

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
            LoginOutTextBlock.Text = SystemContext.User.ULogin;
            DocumentViewingPage documentViewingPage = new DocumentViewingPage();
            openPageFrame.Content = documentViewingPage;
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
                avatarsPhotoBytes = File.ReadAllBytes(filePath);
                imageName.Source = ByteArrayToImage(avatarsPhotoBytes);
                AvatarPhotoImage.Width = 150;
                AvatarPhotoImage.Height = 150;
                AvatarPhotoImage.Stretch = Stretch.Fill;
                Users user = new Users();
                user = SystemContext.User;
                using (var db = new test123Entities1())
                {
                    user.Photo = avatarsPhotoBytes;
                    db.Users.AddOrUpdate(user);
                    db.SaveChanges();
                }
            }
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

        private void sortImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("SortContextMenu");
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void ChangeAccountTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            this.Close();
            loginWindow.ShowDialog();
        }

        private void AvatarPhotoBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter(AvatarPhotoImage);
        }

        private void HiddenFilesButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.FromWhichWindowIsCalled = "DocumentViewingWindow";
            EnteringAccessCodeWindow enteringAccessCodeWindow = new EnteringAccessCodeWindow();
            enteringAccessCodeWindow.Owner = this;
            enteringAccessCodeWindow.ShowDialog();
        }

        private void MyTemplatesButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.WindowType = "MyTemplates";
            SettingsAndPatternWindow settingsAndPatternWindow = new SettingsAndPatternWindow();
            this.Close();
            settingsAndPatternWindow.ShowDialog();
        }

        private void SynchronizationButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SynchronizationWindow synchronizationWindow = new SynchronizationWindow();
            synchronizationWindow.ShowDialog();
        }

        private void AccountSettingsButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AccountSettingsWindow accountSettingsWindow = new AccountSettingsWindow();
            accountSettingsWindow.ShowDialog();
        }

        private void SettingsButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.WindowType = "Settings";
            SettingsAndPatternWindow settingsAndPatternWindow = new SettingsAndPatternWindow();
            this.Close();
            settingsAndPatternWindow.ShowDialog();
        }

        private void MenuItemShowOrHideDocuments_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isDocumentNeedToShow = !SystemContext.isDocumentNeedToShow;
        }

        private void MenuItemShowOrHideCreditCards_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isCreditCardNeedToShow = !SystemContext.isCreditCardNeedToShow;
        }

        private void MenuItemShowOrHideCollections_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isCollectionNeedToShow = !SystemContext.isCollectionNeedToShow;
        }

        private void MenuItemShowOrHideFolders_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isFolderNeedToShow = !SystemContext.isFolderNeedToShow;
        }

        private void MenuItemShowAll_Click(object sender, RoutedEventArgs e)
        {
            SystemContextService.MakeAllElementsShowable();
            var currentPage = openPageFrame.Content as Page;
        }
    }
}
