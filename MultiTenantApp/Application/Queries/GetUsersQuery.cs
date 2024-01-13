using MediatR;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Queries
{
    public class GetUsersQuery : IRequest<List<User>> { }
}
