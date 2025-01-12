using System;
using Microsoft.EntityFrameworkCore;
using Timetables.Core.Entity;

namespace Timetables.Core.Context;

public class PostgresDbContext : DbContext
{
    private readonly IConfiguration Configuration;

    public PostgresDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
        .UseNpgsql(Configuration.GetConnectionString("WebAPiDatabase"))
        .UseSnakeCaseNamingConvention()
        .EnableSensitiveDataLogging();
    }

    public DbSet<TimeTable> TimeTables { get; set; }

}
