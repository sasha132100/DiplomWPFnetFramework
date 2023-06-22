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
            List<Template> templates = new List<Template>();
            using (var db = new LocalMyDocsAppDBEntities())
            {
                templates = (from t in db.Template
                             where t.UserId == SystemContext.User.Id && (t.Status == "New" || t.Status == "Published")
                             orderby t.Date
                             select t).ToList<Template>();
            }
            foreach (var template in templates)
            {
                AddNewButton(template);
            }
        }

        private void AddNewButton(Template template)
        {
            Button templateButton = new Button() { Style = (Style)this.Resources["ButtonProperties"], Resources = (ResourceDictionary)this.Resources["CornerRadiusSetter"], Margin = new Thickness(15,5,15,0) };
            using (var db = new LocalMyDocsAppDBEntities())
            {
                templateButton.Content = template.Name;
                templateButton.Tag = template;
                templateButton.Click += TemplateButton_Click;
                mainStackPanel.Children.Add(templateButton);
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
