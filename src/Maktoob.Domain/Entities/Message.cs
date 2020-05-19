using System;

namespace Maktoob.Domain.Entities
{
    public enum MessageStatus
    {
        READ, DELIVERED
    }
    public class Message
    {
        public DateTime? SentDate { get; set; }
        public DateTime ReadDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public MessageStatus Status { get; set; }
        public Guid SenderId { get; set; }
        public MessageContent Content { get; set; }
    }

    public class MessageContent
    {

    }
}