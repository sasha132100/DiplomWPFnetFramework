using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MyTemplatesShowPage.xaml
    /// </summary>
    public partial class MyTemplatesShowPage : Page
    {
        Window parentWindow;

        public MyTemplatesShowPage()
        {
            InitializeComponent();
            LoadContent();
        }

        private void LoadContent()
        {
            List<Item> items = new List<Item>();
            using (var db = new LocalMyDocsAppDBEntities())
            {
                items = (from i in db.Item
                         where i.UserId == SystemContext.User.Id && i.Type != "Template"
                         orderby i.Priority descending, i.DateCreation
                         select i).ToList<Item>();
            }
            foreach (var item in items)
            {
                AddNewButton(item);
            }
        }

        private void AddNewButton(Item item)
        {
            Button templateButton = new Button() { Style = (Style)this.Resources["ButtonProperties"], Resources = (ResourceDictionary)this.Resources["CornerRadiusSetter"], Margin = new Thickness(15,5,15,0) };
            using (var db = new LocalMyDocsAppDBEntities())
            {
                TemplateDocument templateDocument = (from td in db.TemplateDocument where td.Id == item.Id select td).FirstOrDefault();
                Template currentTemplate = (from t in db.Template where t.Id == templateDocument.TemplateId select t).FirstOrDefault();
                templateButton.Content = currentTemplate.Name;
                templateButton.Tag = currentTemplate;
                templateButton.Click += TemplateButton_Click;
            }
        }

        private void TemplateButton_Click(object sender, RoutedEventArgs e)
        {
            Template template = (sender as Button).Tag as Template;
            SystemContext.Template = template;
            SystemContext.isChange = false;
            UserTemplateWindow userTemplateWindow = new UserTemplateWindow();
            parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            userTemplateWindow.ShowDialog();
        }
    }
}
