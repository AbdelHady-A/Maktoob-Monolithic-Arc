using Maktoob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Events
{
    public class EntityCreatedEvent<TEntity> : IDomainEvent
        where TEntity : Entity<Guid>
    {
        public EntityCreatedEvent(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; }
    }
}
