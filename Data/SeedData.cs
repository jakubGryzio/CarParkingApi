namespace CarParkingWebApi.Data
{
    public static class SeedData
    {
        public async static Task Initialize(IDbContext context)
        {
            if (context.Parkings.Any() || context.Cars.Any() || context.ParkingCars.Any())
            {
                return;
            }

            var parkings = new List<Parking>
            {
                new Parking { Id = 1, Name = "Prosta", ParkingSpaces = 50 },
                new Parking { Id = 2, Name = "Wolska", ParkingSpaces = 100 },
                new Parking { Id = 3, Name = "Narutowicza", ParkingSpaces = 200 }
            };
            context.Parkings.AddRange(parkings);

            var cars = new List<Car>
            {
                new Car { Id = Guid.NewGuid(), Plate = "LUB12345" },
                new Car { Id = Guid.NewGuid(), Plate = "OP12332" },
                new Car { Id = Guid.NewGuid(), Plate = "WX99900" }
            };
            context.Cars.AddRange(cars);

            var parkingCars = new List<ParkingCar>
            {
                new ParkingCar { ParkingId = 1, CarId = cars[0].Id},
                new ParkingCar { ParkingId = 1, CarId = cars[1].Id},
                new ParkingCar { ParkingId = 2, CarId = cars[2].Id}
            };
            context.ParkingCars.AddRange(parkingCars);

            await context.SaveChangesAsync();
        }
    }
}
