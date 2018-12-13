using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MySmoothieTry2.Model;
using Newtonsoft.Json;
using ModernHttpClient;
using System.Collections.Generic;

namespace MySmoothieTry2.RestClients
{
    public class Edamam<T>
    {
        private const string API_ID = "95724e2e";
        private const string API_KEY = "c327fd021d93b10202b4ddec95c1f4fd";

        HttpClient _httpClient = new HttpClient(new NativeMessageHandler());

        public async Task<T> GetAllIngredients(string ingredient)
        {
            var ingrLowerCase = ingredient.ToLower().Trim();

            try
            {
                var response = await _httpClient.GetAsync($"https://api.edamam.com/api/food-database/parser?ingr={ingrLowerCase}&app_id=95724e2e&app_key=c327fd021d93b10202b4ddec95c1f4fd");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var models = JsonConvert.DeserializeObject<T>(content);
                    return models;
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            // TODO - What to return in case of error
            return JsonConvert.DeserializeObject<T>(null);
        }

        public async Task<T> GetSmoothieNutritionInfo(NutritionModelPOST nutrModelPOST)
        {
            var data = JsonConvert.SerializeObject(nutrModelPOST);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            try
            {
                var httpResponse = await _httpClient.PostAsync($"https://api.edamam.com/api/food-database/nutrients?app_id={API_ID}&app_key={API_KEY}", content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var d = JsonConvert.DeserializeObject<T>(response);
                    return d;
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            // TODO - What to return in case of error
            return JsonConvert.DeserializeObject<T>(null);
        }
    }
}
