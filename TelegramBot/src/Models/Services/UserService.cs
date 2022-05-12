using DesktopApp.Models.Data;
using System.Threading.Tasks;
using User = DesktopApp.Models.Entities.User;

namespace DesktopApp.Models.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _usersRepository;

    public UserService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task AddUserIfNotExistAsync(User user)
    {
        if (_usersRepository.FindByChatIdAsync(user.ChatId) == null) return;
        await _usersRepository.AddUserAsync(user);
    }
}