using FluentValidation;
using MultiTenantApp.Domain.DTO;

namespace MultiTenantApp.Validation.FluentValidation
{
    public class UpdateScheduleModelValidator : AbstractValidator<UpdateSchedule>
    {
        public UpdateScheduleModelValidator()
        {
            RuleFor(cust => cust.CustomerId).NotEmpty().WithMessage("Edição inválida.");
            RuleFor(cust => cust.Schedule).NotEmpty().GreaterThan(DateTime.Now).WithMessage("Edição inválida - Data deve ser futura.");
        }
    }
}
