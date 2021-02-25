using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Share.Interfaces.MessageBus
{
    public interface IServiceBus
    {
        Task PublishAsync(string eventName, string message);
    }
}
