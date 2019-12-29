using System;
using System.Collections.Generic;
using System.Text;

namespace Downtime.Alerter.DAL.ViewModels
{
    public class MonitoringHistoryVm
    {
        public long Id { get; set; }
        public bool IsUp { get; set; }
        public string StatusCode { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationUrl { get; set; }
        public int ApplicationId { get; set; }
        public DateTime MonitorTime { get; set; }
        public List<NotificationVm> Notifications { get; set; }
    }
}
