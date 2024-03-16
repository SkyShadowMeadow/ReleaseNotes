using System.Runtime.Serialization;

namespace Services.Exceptions
{
    internal class OpenAIException : Exception
    {
        public OpenAIException()
        {
        }

        public OpenAIException(string? message) : base(message)
        {
        }

        public OpenAIException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OpenAIException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}