using Maktoob.Application.Commands;
using Maktoob.Application.Queries;
using Maktoob.CrossCuttingConcerns.Result;
using System;
using System.Threading.Tasks;

namespace Maktoob.Application
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _provider;

        public Dispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }


        public async Task<T> DispatchAsync<T>(ICommand<T> command) where T : GResult
        {
            Type type = typeof(ICommandHandler<,>);
            Type[] typeArgs = { command.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            T result = await handler.HandleAsync((dynamic)command);

            return result;
        }

        public async Task<T> DispatchAsync<T>(IQuery<T> query) where T : class, new()
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            T result = await handler.HandleAsync((dynamic)query);

            return result;
        }
    }
}