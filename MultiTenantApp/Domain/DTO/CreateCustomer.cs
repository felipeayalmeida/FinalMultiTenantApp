namespace MultiTenantApp.Domain.DTO
{
    public class CreateCustomer
    {
        public string Name { get; set; }
        public DateTime Schedule { get; set; }
        //public int SecretaryId { get; set; }
        //Incluir Tenant
    }
}
