using FluentValidation;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Validation.FluentValidation
{
    public class UserModelValidator : AbstractValidator<CreateUser>
    {
        public UserModelValidator()
        {
            RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("Não foi possível criar usuário");
            RuleFor(user => user.Password).NotEmpty().MinimumLength(6).WithMessage("Não foi possível criar usuário");
        }
    }
}
