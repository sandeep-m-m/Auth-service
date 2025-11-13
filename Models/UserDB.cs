namespace Auth_service.Models
{
    public class UserDB : CommonDB
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

    }
}