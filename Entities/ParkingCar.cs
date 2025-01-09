using System.ComponentModel.DataAnnotations.Schema;

namespace CarParkingWebApi.Entities
{
    [Table("ParkingCars")]
    public class ParkingCar
    {
        public Guid Id { get; set; } = new Guid();
        public int ParkingId { get; set; }
        public Guid CarId { get; set; }
        public DateTime ParkDate { get; set; } = DateTime.Now;
    }
}
