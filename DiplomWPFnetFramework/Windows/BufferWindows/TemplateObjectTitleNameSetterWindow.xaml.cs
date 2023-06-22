using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
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
    /// Логика взаимодействия для TemplateObjectTitleNameSetterWindow.xaml
    /// </summary>
    public partial class TemplateObjectTitleNameSetterWindow : Window
    {
        public TemplateObjectTitleNameSetterWindow()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SystemContext.TemplateObjectTitle = "";
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NewTitleNameTextBox.Text != null && NewTitleNameTextBox.Text != "")
            {
                if (NewTitleNameTextBox.Text.Length > 20)
                {
                    MessageBox.Show("Длина названия не должна превышать 20 сиволов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                SystemContext.TemplateObjectTitle = NewTitleNameTextBox.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите данные!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void NewTitleNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (NewTitleNameTextBox.Text != null && NewTitleNameTextBox.Text != "")
                {
                    if (NewTitleNameTextBox.Text.Length > 20)
                    {
                        MessageBox.Show("Длина нового названия не должна превышать 20 сиволов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    SystemContext.TemplateObjectTitle = NewTitleNameTextBox.Text;
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
