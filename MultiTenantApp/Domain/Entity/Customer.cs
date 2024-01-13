using MultiTenantApp.Application.Interfaces;

namespace MultiTenantApp.Domain.Entity
{
    public class Customer : ITenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Schedule { get; set; }
        public int SecretaryId { get; set; }
        public string TenantId { get; set; }
    }
}
