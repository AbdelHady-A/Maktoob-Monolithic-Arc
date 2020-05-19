using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Application.Queries
{
    public interface IQuery<out TResult> where TResult : class
    {
    }
}
