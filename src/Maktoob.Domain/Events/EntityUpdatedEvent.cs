using Maktoob.Domain.Entities;
using System;

namespace Maktoob.Domain.Events
{
    public class EntityUpdatedEvent<TEntity> : IDomainEvent
        where TEntity : Entity<Guid>
    {
        public EntityUpdatedEvent(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; }
    }
}
