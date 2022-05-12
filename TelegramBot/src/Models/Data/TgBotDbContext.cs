using DesktopApp.Models.Entities;
using DesktopApp.Models.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace DesktopApp.Models.Data;

public class TgBotDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Song> Songs { get; set; }
    //public DbSet<Playlist> Playlists { get; set; }
    //public DbSet<Statistic> Statistics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Song>()
            .Property(s => s.Performers)
            .HasConversion(
                convertToProviderExpression: value => string.Join($"{';'}", value),
                convertFromProviderExpression: value => value.SplitBySemicolon(),
                valueComparer: new ValueComparer<string[]>(
                    equalsExpression: (value1, value2) => value1.SequenceEqual(value2),
                    hashCodeExpression: value => value.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    snapshotExpression: value => value));

        modelBuilder.Entity<Song>()
            .Property(s => s.Genres)
            .HasConversion(
                convertToProviderExpression: value => string.Join($"{';'}", value),
                convertFromProviderExpression: value => value.SplitBySemicolon(),
                valueComparer: new ValueComparer<string[]>(
                    equalsExpression: (value1, value2) => value1.SequenceEqual(value2),
                    hashCodeExpression: value => value.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    snapshotExpression: value => value));

        modelBuilder.Entity<Song>()
            .Property(s => s.Tags)
            .HasConversion(
                convertToProviderExpression: value => string.Join($"{';'}", value),
                convertFromProviderExpression: value => value.SplitBySemicolon(),
                valueComparer: new ValueComparer<string[]>(
                    equalsExpression: (value1, value2) => value1.SequenceEqual(value2),
                    hashCodeExpression: value => value.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    snapshotExpression: value => value));
    }

    public TgBotDbContext(DbContextOptions<TgBotDbContext> options) : base(options)
    {
        Database.Migrate();
    }
}