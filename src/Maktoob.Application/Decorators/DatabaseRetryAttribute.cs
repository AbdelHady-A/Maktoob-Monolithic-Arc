using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Application.Decorators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    class DatabaseRetryAttribute : Attribute
    {
    }
}
