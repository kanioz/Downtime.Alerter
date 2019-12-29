using Downtime.Alerter.DAL.ViewModels;

namespace Downtime.Alerter.DAL.Interface
{
    public interface IMonitoringHistoryService
    {
        void CreateMonitoringHistory(MonitoringHistoryVm model);
        MonitoringHistoryVm GetMonitoringHistoryDetail(long monitoringId, string userId);
    }
}
