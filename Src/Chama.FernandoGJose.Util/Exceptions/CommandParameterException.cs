using System;

namespace Chama.FernandoGJose.Util.Exceptions
{
    [Serializable]
    public class CommandParameterException : Exception
    {
        public CommandParameterException() { }

        public CommandParameterException(string message) : base(message) { }

        public CommandParameterException(string message, Exception innerException) : base(message, innerException) { }
    }
}
