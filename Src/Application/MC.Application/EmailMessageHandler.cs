using MassTransit;
using MC.Application.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MC.Application
{
    public class EmailMessageHandler : IConsumer<IEmailMessage>
    {
        private readonly ILogger _logger;
        public EmailMessageHandler(ILogger<EmailMessageHandler> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IEmailMessage> context)
        {
            _logger.LogInformation(context.Message.Message);

            await context.ConsumeCompleted;
        }
    }
}
