using DiplomWPFnetFramework.DataBase;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;

namespace DiplomWPFnetFramework.Classes
{
    public class ServerConnectPostMethodsClass
    {
        public EncryptedResponse CreateNewUser(User user)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                user.Photo64 = user.Photo == null ? null : Convert.ToBase64String(user.Photo);
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(user)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/users", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public EncryptedResponse UpdateUser(User user)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                user.Photo64 = user.Photo == null ? null : Convert.ToBase64String(user.Photo);
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(user)) };

                string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/users";
                string requestUrl = $"{BASE_URL}/{user.Id}";

                var response = _httpClient.PutAsJsonAsync(requestUrl, encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    MessageBox.Show(response.StatusCode.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public EncryptedResponse UpdateItems(List<Item> items)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                foreach (var item in items)
                {
                    item.Image64 = item.Image == null ? null : Convert.ToBase64String(item.Image);
                }
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(items)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/items", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public EncryptedResponse UpdateCreditCard(List<CreditCard> creditCards)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                foreach (var creditCard in creditCards)
                {
                    creditCard.PhotoPage164 = creditCard.PhotoPage1 == null ? null : Convert.ToBase64String(creditCard.PhotoPage1);
                }
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(creditCards)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/creditcards", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public EncryptedResponse UpdateINN(List<INN> INNs)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                foreach (var inn in INNs)
                {
                    inn.PhotoPage164 = inn.PhotoPage1 == null ? null : Convert.ToBase64String(inn.PhotoPage1);
                }
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(INNs)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/inns", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public EncryptedResponse UpdatePassport(List<Passport> passports)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                foreach (var passport in passports)
                {
                    passport.FacePhoto64 = passport.FacePhoto == null ? null : Convert.ToBase64String(passport.FacePhoto);
                    passport.PhotoPage164 = passport.PhotoPage1 == null ? null : Convert.ToBase64String(passport.PhotoPage1);
                    passport.PhotoPage264 = passport.PhotoPage2 == null ? null : Convert.ToBase64String(passport.PhotoPage2);
                }
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(passports)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/passports", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public EncryptedResponse UpdatePhoto(List<Photo> photos)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                foreach (var photo in photos)
                {
                    photo.Image64 = photo.Image == null ? null : Convert.ToBase64String(photo.Image);
                }
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(photos)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/photos", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public EncryptedResponse UpdatePolis(List<Polis> polises)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                foreach (var polis in polises)
                {
                    polis.PhotoPage164 = polis.PhotoPage1 == null ? null : Convert.ToBase64String(polis.PhotoPage1);
                    polis.PhotoPage264 = polis.PhotoPage2 == null ? null : Convert.ToBase64String(polis.PhotoPage2);
                }
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(polises)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/polis", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public EncryptedResponse UpdateSNILS(List<SNILS> SNILSes)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                foreach (var snils in SNILSes)
                {
                    snils.PhotoPage164 = snils.PhotoPage1 == null ? null : Convert.ToBase64String(snils.PhotoPage1);
                }
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(SNILSes)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/snils", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public EncryptedResponse UpdateTemplates(List<Template> Templates)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(Templates)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/templates", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public EncryptedResponse UpdateTemplateDocuments(List<TemplateDocument> TemplateDocuments)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(TemplateDocuments)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/templatedocuments", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public EncryptedResponse UpdateTemplateObjects(List<TemplateObject> TemplateObjects)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(TemplateObjects)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/templateobjects", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public EncryptedResponse UpdateTemplateDocumentDatas(List<TemplateDocumentData> TemplateDocumentDatas)
        {
            HttpClient _httpClient = new HttpClient();
            try
            {
                EncryptedResponse encrypted = new EncryptedResponse() { EncryptedData = DataEncryption.Encrypt(JsonConvert.SerializeObject(TemplateDocumentDatas)) };
                var response = _httpClient.PostAsJsonAsync("http://ramildevm-001-site1.htempurl.com/api/templatedocumentdata", encrypted).Result;
                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = response.Content.ReadAsAsync<EncryptedResponse>().Result;
                    return encryptedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
