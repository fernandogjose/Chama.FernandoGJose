namespace Chama.FernandoGJose.Domain.Share.Commands
{
    public class ResponseCommand
    {
        public bool Success { get; }

        public object Object { get; }

        public ResponseCommand(bool success, object obj)
        {
            Success = success;
            Object = obj;
        }
    }
}
