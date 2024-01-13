using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;
using MultiTenantApp.Validation.FluentValidation;
using System.Security.Claims;

namespace MultiTenantApp.Application.Handlers.UserHdl
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public CreateUserHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new UserModelValidator();
            ValidationResult result = validator.Validate(request.CreateUser);
            if (!result.IsValid)
            {
                return null;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var emailUser = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isTenant = false;
            User updatedUser = null;
            var user = MappingCreateUserUser(request.CreateUser);

            if (emailUser != null)
                isTenant = UserIsTenant(emailUser);

            if (isTenant)
                updatedUser = await _userRepository.CreateUser(user);

            if (updatedUser?.Id > 0 && updatedUser?.Id != null)
                return updatedUser;

            return null;
        }

        private bool UserIsTenant(string emailCreator)
        {
            var userDb = _userRepository.GetUserByEmailAsync(emailCreator).Result;
            var isTenant = userDb.Role == RoleEnum.Admin;
            if (isTenant)
                return true;
            return false;
        }
        private User MappingCreateUserUser(CreateUser createUser)
        {
            var user = new User();
            user.Email = createUser.Email;
            user.Password = createUser.Password;
            user.Role = RoleEnum.Secretary;

            return user;
        }
    }
}
