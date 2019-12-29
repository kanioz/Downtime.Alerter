using System;
using Downtime.Alerter.Entity.Models;

namespace Downtime.Alerter.DAL.Helper
{
    public static class IntervalCalculator
    {
        public static DateTime GetNexMonitoringDate(MonitoringInterval intervalType)
        {  
            switch (intervalType)
            {
                case MonitoringInterval.Hourly:
                    return DateTime.Now.AddHours(1);
                case MonitoringInterval.Daily:
                    return DateTime.Now.AddDays(1);
                case MonitoringInterval.Weekly:
                    return DateTime.Now.AddDays(7);
                case MonitoringInterval.Monthly:
                    return DateTime.Now.AddMonths(1);
                default:
                    return DateTime.Now.AddHours(1);
            }
        }
    }
}
