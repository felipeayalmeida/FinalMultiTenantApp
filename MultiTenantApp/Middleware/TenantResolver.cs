using Microsoft.IdentityModel.Tokens;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Domain.DTO;
using Newtonsoft.Json;
using System.Text;

namespace MultiTenantApp.Middleware
{
    public class TenantResolver
    {
        private readonly RequestDelegate _next;

        public TenantResolver(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentTenantService currentTenantService)
        {
            //Already logged
            if (context.User.Claims.ToList().FirstOrDefault()?.Value != null)
            {
                var tenant = context.User.Claims.ToList().FirstOrDefault()?.Value;
                if (string.IsNullOrEmpty(tenant) == false)
                {
                    await currentTenantService.SetTenant(tenant);
                }
            }
            //Login in
            else
            {
                if (context.Request.ContentLength.HasValue && context.Request.ContentLength > 0)
                {
                    string requestBody;
                    using (var reader = new StreamReader(context.Request.Body))
                    {
                        requestBody = await reader.ReadToEndAsync();
                    }
                    var user = JsonConvert.DeserializeObject<Login>(requestBody);

                    if(user?.Tenant != null)
                    {
                        await currentTenantService.SetTenant(user.Tenant);
                    }                    
                    context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
                }
            }

            await _next(context);
                                                                                                                }
    }
}
