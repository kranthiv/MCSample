using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MC.Framework
{
    public class MassTransitHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;
        private readonly IBusControl _busControl;
        public MassTransitHostedService(ILogger<MassTransitHostedService> logger,IApplicationLifetime appLifetime,IBusControl busControl)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _busControl = busControl;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);
            await _busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Perform post-startup activities here
        /// </summary>
        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
        }

        /// <summary>
        /// Perform on-stopping activities here
        /// </summary>
        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
        }

        /// <summary>
        /// Perform post-stopped activities here
        /// </summary>
        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
