using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Services.IRepositoryService;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ParkyAPI.Services.RepositoryService
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public bool IsUniqueUser(string username)
        {
            return _context.Users.All(u => u.Username.ToLower().Trim() != username.ToLower().Trim());
        }

        public User AuthenticateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user == null) return null;

            var token = CreateJwtToken(user);

            user.Token = token;
            user.Password = "";

            return user;

        }

        public User RegisterUser(string username, string password, string role)
        {
            var user = new User()
            {
                Password = password,
                Username = username,
                Role = role
            };

            _context.Users.Add(user);

            _context.SaveChanges();

            user.Password = "";

            return user;
        }

        public User GetUser(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username.ToLower().Trim() == username.Trim().ToLower());

            return user;
        }

        private string CreateJwtToken(User user)
        {
            var key = _appSettings.SecretKey;
            var claimsToAddToToken = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role), 
            };

            var claimsIdentity = new ClaimsIdentity(claimsToAddToToken);
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            var signInCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                SigningCredentials = signInCredentials,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
