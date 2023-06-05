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
using System.Windows.Shapes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Classes;

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows
{
    /// <summary>
    /// Логика взаимодействия для SettingsAndPatternWindow.xaml
    /// </summary>
    public partial class SettingsAndPatternWindow : Window
    {
        public SettingsAndPatternWindow()
        {
            InitializeComponent();
            if (SystemContext.WindowType == "Settings")
                WindowTypeTextBlock.Text = "Настройки";
            else
                WindowTypeTextBlock.Text = "Шаблоны";
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            this.Close();
            documentViewingWindow.ShowDialog();
        }
    }
}
