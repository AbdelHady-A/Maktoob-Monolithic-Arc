using Maktoob.Application.Commands;
using Maktoob.CrossCuttingConcerns.Result;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Application.Decorators
{
    class DatabaseRetryDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
        where TResult : GResult
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;

        public DatabaseRetryDecorator(ICommandHandler<TCommand, TResult> handler)
        {
            _handler = handler;
        }
        public async Task<TResult> HandleAsync(TCommand command)
        {
            var result = await _handler.HandleAsync(command);

            return result;
        }
    }
}
