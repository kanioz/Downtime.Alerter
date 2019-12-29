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
    public class TargetApplicationService : ServiceBase, ITargetApplicationService
    {
        public TargetApplicationService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<TargetApplicationVm> GetApplicationsToMonitor()
        {
            var result = DbContext.TargetApplications
                .Include(c => c.NotificationTypes)
                .Include(c => c.Owner)
                .Where(c => c.NextMonitoringTime <= DateTime.Now)
                .Select(f =>
                    new TargetApplicationVm
                    {
                        Id = f.Id,
                        Interval = f.MonitoringInterval,
                        LastMonitorTime = f.LastMonitoringTime,
                        NextMonitorTime = f.NextMonitoringTime,
                        Name = f.ApplicationName,
                        Url = f.ApplicationUrl,
                        UserId = f.Owner.Id,
                        UserName = f.Owner.UserName,
                        NotificationTypes = f.NotificationTypes.Select(c => c.Type).ToList()
                    }).ToList();
            return result;
        }

        public List<TargetApplicationVm> GetTargetApplicationsOfUser(string userId)
        {
            var result = DbContext.TargetApplications
                .Include(c => c.NotificationTypes)
                .Include(c => c.Owner)
                .Where(c => c.Owner.Id == userId)
                .Select(f =>
                    new TargetApplicationVm
                    {
                        Id = f.Id,
                        Interval = f.MonitoringInterval,
                        LastMonitorTime = f.LastMonitoringTime,
                        NextMonitorTime = f.NextMonitoringTime,
                        CreateDate = f.CreateDate,
                        UpdateDate = f.UpdateDate,
                        Name = f.ApplicationName,
                        Url = f.ApplicationUrl,
                        UserId = f.Owner.Id,
                        UserName = f.Owner.UserName,
                        NotificationTypes = f.NotificationTypes.Select(c => c.Type).ToList()
                    }).ToList();
            return result;
        }

        public TargetApplicationVm GeTargetApplicationById(int id, string userId)
        {

            var ta = DbContext.TargetApplications
                .Include(c => c.NotificationTypes)
                .Include(c => c.Owner)
                .Include(c => c.MonitoringHistories)
                .FirstOrDefault(c => c.Id == id && c.Owner.Id == userId);
            if (ta != null)
            {
                var result = new TargetApplicationVm
                {
                    Id = ta.Id,
                    Interval = ta.MonitoringInterval,
                    LastMonitorTime = ta.LastMonitoringTime,
                    NextMonitorTime = ta.NextMonitoringTime,
                    CreateDate = ta.CreateDate,
                    UpdateDate = ta.UpdateDate,
                    Name = ta.ApplicationName,
                    Url = ta.ApplicationUrl,
                    UserId = ta.Owner.Id,
                    UserName = ta.Owner.UserName,
                    NotificationTypes = ta.NotificationTypes.Select(c => c.Type).ToList(),
                    MonitoringHistories = ta.MonitoringHistories.Select(c => new MonitoringHistoryVm
                    {
                        Id = c.Id,
                        IsUp = c.IsUp,
                        StatusCode = c.StatusCode,
                        MonitorTime = c.MonitoringTime
                    }).ToList()
                };
                return result;
            }

            return null;

        }

        public TargetApplicationVm AddEditTargetApplication(TargetApplicationVm model)
        {

            var ta = new TargetApplication();

            if (model.Id > 0)
            {
                ta = DbContext.TargetApplications
                    .Include(c => c.NotificationTypes)
                    .Include(c => c.Owner)
                    .FirstOrDefault(c => c.Id == model.Id);
            }

            if (ta != null && ta.Id > 0)
            {
                if (ta.Owner.Id != model.UserId)
                    throw new Exception("You can not update Application does not belong to you!");

                if (ta.MonitoringInterval != model.Interval)
                {
                    ta.NextMonitoringTime = IntervalCalculator.GetNexMonitoringDate(model.Interval);
                    ta.MonitoringInterval = model.Interval;
                }
                ta.UpdateDate = DateTime.Now;
                ta.NotificationTypes?.Clear();
            }
            else
            {
                ta = new TargetApplication
                {
                    NextMonitoringTime = IntervalCalculator.GetNexMonitoringDate(model.Interval),
                    MonitoringInterval = model.Interval,
                    LastMonitoringTime = DateTime.Now,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Owner = DbContext.Users.SingleOrDefault(c => c.Id == model.UserId),
                    NotificationTypes = new List<TargetApplicationNotificationType>()
                };
            }

            ta.ApplicationName = model.Name;
            ta.ApplicationUrl = model.Url;

            if (model.NotificationTypes != null && model.NotificationTypes.Count > 0)
            {
                if (ta.NotificationTypes == null)
                    ta.NotificationTypes = new List<TargetApplicationNotificationType>();
                foreach (var nt in model.NotificationTypes)
                {
                    ta.NotificationTypes.Add(new TargetApplicationNotificationType
                    {
                        Type = nt,
                    });
                }
            }


            if (model.Id == 0)
            {
                DbContext.TargetApplications.Add(ta);
            }

            DbContext.SaveChanges();





            return model;

        }

        public bool DeleteTargetApplicationById(int id, string userId)
        {

            var ta = DbContext.TargetApplications
                .Include(c => c.Owner)
                .Include(c => c.MonitoringHistories)
                .FirstOrDefault(c => c.Id == id && c.Owner.Id == userId);
            if (ta != null)
            {
                foreach (var mh in ta.MonitoringHistories)
                {
                    mh.Notifications?.Clear();
                }
                ta.MonitoringHistories?.Clear();
                ta.NotificationTypes?.Clear();
                DbContext.TargetApplications.Remove(ta);
                DbContext.SaveChanges();
                return true;
            }

            return false;


        }
    }
}
