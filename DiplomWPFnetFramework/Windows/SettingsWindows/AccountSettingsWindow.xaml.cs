using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows
{
    /// <summary>
    /// Логика взаимодействия для AccountSettingsWindow.xaml
    /// </summary>
    public partial class AccountSettingsWindow : System.Windows.Window
    {
        public AccountSettingsWindow()
        {
            InitializeComponent();
            EmailTextBox.Text = SystemContext.User.Email;
            LoginTextBox.Text = SystemContext.User.Login;
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LoginTextBox.Text == "" && EmailTextBox.Text == "" && NewPasswordTextBox.Password == "" && NewPasswordCheckTextBox.Password == "")
            {
                MessageBox.Show("Заполните хотя бы 1 поле с новыми данными!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (OldPasswordTextBox.Password == "")
            {
                MessageBox.Show("Введите старый пароль для изменения данных", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (OldPasswordTextBox.Password != SystemContext.User.Password)
            {
                MessageBox.Show("Неверный старый пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Regex.IsMatch(EmailTextBox.Text, @"^[\w\.-]+@[\w\.-]+\.\w+$"))
            {
                MessageBox.Show("Неверный формат почты!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (NewPasswordTextBox.Password.Length < 8 && NewPasswordTextBox.Password != "")
            {
                MessageBox.Show("Пароль должен быть от 8 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User user = SystemContext.User;

            if (EmailTextBox.Text != "")
                user.Email = EmailTextBox.Text;
            if (LoginTextBox.Text != "" && LoginTextBox.Text.Length > 1)
                user.Login = LoginTextBox.Text;
            if (NewPasswordTextBox.Password != "" && NewPasswordCheckTextBox.Password == NewPasswordTextBox.Password)
                user.Password = NewPasswordTextBox.Password;
            else if (NewPasswordTextBox.Password != "" && NewPasswordCheckTextBox.Password != NewPasswordTextBox.Password)
                MessageBox.Show("Пароли не совпадают!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

            ServerConnectPostMethodsClass serverConnectPostMethodsClass = new ServerConnectPostMethodsClass();
            if (serverConnectPostMethodsClass.UpdateUser(user) == null)
            {
                MessageBox.Show("Пользователь с такими данными уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    db.User.AddOrUpdate(user);
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно изменены", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Данные не должны превышать 250 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            SystemContext.User = user;
            this.Close();
        }
    }
}
