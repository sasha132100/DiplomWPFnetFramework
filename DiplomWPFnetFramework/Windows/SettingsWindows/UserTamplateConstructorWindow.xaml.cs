using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.BufferWindows;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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

namespace DiplomWPFnetFramework.Windows.SettingsWindows
{
    /// <summary>
    /// Логика взаимодействия для UserTamplateConstructorWindow.xaml
    /// </summary>
    public partial class UserTamplateConstructorWindow : Window
    {
        List<TemplateObject> allTemplateObjects = new List<TemplateObject>();
        Template template = new Template();
        int templateObjectPosition = 0;

        public UserTamplateConstructorWindow()
        {
            InitializeComponent();
            AddNewTemplate();
        }

        private void DeleteObjectButton_Click(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = (sender as Image).Tag as StackPanel;
            TemplateObject templateObject = stackPanel.Tag as TemplateObject;
            mainStackPanel.Children.Remove(stackPanel);
            allTemplateObjects.Remove(templateObject);
        }

        private void AddNewImage()
        {
            templateObjectPosition++;
            TemplateObject templateObject = new TemplateObject();

            Grid grid = new Grid();
            TextBlock textBlock = new TextBlock() { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCF3DC")) };
            Border border = new Border() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D346F")), Style = (Style)this.Resources["BoderProperties"] };
            StackPanel stackPanel = new StackPanel();
            Image image = new Image() { Width = 50, Height = 50, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 10, 0), Cursor = Cursors.Hand, VerticalAlignment = VerticalAlignment.Center };

            image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/RemoveRedImage.png"));
            textBlock.Text = SystemContext.TemplateObjectTitle;

            image.MouseLeftButtonUp += DeleteObjectButton_Click;

            grid.Children.Add(border);
            grid.Children.Add(image);
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(grid);
            mainStackPanel.Children.Add(stackPanel);

            templateObject.Id = Guid.NewGuid();
            templateObject.Position = templateObjectPosition;
            templateObject.Type = "Image";
            templateObject.Title = SystemContext.TemplateObjectTitle;
            templateObject.TemplateId = template.Id;
            allTemplateObjects.Add(templateObject);

            image.Tag = stackPanel;
            stackPanel.Tag = templateObject;
        }

        private void AddNewCheckBox()
        {
            templateObjectPosition++;
            TemplateObject templateObject = new TemplateObject();

            Grid grid = new Grid();
            StackPanel stackPanel = new StackPanel();
            CheckBox checkBox = new CheckBox() { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCF3DC")) };
            Image image = new Image() { Width = 50, Height = 50, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 10, 0), Cursor = Cursors.Hand, VerticalAlignment = VerticalAlignment.Center };

            image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/RemoveRedImage.png"));
            checkBox.Content = SystemContext.TemplateObjectTitle;

            image.MouseLeftButtonUp += DeleteObjectButton_Click;

            grid.Children.Add(checkBox);
            grid.Children.Add(image);
            stackPanel.Children.Add(grid);
            mainStackPanel.Children.Add(stackPanel);

            templateObject.Id = Guid.NewGuid();
            templateObject.Position = templateObjectPosition;
            templateObject.Type = "CheckBox";
            templateObject.Title = SystemContext.TemplateObjectTitle;
            templateObject.TemplateId = template.Id;
            allTemplateObjects.Add(templateObject);

            image.Tag = stackPanel;
            stackPanel.Tag = templateObject;
        }

        private void AddNewTextBox()
        {
            templateObjectPosition++;
            TemplateObject templateObject = new TemplateObject();

            Grid grid = new Grid();
            StackPanel stackPanel = new StackPanel();
            TextBlock textBlock = new TextBlock() { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCF3DC")) };
            TextBox textBox = new TextBox() { Resources = (ResourceDictionary)this.Resources["CornerRadiusSetter"] };
            Image image = new Image() { Width = 50, Height = 50, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0,0,10,0), Cursor = Cursors.Hand };

            image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/RemoveRedImage.png"));
            textBlock.Text = SystemContext.TemplateObjectTitle;

            image.MouseLeftButtonUp += DeleteObjectButton_Click;

            grid.Children.Add(textBox);
            grid.Children.Add(image);
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(grid);
            mainStackPanel.Children.Add(stackPanel);

            templateObject.Id = Guid.NewGuid();
            templateObject.Position = templateObjectPosition;
            templateObject.Type = "TextBox";
            templateObject.Title = SystemContext.TemplateObjectTitle;
            templateObject.TemplateId = template.Id;
            allTemplateObjects.Add(templateObject);

            image.Tag = stackPanel;
            stackPanel.Tag = templateObject;
        }

        private void AddNewTemplate()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                template.Id = Guid.NewGuid();
                template.Name = SystemContext.TemplateObjectTitle;
                template.Status = "New";
                template.Date = DateTime.Now;
                template.UserId = SystemContext.User.Id;
            }
        }

        private void TitleNameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SystemContext.TemplateObjectTitle == "")
            {
                return;
            }
            switch (SystemContext.ObjectType) 
            {
                case "TextBox":
                    AddNewTextBox();
                    break;

                case "NumberBox":
                    AddNewTextBox();
                    break;

                case "CheckBox":
                    AddNewCheckBox();
                    break;

                case "Image":
                    AddNewImage();
                    break;

                case "TemplateName":
                    using (var db = new LocalMyDocsAppDBEntities())
                    {
                        template.Name = SystemContext.TemplateObjectTitle;
                        db.Template.AddOrUpdate(template);
                        db.SaveChanges();
                        foreach (var templateObject in allTemplateObjects)
                        {
                            db.TemplateObject.AddOrUpdate(templateObject);
                            db.SaveChanges();
                        }
                    }
                    this.Close();
                    break;

                default:
                    MessageBox.Show("Ошибка при определении типа объекта!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.ObjectType = "TemplateName";
            TemplateObjectTitleNameSetterWindow templateObjectTitleNameSetterWindow = new TemplateObjectTitleNameSetterWindow();
            templateObjectTitleNameSetterWindow.Closing += TitleNameWindow_Closing;
            templateObjectTitleNameSetterWindow.ShowDialog();
        }

        private void TextCreateButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.ObjectType = "TextBox";
            TemplateObjectTitleNameSetterWindow templateObjectTitleNameSetterWindow = new TemplateObjectTitleNameSetterWindow();
            templateObjectTitleNameSetterWindow.Closing += TitleNameWindow_Closing;
            templateObjectTitleNameSetterWindow.ShowDialog();
        }

        private void NumberCreateButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.ObjectType = "NumberBox";
            TemplateObjectTitleNameSetterWindow templateObjectTitleNameSetterWindow = new TemplateObjectTitleNameSetterWindow();
            templateObjectTitleNameSetterWindow.Closing += TitleNameWindow_Closing;
            templateObjectTitleNameSetterWindow.ShowDialog();
        }

        private void CheckBoxCreateButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.ObjectType = "CheckBox";
            TemplateObjectTitleNameSetterWindow templateObjectTitleNameSetterWindow = new TemplateObjectTitleNameSetterWindow();
            templateObjectTitleNameSetterWindow.Closing += TitleNameWindow_Closing;
            templateObjectTitleNameSetterWindow.ShowDialog();
        }

        private void ImageCreateButton_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.ObjectType = "Image";
            TemplateObjectTitleNameSetterWindow templateObjectTitleNameSetterWindow = new TemplateObjectTitleNameSetterWindow();
            templateObjectTitleNameSetterWindow.Closing += TitleNameWindow_Closing;
            templateObjectTitleNameSetterWindow.ShowDialog();
        }
    }
}
