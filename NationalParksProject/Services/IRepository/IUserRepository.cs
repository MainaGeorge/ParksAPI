using System.Threading.Tasks;
using NationalParksProject.Models;

namespace NationalParksProject.Services.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User user);

        Task<bool> RegisterAsync(string url, User user);
    }
}
