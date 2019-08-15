using AutoMapper;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Service
{
    /// <summary>
    /// Service responsable to subscribe and receive message to the topic ProductionAreaChanged
    /// </summary>
    public class ProductionAreaChanged : BackgroundService
    {
        private const string Topic = "ProductionAreaChanged";
        private IConfiguration _configuration;
        private IMapper _mapper;
        private static ServiceBusConfiguration serviceBusConfiguration;
        private static string _storeId = "8d618778-85d7-411e-878b-846a8eef30c0";
        private const string SubscriptionName = "Los Angeles - Beverly Hills";
        private IServiceBusNamespace _namespace;

        public ProductionAreaChanged(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
            _namespace = _configuration.GetServiceBusNamespace();
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
            var productAreaChangesString = Encoding.UTF8.GetString(message.Body);
            return Task.CompletedTask;
        }

        private static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            var context = arg.ExceptionReceivedContext;
            return Task.CompletedTask;
        }
    }
}