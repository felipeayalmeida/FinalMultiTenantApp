using FluentValidation.Results;
using MediatR;
using MultiTenantApp.Application.Commands.CustomerCmd;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Validation.FluentValidation;

namespace MultiTenantApp.Application.Handlers.CustomerHdl
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerReponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<DeleteCustomerReponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteCustomerModelValidator();
            ValidationResult result = validator.Validate(new DeleteCustomer { Id = request.DeleteCustomer.Id });
            if (!result.IsValid)
                return null;
            var isDeleted = Task.CompletedTask;
            var customer = await _customerRepository.GetCustomerById(request.DeleteCustomer.Id);
            if (customer != null)
                isDeleted = _customerRepository.DeleteCustomer(customer);
            else
                return null;
            if (isDeleted.IsCompletedSuccessfully)
                return new DeleteCustomerReponse { Name = customer.Name, IsDeleted = true };
            return null;
        }
    }
}
