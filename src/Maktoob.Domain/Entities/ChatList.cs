using System;
using System.Collections.Generic;

namespace Maktoob.Domain.Entities
{
    public class ChatList : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
    }
}