using Maktoob.Application.Commands;
using Maktoob.CrossCuttingConcerns.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Application.Decorators
{
    public class AuditLoggingDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
        where TResult : GResult
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;

        public AuditLoggingDecorator(ICommandHandler<TCommand, TResult> handler)
        {
            _handler = handler;
        }
        public async Task<TResult> HandleAsync(TCommand command)
        {
            string commandJson = JsonConvert.SerializeObject(command);
            Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");
            var result = await _handler.HandleAsync(command);

            return result;
        }
    }
}
