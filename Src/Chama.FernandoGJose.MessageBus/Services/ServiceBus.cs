using Chama.FernandoGJose.Domain.Share.Interfaces.MessageBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.MessageBus.Services
{
    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuration;

        public ServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishAsync(string eventName, string message)
        {
            var queueClient = new QueueClient(_configuration.GetConnectionString("CHAMA-FERNANDOGJOSE-SERVICEBUS-CONNECTION"), eventName);
            var messageToPublish = new Message(Encoding.UTF8.GetBytes(message));
            await queueClient.SendAsync(messageToPublish);
        }
    }
}
