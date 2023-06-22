using DiplomWPFnetFramework.DataBase;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using System.Globalization;

namespace DiplomWPFnetFramework.Classes
{
    public class ServerConnectGetMethodsClass
    {
        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/users/byemail";

                string requestUrl = $"{BASE_URL}?email={email}&password={password}";

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                    string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                    User user = JsonConvert.DeserializeObject<User>(decryptedResponseData);
                    user.Photo = (string.IsNullOrEmpty(user.Photo64) ? null : Convert.FromBase64String(user.Photo64));
                    return user; 
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Item>> GetItem(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/items";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<Item> items = JsonConvert.DeserializeObject<List<Item>>(decryptedResponseData);
                        foreach (var item in items)
                        {
                            item.Image = (string.IsNullOrEmpty(item.Image64) ? null : Convert.FromBase64String(item.Image64));
                        }
                        return items;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<CreditCard>> GetCreditCard(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/creditcards";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<CreditCard> creditCards = JsonConvert.DeserializeObject<List<CreditCard>>(decryptedResponseData);
                        foreach (var creditCard in creditCards)
                        {
                            creditCard.PhotoPage1 = (string.IsNullOrEmpty(creditCard.PhotoPage164) ? null : Convert.FromBase64String(creditCard.PhotoPage164));
                        }
                        return creditCards;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<INN>> GetINN(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/inns";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<INN> iNNs = JsonConvert.DeserializeObject<List<INN>>(decryptedResponseData);
                        foreach (var inn in iNNs)
                        {
                            inn.PhotoPage1 = (string.IsNullOrEmpty(inn.PhotoPage164) ? null : Convert.FromBase64String(inn.PhotoPage164));
                        }
                        return iNNs;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Passport>> GetPassport(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/passports";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<Passport> passports = JsonConvert.DeserializeObject<List<Passport>>(decryptedResponseData);
                        foreach (var passport in passports)
                        {
                            passport.FacePhoto = (string.IsNullOrEmpty(passport.FacePhoto64) ? null : Convert.FromBase64String(passport.FacePhoto64));
                            passport.PhotoPage1 = (string.IsNullOrEmpty(passport.PhotoPage164) ? null : Convert.FromBase64String(passport.PhotoPage164));
                            passport.PhotoPage2 = (string.IsNullOrEmpty(passport.PhotoPage264) ? null : Convert.FromBase64String(passport.PhotoPage264));
                        }
                        return passports;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Photo>> GetPhoto(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/photos";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<Photo> photos = JsonConvert.DeserializeObject<List<Photo>>(decryptedResponseData);
                        foreach (var photo in photos)
                        {
                            photo.Image = (string.IsNullOrEmpty(photo.Image64) ? null : Convert.FromBase64String(photo.Image64));
                        }
                        return photos;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Polis>> GetPolis(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/polis";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<Polis> polises = JsonConvert.DeserializeObject<List<Polis>>(decryptedResponseData);
                        foreach (var polis in polises)
                        {
                            polis.PhotoPage1 = (string.IsNullOrEmpty(polis.PhotoPage164) ? null : Convert.FromBase64String(polis.PhotoPage164));
                            polis.PhotoPage2 = (string.IsNullOrEmpty(polis.PhotoPage264) ? null : Convert.FromBase64String(polis.PhotoPage264));
                        }
                        return polises;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<SNILS>> GetSNILS(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/snils";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<SNILS> SNILS = JsonConvert.DeserializeObject<List<SNILS>>(decryptedResponseData);
                        foreach (var snils in SNILS)
                        {
                            snils.PhotoPage1 = (string.IsNullOrEmpty(snils.PhotoPage164) ? null : Convert.FromBase64String(snils.PhotoPage164));
                        }
                        return SNILS;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Template>> GetTemplate(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/templates";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<Template> Templates = JsonConvert.DeserializeObject<List<Template>>(decryptedResponseData);
                        return Templates;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<TemplateDocument>> GetTemplateDocuments(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/templatedocuments";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<TemplateDocument> TemplateDocuments = JsonConvert.DeserializeObject<List<TemplateDocument>>(decryptedResponseData);
                        return TemplateDocuments;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<TemplateObject>> GetTemplateObjects(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/templateobjects";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<TemplateObject> TemplateObjects = JsonConvert.DeserializeObject<List<TemplateObject>>(decryptedResponseData);
                        return TemplateObjects;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<TemplateDocumentData>> GetTemplateDocumentDatas(int userId, DateTime? updateTime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string BASE_URL = "http://ramildevm-001-site1.htempurl.com/api/templatedocumentdata";

                    string formattedDateTime = updateTime.Value.ToString("yyyy-MM-dd+HH:mm:ss");

                    string requestUrl = $"{BASE_URL}?userId={userId}&updateTimeString={formattedDateTime}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        EncryptedResponse encryptedResponse = await response.Content.ReadAsAsync<EncryptedResponse>();
                        string decryptedResponseData = DataEncryption.Decrypt(encryptedResponse.EncryptedData);
                        List<TemplateDocumentData> TemplateDocumentDatas = JsonConvert.DeserializeObject<List<TemplateDocumentData>>(decryptedResponseData);
                        return TemplateDocumentDatas;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
