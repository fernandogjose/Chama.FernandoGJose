using Chama.FernandoGJose.Domain.Share.Commands;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Share.Interfaces.Validations
{
    public interface IValidation
    {
        string Command { get; }

        Task ValidateAsync<T>(T requestCommand) where T : RequestCommand;
    }
}
