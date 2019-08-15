using AutoMapper;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Helper;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using GeekBurger.StoreCatalog.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Service
{
    /// <summary>
    /// Service responsable to Initialize and Load API.
    /// </summary>
    public class InitializeCheck : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private IMapper _mapper;
        private readonly IServiceProvider _provider;

        private IProductsRepository _productsRepository;
        private IProductionRepository _productionRepository;
        private IStoreRepository _storeRepository;
        private IApiService _apiService;
        private ILogService _logService;        
        private IStoreCatalogReadyService _storeCatalogReadyService;
        private ILessOfferService _lessOfferService;

        public InitializeCheck(IServiceProvider serviceProvider, IMapper mapper, IOptions<AppSettings> appSettings, 
            ILogService logService, 
            IStoreCatalogReadyService storeCatalogReadyService, 
            ILessOfferService lessOfferService)
        {
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _provider = serviceProvider;            
            _logService = logService;
            _storeCatalogReadyService = storeCatalogReadyService;
            _lessOfferService = lessOfferService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                Debug.Write($"InicializadorAPI Iniciando...");
                _logService.SendMessagesAsync("StoreCatalog Iniciando InitializeCheck");

                List<Store> stores = new List<Store>();
                stores.AddRange(new List<Store> {
                    new Store { Name = "California - Pasadena", StoreId = new Guid("8048e9ec-80fe-4bad-bc2a-e4f4a75c834e") },
                    new Store { Name = "Los Angeles - Beverly Hills", StoreId = new Guid("8d618778-85d7-411e-878b-846a8eef30c0") }
                });

                _productsRepository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
                _productionRepository = scope.ServiceProvider.GetRequiredService<IProductionRepository>();                
                _storeRepository = scope.ServiceProvider.GetRequiredService<IStoreRepository>();
                _apiService = scope.ServiceProvider.GetRequiredService<IApiService>();

                List<Production.Contract.Production> areas = _apiService.GetProductionAreas().Result;

                var store = stores.Find(x => x.Name.Contains(_appSettings.LojaSettings.Nome));

                var productions = _mapper.Map<IEnumerable<Model.Production>>(areas);
                var products = _mapper.Map<IEnumerable<Product>>(_apiService.GetProducts(_appSettings.LojaSettings.Nome).Result);

                if (products != null)
                {
                    foreach (var item in products)
                    {
                        _productsRepository.Create(item);
                    }
                }
                else
                {
                    _logService.SendMessagesAsync(
                        JsonConvert.SerializeObject(HttpStatusCode.ServiceUnavailable));
                }

                if (store != null)
                {
                    _storeRepository.Create(store);
                }
                else
                {
                    _logService.SendMessagesAsync(
                        JsonConvert.SerializeObject(HttpStatusCode.ServiceUnavailable));
                }

                if (productions != null)
                {
                    foreach (var item in productions)
                    {
                        _productionRepository.Create(item);
                    }
                }
                else
                {
                    _logService.SendMessagesAsync(
                        JsonConvert.SerializeObject(HttpStatusCode.ServiceUnavailable));
                }

                StoreCatalogReady storeCatalogReady = new StoreCatalogReady()
                {
                    Ready = true,
                    StoreId = store.StoreId
                };

                _storeCatalogReadyService.SendMessagesAsync(JsonConvert.SerializeObject(storeCatalogReady));

                _logService.SendMessagesAsync("StoreCatalog Finalizando InitializeCheck");

                stoppingToken.Register(() =>
                    Debug.Write($" InicializadorAPI Parando"));
            }
        }
    }
}
