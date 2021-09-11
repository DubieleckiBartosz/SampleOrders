using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OrderApi.Interfaces;
using OrderApi.Settings;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using OrderApi.Models;

namespace OrderApi.Tests.Services.ShopServiceTests
{
    public class ShopServiceBaseTests
    {
        protected Mock<IServiceProvider> _serviceProviderMock;
        protected Mock<RedisSettings> _redisSettingsMock;
        protected Mock<IServiceScope> _serviceScopeMock;
        protected Mock<IShopClientService> _shopClientServiceMock;
        protected Mock<IDistributedCache<Shop>> _distributedCache;
        protected Fixture _fixture;
        protected AutoMocker _mocker;
        public ShopServiceBaseTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();
            _serviceProviderMock = _mocker.GetMock<IServiceProvider>();
            _redisSettingsMock = _mocker.GetMock<RedisSettings>();
            _shopClientServiceMock = _mocker.GetMock<IShopClientService>();
            _serviceScopeMock = _mocker.GetMock<IServiceScope>();
            _distributedCache = _mocker.GetMock<IDistributedCache<Shop>>();
        }
    }
}
