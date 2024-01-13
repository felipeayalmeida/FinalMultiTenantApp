using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Application.Commands.CustomerCmd;
using MultiTenantApp.Application.Commands.UserCmd;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Validation.FluentValidation;
using System.Security.Claims;

namespace MultiTenantApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomer model)
        {
            var command = new CreateCustomerCommand(model);

            var newCustomer = _mediator.Send(command);

            if (newCustomer != null)
                return Ok(newCustomer.Result);
            else
                return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(DeleteCustomer deleteCustomerId)
        {
            var comand = new DeleteCustomerCommand(deleteCustomerId);
            var response = await _mediator.Send(comand);

            if (response != null)
                return Ok(response);
            return BadRequest("Não foi possível excluir usuário");
        }

        
        [HttpGet]
        public async Task<IActionResult> GetCustomersBySecretaryId()
        {
            var emailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var queryUsers = new GetUsersQuery();
            var usersResult = await _mediator.Send(queryUsers);
            var secretaryId = usersResult.Where(u => u.Email == emailUser).FirstOrDefault().Id;
            var queryCustomer = new GetCustomersQuery(secretaryId);
            var customersResult = await _mediator.Send(queryCustomer);

            if (customersResult != null)
                return Ok(customersResult);
            else
                return NotFound();

        }

        [HttpPut("updateSchedule")]
        public async Task<IActionResult> UpdateCustomerSchedule([FromBody]UpdateSchedule updateSchedule)
        {
            var command = new UpdateScheduleCommand(updateSchedule);
            var newCustomer = await _mediator.Send(command);

            if (newCustomer != null)
                return Ok(newCustomer);
            else
                return BadRequest();
        }

        [HttpPut("updateCustomerStatus")]
        public async Task<IActionResult> UpdateCustomerStatus([FromBody] UpdateCustomerStatus updateStatus)
        {
            var command = new UpdateCustomerStatusCommand(updateStatus);
            var newCustomer = await _mediator.Send(command);

            if (newCustomer != null)
                return Ok(newCustomer);
            else
                return BadRequest();
        }
    }
}
