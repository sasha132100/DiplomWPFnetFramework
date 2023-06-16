﻿using DiplomWPFnetFramework.Classes;
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
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NewTitleNameTextBox.Text != null && NewTitleNameTextBox.Text != "")
            {
                SystemContext.TemplateObjectTitle = NewTitleNameTextBox.Text;
                MessageBox.Show("Название успешно изменено");
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите данные!");
            }
        }

        private void NewTitleNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (NewTitleNameTextBox.Text != null && NewTitleNameTextBox.Text != "")
                {
                    SystemContext.TemplateObjectTitle = NewTitleNameTextBox.Text;
                    MessageBox.Show("Название успешно изменено");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Введите новое название!");
                }
            }
        }
    }
}
