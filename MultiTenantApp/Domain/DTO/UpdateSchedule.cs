namespace MultiTenantApp.Domain.DTO
{
    public class UpdateSchedule
    {
        public int CustomerId { get; set; }
        public DateTime Schedule { get; set; }
    }
}
