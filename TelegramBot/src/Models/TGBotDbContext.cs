using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DesktopApp.Models;

public class TgBotDbContext : DbContext
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

    public bool IsChatExist(long chatId)
    {
        var chat = Chats.FirstOrDefault(c => c.ChatId == chatId);
        return chat != null;
    }

    public bool IsSongExist(string hashCode)
    {
        var song = Songs.FirstOrDefault(s => s.HashCode == hashCode);
        return song != null;
    }
}