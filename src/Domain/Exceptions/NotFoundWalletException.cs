using System;

namespace Domain.Exceptions
{
    public class NotFoundWalletException : Exception
    {
        public NotFoundWalletException() { }
        public NotFoundWalletException(string message) : base(message) { }
    }
}
