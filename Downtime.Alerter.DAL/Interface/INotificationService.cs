using System.Collections.Generic;
using Downtime.Alerter.DAL.ViewModels;
using Downtime.Alerter.Entity.Models;

namespace Downtime.Alerter.DAL.Interface
{
    public interface INotificationService
    {
        List<NotificationVm> GetNotificationsToSend();
        bool UpdateNotificationStatus(long id, NotificationStatus status);
    }
}
