using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Middleware;
using Xunit;

namespace MultiTenantApp.Tests
{
    public class TenantResolverMiddlewareTests
    {
        [Fact]
        public async Task TenantResolverMiddleware_SetTenant_WhenUserIsLoggedIn()
        {
            // Arrange
            var tenantServiceMock = new Mock<ICurrentTenantService>();
            var middleware = new TenantResolver(next: context => Task.CompletedTask);
            var context = CreateHttpContextWithLoggedInUser("token");

            // Act
            await middleware.InvokeAsync(context, tenantServiceMock.Object);

            // Assert
            tenantServiceMock.Verify(x => x.SetTenant(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task TenantResolverMiddleware_SetTenant_WhenUserIsLoggingIn()
        {
            // Arrange
            var tenantServiceMock = new Mock<ICurrentTenantService>();
            var middleware = new TenantResolver(next: context => Task.CompletedTask);
            var context = CreateHttpContextForLoginRequest("tenant");

            // Act
            await middleware.InvokeAsync(context, tenantServiceMock.Object);

            // Assert
            tenantServiceMock.Verify(x => x.SetTenant("tenant"), Times.Once);
        }

        private HttpContext CreateHttpContextWithLoggedInUser(string token)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "user"),
                new Claim("claim_type", "claim_value"),
            }, "Bearer");

            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claimsIdentity)
            };

            context.Request.Headers["Authorization"] = $"Bearer {token}";

            return context;
        }

        private HttpContext CreateHttpContextForLoginRequest(string tenant)
        {
            var requestBody = "{\"Tenant\":\"" + tenant + "\",\"Username\":\"user\",\"Password\":\"123456\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

            var context = new DefaultHttpContext
            {
                Request =
                {
                    ContentLength = stream.Length,
                    Body = stream,
                    ContentType = "application/json",
                    Method = "POST",
                    Path = "/api/login",
                }
            };

            return context;
        }
    }
}
