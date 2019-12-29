using System.ComponentModel.DataAnnotations;

namespace Downtime.Alerter.Entity.Models
{
    public class TargetApplicationNotificationType
    {
        [Key]
        public int Id { get; set; }
        public virtual TargetApplication TargetApplication { get; set; }
        public TypesOfNotification Type { get; set; }
    }
}
