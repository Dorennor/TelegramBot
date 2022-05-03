using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DesktopApp.Models.Entities;

[Index(nameof(ChatId), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long ChatId { get; set; }

    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public User(long chatId, string? username, string? firstName, string? lastName)
    {
        ChatId = chatId;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }
}