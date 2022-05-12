using DesktopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DesktopApp.Models.Data;

public class UsersRepository : IUsersRepository
{
    private readonly TgBotDbContext _context;

    public UsersRepository(TgBotDbContext context)
    {
        _context = context;
        context.Users.Load();
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public Task<User> FindByChatIdAsync(long chatId)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
    }
}