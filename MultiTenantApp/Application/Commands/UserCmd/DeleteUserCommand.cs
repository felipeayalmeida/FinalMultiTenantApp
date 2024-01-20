using MediatR;
using MultiTenantApp.Domain.DTO;

namespace MultiTenantApp.Application.Commands.UserCmd
{
    public class DeleteUserCommand : IRequest<DeleteUserResponse>
    {
        public DeleteUser DeleteUser { get; }

        public DeleteUserCommand(int id)
        {
            DeleteUser = new DeleteUser() { Id = id };
        }
    }
}
