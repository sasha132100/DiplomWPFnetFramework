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
                db.User.AddOrUpdate(user);
                db.SaveChanges();
                SystemContext.User = user;
            }
        }

        private void confirmAccessCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccessCodeTextBox.Text == "")
            {
                MessageBox.Show("Введите код доступа");
                return;
            }

            User user = SystemContext.User;
            user.AccessCode = AccessCodeTextBox.Text;

            using (var db = new LocalMyDocsAppDBEntities())
            {
                db.User.AddOrUpdate(user);
                db.SaveChanges();
                SystemContext.User = user;
            }
        }
    }
}
