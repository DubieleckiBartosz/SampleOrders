using Microsoft.Extensions.Logging;
using OrderApi.Helpers;
using OrderApi.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderApi.Models;

namespace OrderApi.Rabbit
{
    public class RabbitManager : IRabbitManager
    {

        private readonly RabbitConnection _rabbitConnection;
        private IModel _channel;
        private readonly ILogger<RabbitManager> _logger;

        public RabbitManager(RabbitConnection rabbitConnection, ILogger<RabbitManager> logger)
        {
            _rabbitConnection = rabbitConnection;
            _channel = _rabbitConnection.Create();
            _logger = logger;
        }
  
        public void Publish(string integration,CreateOrder order)
        {
            if (!_rabbitConnection.IsEnabled() || order is null)
            {
                return;
            }

            _channel.ExchangeDeclare(
                exchange: "orders",
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);
            _channel.ConfirmSelect();

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            var body = order.Serialize();

            try
            {
                _channel.BasicPublish(exchange: "orders",
                    routingKey: integration,
                    basicProperties: properties,
                    body: body);
                _channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not create connection: {ex.Message}");
                throw new Exception($"Something went wrong");
            }
        }

    }
}
