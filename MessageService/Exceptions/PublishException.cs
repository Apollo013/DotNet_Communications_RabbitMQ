using System;

namespace MessageService.Exceptions
{
    public class PublishException : Exception
    {
        public PublishException()
        { }

        public PublishException(string message) : base(message)
        { }

        public PublishException(string message, Exception inner) : base(message, inner)
        { }
    }
}
