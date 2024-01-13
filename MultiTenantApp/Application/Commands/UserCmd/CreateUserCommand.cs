using MediatR;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Commands.SecretaryCmd
{
    public class CreateUserCommand : IRequest<User>
    {
        public CreateUser CreateUser { get; }
        public CreateUserCommand(CreateUser createUser)
        {
            CreateUser = createUser;
        }
    }
}
