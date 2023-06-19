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
using System.Windows.Shapes;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows
{
    /// <summary>
    /// Логика взаимодействия для AccountSettingsWindow.xaml
    /// </summary>
    public partial class AccountSettingsWindow : Window
    {
        public AccountSettingsWindow()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LoginTextBox.Text == "" && EmailTextBox.Text == "" && NewPasswordTextBox.Password == "" && NewPasswordCheckTextBox.Password == "")
            {
                MessageBox.Show("Заполните хотя бы 1 поле с новыми данными!");
                return;
            }
            if (OldPasswordTextBox.Password == "")
            {
                MessageBox.Show("Введите старый пароль для изменения данных");
                return;
            }
            if (OldPasswordTextBox.Password != SystemContext.User.Password)
            {
                MessageBox.Show("Неверный старый пароль");
                return;
            }

            User changeUser = SystemContext.User;
            User checkUser;

            using (var db = new LocalMyDocsAppDBEntities())
            {
                if (LoginTextBox.Text != "")
                {
                    checkUser = (from u in db.User where LoginTextBox.Text == u.Login select u).FirstOrDefault();
                    if (checkUser != null)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует");
                        return;
                    }
                    changeUser.Login = LoginTextBox.Text;
                }

                if (EmailTextBox.Text != "")
                {
                    checkUser = (from u in db.User where EmailTextBox.Text == u.Email select u).FirstOrDefault();
                    if (checkUser != null)
                    {
                        MessageBox.Show("Пользователь с такой почтой уже существует");
                        return;
                    }
                    changeUser.Email = EmailTextBox.Text;
                }

                if (NewPasswordTextBox.Password != "" && NewPasswordCheckTextBox.Password == NewPasswordTextBox.Password)
                {
                    changeUser.Password = NewPasswordTextBox.Password;
                }

                db.User.AddOrUpdate(changeUser);
                db.SaveChanges();
            }
        }
    }
}
