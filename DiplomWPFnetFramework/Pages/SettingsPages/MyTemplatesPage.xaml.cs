using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.SettingsWindows;
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

namespace DiplomWPFnetFramework.Pages.SettingsPages
{
    /// <summary>
    /// Логика взаимодействия для MyTemplatesPage.xaml
    /// </summary>
    public partial class MyTemplatesPage : Page
    {
        public MyTemplatesPage()
        {
            InitializeComponent();
            LoadContent();
        }

        private void LoadContent()
        {
            List<Template> templates;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                templates = (from t in db.Template where t.UserId == SystemContext.User.Id select t).ToList<Template>();
                foreach (var template in templates)
                {
                    AddNewTemplate(template);
                }
            }
        }

        private void AddNewTemplate(Template template)
        {
            TextBlock textBlock = new TextBlock() { Style = (Style)this.Resources["Templates"], Cursor = Cursors.Hand };
            textBlock.Text = template.Name;
            mainStackPanel.Children.Add(textBlock);
        }

        private void CreateNewTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            UserTamplateConstructorWindow userTamplateConstructorWindow = new UserTamplateConstructorWindow();
            userTamplateConstructorWindow.ShowDialog();
        }
    }
}
