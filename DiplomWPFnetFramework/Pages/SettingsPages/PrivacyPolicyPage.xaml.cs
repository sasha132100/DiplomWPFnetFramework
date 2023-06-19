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
    /// Логика взаимодействия для PrivacyPolicyPage.xaml
    /// </summary>
    public partial class PrivacyPolicyPage : Page
    {
        public PrivacyPolicyPage()
        {
            InitializeComponent();
            Run run1 = new Run("За исключением случаев, указанных иначе, вся документация и программное обеспечение, включенные в пакет Inno Setup, защищены авторским правом Дарчука Александра.\n\n");
            Run run2 = new Run("Авторское право (C) 1997-2023 Дарчук Александр. Все права защищены.\n");
            Run run3 = new Run("Часть авторского права (C) 2000-2023 Рахимов Рамиль. Все права защищены.\n\n");
            Run run4 = new Run("Это программное обеспечение предоставляется 'как есть', без каких-либо явных или подразумеваемых гарантий. В никаком случае автор не несет ответственности за любые убытки, возникшие в результате использования данного программного обеспечения.\n\n");
            Run run5 = new Run("Разрешается любому лицу использовать это программное обеспечение для любых целей, включая коммерческие приложения, а также изменять и распространять его при соблюдении следующих условий:\n");
            Run run6 = new Run("1. Все распространения файлов исходного кода должны сохранять все текущие авторские уведомления и этот список условий без изменений.\n");
            Run run7 = new Run("2. Все распространения в двоичной форме должны сохранять все указания на авторское право и веб-адреса, которые в настоящее время находятся на своих местах (например, в окнах 'О программе').\n");
            Run run8 = new Run("3. Происхождение этого программного обеспечения не должно быть искажено; вы не должны утверждать, что вы написали исходное программное обеспечение. Если вы используете это программное обеспечение для распространения продукта, будет признательно, но не обязательно, включить ссылку на исходное программное обеспечение в документации продукта.\n");
            Run run9 = new Run("4. Измененные версии в исходной или двоичной форме должны быть явно помечены как таковые и не должны искажаться как оригинальное программное обеспечение.\n\n");
            Run run10 = new Run("Дарчук Александр\n");
            Run run11 = new Run("MDA-2023 AT mydocsapp.publicrelations@gmail.com\n");
            Run run12 = new Run("mydocsapp.publicrelations@gmail.com");
            mainTextBlock.Inlines.Add(run1);
            mainTextBlock.Inlines.Add(run2);
            mainTextBlock.Inlines.Add(run3);
            mainTextBlock.Inlines.Add(run4);
            mainTextBlock.Inlines.Add(run5);
            mainTextBlock.Inlines.Add(run6);
            mainTextBlock.Inlines.Add(run7);
            mainTextBlock.Inlines.Add(run8);
            mainTextBlock.Inlines.Add(run9);
            mainTextBlock.Inlines.Add(run10);
            mainTextBlock.Inlines.Add(run11);
            mainTextBlock.Inlines.Add(run12);
        }
    }
}
