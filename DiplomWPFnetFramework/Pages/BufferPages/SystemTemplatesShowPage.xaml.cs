using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
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
            SystemContext.isChange = false;
            PassportWindow passportWindow = new PassportWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            passportWindow.ShowDialog();

        }

        private void SNILSButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            SnilsWindow snilsWindow = new SnilsWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            snilsWindow.ShowDialog();
        }

        private void INNButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            InnWindow innWindow = new InnWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            innWindow.ShowDialog();
        }

        private void PolisButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            PolisWindow polisWindow = new PolisWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            polisWindow.ShowDialog();
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                Item item = new Item();
                item.Id = Guid.NewGuid();
                item.Title = "Новая папка " + (db.Item.Where(i => i.Type == "Folder" && i.UserId == SystemContext.User.Id).Count() + 1);
                item.Type = "Folder";
                item.Priority = 0;
                item.IsHidden = 0;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.FolderId = Guid.Empty;
                item.UserId = SystemContext.User.Id;
                db.Item.AddOrUpdate(item);
                db.SaveChanges();
                SystemContext.NewItem = item;
            }
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void CreditCardButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            CreditCardWindow creditCardWindow = new CreditCardWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            creditCardWindow.ShowDialog();
        }

        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            PhotoWindow photoWindow = new PhotoWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            photoWindow.ShowDialog();
        }
    }
}
