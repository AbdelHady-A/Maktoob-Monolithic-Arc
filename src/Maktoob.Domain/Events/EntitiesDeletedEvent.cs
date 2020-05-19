using Maktoob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Events
{
    public class EntitiesDeletedEvent<TEntity> : IDomainEvent
        where TEntity : Entity<Guid>
    {
        public EntitiesDeletedEvent(IEnumerable<TEntity> entities)
        {
            Entities = entities;
        }

        public IEnumerable<TEntity> Entities { get; }

    }
}
