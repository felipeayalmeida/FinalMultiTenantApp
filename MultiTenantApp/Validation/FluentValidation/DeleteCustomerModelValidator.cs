using FluentValidation;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Validation.FluentValidation
{
    public class DeleteCustomerModelValidator : AbstractValidator<DeleteCustomer>
    {
        public DeleteCustomerModelValidator()
        {
            RuleFor(cust => cust.Id).NotEmpty().WithMessage("Não foi possível deletar cliente");
        }
    }
}
