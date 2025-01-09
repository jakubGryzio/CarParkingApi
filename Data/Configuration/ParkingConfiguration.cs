using CarParkingWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarParkingWebApi.Data.Configuration
{
    public class ParkingConfiguration : IEntityTypeConfiguration<Parking>
    {
        public void Configure(EntityTypeBuilder<Parking> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.ParkingSpaces).IsRequired();
        }
    }
}
