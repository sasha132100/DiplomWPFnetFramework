using DiplomWPFnetFramework.Classes;
using DiplomWPFnetFramework.Pages.SettingsPages;
using DiplomWPFnetFramework.Windows.MainInteractionsWindows.SettingsWindows;
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
using System.Windows.Shapes;
using DiplomWPFnetFramework.DataBase;
using DiplomWPFnetFramework.Pages.MainInteractionsPages;
using Microsoft.Office.Interop.Word;
using System.IO;
using Microsoft.Win32;
using Path = System.IO.Path;
using Application = Microsoft.Office.Interop.Word.Application;
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;
using System.Data.Entity.Migrations;
using CheckBox = System.Windows.Controls.CheckBox;
using Border = System.Windows.Controls.Border;
using Style = System.Windows.Style;
using System.Data.Entity.Validation;

namespace DiplomWPFnetFramework.Windows.DocumentTemplatesWindows
{
    /// <summary>
    /// Логика взаимодействия для UserTemplateWindow.xaml
    /// </summary>
    public partial class UserTemplateWindow : System.Windows.Window
    {
        byte[] coverImage = null;
        byte[] imageBytes = null;
        TemplateDocument templateDocumentForId;
        Dictionary<TemplateObject, object> templateObjectDictionary = new Dictionary<TemplateObject, object>();

        public UserTemplateWindow()
        {
            InitializeComponent();
            LoadContent();
            if (SystemContext.isChange)
                UserTemplateOutTextBlock.Text = SystemContext.Item.Title;
        }

        private void LoadContent()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                List<TemplateObject> templateObjects = db.TemplateObject.Where(to => to.TemplateId == SystemContext.Template.Id).OrderBy(to => to.Position).ToList();
                foreach (var templateObject in templateObjects)
                {
                    switch (templateObject.Type) 
                    {
                        case "EditText":
                            AddNewTextBox(templateObject);
                            break;

                        case "NumberText":
                            AddNewTextBox(templateObject);
                            break;

                        case "CheckBox":
                            AddNewCheckBox(templateObject);
                            break;

                        case "Photo":
                            AddNewImage(templateObject);
                            break;

                        default:
                            break;
                    }
                }

