using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для CreditCardWindow.xaml
    /// </summary>
    public partial class CreditCardWindow : Window
    {
        public CreditCardWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                CreditCardOutTextBlock.Text = SystemContext.Item.Title;
        }

        private void LoadContent()
        {
            if (SystemContext.isChange == false)
                return;
            using (var db = new test123Entities1())
            {
                var creditCard = (from p in db.CreditCard where p.Id == SystemContext.Item.Id select p).FirstOrDefault<CreditCard>();
                CardNumberTextBox.Text = creditCard.Number;
                FIOTextBox.Text = creditCard.FIO;
                MonthAndYearTextBox.Text = creditCard.ExpiryDate;
                CVVCodeTextBox.Text = creditCard.CVV.ToString();
            }

        }

        private CreditCard CreatingCreditCardObject()
        {
            using (var db = new test123Entities1())
            {
                CreditCard creditCard = new CreditCard();
                if (SystemContext.isChange == false)
                    creditCard.Id = SystemContext.NewItem.Id;
                else
                    creditCard.Id = (from p in db.CreditCard where p.Id == SystemContext.Item.Id select p).FirstOrDefault<CreditCard>().Id;

                creditCard.Number = CardNumberTextBox.Text;
                creditCard.FIO = FIOTextBox.Text;
                creditCard.ExpiryDate = MonthAndYearTextBox.Text;
                creditCard.CVV = Convert.ToInt32(CVVCodeTextBox.Text);
                return creditCard;
            }
        }

        private string CheckingTheFullness()
        {
            if (CardNumberTextBox.Text != "" && FIOTextBox.Text != "" && MonthAndYearTextBox.Text != "" &&
                CVVCodeTextBox.Text != "")
                return "Заполнены";
            else
                return "Не заполнены";
        }

        private void AddNewItem()
        {
            using (var db = new test123Entities1())
            {
                Items item = new Items();
                item.Title = "NewTitle" + db.Items.OrderByDescending(items => items.Id).FirstOrDefault().Id.ToString();
                item.IType = "CreditCard";
                item.IPriority = 0;
                item.IsHidden = 0;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.UserId = SystemContext.User.Id;
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
                SystemContext.NewItem = item;
            }
        }

        private string AddNewCreditCard()
        {
            if (CheckingTheFullness() != "Заполнены")
                return "Не заполнены";
            using (var db = new test123Entities1())
            {
                AddNewItem();
                db.CreditCard.Add(CreatingCreditCardObject());
                db.SaveChanges();
                return "Добавлен";
            }
        }

        private void ChangeCreditCard()
        {
            using (var db = new test123Entities1())
            {
                db.CreditCard.AddOrUpdate(CreatingCreditCardObject());
                db.SaveChanges();
                MessageBox.Show("Данные изменены");
            }
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                if (AddNewCreditCard() == "Не заполнены")
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Вы заполнили не все поля, уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.No)
                        return;
                }
            }
            else
            {
                ChangeCreditCard();
            }
            DocumentViewingWindow documentViewingWindow = new DocumentViewingWindow();
            this.Close();
            documentViewingWindow.ShowDialog();
        }

        private void PhotoPageOpen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
