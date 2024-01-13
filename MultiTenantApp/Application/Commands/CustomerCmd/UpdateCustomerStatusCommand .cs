using MediatR;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Commands.CustomerCmd
{
    public class UpdateCustomerStatusCommand : IRequest<Customer>
    {
        public UpdateCustomerStatus UpdateStatusRequest { get; }

        public UpdateCustomerStatusCommand(UpdateCustomerStatus updateStatusRequest)
        {
            UpdateStatusRequest = updateStatusRequest;
        }
    }
}
