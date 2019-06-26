using MassTransit;
using MC.Application;
using MC.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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
                    ConfigBus(services);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                })
                .UseConsoleLifetime();

            await host.RunConsoleAsync()
                .ConfigureAwait(false);

        }

        private static void ConfigBus(IServiceCollection services)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<EmailMessageHandler>();
                cfg.AddBus(sp =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(configurator =>
                    {
                        var host = configurator.Host("localhost", "/", x =>
                                    {
                                        x.Password("guest");
                                        x.Username("guest");
                                    });

                        configurator.ReceiveEndpoint("Comm", ep =>
                        {
                            ep.Consumer<EmailMessageHandler>(sp);
                        });
                    });
                });
            });
        }
    }
}
