using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;
using ParkyAPI.Services.RepositoryService;

namespace ParkyAPI.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
    }
}
