using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.CrossCuttingConcerns.Exceptions
{
    public class InvalidSpecificationException : ApplicationException
    {
        public InvalidSpecificationException(string message) : base(message)
        {
        }

        public InvalidSpecificationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
