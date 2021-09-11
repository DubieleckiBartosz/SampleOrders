using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShopApi.Helpers;
using ShopApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ShopApi.Dto.Orders;
using ShopApi.Wrappers;

namespace ShopApi.Background
{
    public class ConsumeOrderRabbitMq:BackgroundService
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        public ConsumeOrderRabbitMq(ILoggerFactory loggerFactory,IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this._logger = loggerFactory.CreateLogger<ConsumeOrderRabbitMq>();
            GetRabbitMQ();
        }

        private void GetRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost", DispatchConsumersAsync = true };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                "shop_order",
                false, 
                false,
                false,
                null);
         
            
            _channel.QueueBind(
                "shop_order",
                "orders",
                "order.*", 
                null);
            
            _channel.BasicQos(0, 1, false);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                using var scope = _serviceProvider.CreateScope(); 
                var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

                var content = ea.Body.ToArray().Deserialize<CreateOrderDto>();
                var type = ea.RoutingKey;
                if (type == "order.create")
                {
                    var result=await service.CreateOrderAsync(content.ShopId,content);
                    LogMessage(result.Data);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("shop_order", false, consumer);
              return Task.CompletedTask;
        }

        private void LogMessage(string content)
        {
            _logger.LogInformation($"Created new order {content}");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

    }
}
