using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Core;
using Core.Services;
using GamesService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GamesService.Security
{
    public static class TenantAuthenticationExtentions
    {
        public static IApplicationBuilder UseTenantAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof (app));
            return app.UseMiddleware<TenantAuthorizationMiddleware>(Array.Empty<object>());
        }
    }

    public class TenantAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _contextAccessor;

        public TenantAuthorizationMiddleware(RequestDelegate next, IHttpContextAccessor contextAccessor)
        {
            _next = next ?? throw new ArgumentNullException(nameof (next));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public IAuthenticationSchemeProvider Schemes { get; set; }

        public async Task Invoke(HttpContext context)
        {
            var userTenant = _contextAccessor.HttpContext.User.FindFirst(c => c.Type == CustomClaimTypes.TenantId)?.Value;
            var urlTenant = _contextAccessor.HttpContext?.Request?.Host.Host.Split(".")?[0];
            if(String.IsNullOrWhiteSpace(urlTenant))
                await context.ForbidAsync();
            if(urlTenant != userTenant)
                await context.ForbidAsync();
            
            await this._next(context);
        }
    }
}