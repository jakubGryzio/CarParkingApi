using System.Reflection;

namespace CarParkingWebApi.Data
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<Car> Cars => Set<Car>();

        public DbSet<Parking> Parkings => Set<Parking>();

        public DbSet<ParkingCar> ParkingCars => Set<ParkingCar>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
