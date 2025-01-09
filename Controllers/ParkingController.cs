using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CarParkingWebApi.Controllers
{
    public class ParkingController(IDbContext dbContext) : BaseApiController
    {
        private readonly IDbContext dbContext = dbContext;

        [HttpGet("{parkingId}/CarCount")]
        public async Task<ActionResult<CarCountResponse>> GetCarCount(int parkingId)
        {
            var parkingExists = await this.dbContext.Parkings.AsNoTracking().AnyAsync(p => p.Id == parkingId);

            if (!parkingExists) return NotFound(new MessageResponse { Message = $"There is no parking with the given id={parkingId}" });

            var carCount = await this.dbContext.ParkingCars.AsNoTracking().CountAsync(pc => pc.ParkingId == parkingId);

            return Ok(new CarCountResponse { ParkingId = parkingId, Count = carCount});
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{parkingId}/AddCar")]
        public async Task<ActionResult<MessageResponse>> AddCar(int parkingId, [FromBody] AddCarRequest request)
        {
            var parking = await dbContext.Parkings.FindAsync(parkingId);
            if (parking == null) return NotFound(new MessageResponse { Message = $"There is no parking with the given id={parkingId}" });

            if (!ValidPlate(request.Plate))
            {
                return BadRequest(new MessageResponse { Message = "Invalid plate format" });
            }

            var car = await GetCar(request);

            var hasParked = await dbContext.ParkingCars.AsNoTracking().AnyAsync(pc => pc.CarId == car.Id);
            if (hasParked)
            {
                return Ok(new MessageResponse { Message = "The car is already parked" });
            }

            var parkedCarsCount = await dbContext.ParkingCars.AsNoTracking().CountAsync(pc => pc.ParkingId == parkingId);
            if (parkedCarsCount >= parking.ParkingSpaces)
            {
                return BadRequest(new MessageResponse { Message = "There is no free parking space" });
            }

            await dbContext.ParkingCars.AddAsync(new ParkingCar { CarId = car.Id, ParkingId = parking.Id });
            await dbContext.SaveChangesAsync();

            return Ok(new MessageResponse{Message = $"Car with plate={car.Plate} added to parking with id={parking.Id}" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddCarToFirstFree")]
        public async Task<ActionResult<MessageResponse>> AddCar([FromBody] AddCarRequest request)
        {
            if (!ValidPlate(request.Plate))
            {
                return BadRequest(new MessageResponse { Message = "Invalid plate format" });
            }
            
            var car = await GetCar(request);

            var hasParked = await dbContext.ParkingCars.AsNoTracking().AnyAsync(pc => pc.CarId == car.Id);
            if (hasParked)
            {
                return Ok(new MessageResponse { Message = "The car is already parked" });
            }

            var firstFreeParking = await dbContext.Parkings.AsNoTracking().FirstOrDefaultAsync(p => 
                p.ParkingSpaces > dbContext.ParkingCars.AsNoTracking().Count(pc => pc.ParkingId == p.Id));
            if (firstFreeParking == null)
            {
                return BadRequest(new MessageResponse { Message = "There is no free parking space" });
            }

            await dbContext.ParkingCars.AddAsync(new ParkingCar { CarId = car.Id, ParkingId = firstFreeParking.Id });
            await dbContext.SaveChangesAsync();

            return Ok(new MessageResponse { Message = $"Car with plate={car.Plate} added to parking with id={firstFreeParking.Id}" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{parkingId}/RemoveCar/{carId}")]
        public async Task<ActionResult<MessageResponse>> RemoveCar(int parkingId, Guid carId)
        {
            var parkingExists = await this.dbContext.Parkings.AsNoTracking().AnyAsync(p => p.Id == parkingId);
            if (!parkingExists) return NotFound(new MessageResponse { Message = $"There is no parking with the given id={parkingId}" });

            var carExists = await this.dbContext.Cars.AsNoTracking().AnyAsync(c => c.Id == carId);
            if (!carExists) return NotFound(new MessageResponse { Message = $"There is no car with the given id={carId}" });

            var carParking = await this.dbContext.ParkingCars.FirstOrDefaultAsync(pc => pc.ParkingId == parkingId && pc.CarId == carId);
            if (carParking == null) return NotFound(new MessageResponse { Message = $"The car with id={carId} is not parked in the parking with id={parkingId}" });

            this.dbContext.ParkingCars.Remove(carParking);
            await this.dbContext.SaveChangesAsync();

            return Ok(new MessageResponse{ Message = $"The car with id={carId} is removed from the parking with id={parkingId}" });
        }


        private static bool ValidPlate(string plate)
        {
            return Regex.IsMatch(plate, @"^[A-Z]{2,3}[A-Z0-9]{5}$");
        }

        private async Task<Car> GetCar(AddCarRequest request)
        {
            var car = await dbContext.Cars.FirstOrDefaultAsync(car => car.Plate == request.Plate);
            if (car == null)
            {
                car = new Car { Plate = request.Plate };
                await dbContext.Cars.AddAsync(car);
            }

            return car;
        }
    }
}
