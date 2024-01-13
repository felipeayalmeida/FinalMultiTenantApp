using MediatR;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Queries
{
    public class GetCustomersQuery : IRequest<List<Customer>>
    {
        public int SecretaryId {  get; }

        public GetCustomersQuery(int secretaryId)
        {
            SecretaryId = secretaryId;
        }

    }
}
