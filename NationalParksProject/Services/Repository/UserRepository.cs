using NationalParksProject.Models;
using NationalParksProject.Services.IRepository;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NationalParksProject.Services.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public UserRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<User> LoginAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (user.Password == null || user.Username == null) return null;

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK) return null;

            var serializedUser = await response.Content.ReadAsStringAsync();

            var deserializedUser = JsonConvert.DeserializeObject<User>(serializedUser);

            return deserializedUser;



        }

        public async Task<bool> RegisterAsync(string url, User user)
        {
            user.Role = string.IsNullOrWhiteSpace(user.Role) ? "user" : user.Role;

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (user.Password == null || user.Username == null) return false;

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.Created;
        }
    }
}
