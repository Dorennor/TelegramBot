using DesktopApp.Models.Entities;
using System.Threading.Tasks;

namespace DesktopApp.Models.Services;

public interface IUserService
{
    Task AddUserIfNotExistAsync(User user);
}