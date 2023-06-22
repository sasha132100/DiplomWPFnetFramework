using System.Linq;
using System.Windows;
using System.Windows.Input;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using DiplomWPFnetFramework.Windows.BufferWindows;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            IsAlreadyAuthorization();
            /*EmailTextBox.Text = "Test@gmail.com";
            PasswordTextBox.Password = "qqqqwwww";*/
        }

        private void IsAlreadyAuthorization()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                LastLogginedUser lastLogginedUser = (from llu in db.LastLogginedUser select llu).FirstOrDefault();
                if (lastLogginedUser == null)
                {
                    db.LastLogginedUser.Add(new LastLogginedUser());
                    db.SaveChanges();
                    return;
                }
                if (lastLogginedUser.Email != null && lastLogginedUser.Login != null)
                    SystemContext.User = (from u in db.User where u.Email == lastLogginedUser.Email && u.Login == lastLogginedUser.Login select u).FirstOrDefault();
                if (SystemContext.User == null || SystemContext.User.AccessCode == null)
                    return;
                if (!SystemContext.isSystemStart)
                    return;
                if (SystemContext.isGuest)
                    return;
                SystemContext.FromWhichWindowIsCalled = "LoginWindow";
                EnteringAccessCodeWindow enteringAccessCodeWindow = new EnteringAccessCodeWindow();
                SystemContext.loginWindow = this;
                enteringAccessCodeWindow.ShowDialog();
            }
        }

        private async Task<string> LoginMethod(string email, string password)
        {
            string login = null;
            if (email.Length == 0 || password.Length == 0)
                return "Не все поля заполнены!";
            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (!Regex.IsMatch(EmailTextBox.Text, @"^[\w\.-]+@[\w\.-]+\.\w+$"))
                    return "Неверный формат почты!";
                ServerConnectGetMethodsClass serverConnectMethodsClass = new ServerConnectGetMethodsClass();
                User user = await serverConnectMethodsClass.GetUserByEmailAndPassword(email, password);

                if (user == null)
                {
                    MessageBox.Show("Ошибка при попытке входа, проверьте введенные данные и повторите попытку!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                string dateString = "1900-01-01 00:00:00";
                string format = "yyyy-MM-dd HH:mm:ss";
                DateTime result;
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    user.UpdateTime = result;
                }
                if (user == null)
                    return "Пользователя с такой почтой не существует!";
                if (user.Password != password)
                    return "Неверный пароль!";
                db.User.AddOrUpdate(user);
                SystemContext.User = user;
                login = user.Login;
                LastLogginedUser lastLogginedUser = (from llu in db.LastLogginedUser select llu).FirstOrDefault();
                lastLogginedUser.Login = login;
                lastLogginedUser.Email = email;
                db.LastLogginedUser.AddOrUpdate(lastLogginedUser);
                db.SaveChanges();
            }
            return $"Добро пожаловать, {login}!";
        }

        private async void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string result = await LoginMethod(EmailTextBox.Text, PasswordTextBox.Password);
            if (result == $"Добро пожаловать, {SystemContext.User.Login}!")
            {
                MessageBox.Show(result, "Приветствие", MessageBoxButton.OK, MessageBoxImage.Information);
                SystemContext.isGuest = false;
                SystemContext.isFromFolder = false;
                DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                this.Close();
                documentViewingWindow.ShowDialog();
            }
            else
            {
                if (result == null)
                    return;
                MessageBox.Show(result, "Результат", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            this.Close();
            registerWindow.ShowDialog();
        }

        private void GuestLogInTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.isGuest = true;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                User user = new User();
                user = (from u in db.User where u.Email == "Guest" select u).FirstOrDefault(); 
                if (user == null)
                {
                    User user1 = new User() { Id = 0, Email = "Guest", Login = "Guest", Password = "Guest"};
                    db.User.Add(user1);
                    db.SaveChanges();
                    SystemContext.User = user1;
                    DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                    this.Close();
                    documentViewingWindow.ShowDialog();
                }
                else
                {
                    SystemContext.User = user;
                    DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                    this.Close();
                    documentViewingWindow.ShowDialog();
                }
            }
        }
    }
}
