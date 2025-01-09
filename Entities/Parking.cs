namespace CarParkingWebApi.Entities
{
    public class Parking
    {
        public int Id { get; set; } = default;
        public string Name { get; set; } = default!;
        public int ParkingSpaces { get; set; } = default;
    }
}
