using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database
{
    public class TenantDbContext: DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options): base(options) 
        {
            
        }
        public DbSet<Tenant> Tenants { get; set; }
    }
}
