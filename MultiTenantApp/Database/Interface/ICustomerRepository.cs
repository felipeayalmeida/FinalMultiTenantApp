using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Database.Interface
{
    public interface ICustomerRepository
    {
        Task CreateCustomer(Customer customer);
        Task<Customer> GetCustomerById(int idCustomer);
        Task UpdateCustomer(Customer customer);
        Task<List<Customer>> GetCustomersBySecretaryId(int idSecretary);
        Task DeleteCustomer(Customer customer);

    }
}