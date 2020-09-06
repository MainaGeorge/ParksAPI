using System.Net.Http;
using NationalParksProject.Models;
using NationalParksProject.Services.IRepository;

namespace NationalParksProject.Services.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
