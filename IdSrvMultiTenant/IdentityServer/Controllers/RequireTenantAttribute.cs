using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace IdentityServer.Controllers
{
    public class RequireTenantAttribute : ActionFilterAttribute
    {
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public RequireTenantAttribute(IIdentityServerInteractionService identityServerInteractionService)
        {
            _identityServerInteractionService = identityServerInteractionService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var parsedQueryString = context.HttpContext.Request.QueryString.HasValue 
                ? QueryHelpers.ParseQuery(context.HttpContext.Request.QueryString.Value) 
                : new Dictionary<string, StringValues>();

            if (parsedQueryString.TryGetValue("returnUrl", out var returnUrl))
            {
                var authorizationContext =
                    await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);

                if (string.IsNullOrWhiteSpace(authorizationContext.Tenant))
                {
                    context.Result = new BadRequestResult();
                }
            }
            
            await next();
        }
    }
}
