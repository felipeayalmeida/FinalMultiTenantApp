using FluentValidation;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Validation.FluentValidation
{
    public class CustomerModelValidator : AbstractValidator<CreateCustomer>
    {
        public CustomerModelValidator()
        {
            RuleFor(customer => customer.Name).NotEmpty().WithMessage("Cliente inválido (N)");
            //RuleFor(customer => customer.SecretaryId).NotEmpty().WithMessage("Cliente inválido (SID)");
            RuleFor(customer => customer.Schedule).NotEmpty().WithMessage("Cliente inválido (SC)");
        }
    }
}
