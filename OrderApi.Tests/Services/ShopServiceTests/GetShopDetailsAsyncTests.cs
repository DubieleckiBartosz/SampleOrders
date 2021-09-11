using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using OrderApi.Models;
using OrderApi.Services;
using OrderApi.Wrappers;
using Xunit;

namespace OrderApi.Tests.Services.ShopServiceTests
{
    public class GetShopDetailsAsyncTests: ShopServiceBaseTests
    {
        [Fact]
        public async Task Should_Not_Return_Null()
        {

            _shopClientServiceMock.Setup(s => 
                    s.GetShopDetailsAsync(It.IsAny<Guid>(), default(CancellationToken)))
                .ReturnsAsync(_fixture.Build<BaseResponse<ShopDetails>>().Create());
         
            var shopService = new ShopService(
                _shopClientServiceMock.Object,
                _redisSettingsMock.Object,
                _serviceProviderMock.Object);

            var result =await shopService.GetShopDetailsAsync(Guid.NewGuid());
            Assert.NotNull(result);
        }
    }
}
