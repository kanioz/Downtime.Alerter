using Downtime.Alerter.Entity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Downtime.Alerter.Entity
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        

        public virtual DbSet<TargetApplication> TargetApplications { get; set; }
        public virtual DbSet<MonitoringHistory> MonitoringHistories { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<TargetApplicationNotificationType> TargetApplicationNotificationTypes { get; set; }

    }
}
