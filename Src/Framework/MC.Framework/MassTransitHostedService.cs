using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MC.Framework
{
    public class MassTransitHostedService : BackgroundService
    {
        private readonly ILogger<MassTransitHostedService> _logger;
        private readonly IBusControl _busControl;
        public MassTransitHostedService(ILogger<MassTransitHostedService> logger,IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Masstransit stopping");
            await _busControl.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            _logger.LogInformation("Masstransit stopped");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Masstransit starting");
            await _busControl.StartAsync(stoppingToken);
            _logger.LogInformation("Masstransit started");
        }
    }
}
