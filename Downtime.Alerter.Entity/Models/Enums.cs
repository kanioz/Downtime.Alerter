namespace Downtime.Alerter.Entity.Models
{
    public enum NotificationStatus
    {
        NotSend,
        Success,
        Fail
    }
    public enum MonitoringInterval
    {
        Hourly,
        Daily,
        Weekly,
        Monthly
    }

    public enum TypesOfNotification
    {
        Email
    }
}
