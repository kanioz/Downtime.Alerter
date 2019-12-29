using System;
using System.Collections.Generic;
using System.Text;
using Downtime.Alerter.Entity;

namespace Downtime.Alerter.DAL.Interface
{
    public class ServiceBase
    {
        protected ApplicationDbContext DbContext;

        public ServiceBase(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
