using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database
{
    public class ApplicationDbContext: DbContext
    {
        private readonly ICurrentTenantService _currentTenantService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentTenantService currentTenantService) : base(options)
        {
            _currentTenantService = currentTenantService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Tenant> Tenants{ get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasQueryFilter(a => a.TenantId == _currentTenantService.TenantId);
            builder.Entity<Customer>().HasQueryFilter(a => a.TenantId == _currentTenantService.TenantId);
        }

        public int SaveChangesUsers()
        {
            try
            {
                if (!string.IsNullOrEmpty(_currentTenantService.TenantId))
                {
                    foreach (var entry in ChangeTracker.Entries<ITenant>().ToList())
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                            case EntityState.Modified:
                                entry.Entity.TenantId = _currentTenantService.TenantId;
                                break;
                        }

                    }
                        var result = base.SaveChanges();
                        return result;
                }
                else
                {
                    return 0;
                }
                
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Erro",ex.Message);
                return 0;
            }

        }
    }
}
