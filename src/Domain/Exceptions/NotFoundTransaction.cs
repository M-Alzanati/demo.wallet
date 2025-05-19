using System;

namespace Domain.Exceptions
{
    public class NotFoundTransaction : Exception
    {
        public NotFoundTransaction(string message) : base(message)
        {
        }

        public NotFoundTransaction(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
