using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Application.Commands.UserCmd;
using MultiTenantApp.Application.Handlers.UserHdl;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;
using System.Security.Claims;

namespace MultiTenantApp.Test.HandlerTests
{
    public class UserHandlerTests
    {

        [Fact]
        public async void CreateUserHandle_ValidRequest_ReturnsUser()
        {
            var userRepository = new Mock<IUserRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            var createUser = new CreateUser() { Email = "user@email.com", Password = "123456" };
            var tenantUser = new User() { Id = 1, Email = "userTenant@email.com", Password = "123456", TenantId = "tenant1", Role = RoleEnum.Admin };
            var user = new User() { Id = 12, Email = "user@email.com", Password = "123456", TenantId = "tenant1", Role = RoleEnum.Secretary };
            var handler = new CreateUserHandler(httpContextAccessorMock.Object, userRepository.Object);
            var command = new CreateUserCommand(createUser);

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user@email.com")
            }));

            httpContextAccessorMock
                .Setup(h => h.HttpContext)
                .Returns(httpContext);

            userRepository
                .Setup(u => u.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(tenantUser);

            userRepository
                .Setup(u => u.CreateUser(It.IsAny<User>())).ReturnsAsync(user);

            //Act
            var result = handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            userRepository.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void CreateUserHandle_InvalidRequestUserIsNoTenant_ReturnsUser()
        {
            var userRepository = new Mock<IUserRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            var createUser = new CreateUser() { Email = "user@email.com", Password = "123456" };
            var tenantUser = new User() { Id = 1, Email = "userTenant@email.com", Password = "123456", TenantId = "tenant1", Role = RoleEnum.Secretary };
            var user = new User() { Id = 12, Email = "user@email.com", Password = "123456", TenantId = "tenant1", Role = RoleEnum.Secretary };
            var handler = new CreateUserHandler(httpContextAccessorMock.Object, userRepository.Object);
            var command = new CreateUserCommand(createUser);

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user@email.com")
            }));

            httpContextAccessorMock
                .Setup(h => h.HttpContext)
                .Returns(httpContext);

            userRepository
                .Setup(u => u.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(tenantUser);

            userRepository
                .Setup(u => u.CreateUser(It.IsAny<User>())).ReturnsAsync(user);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            userRepository.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async void CreateTenantUserHandle_ValidRequest_ReturnsUser()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var user = new User() { Id = 12, Email = "user@email.com", Password = "123456", TenantId = "tenant1", Role = RoleEnum.Admin };
            var handler = new CreateTenantUserHandler(userRepository.Object);
            var command = new CreateTenantUserCommand(user);

            userRepository.Setup(u => u.CreateTenantUser(It.IsAny<User>())).ReturnsAsync(user);

            //Act
            var result = handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            userRepository.Verify(x => x.CreateTenantUser(It.IsAny<User>()), Times.Once);

        }

        [Fact]
        public async void GetUsersHandle_ValidRequest_ReturnsUsers()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var expectedUsers = new List<User> { };
            expectedUsers.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });
            expectedUsers.Add(new User() { Id = 2, TenantId = "tenant1", Email = "user2@email.com", Password = "123456", Role = RoleEnum.Secretary });
            userRepository.Setup(u => u.GetUsersAsync()).ReturnsAsync(expectedUsers);
            var handler = new GetUsersHandler(userRepository.Object);
            var command = new GetUsersQuery();

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNullOrEmpty();
            userRepository.Verify(x => x.GetUsersAsync(), Times.Once);

        }

        [Fact]
        public async void GetUsersHandle_InvalidRequest_ReturnsNull()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var expectedUsers = new List<User> { };
            userRepository.Setup(u => u.GetUsersAsync()).ReturnsAsync(expectedUsers);
            var handler = new GetUsersHandler(userRepository.Object);
            var command = new GetUsersQuery();

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNullOrEmpty();
            userRepository.Verify(x => x.GetUsersAsync(), Times.Once);

        }

        [Fact]
        public async void DeleteUserHandle_ValidRequest_ReturnsDeletedUser()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var expectedUsers = new List<User> { };
            var userId = new DeleteUser() { Id = 1 };
            expectedUsers.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });
            expectedUsers.Add(new User() { Id = 2, TenantId = "tenant1", Email = "user2@email.com", Password = "123456", Role = RoleEnum.Secretary });
            userRepository.Setup(u => u.GetUsersAsync()).ReturnsAsync(expectedUsers);
            userRepository.Setup(u => u.DeleteUser(It.IsAny<User>())).Returns(Task.CompletedTask);
            var handler = new DeleteUserHandler(userRepository.Object);
            var command = new DeleteUserCommand(userId);

            //Act
            var result = await ((IRequestHandler<DeleteUserCommand, DeleteUserResponse>)handler).Handle(command, CancellationToken.None);

            //Assertt
            result.Email.Should().NotBeNullOrEmpty();
            result.IsDeleted.Should().BeTrue();
            userRepository.Verify(x => x.GetUsersAsync(), Times.Once);
            userRepository.Verify(x => x.DeleteUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void DeleteUserHandle_InvalidRequest_ReturnsFail()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var expectedUsers = new List<User> { };
            var userId = new DeleteUser() { Id = 3 };
            expectedUsers.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });
            expectedUsers.Add(new User() { Id = 2, TenantId = "tenant1", Email = "user2@email.com", Password = "123456", Role = RoleEnum.Secretary });
            userRepository.Setup(u => u.GetUsersAsync()).ReturnsAsync(expectedUsers);
            userRepository.Setup(u => u.DeleteUser(It.IsAny<User>())).Returns(Task.CompletedTask);
            var handler = new DeleteUserHandler(userRepository.Object);
            var command = new DeleteUserCommand(userId);

            //Act
            var result = await ((IRequestHandler<DeleteUserCommand, DeleteUserResponse>)handler).Handle(command, CancellationToken.None);


            //Assert
            result.Should().BeNull();
            userRepository.Verify(x => x.GetUsersAsync(), Times.Once);
            userRepository.Verify(x => x.DeleteUser(It.IsAny<User>()), Times.Never);

        }
    }
}
