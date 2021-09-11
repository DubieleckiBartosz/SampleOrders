using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderApi.Interfaces;
using OrderApi.Models;
using OrderApi.Services;
using OrderApi.Settings;
using OrderApi.Wrappers;
using Xunit;

namespace OrderApi.Tests.Services.ShopServiceTests
{
    public class GetShopsAsyncTests:ShopServiceBaseTests
    {
        [Fact]
        public async Task Should_Return_Data_From_Cache_When_Enabled_Is_True()
        {
            var _redis = _fixture.Build<RedisSettings>()
                .With(w => w.Enabled, true)
                .Create();

            var _factory = new Mock<IServiceScopeFactory>();

            _serviceProviderMock.Setup(x =>
                    x.GetService(typeof(IDistributedCache<Shop>)))
                .Returns(_distributedCache.Object);

            _serviceScopeMock.Setup(x =>
                x.ServiceProvider).Returns(_serviceProviderMock.Object);

            _factory.Setup(x =>
                x.CreateScope()).Returns(_serviceScopeMock.Object);
            
                _serviceProviderMock.Setup(x => 
                        x.GetService(typeof(IServiceScopeFactory)))
                .Returns(_factory.Object);


            _distributedCache.Setup(s =>
                    s.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Build<Shop>().CreateMany());

            var shopService = new ShopService(
                _shopClientServiceMock.Object,
                _redis,
                _serviceProviderMock.Object);

            var result = await shopService.GetShopsAsync();

            _distributedCache.Verify(v=>
                v.GetAsync(It.IsAny<string>()),Times.Once);
            Assert.NotEmpty(result.Data);
        }
        [Fact]
        public async Task Should_Call_ClientService_When_resultFromCache_Is_null()
        {
            var _redis = _fixture.Build<RedisSettings>()
                .With(w => w.Enabled, true)
                .Create();

            var _factory = new Mock<IServiceScopeFactory>();

            _serviceProviderMock.Setup(x =>
                    x.GetService(typeof(IDistributedCache<Shop>)))
                .Returns(_distributedCache.Object);

            _serviceScopeMock.Setup(x =>
                x.ServiceProvider).Returns(_serviceProviderMock.Object);

            _factory.Setup(x =>
                x.CreateScope()).Returns(_serviceScopeMock.Object);

            _serviceProviderMock.Setup(x =>
                    x.GetService(typeof(IServiceScopeFactory)))
                .Returns(_factory.Object);

            _distributedCache.Setup(s =>
                    s.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _shopClientServiceMock.Setup(s => 
                    s.GetShopsAsync(default(CancellationToken)))
                .ReturnsAsync(_fixture.Build<BaseResponse<IEnumerable<Shop>>>().Create());


            var shopService = new ShopService(
                _shopClientServiceMock.Object,
                _redis,
                _serviceProviderMock.Object);

            var result = await shopService.GetShopsAsync();

            _distributedCache.Verify(v =>
                v.GetAsync(It.IsAny<string>()), Times.Once);
            _shopClientServiceMock.Verify(v => 
                v.GetShopsAsync(default(CancellationToken)), Times.Once);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Should_Never_Call_GetAsync_Method_From_DistributedCache_When_Reddis_Is_off()
        {
            var _redis = _fixture.Build<RedisSettings>()
                .With(w => w.Enabled, false)
                .Create();

            _shopClientServiceMock.Setup(s => 
                    s.GetShopsAsync(default(CancellationToken)))
                .ReturnsAsync(_fixture.Build<BaseResponse<IEnumerable<Shop>>>().Create());

            var shopService = new ShopService(
                _shopClientServiceMock.Object,
                _redis,
                _serviceProviderMock.Object);

            var result = await shopService.GetShopsAsync();

            _distributedCache.Verify(v => 
                v.GetAsync(It.IsAny<string>()), Times.Never);
            _shopClientServiceMock.Verify(v => 
                v.GetShopsAsync(default(CancellationToken)), Times.Once);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Should_Return_Null_When_ClientService_Returns_null_And_Reddis_Is_off()
        {
            var _redis = _fixture.Build<RedisSettings>()
                .With(w => w.Enabled, false)
                .Create();

            _shopClientServiceMock.Setup(s =>
                    s.GetShopsAsync(default(CancellationToken)))
                .ReturnsAsync(()=>null);

            var shopService = new ShopService(
                _shopClientServiceMock.Object,
                _redis,
                _serviceProviderMock.Object);

            var result = await shopService.GetShopsAsync();

            _distributedCache.Verify(v =>
                v.GetAsync(It.IsAny<string>()), Times.Never);
            _shopClientServiceMock.Verify(v =>
                v.GetShopsAsync(default(CancellationToken)), Times.Once);
            Assert.Null(result.Data);
        }
    }
}
