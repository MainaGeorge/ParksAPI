using ParkyAPI.Models;

namespace ParkyAPI.Services.IRepositoryService
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User AuthenticateUser(string username, string password);
        User RegisterUser(string username, string password,string role);

        User GetUser(string username);
    }
}
