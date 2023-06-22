using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace DiplomWPFnetFramework.Pages.SettingsPages
{
    /// <summary>
    /// Логика взаимодействия для SecurityPage.xaml
    /// </summary>
    public partial class SecurityPage : Page
    {
        public SecurityPage()
        {
            InitializeComponent();
        }

        private void RemoveAccessCodeImageClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                User user = SystemContext.User;
                user.AccessCode = null;
                ServerConnectPostMethodsClass serverConnectPostMethodsClass = new ServerConnectPostMethodsClass();
                if (serverConnectPostMethodsClass.UpdateUser(user) == null)
                {
                    MessageBox.Show("Пользователь с такими данными уже существует!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                db.User.AddOrUpdate(user);
                db.SaveChanges();
                SystemContext.User = user;
            }
            MessageBox.Show("Код доступа успешно удален.", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void confirmAccessCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccessCodeTextBox.Text == "")
            {
                MessageBox.Show("Введите код доступа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User user = SystemContext.User;
            user.AccessCode = AccessCodeTextBox.Text;

            ServerConnectPostMethodsClass serverConnectPostMethodsClass = new ServerConnectPostMethodsClass();
            if (serverConnectPostMethodsClass.UpdateUser(user) == null)
            {
                MessageBox.Show("Пользователь с такими данными уже существует!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.User.AddOrUpdate(user);
                db.SaveChanges();
                SystemContext.User = user;
            }
            MessageBox.Show("Код доступа успешно установлен.", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AccessCodeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
