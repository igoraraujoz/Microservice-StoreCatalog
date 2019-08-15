using GeekBurger.StoreCatalog.CrossCutting.IoC;
using GeekBurger.StoreCatalog.Helper;
using GeekBurger.StoreCatalog.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace GeekBurger.StoreCatalog
{
    public class Startup
    {
        public static IConfiguration Configuration;
        public IHostingEnvironment HostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "StoreCatalog", Version = "v1" });
            });

            services.AddDbContext<StoreCatalogDbContext>(opt => opt.UseInMemoryDatabase("StoreCatalog"));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            NativeDependencyInjection.RegisterServices(services);

            services.Configure<AppSettings>(appSettings =>
            {
                appSettings.ProductsApiSettings = new ProductsApiSettings()
                {
                    Url = Configuration["AppSettings:ProductsApiSettings:url"],
                };
                appSettings.ProductionApiSettings = new ProductionApiSettings()
                {
                    Url = Configuration["AppSettings:ProductionApiSettings:url"],
                };
                appSettings.LojaSettings = new LojaSettings()
                {
                    Nome = Configuration["AppSettings:LojaSettings:nome"],
                };
                appSettings.IngredientsApiSettings = new IngredientsApiSettings()
                {
                    Url = Configuration["AppSettings:IngredientsApiSettings:url"],
                };
            });

            services.AddMvcCore().AddFormatterMappings().AddJsonFormatters().AddCors().AddApiExplorer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, StoreCatalogDbContext storeCatalogDbContext)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc();            

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
               "StoreCatalog");
            });

            using (var serviceScope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<StoreCatalogDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
