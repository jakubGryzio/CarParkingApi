using CarParkingWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarParkingWebApi.Data.Configuration
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Plate).IsRequired().HasMaxLength(8);
            builder.HasIndex(c => c.Plate).IsUnique();
        }
    }
}
