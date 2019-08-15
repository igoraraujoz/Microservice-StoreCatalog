using GeekBurger.Products.Service;
using GeekBurger.StoreCatalog.Repository;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using GeekBurger.StoreCatalog.Service;
using GeekBurger.StoreCatalog.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GeekBurger.StoreCatalog.CrossCutting.IoC
{
    public class NativeDependencyInjection
    {
        /// <summary>
        /// Injetores de dependencia.
        /// </summary>
        /// <param name="services">Serviços</param>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IProductsRepository, ProductRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<IProductChangedEventRepository, ProductChangedEventRepository>();
            services.AddScoped<IApiService, ApiService>();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, InitializeCheck>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IStoreCatalogReadyService, StoreCatalogReadyService>();
            services.AddSingleton<ILessOfferService, LessOfferService>();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, ProductChangedService>();
            //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, ProductionAreaChanged>();
        }
    }
}
