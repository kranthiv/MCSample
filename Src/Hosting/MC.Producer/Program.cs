using MassTransit;
using MC.Application.Contracts;
using MC.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace MC.Producer
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
               .UseConsoleLifetime()
               .Build();

            var bus = host.Services.GetRequiredService<IBusControl>();
            await bus.StartAsync();

            await bus.Publish<IEmailMessage>(new EmailMessage { Message = "sample message"}).ConfigureAwait(false);

            await host.RunAsync()
                .ConfigureAwait(false);

            await bus.StopAsync();
        }

        private static void ConfigBus(IServiceCollection services)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.AddBus(sp =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(configurator =>
                    {
                        var host = configurator.Host("localhost", "/", x =>
                        {
                            x.Password("guest");
                            x.Username("guest");
                        });
                    });
                });
            });
        }
    }
}
