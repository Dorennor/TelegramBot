﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Models;

[Index(nameof(ChatId), IsUnique = true)]
public class Chat
{
    [Key]
    public int Id { get; set; }
    public long ChatId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public Chat(long chatId, string? username, string? firstName, string? lastName)
    {
        ChatId = chatId;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }
}