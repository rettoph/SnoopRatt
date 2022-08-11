using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    internal sealed class PaginatorRefreshService : BackgroundService
    {
        private PeriodicTimer _timer;
        private PaginatorService _paginators;
        private PaginatorConfiguration _config;

        public PaginatorRefreshService(PaginatorService paginators, PaginatorConfiguration config)
        {
            _paginators = paginators;
            _config = config;
            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_config.RefreshRate));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                await _paginators.Refresh();
            }
        }
    }
}
