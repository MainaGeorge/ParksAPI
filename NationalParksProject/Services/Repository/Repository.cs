using NationalParksProject.Services.IRepository;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NationalParksProject.Services.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> GetById(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{url}/{id}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK) return null;

            var serializedObject = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public async Task<IEnumerable<T>> GetAll(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK) return null;

            var serializedResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<T>>(serializedResponse);

        }

        public async Task<bool> CreateAsync(string url, T createObject)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (createObject != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(createObject), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                return response.StatusCode == HttpStatusCode.Created;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{url}/{id}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> UpdateAsync(string url, int id, T updateObject)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"{url}/{id}");

            if (updateObject != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(updateObject),
                    Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                return response.StatusCode == HttpStatusCode.NoContent;
            }
            else
            {
                return false;
            }
        }
    }
}
