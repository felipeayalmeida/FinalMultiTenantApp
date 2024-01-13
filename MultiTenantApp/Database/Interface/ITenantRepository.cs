using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Interface
{
    public interface ITenantRepository
    {
        Task<Tenant> CreateTenant(Tenant tenant);
        Task<Tenant> GetTenantUserByEmail(string tenantEmail);
    }
}