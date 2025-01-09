namespace CarParkingWebApi.Entities
{
    [Table("Cars")]
    public class Car
    {
        public Guid Id { get; set; } = new Guid();

        public string Plate { get; set; } = default!;
    }
}
