using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Application.Commands.TenantCmd;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Controllers;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;
using System.Security.Claims;

namespace MultiTenantApp.Test.ControllerTests
{
    public class AdminControllerTests
    {
        [Fact]
        public async void CreateTenant_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new AdminController(mediatorMock.Object);

            var createTenant = new Tenant { Id = "tenant", Email = "tenant@email.com", Name = "Tenant", Paswword = "123456" };
            var createTenantCommand = new CreateTenantCommand(createTenant);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTenantCommand>(), CancellationToken.None))
                .ReturnsAsync(createTenant);

            // Act
            var result = await controller.CreateTenant(createTenant);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(createTenant);
        }

        [Fact]
        public async void CreateTenant_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new AdminController(mediatorMock.Object);

            var createTenant = new Tenant { Id = "tenant", Email = "tenant.email", Name = "Tenant", Paswword = "123456" };
            var createTenantCommand = new CreateTenantCommand(createTenant);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTenantCommand>(), CancellationToken.None))
                .ReturnsAsync((Tenant)null);

            // Act
            var result = await controller.CreateTenant(createTenant);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void CreateUser_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var controller = new AdminController(mediatorMock.Object);

            var createUser = new CreateUser { Email = "user@email.com", Password="123456"};
            var createUserCommand = new CreateUserCommand(createUser);

            var user = new User {Id=1,TenantId="tenant1", Email = "user@email.com", Password = "123456", Role=RoleEnum.Secretary };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None))
                .ReturnsAsync(user);

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "user@email.com") 
            }));

            httpContextAccessorMock
                .Setup(h => h.HttpContext)
                .Returns(httpContext);

            // Act
            var result =  await controller.CreateUser(createUser);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async void CreateUser_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var controller = new AdminController(mediatorMock.Object);

            var createUser = new CreateUser { Email = "user.email.com", Password = "123456" };
            var createUserCommand = new CreateUserCommand(createUser);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None))
                .ReturnsAsync((User)null);

            // Act
            var result = await controller.CreateUser(createUser);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void GetAllUsers_ReturnsOkResultWithUsers()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new AdminController(mediatorMock.Object);

            var getUsersQuery = new GetUsersQuery();
            var expectedUsers = new List<User> { };
            expectedUsers.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });
            expectedUsers.Add(new User() { Id = 2, TenantId = "tenant1", Email = "user2@email.com", Password = "123456", Role = RoleEnum.Secretary });
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public async void GetAllUsers_ReturnsBadRequestWhenNoUsers()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new AdminController(mediatorMock.Object);

            var getUsersQuery = new GetUsersQuery();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                .ReturnsAsync((List<User>)null);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
