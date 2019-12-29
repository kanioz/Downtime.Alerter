using System;
using System.Threading;
using System.Threading.Tasks;
using Downtime.Alerter.DAL.Interface;
using Downtime.Alerter.DAL.ViewModels;
using Downtime.Alerter.Entity.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;

namespace Downtime.Alerter.Notification.Worker
{
    public class NotificationWorker : BackgroundService
    {
        private readonly ILogger<NotificationWorker> _logger;
        public IServiceProvider Services { get; }
        public NotificationWorker(ILogger<NotificationWorker> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Notification Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    using (var scope = Services.CreateScope())
                    {

                        var notificationService =
                            scope.ServiceProvider.GetRequiredService<INotificationService>();

                        var notifications = notificationService.GetNotificationsToSend();


                        foreach (var notification in notifications)
                        {
                            var result = SendNotification(notification);
                            notificationService.UpdateNotificationStatus(notification.Id, result.Result ? NotificationStatus.Success : NotificationStatus.Fail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error,ex, ex.Message);
                }

                await Task.Delay(60 * 1000, stoppingToken);
            }
        }

        private async Task<bool> SendNotification(NotificationVm notification)
        {
            switch (notification.NotificationType)
            {
                case TypesOfNotification.Email:
                    var result = await SendEmail(notification.MessageTo, notification.Message);
                    return result;
                default:
                    return false;
            }
        }

        private async Task<bool> SendEmail(string emailTo, string message)
        {
            _logger.LogInformation($"An email will be send to {emailTo}.");


            try
            {
                var emailMessage = new MimeMessage();
                var from = new MailboxAddress("Downtime Alerter", "amin@downtime.alerter");
                emailMessage.From.Add(from);
                var to = new MailboxAddress("User", emailTo);
                emailMessage.To.Add(to);
                emailMessage.Subject = "Downtime Alerter - Your application is down!";
                var bodyBuilder = new BodyBuilder { TextBody = message };

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("downtime.alerter@gmail.com", "DontimeAlerter1!");
                    await client.SendAsync(emailMessage);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
            }

            return false;
        }
    }
}
