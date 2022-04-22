using DesktopApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesktopApp.Database;

public class TGBotDbContext : DbContext
{
    //private static readonly string _localDb = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true";
    private static readonly string _serverDb = @"Server=VLADIMIRPC;Database=TGBotDb;Trusted_Connection=True";

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_serverDb);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}