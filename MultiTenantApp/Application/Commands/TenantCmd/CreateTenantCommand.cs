using MediatR;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Commands.TenantCmd
{
    public class CreateTenantCommand : IRequest<Tenant>
    {
        public Tenant Tenant{ get;}

        public CreateTenantCommand(Tenant tenant)
        {
            Tenant = tenant;
        }
    }
}
