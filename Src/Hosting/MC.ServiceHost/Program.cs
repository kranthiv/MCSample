using MC.Framework;
using MC.Framework.WindowsService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MC.ServiceHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<MassTransitHostedService>();
                })
                .UseWindowsService()
                .Build();

            await host.RunAsync().
                ConfigureAwait(false);

        }
    }
}
