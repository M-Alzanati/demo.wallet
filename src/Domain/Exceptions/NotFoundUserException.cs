using System;

namespace Domain.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException() { }
        public NotFoundUserException(string message) : base(message) { }
    }
}
