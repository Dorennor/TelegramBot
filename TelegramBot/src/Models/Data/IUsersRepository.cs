using DesktopApp.Models.Entities;
using System.Threading.Tasks;

namespace DesktopApp.Models.Data;

public interface IUsersRepository
{
    Task AddUserAsync(User user);
    Task<User> FindByChatIdAsync(long chatId);
}