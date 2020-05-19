using System;
using System.Collections.Generic;

namespace Maktoob.Domain.Entities
{
    public class ChatGroup : Chat
    {
        public string Name { get; set; }
        public IEnumerable<ChatMember> Members { get; set; }
        public string Description { get; set; }
        public Photo CoverPhoto { get; set; }
    }
}