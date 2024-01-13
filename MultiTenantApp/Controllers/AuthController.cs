using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Application.Commands.AuthCmd;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Validation.FluentValidation;

namespace MultiTenantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Login model)
        {
            
            var command = new LoginCommand(model);
            var token = await _mediator.Send(command);

            if (token != null)
                return Ok(token);
            else
                return BadRequest("Não foi possível realizar o login");
        }
    }
}
