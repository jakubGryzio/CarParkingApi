using System.ComponentModel.DataAnnotations;

namespace CarParkingWebApi.Models.Request
{
    public class AddCarRequest
    {
        [Required]
        public string Plate { get; set; } = default!;
    }
}
