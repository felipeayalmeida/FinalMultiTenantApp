using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            _dbContext.Add(user);
            _dbContext.SaveChangesUsers();

            return user;
        }
        public async Task DeleteUser(User user)
        {
            _dbContext.Remove(user);
            _dbContext.SaveChangesUsers();
        }
        public async Task<User> CreateTenantUser(User user)
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();

            return user;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possível buscar usuário");
            }
        }
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return _dbContext.Users.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possível buscar usuários");
            }
        }


    }
}
