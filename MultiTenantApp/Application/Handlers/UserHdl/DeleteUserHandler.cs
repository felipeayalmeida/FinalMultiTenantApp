using FluentValidation.Results;
using MediatR;
using MultiTenantApp.Application.Commands.UserCmd;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Validation.FluentValidation;

namespace MultiTenantApp.Application.Handlers.UserHdl
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        async Task<DeleteUserResponse> IRequestHandler<DeleteUserCommand, DeleteUserResponse>.Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteUserModelValidator();
            ValidationResult result = validator.Validate(new DeleteUser { Id = request.DeleteUser.Id });
            if (!result.IsValid)
                return null;
            var isDeleted = Task.CompletedTask;
            var users = await _userRepository.GetUsersAsync();
            var user = users.Where(u => u.Id == request.DeleteUser.Id).FirstOrDefault();

            if (user != null)
                isDeleted = _userRepository.DeleteUser(user);
            else
                return null;

            if (isDeleted.IsCompletedSuccessfully)
                return new DeleteUserResponse { Email=user.Email, IsDeleted=true };
            return null;
        }

    }
}
