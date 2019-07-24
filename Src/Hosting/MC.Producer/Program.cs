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
            
            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host("localhost", "/", x =>
                {
                    x.Password("guest");
                    x.Username("guest");
                });
            });

            await bus.StartAsync();

            for (int i = 0; i < 10; i++)
            {
                await bus.Publish<IEmailMessage>(new EmailMessage { Message = "sample message" }).ConfigureAwait(false);
            }

            await bus.StopAsync();
        }
    }
}
