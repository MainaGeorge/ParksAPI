using System.Net.Http;
using NationalParksProject.Models;

namespace NationalParksProject.Services.Repository
{
    public class NationalParkRepository : Repository<NationalPark>
    {
        public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
