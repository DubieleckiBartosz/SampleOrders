using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using OrderApi.Settings;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Rabbit
{
    public class RabbitConnection:IDisposable
    {
        private readonly RabbitSettings _rabbitSettings;
        private readonly IConnection _connection; 
        public RabbitConnection(IOptions<RabbitSettings> settings)
        {
            _rabbitSettings = settings.Value;
            _connection=GetConnection();
        }
        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitSettings.RabbitMqHost,
                Password = _rabbitSettings.RabbitMqPassword,
                Port = _rabbitSettings.RabbitMqPort,
                UserName = _rabbitSettings.RabbitMqUserName,
                VirtualHost = _rabbitSettings.VHost,
            };
            return factory.CreateConnection();
        }
        public IModel Create()
        {
            return _connection.CreateModel();
        }
        public bool IsEnabled() =>
            _rabbitSettings.RabbitEnabled is true ? true : false;

        public void Dispose()
        {
            try
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Cannot dispose RabbitMQ connection");
            }
        }
    }
}
