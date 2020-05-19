using System;

namespace Maktoob.Domain.Entities
{
    public class DirectChat : Chat
    {
        public DirectChat(Guid peerId1, Guid peerId2)
        {
            PeerId1 = peerId1;
            PeerId2 = peerId2;
        }

        public Guid PeerId1 { get; private set; }
        public Guid PeerId2 { get; private set; }
    }
}