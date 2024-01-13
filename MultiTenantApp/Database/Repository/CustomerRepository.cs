using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task CreateCustomer(Customer customer)
        {
            _dbContext.Add(customer);
            _dbContext.SaveChangesUsers();
        }
        public async Task UpdateCustomer(Customer customer)
        {
            _dbContext.Update(customer);
            _dbContext.SaveChangesUsers();
        }
        public async Task<Customer> GetCustomerById(int idCustomer)
        {
            var customer = _dbContext.Customers.Where(c => c.Id == idCustomer).AsNoTracking().FirstOrDefault();
            return customer;
        }
        public async Task<List<Customer>> GetCustomersBySecretaryId(int idSecretary)
        {
            var customers = _dbContext.Customers.Where(c => c.SecretaryId == idSecretary).AsNoTracking().ToList();
            return customers;
        }

        public async Task DeleteCustomer(Customer customer)
        {
            _dbContext.Remove(customer);
            _dbContext.SaveChangesUsers();
        }
    }
}
