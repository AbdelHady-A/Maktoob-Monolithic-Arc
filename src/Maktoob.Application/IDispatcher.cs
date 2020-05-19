using Maktoob.Application.Commands;
using Maktoob.Application.Queries;
using Maktoob.CrossCuttingConcerns.Result;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Application
{
    public interface IDispatcher
    {
        Task<T> DispatchAsync<T>(ICommand<T> command) where T : GResult;
        Task<T> DispatchAsync<T>(IQuery<T> query) where T : class, new();
    }
}
