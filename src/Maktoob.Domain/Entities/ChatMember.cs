using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Entities
{
    public enum ChatMemberRole
    {
        ADMIN, MEMBER
    }
    public class ChatMember
    {
        public Guid UserId { get; set; }
        public ChatMemberRole Role { get; set; }
    }
}
