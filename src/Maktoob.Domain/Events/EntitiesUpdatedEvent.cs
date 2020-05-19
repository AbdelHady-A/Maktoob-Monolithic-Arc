using Maktoob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Events
{
    public class EntitiesUpdatedEvent<TEntity> : IDomainEvent
        where TEntity : Entity<Guid>
    {
        public EntitiesUpdatedEvent(IEnumerable<TEntity> entities)
        {
            Entities = entities;
        }

        public IEnumerable<TEntity> Entities { get; set; }
    }
}
