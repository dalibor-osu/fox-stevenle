using FoxStevenle.API.Models;
using FoxStevenle.API.Types.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoxStevenle.API.Database;

public class FoxStevenleDatabaseContext(DbContextOptions<FoxStevenleDatabaseContext> options) : DbContext(options)
{
    public const string SchemaName = "fox_stevenle";

    public DbSet<DailyQuiz> DailyQuizzes { get; set; }
    public DbSet<QuizEntry> QuizEntries { get; set; }
    public DbSet<Song> Songs { get; set; }


    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.UseIdentityColumns();

        SetDefaultValues(modelBuilder);

        modelBuilder.Entity<DailyQuiz>().HasIndex(x => x.Date).IsUnique();
    }
    
    private static ModelBuilder SetDefaultValues(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Select(x => x.ClrType))
        {
            if (typeof(IIdentifiable).IsAssignableFrom(entityType))
            {
                modelBuilder.Entity(entityType).HasIndex(nameof(IIdentifiable.Id)).IsUnique();
                modelBuilder.Entity(entityType).Property(nameof(IIdentifiable.Id)).UseIdentityAlwaysColumn();
            }
        }

        return modelBuilder;
    }
}