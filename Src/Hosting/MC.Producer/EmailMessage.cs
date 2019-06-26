using MC.Application.Contracts;

namespace MC.Producer
{
    public class EmailMessage : IEmailMessage
    {
        public string Message { get; set; }
    }
}
