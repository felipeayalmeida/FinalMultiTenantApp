using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> CreateTenantUser(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetUsersAsync();
        Task DeleteUser(User user);
    }
}