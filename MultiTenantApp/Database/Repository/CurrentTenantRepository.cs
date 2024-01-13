using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Repository
{
    public class CurrentTenantRepository : ICurrentTenantRepository
    {
        private readonly TenantDbContext _dbContext;
        public CurrentTenantRepository(TenantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tenant> GetTenantInfo(string tenant)
        {
            var tenantInfo = await _dbContext.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync();
            if(tenantInfo != null)
                return tenantInfo;
            return null; 
        }
    }
}
