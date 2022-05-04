using DesktopApp.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = DesktopApp.Models.Entities.User;

namespace DesktopApp.Models.Controllers;

public class UserController : IDisposable
{
    private readonly TgBotDbContext _context;

    public UserController()
    {
        _context = new TgBotDbContext();
        _context.Users.Load();
    }

    public async Task AddToDbIfUserDoesNotExist(Chat chat)
    {
        var userInDb = _context.Users.FirstOrDefault(c => c.ChatId == chat.Id);

        if (userInDb != null) return;

        await _context.Users.AddAsync(new User(chat.Id, chat.Username, chat.FirstName, chat.LastName));
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}