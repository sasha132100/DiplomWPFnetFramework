using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.SettingsPages;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows;
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

namespace DiplomWPFnetFramework.Windows.BufferWindows
{
    /// <summary>
    /// Логика взаимодействия для EnteringAccessCodeWindow.xaml
    /// </summary>
    public partial class EnteringAccessCodeWindow : Window
    {
        public EnteringAccessCodeWindow()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.FromWhichWindowIsCalled == "DocumentViewingWindow")
            {
                if (CodeCheckTextBox.Password == null || CodeCheckTextBox.Password == "")
                {
                    MessageBox.Show("Введите код доступа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CodeCheckTextBox.Password == SystemContext.User.AccessCode)
                {
                    SystemContext.isFromHiddenFiles = true;
                    HiddenFilesViewerWindow hiddenFilesViewerWindow = new HiddenFilesViewerWindow();
                    this.Owner.Close();
                    this.Close();
                    hiddenFilesViewerWindow.ShowDialog();
                }
                else
                    MessageBox.Show("Код доступа введен неправильно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (SystemContext.FromWhichWindowIsCalled == "LoginWindow")
            {
                if (CodeCheckTextBox.Password == null || CodeCheckTextBox.Password == "")
                {
                    MessageBox.Show("Введите код доступа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (CodeCheckTextBox.Password == SystemContext.User.AccessCode)
                {
                    SystemContext.isFromHiddenFiles = false;
                    DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                    SystemContext.loginWindow.Close();
                    this.Close();
                    documentViewingWindow.ShowDialog();
                }
                else
                    MessageBox.Show("Код введен неправильно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (CodeCheckTextBox.Password == null || CodeCheckTextBox.Password == "")
                {
                    MessageBox.Show("Введите код доступа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CodeCheckTextBox.Password == SystemContext.User.AccessCode)
                {
                    SystemContext.isFromHiddenFiles = false;
                    if (this.Owner is SettingsAndPatternWindow)
                    {
                        SettingsAndPatternWindow settingsAndPatternWindow = this.Owner as SettingsAndPatternWindow;
                        SecurityPage securityPage = new SecurityPage();
                        settingsAndPatternWindow.RightFrame.Content = securityPage;
                    }
                    this.Close();
                }
                else
                    MessageBox.Show("Код введен неправильно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CodeCheckTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if (SystemContext.FromWhichWindowIsCalled == "DocumentViewingWindow")
                {
                    if (CodeCheckTextBox.Password == null || CodeCheckTextBox.Password == "")
                    {
                        MessageBox.Show("Введите код доступа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (CodeCheckTextBox.Password == SystemContext.User.AccessCode)
                    {
                        SystemContext.isFromHiddenFiles = true;
                        HiddenFilesViewerWindow hiddenFilesViewerWindow = new HiddenFilesViewerWindow();
                        this.Owner.Close();
                        this.Close();
                        hiddenFilesViewerWindow.ShowDialog();
                    }
                    else
                        MessageBox.Show("Код доступа введен неправильно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (SystemContext.FromWhichWindowIsCalled == "LoginWindow")
                {
                    if (CodeCheckTextBox.Password == null || CodeCheckTextBox.Password == "")
                    {
                        MessageBox.Show("Введите код доступа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (CodeCheckTextBox.Password == SystemContext.User.AccessCode)
                    {
                        SystemContext.isFromHiddenFiles = false;
                        DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                        SystemContext.loginWindow.Close();
                        this.Close();
                        documentViewingWindow.ShowDialog();
                    }
                    else
                        MessageBox.Show("Код введен неправильно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (CodeCheckTextBox.Password == null || CodeCheckTextBox.Password == "")
                    {
                        MessageBox.Show("Введите код доступа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (CodeCheckTextBox.Password == SystemContext.User.AccessCode)
                    {
                        SystemContext.isFromHiddenFiles = false;
                        if (this.Owner is SettingsAndPatternWindow)
                        {
                            SettingsAndPatternWindow settingsAndPatternWindow = this.Owner as SettingsAndPatternWindow;
                            SecurityPage securityPage = new SecurityPage();
                            settingsAndPatternWindow.RightFrame.Content = securityPage;
                        }
                        this.Close();
                    }
                    else
                        MessageBox.Show("Код введен неправильно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void CodeCheckTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }
    }
}
