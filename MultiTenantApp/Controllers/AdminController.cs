using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Application.Commands.TenantCmd;
using MultiTenantApp.Application.Commands.UserCmd;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;

namespace MultiTenantApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTenant(Tenant createTenant)
        {
            var command = new CreateTenantCommand(createTenant);
            var newUser =  await _mediator.Send(command);

            if (newUser != null)
                return Ok(newUser);
            return BadRequest("Não foi possível criar Admin");
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(CreateUser createUser)
        {
            var command = new CreateUserCommand(createUser);
            var response = await _mediator.Send(command);

            if (response != null)
                return Ok(response);
            return BadRequest("Nao foi possível criar usuário");
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteUser(DeleteUser deleteUser)
        {
            var comand = new DeleteUserCommand(deleteUser);
            var response = await _mediator.Send(comand);

            if (response != null)
                return Ok(response);
            return BadRequest("Não foi possível excluir usuário");
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var users = await _mediator.Send(query);

            if (users != null)
                return Ok(users);
            return BadRequest("Não foi possível buscar secretários");
        }
    }
}
