using System;

namespace Downtime.Alerter.Entity.Models
{
    public class BaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
