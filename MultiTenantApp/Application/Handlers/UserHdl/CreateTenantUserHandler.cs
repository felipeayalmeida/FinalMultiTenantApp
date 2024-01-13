using MediatR;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Handlers.UserHdl
{
    public class CreateTenantUserHandler : IRequestHandler<CreateTenantUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public CreateTenantUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> Handle(CreateTenantUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.CreateTenantUser(request.User);
        }
    }
}
