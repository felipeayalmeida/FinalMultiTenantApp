using MediatR;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;
using System.Security.Claims;

namespace MultiTenantApp.Application.Handlers.CustomerHdl
{
    public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, List<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        public GetCustomersHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customersResult = await _customerRepository.GetCustomersBySecretaryId(request.SecretaryId);

            if (customersResult != null && customersResult.Count()>0)
                return customersResult;

            return null;
        }
    }
}
