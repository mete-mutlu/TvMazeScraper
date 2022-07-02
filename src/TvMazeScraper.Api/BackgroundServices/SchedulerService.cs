using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TvMazeScraper.Api.BackgroundServices
{
    public class SchedulerService : BackgroundService
    {
        private readonly ILogger<SchedulerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SchedulerService(ILogger<SchedulerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool initiallyExecuted = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var scopedSchedulerService = scope.ServiceProvider.GetRequiredService<IScraperBackgroundService>();
                if (!initiallyExecuted)
                {
                    await scopedSchedulerService.ExecuteAsync(stoppingToken);
                    initiallyExecuted = true;
                }

                //Every 3 hours 
                await WaitForNextSchedule("0 */3 * * *");
                await scopedSchedulerService.ExecuteAsync(stoppingToken);
            }
        }

        private async Task WaitForNextSchedule(string cronExpression)
        {
            var parsedExp = CronExpression.Parse(cronExpression);
            var currentUtcTime = DateTimeOffset.UtcNow.UtcDateTime;
            var occurenceTime = parsedExp.GetNextOccurrence(currentUtcTime);

            var delay = occurenceTime.GetValueOrDefault() - currentUtcTime;
            _logger.LogInformation("The run is delayed for {delay}. Current time: {time}", delay, DateTimeOffset.Now);

            await Task.Delay(delay);
        }
    }
}
