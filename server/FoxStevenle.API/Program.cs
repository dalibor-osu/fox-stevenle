using FoxStevenle.API;
using FoxStevenle.API.Database;
using FoxStevenle.API.Middleware;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication
    .CreateBuilder(args)
    .Configure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHangfireDashboard();
    app.MapHangfireDashboard();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<FoxStevenleDatabaseContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseMiddleware<ExceptionLoggingMiddleware>();

#if !DEBUG
app.UseHttpsRedirection();
#endif

app.MapControllers();

await app.RunAsync();