using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Interface
{
    public interface ICurrentTenantRepository
    {
        Task<Tenant> GetTenantInfo(string tenant);
    }
}
