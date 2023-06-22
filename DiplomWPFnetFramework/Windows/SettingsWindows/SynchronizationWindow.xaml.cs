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

namespace DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows
{
    /// <summary>
    /// Логика взаимодействия для SynchronizationWindow.xaml
    /// </summary>
    public partial class SynchronizationWindow : Window
    {
        private bool isStart = false;

        public SynchronizationWindow()
        {
            InitializeComponent();
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void GetItemsFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<Item> items = await serverConnectGetMethodsClass.GetItem(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (items == null)
                {
                    MessageBox.Show("Данные объектов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var item in items)
                {
                    if ((db.Item.Count(i => i.Id == item.Id) > 0 && db.Item.Where(i => i.Id == item.Id).FirstOrDefault().UpdateTime == item.UpdateTime) || (item.UpdateTime == null && db.Item.Count(i => i.Id == item.Id) == 0))
                        continue;
                    if (item.FolderId != new Guid())
                        item.IsSelected = 1;
                    if (item.UpdateTime == null && db.Item.Count(i => i.Id == item.Id) > 0)
                        db.Item.Remove(item);
                    else if (item.UpdateTime != null)
                        db.Item.AddOrUpdate(item);
                    db.SaveChanges();
                }
            }
        }

        private async void GetCreditCardsFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<CreditCard> creditCards = await serverConnectGetMethodsClass.GetCreditCard(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (creditCards == null)
                {
                    MessageBox.Show("Данные объектов банковских карт на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var creditCard in creditCards)
                {
                    if ((db.CreditCard.Count(i => i.Id == creditCard.Id) > 0 && db.CreditCard.Where(i => i.Id == creditCard.Id).FirstOrDefault().UpdateTime == creditCard.UpdateTime) || (creditCard.UpdateTime == null && db.Item.Count(i => i.Id == creditCard.Id) == 0))
                        continue;
                    if (creditCard.UpdateTime == null && db.Item.Count(i => i.Id == creditCard.Id) > 0)
                        db.CreditCard.Remove(creditCard);
                    else if (creditCard.UpdateTime != null)
                        db.CreditCard.AddOrUpdate(creditCard);
                    db.SaveChanges();
                }
            }
        }

        private async void GetINNsFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<INN> INNs = await serverConnectGetMethodsClass.GetINN(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (INNs == null)
                {
                    MessageBox.Show("Данные объектов инн на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var inn in INNs)
                {
                    if ((db.INN.Count(i => i.Id == inn.Id) > 0 && db.INN.Where(i => i.Id == inn.Id).FirstOrDefault().UpdateTime == inn.UpdateTime) || (inn.UpdateTime == null && db.Item.Count(i => i.Id == inn.Id) == 0))
                        continue;
                    if (inn.UpdateTime == null && db.Item.Count(i => i.Id == inn.Id) > 0)
                        db.INN.Remove(inn);
                    else if (inn.UpdateTime != null)
                        db.INN.AddOrUpdate(inn);
                    db.SaveChanges();
                }
            }
        }

        private async void GetPassportFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<Passport> passports = await serverConnectGetMethodsClass.GetPassport(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (passports == null)
                {
                    MessageBox.Show("Данные объектов пасспортов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var passport in passports)
                {
                    if ((db.Passport.Count(i => i.Id == passport.Id) > 0 && db.Passport.Where(i => i.Id == passport.Id).FirstOrDefault().UpdateTime == passport.UpdateTime) || (passport.UpdateTime == null && db.Item.Count(i => i.Id == passport.Id) == 0))
                        continue;
                    if (passport.UpdateTime == null && db.Item.Count(i => i.Id == passport.Id) > 0)
                        db.Passport.Remove(passport);
                    else if (passport.UpdateTime != null)
                        db.Passport.AddOrUpdate(passport);
                    db.SaveChanges();
                }
            }
        }

        private async void GetPhotoFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<Photo> photos = await serverConnectGetMethodsClass.GetPhoto(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (photos == null)
                {
                    MessageBox.Show("Данные объектов фото на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var photo in photos)
                {
                    if ((db.Photo.Count(i => i.Id == photo.Id) > 0 && db.Photo.Where(i => i.Id == photo.Id).FirstOrDefault().UpdateTime == photo.UpdateTime) || (photo.UpdateTime == null && db.Item.Count(i => i.Id == photo.Id) == 0))
                        continue;
                    if (photo.UpdateTime == null && db.Item.Count(i => i.Id == photo.Id) > 0)
                        db.Photo.Remove(photo);
                    else if (photo.UpdateTime != null)
                        db.Photo.AddOrUpdate(photo);
                    db.SaveChanges();
                }
            }
        }

        private async void GetPolisFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<Polis> polises = await serverConnectGetMethodsClass.GetPolis(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (polises == null)
                {
                    MessageBox.Show("Данные объектов полисов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var polis in polises)
                {
                    if ((db.Polis.Count(i => i.Id == polis.Id) > 0 && db.Polis.Where(i => i.Id == polis.Id).FirstOrDefault().UpdateTime == polis.UpdateTime) || (polis.UpdateTime == null && db.Item.Count(i => i.Id == polis.Id) == 0))
                        continue;
                    if (polis.UpdateTime == null && db.Item.Count(i => i.Id == polis.Id) > 0)
                        db.Polis.Remove(polis);
                    else if (polis.UpdateTime != null)
                        db.Polis.AddOrUpdate(polis);
                    db.SaveChanges();
                }
            }
        }

        private async void GetSNILSFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<SNILS> SNILS = await serverConnectGetMethodsClass.GetSNILS(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (SNILS == null)
                {
                    MessageBox.Show("Данные объектов снилсов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var snils in SNILS)
                {
                    if ((db.SNILS.Count(i => i.Id == snils.Id) > 0 && db.SNILS.Where(i => i.Id == snils.Id).FirstOrDefault().UpdateTime == snils.UpdateTime) || (snils.UpdateTime == null && db.Item.Count(i => i.Id == snils.Id) == 0))
                        continue;
                    if (snils.UpdateTime == null && db.Item.Count(i => i.Id == snils.Id) > 0)
                        db.SNILS.Remove(snils);
                    else if (snils.UpdateTime != null)
                        db.SNILS.AddOrUpdate(snils);
                    db.SaveChanges();
                }
            }
        }

        private async void GetTemplatesFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<Template> Templates = await serverConnectGetMethodsClass.GetTemplate(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (Templates == null)
                {
                    MessageBox.Show("Данные объектов снилсов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var template in Templates)
                {
                    if ((db.Template.Count(i => i.Id == template.Id) > 0 && db.Template.Where(i => i.Id == template.Id).FirstOrDefault().UpdateTime == template.UpdateTime) || (template.UpdateTime == null && db.Item.Count(i => i.Id == template.Id) == 0))
                        continue;
                    if (template.UpdateTime == null && db.Item.Count(i => i.Id == template.Id) > 0)
                        db.Template.Remove(template);
                    else if (template.UpdateTime != null)
                        db.Template.AddOrUpdate(template);
                    db.SaveChanges();
                }
            }
        }

        private async void GetTemplateDocumentsFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<TemplateDocument> TemplateDocuments = await serverConnectGetMethodsClass.GetTemplateDocuments(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (TemplateDocuments == null)
                {
                    MessageBox.Show("Данные объектов снилсов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var templateDocument in TemplateDocuments)
                {
                    if ((db.TemplateDocument.Count(i => i.Id == templateDocument.Id) > 0 && db.TemplateDocument.Where(i => i.Id == templateDocument.Id).FirstOrDefault().UpdateTime == templateDocument.UpdateTime) || (templateDocument.UpdateTime == null && db.Item.Count(i => i.Id == templateDocument.Id) == 0))
                        continue;
                    if (templateDocument.UpdateTime == null && db.Item.Count(i => i.Id == templateDocument.Id) > 0)
                        db.TemplateDocument.Remove(templateDocument);
                    else if (templateDocument.UpdateTime != null)
                        db.TemplateDocument.AddOrUpdate(templateDocument);
                    db.SaveChanges();
                }
            }
        }

        private async void GetTemplateObjectsFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<TemplateObject> TemplateObjects = await serverConnectGetMethodsClass.GetTemplateObjects(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (TemplateObjects == null)
                {
                    MessageBox.Show("Данные объектов снилсов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var templateObject in TemplateObjects)
                {
                    if ((db.TemplateObject.Count(i => i.Id == templateObject.Id) > 0 && db.TemplateObject.Where(i => i.Id == templateObject.Id).FirstOrDefault().UpdateTime == templateObject.UpdateTime) || (templateObject.UpdateTime == null && db.Item.Count(i => i.Id == templateObject.Id) == 0))
                        continue;
                    if (templateObject.UpdateTime == null && db.Item.Count(i => i.Id == templateObject.Id) > 0)
                        db.TemplateObject.Remove(templateObject);
                    else if (templateObject.UpdateTime != null)
                        db.TemplateObject.AddOrUpdate(templateObject);
                    db.SaveChanges();
                }
            }
        }

        private async void GetTemplateDocumentDatasFromServer()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                ServerConnectGetMethodsClass serverConnectGetMethodsClass = new ServerConnectGetMethodsClass();
                List<TemplateDocumentData> TemplateDocumentDatas = await serverConnectGetMethodsClass.GetTemplateDocumentDatas(SystemContext.User.Id, SystemContext.User.UpdateTime);
                if (TemplateDocumentDatas == null)
                {
                    MessageBox.Show("Данные объектов снилсов на сервере отсутствуют.", "Оповещение.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                foreach (var templateDocumentData in TemplateDocumentDatas)
                {
                    if ((db.TemplateDocumentData.Count(i => i.Id == templateDocumentData.Id) > 0 && db.TemplateDocumentData.Where(i => i.Id == templateDocumentData.Id).FirstOrDefault().UpdateTime == templateDocumentData.UpdateTime) || (templateDocumentData.UpdateTime == null && db.Item.Count(i => i.Id == templateDocumentData.Id) == 0))
                        continue;
                    if (templateDocumentData.UpdateTime == null && db.Item.Count(i => i.Id == templateDocumentData.Id) > 0)
                        db.TemplateDocumentData.Remove(templateDocumentData);
                    else if (templateDocumentData.UpdateTime != null)
                        db.TemplateDocumentData.AddOrUpdate(templateDocumentData);
                    db.SaveChanges();
                }
            }
        }

        private void DownloadFromServerButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isStart)
                    return;
                isStart = true;
                LoadingGrid.Visibility = Visibility.Visible;
                GetItemsFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 10%";
                GetCreditCardsFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 20%";
                GetINNsFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 30%";
                GetPassportFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 40%";
                GetPhotoFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 50%";
                GetPolisFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 60%";
                GetSNILSFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 70%";
                GetTemplatesFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 80%";
                GetTemplateDocumentsFromServer();
                WhiteLoadBorder.Width -= 30;
                PurpleLoadBorder.Width += 30;
                LoadingTextBlock.Text = "Загрузка: 90%";
                GetTemplateObjectsFromServer();
                WhiteLoadBorder.Width = 0;
                PurpleLoadBorder.Width = 300;
                LoadingTextBlock.Text = "Загрузка: 100%";
                GetTemplateDocumentDatasFromServer();
                MessageBox.Show("Успешно", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                isStart = false;
                WhiteLoadBorder.Width = 300;
                PurpleLoadBorder.Width = 0;
                LoadingTextBlock.Text = "Загрузка: 0%";
                LoadingGrid.Visibility = Visibility.Hidden;
            }
            catch
            {
                MessageBox.Show("Нету связи с сервером!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadOnServerButtonClick_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isStart)
                    return;
                isStart = true;
                LoadingGrid.Visibility = Visibility.Visible;
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    ServerConnectPostMethodsClass serverConnectPostMethodsClass = new ServerConnectPostMethodsClass();
                    serverConnectPostMethodsClass.UpdateItems(db.Item.Where(i => i.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 10%";
                    serverConnectPostMethodsClass.UpdateCreditCard(db.CreditCard.Where(c => c.Item.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 20%";
                    serverConnectPostMethodsClass.UpdateINN(db.INN.Where(inn => inn.Item.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 30%";
                    serverConnectPostMethodsClass.UpdatePassport(db.Passport.Where(pas => pas.Item.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 40%";
                    serverConnectPostMethodsClass.UpdatePhoto(db.Photo.Where(pho => pho.Item.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 50%";
                    serverConnectPostMethodsClass.UpdatePolis(db.Polis.Where(pol => pol.Item.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 60%";
                    serverConnectPostMethodsClass.UpdateSNILS(db.SNILS.Where(s => s.Item.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 70%";
                    serverConnectPostMethodsClass.UpdateTemplates(db.Template.Where(t => t.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 80%";
                    serverConnectPostMethodsClass.UpdateTemplateDocuments(db.TemplateDocument.Where(td => td.Template.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width -= 30;
                    PurpleLoadBorder.Width += 30;
                    LoadingTextBlock.Text = "Загрузка: 90%";
                    serverConnectPostMethodsClass.UpdateTemplateObjects(db.TemplateObject.Where(to => to.Template.UserId == SystemContext.User.Id).ToList());
                    WhiteLoadBorder.Width = 0;
                    PurpleLoadBorder.Width = 300;
                    LoadingTextBlock.Text = "Загрузка: 100%";
                    serverConnectPostMethodsClass.UpdateTemplateDocumentDatas(db.TemplateDocumentData.Where(tdd => tdd.TemplateDocument.Template.UserId == SystemContext.User.Id).ToList());
                    MessageBox.Show("Успешно", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                isStart = false;
                WhiteLoadBorder.Width = 300;
                PurpleLoadBorder.Width = 0;
                LoadingTextBlock.Text = "Загрузка: 0%";
                LoadingGrid.Visibility = Visibility.Hidden;
            }
            catch
            {
                MessageBox.Show("Нету связи с сервером!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
