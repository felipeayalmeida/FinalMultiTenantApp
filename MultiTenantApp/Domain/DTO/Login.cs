namespace MultiTenantApp.Domain.DTO
{
    public class Login
    {
        public string Tenant { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
