using System;
using Downtime.Alerter.DAL.Implementation;
using Downtime.Alerter.DAL.Interface;
using Downtime.Alerter.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace Downtime.Alerter.Monitor.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/monitor.worker.log.ndjson")
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseWindowsService() // this is for windows service
                //.UseSystemd() // this is for linux service
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(
                            configuration.GetConnectionString("DefaultConnection")));

                    services.AddTransient<ITargetApplicationService, TargetApplicationService>();
                    services.AddTransient<IMonitoringHistoryService, MonitoringHistoryService>();

                    services.AddHostedService<MonitoringWorker>();

                });
    }
}
