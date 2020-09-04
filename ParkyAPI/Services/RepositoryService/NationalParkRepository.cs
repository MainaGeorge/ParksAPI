using System.Collections.Generic;
using System.Linq;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Services.IRepositoryService;

namespace ParkyAPI.Services.RepositoryService
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationContext _context;

        public NationalParkRepository(ApplicationContext context)
        {
            _context = context;
        }
        public IEnumerable<NationalPark> GetAllNationalParks()
        {
            return _context.NationalParks.ToList();
        }

        public NationalPark GetNationalParkById(int nationalParkId)
        {
            return _context.NationalParks.FirstOrDefault(p => p.Id == nationalParkId);
        }

        public bool Update(NationalPark nationalPark)
        {
            var parkToUpdate = _context.NationalParks.FirstOrDefault(p => p.Id == nationalPark.Id);

            if (parkToUpdate == null) return false;

            parkToUpdate.Name = nationalPark.Name;
            parkToUpdate.Created = nationalPark.Created;
            parkToUpdate.Established = nationalPark.Established;
            parkToUpdate.State = nationalPark.State;

            return SaveChanges();


        }

        public bool AddNationalParkToDatabase(NationalPark nationalPark)
        {
            _context.NationalParks.Add(nationalPark);
            return SaveChanges();
        }

        public bool DeleteNationalParkFromDatabase(int nationalParkId)
        {
            var parkToDelete = _context.NationalParks.FirstOrDefault(p => p.Id == nationalParkId);

            if (parkToDelete == null) return false;

            _context.Remove(parkToDelete);
            return SaveChanges();
        }

        public bool NationalParkExists(string nationalParkName)
        {
            return _context.NationalParks.Any(p => p.Name == nationalParkName);

        }

        public bool NationalParkExists(int nationalParkId)
        {
            return  _context.NationalParks.Any(p => p.Id == nationalParkId);

        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
