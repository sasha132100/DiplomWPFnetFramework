using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.SettingsWindows;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

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

        public void LoadContent()
        {
            List<Template> templates = null;
            mainStackPanel.Children.Clear();
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
            Grid mainGrid = new Grid() { Name = "mainGrid", Margin = new Thickness(10, 15, 0, 0) };
            TextBlock textBlock = new TextBlock() { Style = (Style)this.Resources["Templates"], Cursor = Cursors.Hand };
            Image newImage = new Image() { Name = "templateStatusTypeImage", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(5, 0, 0, 0), Height = 25, Width = 25, Cursor = Cursors.Hand };
            Image publicImage = new Image() { Name = "templateStatusTypeImage", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(4, -1, 0, 0), Height = 29, Width = 29, Cursor = Cursors.Hand };
            ContextMenu contextMenu = (ContextMenu)this.FindResource("MyContextMenu");

            if (template.Status == "New")
            {
                newImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/NewUserTemplate.png"));
                mainGrid.Children.Add(newImage);
            }
            else
            {
                publicImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/PublicUserTemplate.png"));
                mainGrid.Children.Add(publicImage);
            }

            textBlock.Text = template.Name;

            mainGrid.Tag = template;

            mainGrid.ContextMenu = contextMenu;

            mainGrid.Children.Add(textBlock);
            mainStackPanel.Children.Add(mainGrid);
        }

        private void CreateNewTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.PageForLoadContent = this;
            UserTamplateConstructorWindow userTamplateConstructorWindow = new UserTamplateConstructorWindow();
            userTamplateConstructorWindow.ShowDialog();
        }

        private void MenuItemDeleteTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Grid grid = (Grid)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
                Template template = grid.Tag as Template;
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    db.Entry(template).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                LoadContent();
            }
            catch
            {
                MessageBox.Show("Нельзя удалить шаблон, имеющий созданные документы!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MenuItemPublicyTemplate_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)((ContextMenu)(sender as MenuItem).Parent).PlacementTarget;
            Template template = grid.Tag as Template;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                template.Status = "Published";
                db.Template.AddOrUpdate(template);
                db.SaveChanges();
            }
            LoadContent();
        }
    }
}
