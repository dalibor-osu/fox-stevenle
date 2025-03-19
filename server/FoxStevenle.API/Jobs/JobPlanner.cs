using Hangfire;

namespace FoxStevenle.API.Jobs;

public class JobPlanner : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        RecurringJob.RemoveIfExists(nameof(CreateDailyQuizJob));
        RecurringJob.AddOrUpdate<CreateDailyQuizJob>(nameof(CreateDailyQuizJob), (job) => job.Run(), Cron.Minutely,
            new RecurringJobOptions { MisfireHandling = MisfireHandlingMode.Ignorable, TimeZone = TimeZoneInfo.Utc });
        return Task.CompletedTask;
    }
}