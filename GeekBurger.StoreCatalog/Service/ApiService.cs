using GeekBurger.Ingredients.Contract.Response;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Helper;
using GeekBurger.StoreCatalog.Service.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Service
{
    /// <summary>
    /// Service responsable to comunicate using HttpClient.
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly AppSettings _appSettings;
        private ILogService _logService;
        
        public ApiService(IOptions<AppSettings> appSettings, ILogService logService)
        {
            _appSettings = appSettings.Value;
            _logService = logService;
        }

        /// <summary>
        /// Get List of Products that API Products.
        /// </summary>
        /// <param name="storeName">StoreName</param>
        /// <returns>Products</returns>
        public async Task<List<ProductToGet>> GetProducts(string storeName)
        {
            CancellationToken cancellationToken = new CancellationToken();
            var client = new HttpClient();
            var uri = new Uri(_appSettings.ProductsApiSettings.Url);

            var policy = Policy.Handle<HttpRequestException>().WaitAndRetry( new[]{
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(30),
                TimeSpan.FromSeconds(60)
            }, (exception, timeSpan) => {
                _logService.SendMessagesAsync($"API Products is not responding. Error: {exception.Message}");
            });

            var response = await policy.Execute(ct => client.GetAsync($"{uri}api/products?storeName={storeName}", ct), cancellationToken);

            var result = "";
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }

            return JsonConvert.DeserializeObject<List<ProductToGet>>(result);
        }

        /// <summary>
        /// Get List of Productions Areas that API Production.
        /// </summary>
        /// <returns>ProductionAreas</returns>
        public async Task<List<Production.Contract.Production>> GetProductionAreas()
        {
            CancellationToken cancellationToken = new CancellationToken();
            var client = new HttpClient();
            var uri = new Uri(_appSettings.ProductionApiSettings.Url);

            var policy = Policy.Handle<HttpRequestException>().WaitAndRetry(new[]{
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(30),
                TimeSpan.FromSeconds(60)
            }, (exception, timeSpan) => {
                _logService.SendMessagesAsync($"API Production is not responding. Error: {exception.Message}");
            });

            var response = await policy.Execute(ct => client.GetAsync(uri + "api/production/areas", ct), cancellationToken);
            //HttpResponseMessage response = await client.GetAsync(uri + "api/production/areas");

            var result = "";
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }

            return JsonConvert.DeserializeObject<List<Production.Contract.Production>>(result);
        }

        /// <summary>
        /// Get Products filtred by restricion of users.
        /// </summary>
        /// <param name="storeId">StoreID</param>
        /// <param name="restrictions">Restrictions of User</param>
        /// <returns>Products and ingredients</returns>
        public async Task<List<IngredientsToUpsert>> GetProductsByRestrictions(Guid storeId, string restrictions)
        {
            CancellationToken cancellationToken = new CancellationToken();
            var client = new HttpClient();
            var result = string.Empty;
            var uri = new Uri(_appSettings.IngredientsApiSettings.Url);
            
            try
            {
                var policy = Policy.Handle<HttpRequestException>().WaitAndRetry(new[]{
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(30),
                    TimeSpan.FromSeconds(60)
                }, (exception, timeSpan) => {
                    string mensagem = $"API ingredients is not responding. Error: {exception.Message}";
                    _logService.SendMessagesAsync(mensagem);
                    Debug.Write(mensagem);
                });

                var response = await policy.Execute(ct => client.GetAsync(uri + $"api/Products/byrestrictions/{storeId}/{restrictions}", ct), cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                //Necessary mock because this APP it's not On-Line.
                _logService.SendMessagesAsync("Serviço http://geekburgergrupotres.azurewebsites.net/ Fora do AR.");
                List<Ingredients.Contract.Response.IngredientsToUpsert> ingredientsToUpserts = new List<IngredientsToUpsert>();
                IngredientsToUpsert ingredient = new IngredientsToUpsert();
                List<string> lstIngredients = new List<string>();
                lstIngredients.Add("soy");
                lstIngredients.Add("gluten");
                ingredient.Ingredients = lstIngredients;
                ingredient.ProductId = Guid.Parse("0d61770a-9c75-4922-a1bd-b1b853a7e04c");
                ingredientsToUpserts.Add(ingredient);
                return ingredientsToUpserts;
            }

            return JsonConvert.DeserializeObject<List<Ingredients.Contract.Response.IngredientsToUpsert>>(result);

        }
    }
}
