using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Services.IRepositoryService
{
    public interface INationalParkRepository
    {
        IEnumerable<NationalPark> GetAllNationalParks();

        NationalPark GetNationalParkById(int nationalParkId);

        bool Update(NationalPark nationalPark);

        bool AddNationalParkToDatabase(NationalPark nationalPark);

        bool DeleteNationalParkFromDatabase(int nationalParkId);

        bool NationalParkExists(string nationalParkName);

        bool NationalParkExists(int nationalParkId);

        bool SaveChanges();
    }
}
