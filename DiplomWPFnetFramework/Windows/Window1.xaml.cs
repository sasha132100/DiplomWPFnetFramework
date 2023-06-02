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
using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Windows.DocumentTemplatesWindows;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;

namespace DiplomWPFnetFramework.Windows
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            this.Close();
            documentViewingWindow.ShowDialog();
        }

        private void CreditCardWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            CreditCardWindow creditCardWindow = new CreditCardWindow();
            this.Close();
            creditCardWindow.ShowDialog();
        }

        private void INNWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            InnWindow innWindow = new InnWindow();
            this.Close();
            innWindow.ShowDialog();
        }

        private void PassportWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            PassportWindow passportWindow = new PassportWindow();
            this.Close();
            passportWindow.ShowDialog();
        }

        private void PhotoWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            PhotoWindow photoWindow = new PhotoWindow();
            this.Close();
            photoWindow.ShowDialog();
        }

        private void PolisWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            PolisWindow polisWindow = new PolisWindow();
            this.Close();
            polisWindow.ShowDialog();
        }

        private void SnilsWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            SystemContext.isChange = false;
            SnilsWindow snilsWindow = new SnilsWindow();
            this.Close();
            snilsWindow.ShowDialog();
        }
    }
}
