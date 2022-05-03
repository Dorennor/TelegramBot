using DesktopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace DesktopApp.Models.Data;

public class TgBotDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Song> Songs { get; set; }
    //public DbSet<Playlist> Playlists { get; set; }
    //public DbSet<Statistic> Statistics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["TGBotDb"].ConnectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}