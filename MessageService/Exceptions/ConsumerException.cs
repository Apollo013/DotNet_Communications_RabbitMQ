using System;

namespace MessageService.Exceptions
{
    public class ConsumerException : Exception
    {
        public ConsumerException()
        { }

        public ConsumerException(string message) : base(message)
        { }

        public ConsumerException(string message, Exception inner) : base(message, inner)
        { }
    }
}
