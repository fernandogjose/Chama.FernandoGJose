using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Share.Interfaces.Emails
{
    public interface ISendEmail
    {
        string TypeEmail { get; }

        Task Send(string subject, string email, string name, Dictionary<string, string> parameter);
    }
}
