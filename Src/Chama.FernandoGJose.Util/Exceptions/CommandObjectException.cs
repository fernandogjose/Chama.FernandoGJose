using System;

namespace Chama.FernandoGJose.Util.Exceptions
{
    [Serializable]
    public class CommandObjectException : Exception
    {
        public CommandObjectException() { }

        public CommandObjectException(string name) : base(String.Format("Invalid command object: {0}", name)) { }
    }
}
