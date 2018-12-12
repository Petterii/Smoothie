using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MySmoothieTry2.Model;
using Newtonsoft.Json;

namespace MySmoothieTry2.Data
{
    public class RestService : IRestService
    {
        HttpClient client;

        public List<SmoothieRestItem> Items { get; private set; }

        public RestService()
        {
            //var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task<List<SmoothieRestItem>> RefreshDataAsync()
        {
            Items = new List<SmoothieRestItem>();

            // RestUrl = http://developer.xamarin.com:8081/api/SmoothieRestItems
            // RestUrl = https://smoothieexpress-82203.firebaseapp.com/smoothies
            var uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<SmoothieRestItem>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }

            return Items;
        }

        //Task<List<SmoothieRestItem>> IRestService.RefreshDataAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public Task SaveSmoothieRestItemAsync(SmoothieRestItem item, bool isNewItem)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSmoothieRestItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        //public async Task SaveSmoothieRestItemAsync(SmoothieRestItem item, bool isNewItem = false)
        //{
        //    // RestUrl = http://developer.xamarin.com:8081/api/SmoothieRestItems
        //    var uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(item);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = null;
        //        if (isNewItem)
        //        {
        //            response = await client.PostAsync(uri, content);
        //        }
        //        else
        //        {
        //            response = await client.PutAsync(uri, content);
        //        }

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Debug.WriteLine(@"             SmoothieRestItem successfully saved.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(@"             ERROR {0}", ex.Message);
        //    }
        //}

        //public async Task DeleteSmoothieRestItemAsync(string id)
        //{
        //    // RestUrl = http://developer.xamarin.com:8081/api/SmoothieRestItems/{0}
        //    var uri = new Uri(string.Format(Constants.RestUrl, id));

        //    try
        //    {
        //        var response = await client.DeleteAsync(uri);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Debug.WriteLine(@"             SmoothieRestItem successfully deleted.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(@"             ERROR {0}", ex.Message);
        //    }
        //}
    }
}
