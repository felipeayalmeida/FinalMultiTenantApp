using MediatR;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Commands.CustomerCmd
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public CreateCustomer CreateCustomerRequest { get; }

        public CreateCustomerCommand(CreateCustomer createCustomerRequest)
        {
            CreateCustomerRequest = createCustomerRequest;
        }
    }
}
