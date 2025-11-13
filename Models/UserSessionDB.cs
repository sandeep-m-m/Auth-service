namespace Auth_service.Models
{
    public class UserSessionDB
    {
        public int UserId { get; set; }
        public string IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenValidity { get; set; }

    }
}