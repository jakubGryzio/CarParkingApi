namespace CarParkingWebApi.Data.Configuration
{
    public class ParkingCarConfiguration : IEntityTypeConfiguration<ParkingCar>
    {
        public void Configure(EntityTypeBuilder<ParkingCar> builder)
        {
            builder.HasKey(pc => pc.Id);
            builder.Property(pc => pc.ParkingId).IsRequired();
            builder.Property(pc => pc.CarId).IsRequired();
            builder.Property(pc => pc.ParkDate).IsRequired();
        }
    }
}
