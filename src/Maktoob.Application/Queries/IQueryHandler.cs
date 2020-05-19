using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Application.Queries
{
    public interface IQueryHandler<in TQuery, TResult> 
        where TResult : class, new() 
        where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
