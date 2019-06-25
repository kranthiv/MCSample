using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace MC.Framework.WindowsService
{
    /// <summary>
    /// Extension methods for setting up WindowsServiceLifetime.
    /// </summary>
    public static class WindowsServiceLifetimeHostBuilderExtensions
    {
        /// <summary>
        /// Sets the host lifetime to WindowsServiceLifetime, sets the Content Root,
        /// and enables logging to the event log with the application name as the default source name.
        /// </summary>
        /// <remarks>
        /// This is context aware and will only activate if it detects the process is running
        /// as a Windows Service.
        /// </remarks>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseWindowsService(this IHostBuilder hostBuilder)
        {
                // Host.CreateDefaultBuilder uses CurrentDirectory for VS scenarios, but CurrentDirectory for services is c:\Windows\System32.
                hostBuilder.UseContentRoot(AppContext.BaseDirectory);
                hostBuilder.ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddEventLog();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostLifetime, WindowsServiceLifetime>();
                    services.Configure<EventLogSettings>(settings =>
                    {
                        if (string.IsNullOrEmpty(settings.SourceName))
                        {
                            settings.SourceName = hostContext.HostingEnvironment.ApplicationName;
                        }
                    });
                });
            return hostBuilder;
        }
    }
}
