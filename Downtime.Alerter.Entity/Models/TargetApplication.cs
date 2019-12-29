using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Downtime.Alerter.Entity.Models
{
    public class TargetApplication : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationUrl { get; set; }
        public MonitoringInterval MonitoringInterval { get; set; }
        public DateTime LastMonitoringTime { get; set; }
        public DateTime NextMonitoringTime { get; set; }
        public virtual IdentityUser Owner { get; set; }
        public virtual ICollection<MonitoringHistory> MonitoringHistories { get; set; }
        public virtual ICollection<TargetApplicationNotificationType> NotificationTypes { get; set; }
    }
}
