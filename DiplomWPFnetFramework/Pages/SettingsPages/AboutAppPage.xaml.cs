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
    /// Логика взаимодействия для AboutAppPage.xaml
    /// </summary>
    public partial class AboutAppPage : Page
    {
        public AboutAppPage()
        {
            InitializeComponent();
            Run run = new Run("MyDocsApp.PublicRelations@gmail.com");
            run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D346F"));
            mainTextBlock.Inlines.Add(run);
        }
    }
}
