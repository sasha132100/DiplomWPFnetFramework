using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
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
    /// Логика взаимодействия для MainSettingsPage.xaml
    /// </summary>
    public partial class MainSettingsPage : Page
    {
        Window parentWindow;

        public MainSettingsPage()
        {
            InitializeComponent();
        }

        private void SecurityButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            if (SystemContext.User.PinCode != null)
            {

            }
            else
            {
                Frame openSecurityoPageFrame = parentWindow.FindName("RightFrame") as Frame;
                SecurityPage securityPage = new SecurityPage();
                openSecurityoPageFrame.Content = securityPage;
            }
        }

        private void HelpButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //
        }

        private void InfoButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            Frame openInfoPageFrame = parentWindow.FindName("RightFrame") as Frame;
            AboutAppPage aboutAppPage = new AboutAppPage();
            openInfoPageFrame.Content = aboutAppPage;
        }

        private void PrivatePolicyButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            Frame openPrivatePolicyPageFrame = parentWindow.FindName("RightFrame") as Frame;
            PrivacyPolicyPage privacyPolicyPage = new PrivacyPolicyPage();
            openPrivatePolicyPageFrame.Content = privacyPolicyPage;
        }
    }
}
