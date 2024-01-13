using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using MultiTenantApp.Application.Commands.AuthCmd;
using MultiTenantApp.Application.Handlers.AuthHdl;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;

namespace MultiTenantApp.Test.HandlerTests
{
    public class AuthHandlerTests
    {
     
        [Fact]
        public async void Handle_ValidRequest_ReturnsLogin()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            var mediatorMock = new Mock<IMediator>();
            var users = new List<User>
            {
                new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary }
            };
            var login = new Login() { Email = "user@email.com", Password = "123456", Tenant = "tenant1" };
            var commad = new LoginCommand(login);
            var handler = new LoginHandler(
                configurationMock.Object,
                mediatorMock.Object
            );

            configurationMock.Setup(c => c.GetSection("JwtSettings")["SecretKey"]).Returns("my-32-character-ultra-secure-and-ultra-long-secret-1231241234-54234234-23423-24234342342-2342423434-2342434322");

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                .ReturnsAsync(users);

            //Act
            var result = await handler.Handle(commad,CancellationToken.None);

            //Arrange
            result.Should().NotBeNull();
        
        }

        [Fact]
        public async void Handle_InvalidRequest_ReturnsLogin()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            var mediatorMock = new Mock<IMediator>();
            var users = new List<User>
            {
                new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary }
            };
            var login = new Login() { Email = "useremail.com", Password = "123456", Tenant = "tenant1" };
            var commad = new LoginCommand(login);
            var handler = new LoginHandler(
                configurationMock.Object,
                mediatorMock.Object
            );

            configurationMock.Setup(c => c.GetSection("JwtSettings")["SecretKey"]).Returns("my-32-character-ultra-secure-and-ultra-long-secret-1231241234-54234234-23423-24234342342-2342423434-2342434322");

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                .ReturnsAsync(users);

            //Act
            var result = await handler.Handle(commad,CancellationToken.None);

            //Arrange
            result.Should().BeNull();
        
        }
    }
}
