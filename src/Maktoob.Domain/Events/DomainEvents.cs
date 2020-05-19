using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Maktoob.Domain.Events
{
    public static class DomainEvents
    {
        private static List<Type> _handlers = new List<Type>();
        private static IServiceProvider _serviceProvider;

        public static void RgisterHandlers(Assembly assembly, IServiceProvider serviceProvider)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(
                    i => i.IsGenericType 
                    && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
                .ToList();

            _handlers.AddRange(types);
            _serviceProvider = serviceProvider;
        }

        public static void Dispatch(IDomainEvent domainEvent)
        {
            foreach(Type handlerType in _handlers)
            {
                bool canHandleEvent = handlerType.GetInterfaces()
                    .Any(t => t.IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                        && t.GenericTypeArguments[0] == domainEvent.GetType());
                if (canHandleEvent)
                {
                    dynamic handler = _serviceProvider.GetService(handlerType);
                    handler.Handle((dynamic)domainEvent);
                }
            }
        }
    }
}
