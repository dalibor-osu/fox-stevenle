using FoxStevenle.API.Utils;

namespace FoxStevenle.API.Jobs;

/// <summary>
/// Runs daily to create a new quiz for the next day
/// </summary>
/// <param name="serviceProvider">Provider of the required services</param>
public class CreateDailyQuizJob(IServiceProvider serviceProvider)
{
    public async Task Run()
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateDailyQuizJob>>();
        logger.LogInformation("{JobName}: Starting...", nameof(CreateDailyQuizJob));
        var generator = scope.ServiceProvider.GetRequiredService<DailyQuizGenerator>();
        var nextDay = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
        await generator.GenerateForDate(nextDay);
        logger.LogInformation("{JobName}: Finished!", nameof(CreateDailyQuizJob));

    }
}