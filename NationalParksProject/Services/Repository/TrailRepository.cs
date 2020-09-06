using System.Net.Http;
using NationalParksProject.Models;
using NationalParksProject.Services.IRepository;

namespace NationalParksProject.Services.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
