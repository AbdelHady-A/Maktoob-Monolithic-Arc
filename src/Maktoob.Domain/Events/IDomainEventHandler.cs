using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Events
{
    public interface IDomainEventHandler<T>
        where T : IDomainEvent
    {
        Task HandleAsync(T domainEvent);
    }
}
