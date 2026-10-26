using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public Guid ChatSignture { get; set; }

        public string Text { get; set; } = string.Empty;

        public eNotificationStatus NotficationStatus { get; set; }
        public eAgentType AgentType { get; set; }
        public string? Data { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Store Store { get; set; } = null!;
    }
}
