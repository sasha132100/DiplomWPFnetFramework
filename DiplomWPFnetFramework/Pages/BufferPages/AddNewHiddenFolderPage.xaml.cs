using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
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
    /// Логика взаимодействия для AddNewHiddenFolderPage.xaml
    /// </summary>
    public partial class AddNewHiddenFolderPage : Page
    {
        Window parentWindow;

        public AddNewHiddenFolderPage()
        {
            InitializeComponent();
        }

        private void HiddenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new test123Entities1())
            {
                Items item = new Items();
                item.Title = "Новая папка" + db.Items.Where(i => i.IType == "Folder" && i.UserId == SystemContext.User.Id).Count();
                item.IType = "Folder";
                item.IPriority = 0;
                item.IsHidden = 1;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.UserId = SystemContext.User.Id;
                db.Items.AddOrUpdate(item);
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
    }
}
