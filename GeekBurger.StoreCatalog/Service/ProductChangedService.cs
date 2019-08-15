using AutoMapper;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using GeekBurger.StoreCatalog.Service.Interfaces;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SubscriptionClient = Microsoft.Azure.ServiceBus.SubscriptionClient;

namespace GeekBurger.StoreCatalog.Service
{
    /// <summary>
    /// Service responsable to subscribe and receive message to the topic ProductChanged
    /// </summary>
    public class ProductChangedService : BackgroundService
    {
        private const string Topic = "ProductChanged";
        private IConfiguration _configuration;
        private static IMapper _mapper;
        private static ServiceBusConfiguration serviceBusConfiguration;
        private static string _storeId = "8d618778-85d7-411e-878b-846a8eef30c0";
        private const string SubscriptionName = "Los_Angeles_Pasadena_store";
        private IServiceBusNamespace _namespace;       
        private static IServiceProvider _provider;
        private static IProductChangedEventRepository _productChangedEventRepository;
        private static ILogService _logService;

        public ProductChangedService(IServiceProvider serviceProvider, IMapper mapper, IConfiguration configuration, ILogService logService)
        {
            _provider = serviceProvider;
            _mapper = mapper;
            _configuration = configuration;
            _namespace = _configuration.GetServiceBusNamespace();
            _logService = logService;
            EnsureTopicIsCreated();
        }

        public void EnsureTopicIsCreated()
        {
            if (!_namespace.Topics.List()
                .Any(topic => topic.Name
                    .Equals(Topic, StringComparison.InvariantCultureIgnoreCase)))
                _namespace.Topics.Define(Topic)
                    .WithSizeInMB(1024).Create();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            serviceBusConfiguration = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var serviceBusNamespace = _configuration.GetServiceBusNamespace();
            var topic = serviceBusNamespace.Topics.GetByName(Topic);
            topic.Subscriptions.DeleteByName(SubscriptionName);
            if (!topic.Subscriptions.List()
                   .Any(subscription => subscription.Name
                   .Equals(SubscriptionName, StringComparison.InvariantCultureIgnoreCase))) topic.Subscriptions
                    .Define(SubscriptionName)
                    .Create();

            ReceiveMessages();
        }

        private static async void ReceiveMessages()
        {
            var subscriptionClient = new SubscriptionClient(serviceBusConfiguration.ConnectionString, Topic, SubscriptionName);
            await subscriptionClient.RemoveRuleAsync("$Default");
            await subscriptionClient.AddRuleAsync(new RuleDescription
            {
                Filter = new CorrelationFilter { Label = _storeId },
                Name = "filter-store"
            });
            var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };
            subscriptionClient.RegisterMessageHandler(Handle, mo);
        }

        private static Task Handle(Message message, CancellationToken arg2)
        {
            var productChangesString = Encoding.UTF8.GetString(message.Body);

            var obj = JsonConvert.DeserializeObject<Products.Contract.ProductChangedMessage>(productChangesString);
            var productChanged = _mapper.Map<ProductChangedEvent>(obj);

            using (IServiceScope scope = _provider.CreateScope())
            {
                _productChangedEventRepository = scope.ServiceProvider.GetRequiredService<IProductChangedEventRepository>();
                var retorno = _productChangedEventRepository.Maintain(productChanged);

                if(!retorno)
                {
                    _logService.SendMessagesAsync("Error in UpSert ProductChanged.");
                }
            }               

            return Task.CompletedTask;
        }

        private static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            var context = arg.ExceptionReceivedContext;
            return Task.CompletedTask;
        }
    }
}
