using FluentAssertions;
using MediatR;
using Moq;
using MultiTenantApp.Application.Commands.SecretaryCmd;
using MultiTenantApp.Application.Commands.TenantCmd;
using MultiTenantApp.Application.Handlers.TenantHdl;
using MultiTenantApp.Database.Interface;
using MultiTenantApp.Domain.Entity;
using MultiTenantApp.Helpers.Enums;

namespace MultiTenantApp.Test.HandlerTests
{
    public class TenantHandlerTests
    {
        [Fact]
        public async void CreateTenantHandle_ValidRequest_ReturnsTenant()
        {
            var tenantRepositoryMock = new Mock<ITenantRepository>();
            var mediatorMock = new Mock<IMediator>();
            var handler = new CreateTenantHandler(tenantRepositoryMock.Object, mediatorMock.Object);
            var tenant = new Tenant() { Id = "tenant1", Name = "Tenant1", Email = "tenant1@email.com", Paswword = "123456" };
            var user = new User() { Id = 1, Email = tenant.Email, Password = tenant.Paswword, TenantId = tenant.Id, Role = RoleEnum.Admin };
            var command = new CreateTenantCommand(tenant);

            tenantRepositoryMock
                 .Setup(t => t.CreateTenant(It.IsAny<Tenant>()))
                 .ReturnsAsync(tenant);
            
            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTenantUserCommand>(), CancellationToken.None)).ReturnsAsync(user);

            //Act
            var result = handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            tenantRepositoryMock.Verify(x => x.CreateTenant(It.IsAny<Tenant>()), Times.Once);
        }

        [Fact]
        public async void CreateTenantHandle_InvalidRequest_ReturnsNull()
        {
            var tenantRepositoryMock = new Mock<ITenantRepository>();
            var mediatorMock = new Mock<IMediator>();
            var handler = new CreateTenantHandler(tenantRepositoryMock.Object, mediatorMock.Object);
            var tenant = new Tenant() { Id = "tenant1", Name = "Tenant1", Email = "tenant1email.com", Paswword = "123456" };
            var user = new User() { Id = 1, Email = tenant.Email, Password = tenant.Paswword, TenantId = tenant.Id, Role = RoleEnum.Admin };
            var command = new CreateTenantCommand(tenant);

            tenantRepositoryMock
                 .Setup(t => t.CreateTenant(It.IsAny<Tenant>()))
                 .ReturnsAsync(tenant);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTenantUserCommand>(), CancellationToken.None)).ReturnsAsync(user);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            tenantRepositoryMock.Verify(x => x.CreateTenant(It.IsAny<Tenant>()), Times.Never);
        }


    }
}
