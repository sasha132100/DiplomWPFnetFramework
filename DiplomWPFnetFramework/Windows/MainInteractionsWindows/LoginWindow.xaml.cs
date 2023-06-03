using System.Linq;
using System.Windows;
using System.Windows.Input;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;

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
            EmailTextBox.Text = "User1";
            PasswordTextBox.Password = "123321";
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
                login = user.Email;
            }
            return $"Добро пожаловать, {login}!";
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string result = LoginMethod(EmailTextBox.Text, PasswordTextBox.Password);
            if (result == $"Добро пожаловать, {EmailTextBox.Text}!")
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
            /*DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            this.Close();
            documentViewingWindow.ShowDialog();*/
            MessageBox.Show("В разработке");
        }
    }
}
