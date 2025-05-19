using System;

namespace Domain.Exceptions
{
    public class NotFoundTransactionException : Exception
    {
        public NotFoundTransactionException(string message) : base(message)
        {
        }

        public NotFoundTransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
