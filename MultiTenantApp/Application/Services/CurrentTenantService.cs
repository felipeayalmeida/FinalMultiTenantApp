using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Database.Interface;

namespace MultiTenantApp.Application.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly ICurrentTenantRepository _currentTenantRepository;
        public CurrentTenantService(ICurrentTenantRepository currentTenantRepository)
        {
            _currentTenantRepository = currentTenantRepository;
        }
        public string? TenantId { get; set; }

        public async Task<bool> SetTenant(string tenant)
        {
            var tenantInfo = await _currentTenantRepository.GetTenantInfo(tenant);
            if(tenantInfo != null)
            {
                TenantId = tenantInfo.Id;
                return true;
            }
            else
            {
                throw new Exception("Invalid Tenant for this User");
            }
        }
    }
}
