using Azure.Core;
using FluentValidation.Results;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using MultiTenantApp.Application.Commands.AuthCmd;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Validation.FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenantApp.Application.Handlers.AuthHdl
{
    public class LoginHandler : IRequestHandler<LoginCommand, object>
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        public LoginHandler(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }
        public async Task<object> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validator = new LoginModelValidator();
            ValidationResult result = validator.Validate(request.Login);
            if (!result.IsValid)
            {
                return null;
            }

            var query = new GetUsersQuery();
            var users = _mediator.Send(query);

            var user = users.Result.Where(u => u.Email == request.Login.Email).FirstOrDefault();
            if (validatingUser(request,user))
            {
                var token = GenerateJwtToken(user, request.Login);
                return new { Token = token };
            }

            return null;
        }

        private string GenerateJwtToken(User user, Login model)
        {
            var secretKey = _configuration.GetSection("JwtSettings")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim("tenantId",model.Tenant),
            new Claim(ClaimTypes.NameIdentifier, user.Email),
        };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private bool validatingUser(LoginCommand request, User user)
        {
            if (user != null && request.Login.Tenant == user.TenantId && user.Email == request.Login.Email && user.Password == request.Login.Password)
            {
                return true;
            }
            return false;
        }

    }
}
