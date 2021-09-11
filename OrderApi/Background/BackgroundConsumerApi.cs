using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace OrderApi.Background
{
    public class BackgroundConsumerApi : BackgroundService
    {
        public BackgroundConsumerApi()
        {
                
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
