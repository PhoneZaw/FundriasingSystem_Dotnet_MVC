using System;

namespace FundraisingApp.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() { }

        public DomainException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
