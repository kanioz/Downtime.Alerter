using System;
using System.Collections.Generic;
using System.Linq;
using Downtime.Alerter.DAL.Interface;
using Downtime.Alerter.DAL.ViewModels;
using Downtime.Alerter.Entity;
using Downtime.Alerter.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Downtime.Alerter.DAL.Implementation
{
    public class NotificationService : ServiceBase, INotificationService
    {
        public NotificationService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<NotificationVm> GetNotificationsToSend()
        {

            var result = DbContext.Notifications
                .Include(c => c.MonitoringHistory).ThenInclude(e=> e.TargetApplication).ThenInclude(f=> f.Owner)
                .Where(c => c.Status == NotificationStatus.NotSend)
                .Select(c => new NotificationVm
                {
                    Id = c.Id,
                    NotificationType = c.NotificationType,
                    Message = c.Message,
                    MessageTo = c.MonitoringHistory.TargetApplication.Owner.UserName
                }).ToList();
            return result;



        }

        public bool UpdateNotificationStatus(long id, NotificationStatus status)
        {

            var notification = DbContext.Notifications.FirstOrDefault(c => c.Id == id);
            if (notification != null)
            {
                notification.Status = status;
                notification.UpdateDate = DateTime.Now;
                var result = DbContext.SaveChanges();
                return result > 0;
            }

            return false;

        }
    }
}
