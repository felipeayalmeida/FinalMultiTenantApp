using MediatR;
using MultiTenantApp.Domain.DTO;

namespace MultiTenantApp.Application.Commands.AuthCmd
{
    public class LoginCommand : IRequest<object>
    {
        public Login Login { get; }

        public LoginCommand(Login login)
        {
            Login = login;
        }
    }
}
