using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Helpers.Enums;

namespace MultiTenantApp.Domain.Entity
{
    public class User: ITenant
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TenantId { get; set; }
        public RoleEnum Role { get; set; }
    }
}
