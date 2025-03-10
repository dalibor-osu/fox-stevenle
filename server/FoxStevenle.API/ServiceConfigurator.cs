using System.Data;
using System.Text.Json.Serialization;
using Dapper;
using FoxStevenle.API.Database;
using FoxStevenle.API.Database.Handlers;
using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FoxStevenle.API;

public static class ServiceConfigurator
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder
            .ConfigureGeneral()
            .ConfigureDatabase()
            .ConfigureManagers();
        return builder;
    }

    private static WebApplicationBuilder ConfigureGeneral(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", b =>
                b.WithOrigins("http://localhost:8000", "http://0.0.0.0:8000")  // Frontend URL
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        builder.Services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddMvc();
        builder.Services.AddOpenApi();
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
        
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
        return builder;
    }
    
    private static WebApplicationBuilder ConfigureManagers(this WebApplicationBuilder builder)
    {
        return builder;
    }
}