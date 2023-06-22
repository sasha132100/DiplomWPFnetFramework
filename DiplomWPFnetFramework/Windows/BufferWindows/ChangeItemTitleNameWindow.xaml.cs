using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
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

namespace DiplomWPFnetFramework.Windows.BufferWindows
{
    /// <summary>
    /// Логика взаимодействия для ChangeItemTitleNameWindow.xaml
    /// </summary>
    public partial class ChangeItemTitleNameWindow : Window
    {
        public ChangeItemTitleNameWindow()
        {
            InitializeComponent();
            TitleNameTextBlock.Text = SystemContext.Item.Title;
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NewTitleNameTextBox.Text != null && NewTitleNameTextBox.Text != "")
            {
                Item item;
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    item = SystemContext.Item;
                    item.Title = NewTitleNameTextBox.Text;
                    db.Item.AddOrUpdate(item);
                    db.SaveChanges();
                }
                MessageBox.Show("Название успешно изменено.", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите данные!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void NewTitleNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (NewTitleNameTextBox.Text != null && NewTitleNameTextBox.Text != "")
                {
                    Item item;
                    using (var db = new LocalMyDocsAppDBEntities())
                    {
                        item = SystemContext.Item;
                        item.Title = NewTitleNameTextBox.Text;
                        db.Item.AddOrUpdate(item);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Название успешно изменено.", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Введите данные!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
