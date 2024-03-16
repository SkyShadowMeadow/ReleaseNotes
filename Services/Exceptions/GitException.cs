using System.Runtime.Serialization;

namespace Services.Exceptions
{
    public class GitException : Exception
    {
        public GitException()
        {
        }

        public GitException(string? message) : base(message)
        {
        }

        public GitException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}