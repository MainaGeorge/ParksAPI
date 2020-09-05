using System.Net.Http;
using NationalParksProject.Models;

namespace NationalParksProject.Services.Repository
{
    public class TrailRepository : Repository<Trail>
    {
        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
