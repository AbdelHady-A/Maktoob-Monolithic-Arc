using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Entities
{
    public interface IHasKey<T> where T : IEquatable<T>
    {
        T Id { get; set; }
    }
}
