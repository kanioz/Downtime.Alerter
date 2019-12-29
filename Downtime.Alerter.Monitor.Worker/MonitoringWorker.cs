using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Downtime.Alerter.DAL.Interface;
using Downtime.Alerter.DAL.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Downtime.Alerter.Monitor.Worker
{
    public class MonitoringWorker : BackgroundService
    {
        private readonly ILogger<MonitoringWorker> _logger;
        public IServiceProvider Services { get; }

        public MonitoringWorker(IServiceProvider services, ILogger<MonitoringWorker> logger)
        {
            _logger = logger;
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Monitoring Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    using (var scope = Services.CreateScope())
                    {
                        var targetApplicationService =
                            scope.ServiceProvider.GetRequiredService<ITargetApplicationService>();

                        var monitoringService =
                            scope.ServiceProvider.GetRequiredService<IMonitoringHistoryService>();

                        var applications = targetApplicationService.GetApplicationsToMonitor();

                        foreach (var app in applications)
                        {
                            var result = await MonitorApplication(app);

                            monitoringService.CreateMonitoringHistory(result);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, ex.Message);
                }

                await Task.Delay(60 * 1000, stoppingToken);
            }
        }
        private async Task<MonitoringHistoryVm> MonitorApplication(TargetApplicationVm application)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(application.Url);
                    return new MonitoringHistoryVm
                    {
                        ApplicationId = application.Id,
                        IsUp = (int)response.StatusCode < 300 && (int)response.StatusCode >= 200,
                        StatusCode = ((int)response.StatusCode).ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return new MonitoringHistoryVm
                {
                    ApplicationId = application.Id,
                    IsUp = false,
                    StatusCode = "Error"
                };
            }

        }
    }
}
