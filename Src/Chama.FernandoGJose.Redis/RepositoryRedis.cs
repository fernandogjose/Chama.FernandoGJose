using Chama.FernandoGJose.Domain.Share.Interfaces.Redis;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Chama.FernandoGJose.Redis
{
    // Simple implementation, Refactor it
    public class RepositoryRedis : IRepositoryRedis
    {
        private readonly ConnectionMultiplexer _conexao;

        public RepositoryRedis(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CHAMA-FERNANDOGJOSE-REDIS-CONNECTION");
            if (!string.IsNullOrEmpty(connectionString)) _conexao = ConnectionMultiplexer.Connect(connectionString);
        }

        public string GetValueFromKey(string key)
        {
            var dbRedis = _conexao.GetDatabase();
            return dbRedis.StringGet(key);
        }

        public void SetValueFromKey(string key, string value)
        {
            var dbRedis = _conexao.GetDatabase();
            dbRedis.StringSet(key, value);
        }
    }
}
