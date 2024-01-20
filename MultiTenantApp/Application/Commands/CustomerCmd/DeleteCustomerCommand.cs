using MediatR;
using MultiTenantApp.Domain.DTO;

namespace MultiTenantApp.Application.Commands.CustomerCmd
{
    public class DeleteCustomerCommand : IRequest<DeleteCustomerReponse>
    {
        public DeleteCustomer DeleteCustomer { get; }
        public DeleteCustomerCommand(int deleteCustomer)
        {
            DeleteCustomer = new DeleteCustomer() { Id = deleteCustomer };
        }
    }
}
