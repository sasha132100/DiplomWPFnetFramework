using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
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

namespace DiplomWPFnetFramework.Pages.BufferPages
{
    /// <summary>
    /// Логика взаимодействия для SystemTemplatesShowPage.xaml
    /// </summary>
    public partial class SystemTemplatesShowPage : Page
    {
        Window parentWindow;

        public SystemTemplatesShowPage()
        {
            InitializeComponent();
        }

        private void PassportButton_Click(object sender, RoutedEventArgs e)
        {
            PassportWindow passportWindow = new PassportWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            passportWindow.ShowDialog();

        }

        private void SNILSButton_Click(object sender, RoutedEventArgs e)
        {
            SnilsWindow snilsWindow = new SnilsWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            snilsWindow.ShowDialog();
        }

        private void INNButton_Click(object sender, RoutedEventArgs e)
        {
            InnWindow innWindow = new InnWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            innWindow.ShowDialog();
        }

        private void PolisButton_Click(object sender, RoutedEventArgs e)
        {
            PolisWindow polisWindow = new PolisWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            polisWindow.ShowDialog();
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new test123Entities1())
            {
                Items item = new Items();
                item.Title = "NewFolder" + db.Items.OrderByDescending(items => items.Id).FirstOrDefault().Id.ToString();
                item.IType = "Folder";
                item.IPriority = 0;
                item.IsHidden = 0;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.UserId = SystemContext.User.Id;
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
                SystemContext.NewItem = item;
            }
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void CreditCardButton_Click(object sender, RoutedEventArgs e)
        {
            CreditCardWindow creditCardWindow = new CreditCardWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            creditCardWindow.ShowDialog();
        }
    }
}
