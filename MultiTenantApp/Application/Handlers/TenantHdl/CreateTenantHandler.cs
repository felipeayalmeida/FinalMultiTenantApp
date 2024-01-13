using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Application.Commands.TenantCmd;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;
using MultiTenantApp.Validation.FluentValidation;

namespace MultiTenantApp.Application.Handlers.TenantHdl
{
    public class CreateTenantHandler : IRequestHandler<CreateTenantCommand, Tenant>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMediator _mediator;
        public CreateTenantHandler(ITenantRepository tenantRepository, IMediator mediator)
        {
            _tenantRepository = tenantRepository;
            _mediator = mediator;
        }

        public async Task<Tenant> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var validator = new TenantModelValidator();
            ValidationResult result = validator.Validate(request.Tenant);
            if (!result.IsValid)
            {
                return null;
            }

            var tenant = await _tenantRepository.CreateTenant(request.Tenant);
            await CreateUserForThisTenant(request);
            return tenant;
        }

        private async Task<User> CreateUserForThisTenant(CreateTenantCommand request)
        {
            var user = new User();
            user.Email = request.Tenant.Email;
            user.Password = request.Tenant.Paswword;
            user.TenantId = request.Tenant.Id;
            user.Role = RoleEnum.Admin;

            var commad = new CreateTenantUserCommand(user);
            return await _mediator.Send(commad);
        }
    }
}
