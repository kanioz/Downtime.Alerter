using System;
using System.Collections.Generic;
using System.Text;
using Downtime.Alerter.Entity.Models;

namespace Downtime.Alerter.DAL.ViewModels
{
    public class TargetApplicationVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public MonitoringInterval Interval { get; set; }
        public DateTime LastMonitorTime { get; set; }
        public DateTime NextMonitorTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<TypesOfNotification> NotificationTypes { get; set; }
        public List<MonitoringHistoryVm> MonitoringHistories{ get; set; }
    }
}
