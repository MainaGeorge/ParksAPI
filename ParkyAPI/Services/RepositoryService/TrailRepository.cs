using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Services.IRepositoryService;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Services.RepositoryService
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationContext _db;

        public TrailRepository(ApplicationContext db)
        {
            _db = db;
        }
        public bool TrailExists(int trailId)
        {
            return _db.Trails.Any(t => t.Id == trailId);
        }

        public bool TrailExists(string trailName)
        {
            return _db.Trails.Any(t => t.Name.ToLower().Trim() == trailName.ToLower().Trim());
        }

        public Trail GetTrailById(int trailId)
        {
            return _db.Trails.Include(t => t.NationalPark)
                .FirstOrDefault(t => t.Id == trailId);
        }

        public IEnumerable<Trail> GetAllTrails()
        {
            return _db.Trails.Include(t => t.NationalPark);
        }

        public bool UpdateTrial(int trailId, Trail trail)
        {
            if (!TrailExists(trailId)) return false;
            var trailToUpdate = _db.Trails.Single(t => t.Id == trailId);

            _db.Entry(trailToUpdate).State = EntityState.Modified;

            return SaveChanges();
        }

        public bool PostNewTrail(Trail trail)
        {
            _db.Trails.Add(trail);

            return SaveChanges();
        }

        public bool DeleteTrail(int trailId)
        {
            if (!TrailExists(trailId)) return false;
            var trailToDelete = _db.Trails.Single(t => t.Id == trailId);

            _db.Trails.Remove(trailToDelete);
            return SaveChanges();
        }

        public IEnumerable<Trail> GetAllTrailsInANationalPark(int nationalParkId)
        {
            return _db.Trails.Include(t => t.NationalPark).Where(t => t.NationalParkId == nationalParkId);
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
