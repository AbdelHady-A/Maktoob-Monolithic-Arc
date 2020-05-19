using Maktoob.CrossCuttingConcerns.Result;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Application.Commands
{
    public interface ICommandHandler<in TCommand, TResult> 
        where TCommand : ICommand<TResult>
        where TResult : GResult
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
