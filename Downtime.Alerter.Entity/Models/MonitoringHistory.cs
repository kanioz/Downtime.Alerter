using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Downtime.Alerter.Entity.Models
{
    public class MonitoringHistory
    {
        [Key]
        public long Id { get; set; }
        public DateTime MonitoringTime { get; set; }
        public bool IsUp { get; set; }
        public string StatusCode { get; set; }
        public virtual TargetApplication TargetApplication { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
