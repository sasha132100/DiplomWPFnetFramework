using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using Newtonsoft.Json;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        string registerMethod(string email, string login, string password, string confirmPas)
        {
            if (email.Length < 5 || email.Length == 0)
                return "Почта должна содержать от 5 символов!";
            if (login.Length <= 1 || login.Length == 0)
                return "Логин должен содержать от 2 символов!";
            if (password.Length < 8 || password.Length == 0)
                return "Минимальный размер пароля: 8 символов!";
            if (password != confirmPas)
                return "Пароли не соответсвуют друг другу!";
            if (!Regex.IsMatch(EmailTextBox.Text, @"^[\w\.-]+@[\w\.-]+\.\w+$"))
                return "Неверный формат почты!";

            var user = new User() { Email = email, Login = login, Password = password };
            ServerConnectPostMethodsClass serverConnectPostMethodsClass = new ServerConnectPostMethodsClass();
            if (serverConnectPostMethodsClass.CreateNewUser(user) == null)
                return "Пользователь с такими данными уже существует";
            return "Регистрация прошла успешно!";
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string result = registerMethod(EmailTextBox.Text, LoginTextBox.Text, PasswordTextBox.Password, ConfirmPasswordTextBox.Password);
            MessageBox.Show(result, "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == "Регистрация прошла успешно!")
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
                BackToMainWindowButton_Click(this, new RoutedEventArgs());
            }
        }

        private void BackToMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow mainWindow = new LoginWindow();
            this.Close();
            mainWindow.ShowDialog();
        }
    }
}
