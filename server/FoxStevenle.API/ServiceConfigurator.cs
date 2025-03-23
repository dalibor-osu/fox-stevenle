using System.Data;
using System.Text.Json.Serialization;
using Dapper;
using FoxStevenle.API.Database;
using FoxStevenle.API.Database.Handlers;
using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Exceptions;
using FoxStevenle.API.Jobs;
using FoxStevenle.API.Utils;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Npgsql;

namespace FoxStevenle.API;

public static class ServiceConfigurator
{
    /// <summary>
    /// Configures all services needed by the app
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/> to use</param>
    /// <returns>Enriched <see cref="WebApplicationBuilder"/></returns>
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder
            .ConfigureGeneral()
            .ConfigureDatabase()
            .ConfigureLocalServices();
        return builder;
    }

    private static WebApplicationBuilder ConfigureGeneral(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", b =>
                b.WithOrigins("http://localhost:8000", "http://0.0.0.0:8000")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        builder.Services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddMvc();
        builder.Services.AddOpenApi();
        builder.Services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(x =>
            {
                x.UseNpgsqlConnection(builder.Configuration.GetConnectionString("PostgreSQL"));
            }));
        builder.Services.AddHangfireServer();
        return builder;
    }
    
    private static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        string databaseConnectionString = configuration.GetConnectionString("PostgreSQL")
            ?? throw new ConfigurationException("Could not find PostgreSQL connection string");

        builder.Services.AddDbContext<FoxStevenleDatabaseContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
            options.UseNpgsql(x => x.MigrationsAssembly("FoxStevenle.API"));
            options.UseNpgsql(x => x.MigrationsHistoryTable("__EFMigrationsHistory", FoxStevenleDatabaseContext.SchemaName));
        });
        
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(databaseConnectionString));
        builder.Services.AddScoped<Func<IDbConnection>>(_ => () => new NpgsqlConnection(databaseConnectionString));
        
        builder.Services.AddScoped<QuizEntryDatabaseService>();
        builder.Services.AddScoped<DailyQuizDatabaseService>();
        builder.Services.AddScoped<SongDatabaseService>();
        
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
        return builder;
    }
    
    private static WebApplicationBuilder ConfigureLocalServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DailyQuizGenerator>();

        // Jobs
        builder.Services.AddHostedService<JobPlanner>();
        builder.Services.AddHostedService<GenerateTodayService>();
        builder.Services.AddScoped<CreateDailyQuizJob>();
        return builder;
    }
}