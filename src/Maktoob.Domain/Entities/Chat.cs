using System;
using System.Collections;
using System.Collections.Generic;

namespace Maktoob.Domain.Entities
{
    public class Chat : Entity<Guid>
    {
        public IEnumerable<Message> Messages { get; set; }
    }
}