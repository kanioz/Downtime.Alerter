using System.ComponentModel.DataAnnotations;

namespace Downtime.Alerter.Entity.Models
{
    public class Notification : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public TypesOfNotification NotificationType { get; set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public virtual MonitoringHistory MonitoringHistory { get; set; }
    }
}
