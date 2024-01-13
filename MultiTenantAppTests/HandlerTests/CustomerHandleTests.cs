using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MultiTenantApp.Application.Commands.CustomerCmd;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Application.Handlers.CustomerHdl;
using MultiTenantApp.Application.Handlers.UserHdl;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Database.Repository;
using MultiTenantApp.Domain.DTO;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantApp.Test.HandlerTests
{
    public class CustomerHandleTests
    {
        [Fact]
        public async void CreateCustomerHandler_ValidRequest_ReturnsCreatedCustomer()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userEmail = "teste@email.com";
            var user = new User() { Id = 12, Email = "user@email.com", Password = "123456", TenantId = "tenant1", Role = RoleEnum.Admin };
            var newCustomer = new CreateCustomer() { Name = userEmail, Schedule = DateTime.Now };
            var handler = new CreateCustomerHandler(userRepositoryMock.Object, customerRepositoryMock.Object, httpContextAccessorMock.Object);
            var command = new CreateCustomerCommand(newCustomer);

            httpContextAccessorMock.Setup(h => h.HttpContext.User.FindFirst(It.IsAny<string>()))
                          .Returns((Func<string, Claim>)(claimType => new Claim(ClaimTypes.NameIdentifier, userEmail)));
            userRepositoryMock.Setup(u => u.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            customerRepositoryMock.Setup(c => c.CreateCustomer(It.IsAny<Customer>()));

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            userRepositoryMock.Verify(x => x.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
            httpContextAccessorMock.Verify(h => h.HttpContext.User.FindFirst(It.IsAny<string>()), Times.Once);
            customerRepositoryMock.Verify(c => c.CreateCustomer(It.IsAny<Customer>()), Times.Once);

        }

        [Fact]
        public async void CreateCustomerHandler_InvalidRequest_ReturnsNull()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userEmail = "teste@email.com";
            var user = new User() { Id = 12, Email = "user@email.com", Password = "123456", TenantId = "tenant1", Role = RoleEnum.Admin };
            var newCustomer = new CreateCustomer() { Name = userEmail, Schedule = new DateTime() };
            var handler = new CreateCustomerHandler(userRepositoryMock.Object, customerRepositoryMock.Object, httpContextAccessorMock.Object);
            var command = new CreateCustomerCommand(newCustomer);

            httpContextAccessorMock.Setup(h => h.HttpContext.User.FindFirst(It.IsAny<string>()))
                          .Returns((Func<string, Claim>)(claimType => new Claim(ClaimTypes.NameIdentifier, userEmail)));
            userRepositoryMock.Setup(u => u.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            customerRepositoryMock.Setup(c => c.CreateCustomer(It.IsAny<Customer>()));

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            userRepositoryMock.Verify(x => x.GetUserByEmailAsync(It.IsAny<string>()), Times.Never);
            httpContextAccessorMock.Verify(h => h.HttpContext.User.FindFirst(It.IsAny<string>()), Times.Never);
            customerRepositoryMock.Verify(c => c.CreateCustomer(It.IsAny<Customer>()), Times.Never);

        }
        [Fact]
        public async void DeleteCustomerHandler_ValidRequest_ReturnsDeletedCustomerTrue()
        {
            //Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customer = new Customer() {Id=1, Name = "Cliente 1", Schedule = new DateTime(), SecretaryId = 12 };
            var deleteCustomer = new DeleteCustomer() { Id = 1 };
            var handler = new DeleteCustomerHandler(customerRepositoryMock.Object);
            var command = new DeleteCustomerCommand(deleteCustomer);
            customerRepositoryMock.Setup(u => u.GetCustomerById(It.IsAny<int>())).ReturnsAsync(customer);
            customerRepositoryMock.Setup(u => u.DeleteCustomer(customer)).Returns(Task.CompletedTask);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.IsDeleted.Should().BeTrue();
            customerRepositoryMock.Verify(u => u.GetCustomerById(It.IsAny<int>()), Times.Once);
            customerRepositoryMock.Verify(u => u.DeleteCustomer(customer), Times.Once);


        }
        [Fact]
        public async void DeleteCustomerHandler_InvalidRequest_ReturnsDeletedCustomerFalse()
        {
            //Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customer = new Customer() { Id = 1, Name = "Cliente 1", Schedule = new DateTime(), SecretaryId = 12 };
            var deleteCustomer = new DeleteCustomer() { Id = 2 };
            var handler = new DeleteCustomerHandler(customerRepositoryMock.Object);
            var command = new DeleteCustomerCommand(deleteCustomer);
            customerRepositoryMock.Setup(u => u.GetCustomerById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Customer>());
            customerRepositoryMock.Setup(u => u.DeleteCustomer(customer)).Returns(Task.CompletedTask);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            customerRepositoryMock.Verify(u => u.GetCustomerById(It.IsAny<int>()), Times.Once);
            customerRepositoryMock.Verify(u => u.DeleteCustomer(customer), Times.Never);

        }

        [Fact]
        public async void GetCustomersHandler_ValidRequest_ReturnsCustomers()
        {
            //Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var customers = new List<Customer>
            {
                new Customer() { Id = 1, Name = "Cliente 1", Schedule = new DateTime(), SecretaryId = 12 },
                new Customer() { Id = 12, Name = "Cliente 12", Schedule = new DateTime(), SecretaryId = 12 }
            };
            var deleteCustomer = new DeleteCustomer() { Id = 1 };
            var secretaryId = 12;

            var handler = new GetCustomersHandler(customerRepositoryMock.Object);
            var query = new GetCustomersQuery(secretaryId);

            customerRepositoryMock.Setup(u => u.GetCustomersBySecretaryId(It.IsAny<int>())).ReturnsAsync(customers);
            
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().BeGreaterThan(0);
            customerRepositoryMock.Verify(u => u.GetCustomersBySecretaryId(It.IsAny<int>()), Times.Once);


        }

        [Fact]
        public async void GetCustomersHandler_inValidRequest_ReturnsNull()
        {
            //Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customers = new List<Customer>();
            var deleteCustomer = new DeleteCustomer() { Id = 1 };
            var secretaryId = 12;

            var handler = new GetCustomersHandler(customerRepositoryMock.Object);
            var query = new GetCustomersQuery(secretaryId);

            customerRepositoryMock.Setup(u => u.GetCustomersBySecretaryId(It.IsAny<int>())).ReturnsAsync(customers);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            customerRepositoryMock.Verify(u => u.GetCustomersBySecretaryId(It.IsAny<int>()), Times.Once);

        }


        [Fact]
        public async void UpdateCustomerScheduleHandler_ValidRequest_ReturnsTrue()
        {
            //Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customer = new Customer() { Id = 1, Name = "Cliente 1", Schedule = new DateTime(), SecretaryId = 12 };
            var updateSchedule = new UpdateSchedule() { CustomerId = 1, Schedule = DateTime.Now.AddDays(1) };
            var deleteCustomer = new DeleteCustomer() { Id = 1 };
            var secretaryId = 12;

            var handler = new UpdateScheduleHandler(customerRepositoryMock.Object);
            var command = new UpdateScheduleCommand(updateSchedule);

            customerRepositoryMock.Setup(u => u.GetCustomerById(It.IsAny<int>())).ReturnsAsync(customer);
            customerRepositoryMock.Setup(u => u.UpdateCustomer(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().NotBeEmpty();
            customerRepositoryMock.Verify(u => u.GetCustomerById(It.IsAny<int>()), Times.Once);
            customerRepositoryMock.Verify(u => u.UpdateCustomer(It.IsAny<Customer>()), Times.Once);


        }

        [Fact]
        public async void UpdateCustomerScheduleHandler_inValidRequest_ReturnsFalse()
        {
            //Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customer = new Customer() { Id = 1, Name = "Cliente 1", Schedule = new DateTime(), SecretaryId = 12 };
            var updateSchedule = new UpdateSchedule() { CustomerId = 1, Schedule = DateTime.Now };
            var deleteCustomer = new DeleteCustomer() { Id = 1 };
            var secretaryId = 12;

            var handler = new UpdateScheduleHandler(customerRepositoryMock.Object);
            var command = new UpdateScheduleCommand(updateSchedule);

            customerRepositoryMock.Setup(u => u.GetCustomerById(It.IsAny<int>())).ReturnsAsync(customer);
            customerRepositoryMock.Setup(u => u.UpdateCustomer(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            customerRepositoryMock.Verify(u => u.GetCustomerById(It.IsAny<int>()), Times.Never);
            customerRepositoryMock.Verify(u => u.UpdateCustomer(It.IsAny<Customer>()), Times.Never);


        }
    }
}
