using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maktoob.Domain.Entities
{
    public abstract class Entity<TKey> : IHasKey<TKey>, ITrackable
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public abstract class EntityHasName<TKey> : Entity<TKey>, IHasName
        where TKey : IEquatable<TKey>
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
