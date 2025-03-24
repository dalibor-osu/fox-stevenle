using Hangfire;

namespace FoxStevenle.API.Jobs;

/// <summary>
/// Plans all needed jobs
/// </summary>
public class JobPlanner : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        RecurringJob.AddOrUpdate<CreateDailyQuizJob>(nameof(CreateDailyQuizJob), (job) => job.Run(), Cron.Daily,
            new RecurringJobOptions { MisfireHandling = MisfireHandlingMode.Ignorable, TimeZone = TimeZoneInfo.Utc });
        return Task.CompletedTask;
    }
}