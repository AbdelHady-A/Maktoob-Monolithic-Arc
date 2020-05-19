using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.CrossCuttingConcerns.Exceptions
{
    public class InvalidPasswordHashException : ApplicationException
    {
        public InvalidPasswordHashException(string message) : base(message)
        {
        }

        public InvalidPasswordHashException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
