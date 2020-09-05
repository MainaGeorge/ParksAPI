using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Services.IRepositoryService
{
    public interface ITrailRepository
    {
        bool TrailExists(int trailId);
        bool TrailExists(string trailName);
        Trail GetTrailById(int trailId);
        IEnumerable<Trail> GetAllTrails();
        bool UpdateTrial(int trailId, UpdateTrailDto trail);
        bool PostNewTrail(Trail trail);
        bool DeleteTrail(int trailId);
        IEnumerable<Trail> GetAllTrailsInANationalPark(int nationalParkId);
        bool SaveChanges();
    }
}
