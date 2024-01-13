namespace MultiTenantApp.Domain.DTO
{
    public class DeleteUserResponse
    {
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }
}
