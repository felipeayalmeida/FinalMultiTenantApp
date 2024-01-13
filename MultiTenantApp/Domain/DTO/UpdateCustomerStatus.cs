namespace MultiTenantApp.Domain.DTO
{
    public class UpdateCustomerStatus
    {
        public int CustomerId { get; set; }
        public bool ShowedUp{ get; set; }
    }
}
