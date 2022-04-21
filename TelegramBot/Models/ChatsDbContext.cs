using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Models;

public class ChatsDbContext : DbContext
{
    private readonly string _connectionString = @"Server=VLADIMIRPC;Database=ChatDb;Trusted_Connection=True";
    public DbSet<Chat> Chats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Chat>()
            .HasIndex(c => c.ChatId)
            .IsUnique();
    }
}