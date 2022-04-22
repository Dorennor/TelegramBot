using DesktopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesktopApp.Models.Database;

public class TGBotDbContext : DbContext
{
    private readonly string _connectionString = @"Server=VLADIMIRPC;Database=TGBotDb;Trusted_Connection=True";
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}