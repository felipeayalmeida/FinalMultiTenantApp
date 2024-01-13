using FluentValidation;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Validation.FluentValidation
{
    public class DeleteUserModelValidator : AbstractValidator<DeleteUser>
    {
        public DeleteUserModelValidator()
        {
            RuleFor(user => user.Id).NotEmpty().WithMessage("Não foi possível deletar usuário");
        }
    }
}
