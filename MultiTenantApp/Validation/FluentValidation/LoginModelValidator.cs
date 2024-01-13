using FluentValidation;
using MultiTenantApp.Domain.DTO;

namespace MultiTenantApp.Validation.FluentValidation
{
    public class LoginModelValidator : AbstractValidator<Login>
    {
        public LoginModelValidator()
        {
            RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("Login inválido");
            RuleFor(user => user.Password).NotEmpty().MinimumLength(6).WithMessage("Login inválido");
            RuleFor(user => user.Tenant).NotEmpty().WithMessage("Login inválido");
        }
    }
}
