using LinkitAir.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace LinkitAir.Service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>  options) : base (options) {}

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        
    }
}