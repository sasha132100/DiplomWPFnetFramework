using System.Linq;
using System.Windows;
using System.Windows.Input;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using DiplomWPFnetFramework.Windows.BufferWindows;
using System.Data.Entity.Migrations;

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
            EmailTextBox.Text = "Test@gmail.com";
            PasswordTextBox.Password = "qqqqwwww";
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

        private string LoginMethod(string email, string password)
        {
            string login = null;
            if (email.Length == 0 || password.Length == 0)
                return "Не все поля заполнены!";
            using (var db = new LocalMyDocsAppDBEntities())
            {
                User user = (from u in db.User where u.Email == email select u).FirstOrDefault();
                if (user == null)
                    return "Пользователя с такой почтой не существует!";
                if (user.Password != password)
                    return "Неверный пароль!";
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

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string result = LoginMethod(EmailTextBox.Text, PasswordTextBox.Password);
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
                User user = (from u in db.User where u.Email == "Guest" select u).FirstOrDefault();
                SystemContext.User = user;
                DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                this.Close();
                documentViewingWindow.ShowDialog();
            }
        }
    }
}
