using FluentValidation;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Validation.FluentValidation
{
    public class TenantModelValidator : AbstractValidator<Tenant>
    {
        public TenantModelValidator()
        {
            RuleFor(tenant => tenant.Name).NotEmpty().MinimumLength(2).WithMessage("Não foi possível criar tenant");
            RuleFor(tenant => tenant.Email).NotEmpty().EmailAddress().WithMessage("Não foi possível criar tenant");
            RuleFor(tenant => tenant.Paswword).NotEmpty().MinimumLength(6).WithMessage("Não foi possível criar tenant");
        }
    }
}
