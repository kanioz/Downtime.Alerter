using System;
using Downtime.Alerter.Entity.Models;

namespace Downtime.Alerter.DAL.ViewModels
{
    public class NotificationVm
    {
        public long Id { get; set; }
        public TypesOfNotification NotificationType { get; set; }
        public string MessageTo { get; set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
