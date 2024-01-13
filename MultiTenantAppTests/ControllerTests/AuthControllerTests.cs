using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using MultiTenantApp.Application.Commands.AuthCmd;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Controllers;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenantApp.Test.ControllerTests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ValidModel_ReturnsOkResultWithToken()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            //var configurationMock = new Mock<IConfiguration>();
            var controller = new AuthController(mediatorMock.Object);

            var loginModel = new Login { Email = "user@email.com", Password = "123456", Tenant = "tenant" };
            var loginCommand = new LoginCommand(loginModel);

            var users = new List<User> { };
            users.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                .ReturnsAsync(users);

            var expectedToken = GenerateJwtToken(users.First(), loginModel);
            mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), CancellationToken.None))
                .ReturnsAsync(new { Token = expectedToken });

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(new { Token = expectedToken });
        }

        [Fact]
        public async Task Login_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var configurationMock = new Mock<IConfiguration>();
            var controller = new AuthController(mediatorMock.Object);

            var loginModel = new Login { Email = "user@email.com", Password = "123456" };
            var loginCommand = new LoginCommand(loginModel);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), CancellationToken.None))
                .ReturnsAsync((object)null);

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        private string GenerateJwtToken(User user, Login model)
        {
            var secretKey = "my-32-character-ultra-secure-and-ultra-long-secret-1231241234-54234234-23423-24234342342-2342423434-2342434322";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("tenantId", model.Tenant),
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

    }

}