using FoxStevenle.API.Utils;

namespace FoxStevenle.API.Jobs;

public class CreateDailyQuizJob(IServiceProvider serviceProvider)
{
    public async Task Run()
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var generator = scope.ServiceProvider.GetRequiredService<DailyQuizGenerator>();
        var nextDay = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
        await generator.GenerateForDate(nextDay);
    }
}