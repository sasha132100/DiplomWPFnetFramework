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

namespace DiplomWPFnetFramework.Pages.DocumentPages
{
    /// <summary>
    /// Логика взаимодействия для CreditCardPhotoPage.xaml
    /// </summary>
    public partial class CreditCardPhotoPage : Page
    {
        public CreditCardPhotoPage()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            documentViewingWindow.ShowDialog();
        }

        private void CreditCardDataOpen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
