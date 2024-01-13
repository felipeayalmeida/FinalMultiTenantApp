using MediatR;
using Microsoft.IdentityModel.Tokens;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Handlers.UserHdl
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        private readonly IUserRepository _userRepository;
        public GetUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUsersAsync();
            if (!users.IsNullOrEmpty() && users.Count > 0)
                return users;
            return null;
        }
    }
}
