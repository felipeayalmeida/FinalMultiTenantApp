using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Repository
{
    public class TenantRepository :ITenantRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TenantRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Tenant> CreateTenant(Tenant tenant)
        {
            try
            {
                _dbContext.Add(tenant);
                _dbContext.SaveChanges();
                return tenant;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível criar Tenant");
            }
        }
        public async Task<Tenant> GetTenantUserByEmail(string tenantEmail)
        {
            try
            {
                var email = _dbContext.Tenants.Where(x => x.Email == tenantEmail).FirstOrDefault();
                return email;
            }
            catch (Exception)
            {

                throw new Exception("Não foi possível buscar Tenant");
            }

        }
    }
}
