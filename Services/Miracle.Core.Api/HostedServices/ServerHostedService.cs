using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miracle.Core.Api.Services.UserWatch;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Miracle.Core.Api.HostedServices
{
    public class ServerHostedService : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IServiceScopeFactory scopeFactory;

        public ServerHostedService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(1.5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userWatchService = scope.ServiceProvider.GetRequiredService<IUserWatchService>();
                userWatchService.CheckOnlineStates();
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
