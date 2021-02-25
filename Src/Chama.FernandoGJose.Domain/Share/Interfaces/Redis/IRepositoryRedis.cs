namespace Chama.FernandoGJose.Domain.Share.Interfaces.Redis
{
    public interface IRepositoryRedis
    {
        string GetValueFromKey(string key);

        void SetValueFromKey(string key, string value);
    }
}
