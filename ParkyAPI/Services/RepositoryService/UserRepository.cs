using System;
using System.Linq;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Services.IRepositoryService;

namespace ParkyAPI.Services.RepositoryService
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public bool IsUniqueUser(string username)
        {
            return _context.Users.All(u => u.Name.ToLower().Trim() != username.ToLower().Trim());
        }

        public User AuthenticateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public User RegisterUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
