using MediatR;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Commands.SecretaryCmd
{
    public class CreateTenantUserCommand : IRequest<User>
    {
        public User User { get; }
        public CreateTenantUserCommand(User user)
        {
            User = user;
        }
    }
}