                if (SystemContext.isChange)
                {
                    foreach (var dictionaryItem in templateObjectDictionary)
                    {
                        var key = dictionaryItem.Key;
                        var value = dictionaryItem.Value;

                        switch (value)
                        {
                            case TextBox textBox:
                                textBox.Text = db.TemplateDocumentData.Where(tdd => tdd.TemplateObjectId == key.Id).FirstOrDefault().Value;
                                break;

                            case CheckBox checkBox:
                                if (db.TemplateDocumentData.Where(tdd => tdd.TemplateObjectId == key.Id).FirstOrDefault().Value == "true")
                                    checkBox.IsChecked = true;
                                else
                                    checkBox.IsChecked = false;
                                break;

                            case Image image:
                                var takeImage = db.TemplateDocumentData.Where(tdd => tdd.TemplateObjectId == key.Id).FirstOrDefault();
                                if (takeImage == null || takeImage.Value == "" || takeImage.Value == null)
                                    break;
                                image.Source = ByteArrayToImage(Convert.FromBase64String(takeImage.Value));
                                break;

                            default:
                                break;
                        }
                    }
                }
                else 
                {
                    DocumentMoreInteractionsGrid.Visibility = Visibility.Hidden;
                    confirmButtonImage.Margin = new Thickness(0, 0, 10, 0);
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; 
                    break;
                }
            }
        }

        private void AddNewImage(TemplateObject currentTemplateObject)
        {
            TemplateObject templateObject = currentTemplateObject;

            Grid grid = new Grid();
            TextBlock textBlock = new TextBlock() { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCF3DC")) };
            Border border = new Border() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D346F")), Style = (Style)this.Resources["BoderProperties"], Cursor = Cursors.Hand };
            Image image = new Image();
            StackPanel stackPanel = new StackPanel();

            textBlock.Text = currentTemplateObject.Title;

            border.Child = image;
            grid.Children.Add(border);
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(grid);
            mainStackPanel.Children.Add(stackPanel);

            stackPanel.Tag = templateObject;
            border.MouseLeftButtonUp += Photo_MouseLeftButtonUp;

            templateObjectDictionary.Add(templateObject, image);
        }

        private void AddNewCheckBox(TemplateObject currentTemplateObject)
        {
            TemplateObject templateObject = currentTemplateObject;

            Grid grid = new Grid();
            StackPanel stackPanel = new StackPanel();
            CheckBox checkBox = new CheckBox() { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCF3DC")) };

            checkBox.Content = currentTemplateObject.Title;

            grid.Children.Add(checkBox);
            stackPanel.Children.Add(grid);
            mainStackPanel.Children.Add(stackPanel);

            stackPanel.Tag = templateObject;

            templateObjectDictionary.Add(templateObject, checkBox);
        }

        private void AddNewTextBox(TemplateObject currentTemplateObject)
        {
            TemplateObject templateObject = currentTemplateObject;

            Grid grid = new Grid();
            StackPanel stackPanel = new StackPanel();
            TextBlock textBlock = new TextBlock() { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCF3DC")) };
            TextBox textBox = new TextBox() { Resources = (ResourceDictionary)this.Resources["CornerRadiusSetter"] };

            textBlock.Text = currentTemplateObject.Title;

            if (currentTemplateObject.Type == "NumberText")
                textBox.PreviewTextInput += TextBox_PreviewTextInput;

            grid.Children.Add(textBox);
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(grid);
            mainStackPanel.Children.Add(stackPanel);

            stackPanel.Tag = templateObject;

            templateObjectDictionary.Add(templateObject, textBox);
        }

        private TemplateDocumentData CreatingTemplateDocumntData(TemplateObject templateObject, TemplateDocument templateDocument, string Value)
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                TemplateDocumentData templateDocumentData = new TemplateDocumentData();
                if (SystemContext.isChange == false)
                    templateDocumentData.Id = Guid.NewGuid();
                else
                    templateDocumentData = db.TemplateDocumentData.Where(tdd => tdd.TemplateObjectId == templateObject.Id).FirstOrDefault();

                templateDocumentData.Value = Value;
                templateDocumentData.TemplateObjectId = templateObject.Id;
                templateDocumentData.TemplateDocumentId = templateDocument.Id;
                templateDocumentData.UpdateTime = DateTime.Now;

                return templateDocumentData;
            }
        }

        private TemplateDocument CreatingTemplateDocumnt()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                TemplateDocument templateDocument = new TemplateDocument();
                if (!SystemContext.isChange)
                    templateDocument.Id = SystemContext.NewItem.Id;
                else
                    templateDocument.Id = (from td in db.TemplateDocument where td.Id == SystemContext.Item.Id select td).FirstOrDefault<TemplateDocument>().Id;

                templateDocument.TemplateId = SystemContext.Template.Id;
                templateDocument.UpdateTime = DateTime.Now;

                return templateDocument;
            }
        }

        private void AddNewItem()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                Item item = new Item();
                item.Id = Guid.NewGuid();
                item.Title = "Новый документ " + (db.Item.Where(i => i.Type == "Template" && i.UserId == SystemContext.User.Id).Count() + 1);
                item.Type = "Template";
                item.Priority = 0;
                item.IsHidden = 0;
                item.IsSelected = 0;
                item.DateCreation = DateTime.Now;
                item.FolderId = Guid.Empty;
                item.UserId = SystemContext.User.Id;
                item.UpdateTime = DateTime.Now;
                db.Item.Add(item);
                db.SaveChanges();
                SystemContext.NewItem = item;
            }
        }

        private void AddNewUserTamplateData()
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                AddNewItem();
                templateDocumentForId = CreatingTemplateDocumnt();
                db.TemplateDocument.Add(templateDocumentForId);
                db.SaveChanges();
                foreach (var dictionaryItem in templateObjectDictionary)
                {
                    var dictionaryValue = dictionaryItem.Value;
                    string value = null;

                    switch (dictionaryValue)
                    {
                        case TextBox textBox:
                            value = textBox.Text;
                            break;

                        case CheckBox checkBox:
                            if (checkBox.IsChecked == true)
                                value = "true";
                            else
                                value = "false";
                            break;

                        case Image image:
                            if (image.Source == null)
                                break;
                            imageBytes = ConvertBitmapSourceToByteArray(image.Source);
                            value = Convert.ToBase64String(imageBytes);
                            break;

                        default:
                            break;
                    }
                    db.TemplateDocumentData.AddOrUpdate(CreatingTemplateDocumntData(dictionaryItem.Key, templateDocumentForId, value));
                    db.SaveChanges();
                }
            }
        }

        private void ChangeUserTamplateData()
        {
            try
            {
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    templateDocumentForId = db.TemplateDocument.Where(td => td.Id == SystemContext.Item.Id).FirstOrDefault();
                    foreach (var dictionaryItem in templateObjectDictionary)
                    {
                        var key = dictionaryItem.Key;
                        var dictionaryValue = dictionaryItem.Value;
                        string value = null;

                        switch (dictionaryValue)
                        {
                            case TextBox textBox:
                                value = textBox.Text;
                                break;

                            case CheckBox checkBox:
                                if (checkBox.IsChecked == true)
                                    value = "true";
                                else
                                    value = "false";
                                break;

                            case Image image:
                                if (image.Source == null)
                                    break;
                                imageBytes = ConvertBitmapSourceToByteArray(image.Source);
                                value = Convert.ToBase64String(imageBytes);
                                break;

                            default:
                                break;
                        }
                        db.TemplateDocumentData.AddOrUpdate(CreatingTemplateDocumntData(dictionaryItem.Key, templateDocumentForId, value));
                    }
                    db.SaveChanges();
                    MessageBox.Show("Данные изменены.", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        MessageBox.Show($"Property: {validationError.PropertyName}");
                        MessageBox.Show($"Error: {validationError.ErrorMessage}");
                    }
                }
            }
        }

        private BitmapSource ByteArrayToImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        public static byte[] ConvertBitmapSourceToByteArray(ImageSource imageSource)
        {
            BitmapSource bitmapSource = (BitmapSource)imageSource;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);

                return stream.ToArray();
            }
        }

        private void ImageSetter(Image image)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileImage = openFileDialog.FileName;
                imageBytes = File.ReadAllBytes(fileImage);
                image.Source = ByteArrayToImage(imageBytes);
            }
        }

        private void ChangeCoverImage()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileImage = openFileDialog.FileName;
                coverImage = File.ReadAllBytes(fileImage);
                Item item;
                item = SystemContext.Item;
                using (var db = new LocalMyDocsAppDBEntities())
                {
                    item.Image = coverImage;
                    item.UpdateTime = DateTime.Now;
                    db.Item.AddOrUpdate(item);
                    db.SaveChanges();
                }
            }
        }

        private void Photo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSetter((sender as Border).Child as Image);
        }

        private void BackWindowButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
            this.Close();
        }

        private void confirmButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewUserTamplateData();
                BackWindowButtonImage_MouseLeftButtonUp(sender, e);
            }
            else
            {
                ChangeUserTamplateData();
            }
            this.Close();
        }

        private void DocumentMoreInteractionsButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var contextMenu = (ContextMenu)this.FindResource("DocumentMoreInteractionsContextMenu");
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void MenuItemave_Click(object sender, RoutedEventArgs e)
        {
            if (SystemContext.isChange == false)
            {
                AddNewUserTamplateData();
            }
            else
            {
                ChangeUserTamplateData();
            }
            this.Close();
        }

        private void MenuItemChangeCover_Click(object sender, RoutedEventArgs e)
        {
            ChangeCoverImage();
        }

        private void MenuItemCreateDocumentScan_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new LocalMyDocsAppDBEntities())
            {
                Application wordApp = new Application();

                Document doc = wordApp.Documents.Add();

                Paragraph passportParagraph = doc.Content.Paragraphs.Add();
                passportParagraph.Range.Text = "";

                foreach (var dictionaryItem in templateObjectDictionary)
                {
                    var key = dictionaryItem.Key;
                    var value = dictionaryItem.Value;

                    switch (value)
                    {
                        case TextBox textBox:
                            passportParagraph.Range.Text += $"{key.Title}: {db.TemplateDocumentData.Where(tdd => tdd.TemplateObjectId == key.Id).FirstOrDefault().Value}";
                            break;

                        case CheckBox checkBox:
                            if (db.TemplateDocumentData.Where(tdd => tdd.TemplateObjectId == key.Id).FirstOrDefault().Value == "true")
                                passportParagraph.Range.Text += $"{key.Title}: Выбрано";
                            else
                                passportParagraph.Range.Text += $"{key.Title}: Не выбрано";
                            break;

                        case Image image:
                            var takeImage = db.TemplateDocumentData.Where(tdd => tdd.TemplateObjectId == key.Id).FirstOrDefault();
                            if (takeImage == null || takeImage.Value == "" || takeImage.Value == null)
                                break;

                            doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);

                            Range imageRange = doc.Content.Paragraphs.Add().Range;
                            imageRange.InsertParagraphAfter();

                            string tempImagePath = Path.GetTempFileName();
                            File.WriteAllBytes(tempImagePath, Convert.FromBase64String(takeImage.Value));
                            InlineShape shape = imageRange.InlineShapes.AddPicture(tempImagePath);
                            File.Delete(tempImagePath);
                            break;

                        default:
                            break;
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"{SystemContext.Item.Title}.pdf";

                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        string outputPath = saveFileDialog.FileName;

                        doc.ExportAsFixedFormat(outputPath, WdExportFormat.wdExportFormatPDF);
                    }
                    catch
                    {
                        MessageBox.Show("Закойте документ для обновления!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Item item;
            item = SystemContext.Item;
            using (var db = new LocalMyDocsAppDBEntities())
            {
                templateDocumentForId = db.TemplateDocument.Where(td => td.Id == SystemContext.Item.Id).FirstOrDefault();
                List<TemplateDocumentData> templateDocumentDataForDelete = db.TemplateDocumentData.Where(tdd => tdd.TemplateDocumentId == templateDocumentForId.Id).ToList();
                foreach (var templateDocumentData in templateDocumentDataForDelete)
                {
                    db.Entry(templateDocumentData).State = System.Data.Entity.EntityState.Deleted;
                }
                db.Entry(templateDocumentForId).State = System.Data.Entity.EntityState.Deleted;
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            if (SystemContext.PageForLoadContent is DocumentViewingPage)
            {
                DocumentViewingPage documentViewingPage = (DocumentViewingPage)SystemContext.PageForLoadContent;
                documentViewingPage.LoadContent();
            }
            else
            {
                FolderContentPage folderContentPage = (FolderContentPage)SystemContext.PageForLoadContent;
                folderContentPage.LoadContent();
            }
            this.Close();
        }
    }
}
