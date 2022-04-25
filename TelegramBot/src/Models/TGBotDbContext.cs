using Microsoft.EntityFrameworkCore;

namespace DesktopApp.Models;

public class TGBotDbContext : DbContext
{
    private static readonly string _sqlite = @"Data Source=.\TGBotDb.db";

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_sqlite);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}