namespace CarParkingWebApi.Models.Request
{
    public class UserLoginRequest
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
