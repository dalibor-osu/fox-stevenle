using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Utils;

namespace FoxStevenle.API.Jobs;

/// <summary>
/// Generates a <see cref="DailyQuizGenerator"/> for the current date if it doesn't exist on service startup
/// </summary>
/// <param name="serviceProvider"></param>
public class GenerateTodayService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GenerateTodayService>>();
        var dailyQuizDatabaseService = scope.ServiceProvider.GetRequiredService<DailyQuizDatabaseService>();
        var currentDate = DateOnlyHelper.GetCurrentDateOnly();

        if (await dailyQuizDatabaseService.ExistsByDateAsync(currentDate))
        {
            logger.LogInformation("Quiz for today ({Date}) exists. Skipping creation...", currentDate);
            return;
        }

        var generator = scope.ServiceProvider.GetRequiredService<DailyQuizGenerator>();
        await generator.GenerateForDate(currentDate);
    }
}