using FluentValidation.Results;
using MediatR;
using MultiTenantApp.Application.Commands.CustomerCmd;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Validation.FluentValidation;
using System.Security.Claims;

namespace MultiTenantApp.Application.Handlers.CustomerHdl
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateCustomerHandler(IUserRepository userRepository,
                                    ICustomerRepository customerRepository,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validator = new CustomerModelValidator();
            ValidationResult result = validator.Validate(request.CreateCustomerRequest);
            if (!result.IsValid)
            {
                return null;
            }
            var emailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = emailUser != null ? await _userRepository.GetUserByEmailAsync(emailUser) : null;

            if (user != null)
            {
                var newCustomer = new Customer()
                {
                    Name = request.CreateCustomerRequest.Name,
                    Schedule = request.CreateCustomerRequest.Schedule,
                    SecretaryId = user.Id,
                    CustomerShowedUp = false
                };

                await _customerRepository.CreateCustomer(newCustomer);
                return newCustomer;
            }
            return null;
        }
    }
}
