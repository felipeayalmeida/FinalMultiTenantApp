using MediatR;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Application.Commands.CustomerCmd
{
    public class UpdateScheduleCommand : IRequest<Customer>
    {
        public UpdateSchedule UpdateScheduleRequest { get; }

        public UpdateScheduleCommand(UpdateSchedule updateScheduleRequest)
        {
            UpdateScheduleRequest = updateScheduleRequest;
        }
    }
}
