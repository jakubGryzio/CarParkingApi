namespace CarParkingWebApi.Data
{
    public interface IDbContext
    {
        DbSet<Car> Cars { get; }
        DbSet<Parking> Parkings { get; }
        DbSet<ParkingCar> ParkingCars { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
