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
            Closing += ClosingWindow;
            TitleNameTextBlock.Text = SystemContext.Item.Title;
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.isChangeTitleName = false;
            this.Close();
        }

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SystemContext.isChangeTitleName = false;
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NewTitleNameTextBox.Text != null && NewTitleNameTextBox.Text != "")
            {
                Items item = new Items();
                using (var db = new test123Entities1())
                {
                    item = SystemContext.Item;
                    item.Title = NewTitleNameTextBox.Text;
                    db.Items.AddOrUpdate(item);
                    db.SaveChanges();
                }
                MessageBox.Show("Название успешно изменено");
                this.Close();
            }
        }
    }
}
