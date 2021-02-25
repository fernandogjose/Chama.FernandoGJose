using System;

namespace Chama.FernandoGJose.Util.Exceptions
{
    [Serializable]
    public class CapacityOfStudentsByCourseException : Exception
    {
        public CapacityOfStudentsByCourseException() { }

        public CapacityOfStudentsByCourseException(string message) : base(message) { }

        public CapacityOfStudentsByCourseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
