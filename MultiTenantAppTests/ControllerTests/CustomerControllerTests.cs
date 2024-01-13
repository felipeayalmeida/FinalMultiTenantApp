using Castle.Core.Resource;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MultiTenantApp.Application.Commands.CustomerCmd;
using MultiTenantApp.Application.Handlers.CustomerHdl;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Controllers;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantApp.Test.ControllerTests
{
    public class CustomerControllerTests
    {
        [Fact]
        public async void Handle_ValidRequest_ReturnsCreatedCustomer()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var date = DateTime.Now;
            var handler = new CreateCustomerHandler(
                userRepositoryMock.Object,
                customerRepositoryMock.Object,
                httpContextAccessorMock.Object
            );

            var userEmail = "user@example.com";
            var user = new User { Id = 1, Email = userEmail };

            var createCustomerCommand = new CreateCustomerCommand(new CreateCustomer
            {
                Name = "TestCustomer",
                Schedule = date,
            });

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userEmail)
            }));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);
            userRepositoryMock.Setup(x => x.GetUserByEmailAsync(userEmail)).ReturnsAsync(user);

            // Act
            var result = await handler.Handle(createCustomerCommand, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("TestCustomer");
            result.Schedule.Should().Be(date);
            result.SecretaryId.Should().Be(user.Id);

            customerRepositoryMock.Verify(x => x.CreateCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidUser_ReturnsNull()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var date = new DateTime();

            var handler = new CreateCustomerHandler(
                userRepositoryMock.Object,
                customerRepositoryMock.Object,
                httpContextAccessorMock.Object
            );

            var userEmail = "user@example.com";

            var createCustomerCommand = new CreateCustomerCommand(new CreateCustomer
            {
                Name = "TestCustomer",
                Schedule = date
            });

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userEmail)
             }));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);
            userRepositoryMock.Setup(x => x.GetUserByEmailAsync(userEmail)).ReturnsAsync((User)null);

            // Act
            var result = await handler.Handle(createCustomerCommand, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            customerRepositoryMock.Verify(x => x.CreateCustomer(It.IsAny<Customer>()), Times.Never);
        }


        [Fact]
        public async void GetCustomersBySecretaryId_ValidSecretaryId_ReturnsOkResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            var controller = new CustomerController(mediatorMock.Object, httpContextAccessorMock.Object);
            var date = new DateTime();
            var secretaryId = 1;
             var expectedUsers = new List<User> { };
            expectedUsers.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });
            expectedUsers.Add(new User() { Id = 2, TenantId = "tenant1", Email = "user2@email.com", Password = "123456", Role = RoleEnum.Secretary });

            var customers = new List<Customer> { };
            customers.Add(new Customer() { Id = 1,Name="Customer",Schedule=date,SecretaryId=secretaryId });

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "user@email.com")
            }));

            httpContextAccessorMock
                .Setup(h => h.HttpContext)
                .Returns(httpContext);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                .ReturnsAsync(expectedUsers);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCustomersQuery>(), CancellationToken.None))
                .ReturnsAsync(customers);

            // Act
            var result = await controller.GetCustomersBySecretaryId();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(customers);
        }

        [Fact]
        public async void GetCustomersBySecretaryId_InvalidSecretaryId_ReturnsNotFoundResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var controller = new CustomerController(mediatorMock.Object, httpContextAccessorMock.Object);
            var httpContext = new DefaultHttpContext();
            var expectedUsers = new List<User> { };
            expectedUsers.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });
            expectedUsers.Add(new User() { Id = 2, TenantId = "tenant1", Email = "user2@email.com", Password = "123456", Role = RoleEnum.Secretary });


            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "user@email.com") 
            }));


            httpContextAccessorMock
                .Setup(h => h.HttpContext)
                .Returns(httpContext);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await controller.GetCustomersBySecretaryId();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void GetCustomersBySecretaryId_NullResult_ReturnsNotFoundResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            var expectedUsers = new List<User> { };
            expectedUsers.Add(new User() { Id = 1, TenantId = "tenant1", Email = "user@email.com", Password = "123456", Role = RoleEnum.Secretary });
            expectedUsers.Add(new User() { Id = 2, TenantId = "tenant1", Email = "user2@email.com", Password = "123456", Role = RoleEnum.Secretary });

            var controller = new CustomerController(mediatorMock.Object, httpContextAccessorMock.Object);

            var secretaryId = 1;

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "user@email.com")
            }));

            mediatorMock
                 .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), CancellationToken.None))
                 .ReturnsAsync(expectedUsers);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCustomersQuery>(), CancellationToken.None))
                .ReturnsAsync((List<Customer>)null);

            httpContextAccessorMock
                .Setup(h => h.HttpContext)
                 .Returns(httpContext);


            // Act
            var result = await controller.GetCustomersBySecretaryId();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
