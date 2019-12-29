using System.Collections.Generic;
using Downtime.Alerter.DAL.ViewModels;

namespace Downtime.Alerter.DAL.Interface
{
    public interface ITargetApplicationService
    {
        List<TargetApplicationVm> GetApplicationsToMonitor();
        List<TargetApplicationVm> GetTargetApplicationsOfUser(string userId);
        TargetApplicationVm GeTargetApplicationById(int id, string userId);
        TargetApplicationVm AddEditTargetApplication(TargetApplicationVm model);
        bool DeleteTargetApplicationById(int id, string userId);
    }
}
