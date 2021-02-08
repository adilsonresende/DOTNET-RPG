using System.Threading.Tasks;
using DOTNET_RPG.Models;

namespace DOTNET_RPG.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<int>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}