using System.Linq;
using System.Windows;
using System.Windows.Input;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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
            EmailTextBox.Text = "User1234.us@gmail.com";
            PasswordTextBox.Password = "qqqqwwww";
        }

        private string LoginMethod(string email, string password)
        {
            string login = null;
            if (email.Length == 0 || password.Length == 0)
                return "Не все поля заполнены!";
            using (var db = new test123Entities1())
            {
                Users user = (from u in db.Users where u.Email == email select u).FirstOrDefault();
                if (user == null)
                    return "Пользователя с такой почтой не существует!";
                if (user.UPassword != password)
                    return "Неверный пароль!";
                SystemContext.User = user;
                login = user.ULogin;
            }
            return $"Добро пожаловать, {login}!";
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string result = LoginMethod(EmailTextBox.Text, PasswordTextBox.Password);
            if (result == $"Добро пожаловать, {SystemContext.User.ULogin}!")
            {
                MessageBox.Show(result, "Приветствие", MessageBoxButton.OK, MessageBoxImage.Information);
                SystemContext.isGuest = false;
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
            using (var db = new test123Entities1())
            {
                Users user = (from u in db.Users where u.Email == "Guest" select u).FirstOrDefault();
                SystemContext.User = user;
                DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
                this.Close();
                documentViewingWindow.ShowDialog();
            }
        }
    }
}
