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
            Run run1 = new Run("Приложение MyDocs разработано с целью создания мобильного инструмента для хранения и организации личных данных. В рамках этого приложения под личными данными понимаются персональные документы (паспорт, СНИЛС и пр.), банковские карты и другая ценная информация, которая важна для пользователей.\n");
            Run run2 = new Run("Наше приложение помогает пользователю структурировать и хранить его персональные данные, обеспечивая быстрый доступ к ним и гарантируя полную безопасность. Мы предоставляем возможность создавать сканы документов, а также обеспечиваем защиту данных, чтобы ваши личные сведения оставались в безопасности.\n");
            Run run3 = new Run("MyDocs позволяет хранить данные непосредственно на вашем устройстве, обеспечивая доступ к ним в любое удобное время. Помимо этого, мы предлагаем возможность синхронизации ваших данных с сервером, чтобы вы могли иметь доступ к своим личным данным в любом месте и на разных устройствах.\n");
            Run run4 = new Run("Мы придерживаемся высоких стандартов безопасности и конфиденциальности данных, и мы уделяем особое внимание защите ваших личных данных. Мы стремимся предоставить вам надежный инструмент для хранения и организации вашей ценной информации.\n");
            Run run5 = new Run("Если у вас возникнут вопросы или предложения, не стесняйтесь обращаться к нам.\n");
            Run run6 = new Run("Почта для связи с разработчиком: ");
            Run run = new Run("MyDocsApp.PublicRelations@gmail.com");
            run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D346F"));
            mainTextBlock.Inlines.Add(run1);
            mainTextBlock.Inlines.Add(run2);
            mainTextBlock.Inlines.Add(run3);
            mainTextBlock.Inlines.Add(run4);
            mainTextBlock.Inlines.Add(run5);
            mainTextBlock.Inlines.Add(run6);
            mainTextBlock.Inlines.Add(run);
        }
    }
}
