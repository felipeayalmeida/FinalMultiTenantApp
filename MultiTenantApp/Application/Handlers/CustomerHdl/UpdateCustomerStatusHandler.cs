using FluentValidation.Results;
using MediatR;
using MultiTenantApp.Application.Commands.CustomerCmd;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Validation.FluentValidation;
using System.Security.Claims;

namespace MultiTenantApp.Application.Handlers.CustomerHdl
{
    public class UpdateCustomerStatusHandler : IRequestHandler<UpdateCustomerStatusCommand, Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        public UpdateCustomerStatusHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
        {
            //var validator = new UpdateScheduleModelValidator();
            //ValidationResult result = validator.Validate(request.UpdateScheduleRequest);
            //if (!result.IsValid)
            //{
            //    return null;
            //}

            var customer = await _customerRepository.GetCustomerById(request.UpdateStatusRequest.CustomerId);
            if (customer != null)
            {
                var updatedCustomer = new Customer()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Schedule = customer.Schedule,
                    SecretaryId = customer.SecretaryId,
                    CustomerShowedUp = request.UpdateStatusRequest.ShowedUp
                };

                await _customerRepository.UpdateCustomer(updatedCustomer);
                return updatedCustomer;
            }
            return null;
        }
    }
}
