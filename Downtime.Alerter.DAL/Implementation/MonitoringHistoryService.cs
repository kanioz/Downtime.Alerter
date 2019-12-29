using System;
using System.Collections.Generic;
using System.Linq;
using Downtime.Alerter.DAL.Helper;
using Downtime.Alerter.DAL.Interface;
using Downtime.Alerter.DAL.ViewModels;
using Downtime.Alerter.Entity;
using Downtime.Alerter.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Downtime.Alerter.DAL.Implementation
{
    public class MonitoringHistoryService : ServiceBase, IMonitoringHistoryService
    {
        public MonitoringHistoryService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public void CreateMonitoringHistory(MonitoringHistoryVm model)
        {
            var ta = DbContext.TargetApplications
                .Include(c => c.NotificationTypes)
                .Include(c => c.Owner)
                .FirstOrDefault(c => c.Id == model.ApplicationId);
            if (ta != null)
            {
                ta.NextMonitoringTime = IntervalCalculator.GetNexMonitoringDate(ta.MonitoringInterval);
                var mh = new MonitoringHistory
                {
                    IsUp = model.IsUp,
                    MonitoringTime = DateTime.Now,
                    StatusCode = model.StatusCode,
                    TargetApplication = ta
                };
                if (!mh.IsUp)
                {
                    mh.Notifications = ta.NotificationTypes.Select(c => new Notification
                    {
                        NotificationType = c.Type,
                        CreateDate = DateTime.Now,
                        Message = $"Dear User, your application \"{ta.ApplicationName}\" is down!",
                        Status = NotificationStatus.NotSend,
                        UpdateDate = DateTime.Now
                    }).ToList();
                }
                if (ta.MonitoringHistories == null)
                    ta.MonitoringHistories = new List<MonitoringHistory>();
                ta.MonitoringHistories.Add(mh);
                DbContext.SaveChanges();
            }
        }

        public MonitoringHistoryVm GetMonitoringHistoryDetail(long monitoringId, string userId)
        {
            var result = DbContext.MonitoringHistories
                .Include(c => c.TargetApplication).ThenInclude(f=> f.Owner)
                .Include(c => c.Notifications)
                .Where(c => c.Id == monitoringId && c.TargetApplication.Owner.Id == userId)
                .Select(c => new MonitoringHistoryVm
                {
                    Id = c.Id,
                    IsUp = c.IsUp,
                    StatusCode = c.StatusCode,
                    ApplicationName = c.TargetApplication.ApplicationName,
                    ApplicationUrl = c.TargetApplication.ApplicationUrl,
                    ApplicationId = c.TargetApplication.Id,
                    MonitorTime = c.MonitoringTime,
                    Notifications = c.Notifications.Select(f=> new NotificationVm
                    {
                        NotificationType = f.NotificationType,
                        Message = f.Message,
                        Status = f.Status,
                        CreateDate = f.CreateDate,
                        UpdateDate = f.UpdateDate
                    }).ToList()
                }).FirstOrDefault();
            return result;
        }
    }
}
