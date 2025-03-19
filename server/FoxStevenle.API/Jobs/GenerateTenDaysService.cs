using FoxStevenle.API.Utils;

namespace FoxStevenle.API.Jobs;

public class GenerateTenDaysService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var generator = scope.ServiceProvider.GetRequiredService<DailyQuizGenerator>();
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        for (int i = 0; i < 10; i++)
        {
            await generator.GenerateForDate(currentDate.AddDays(i));
        }
    }
}