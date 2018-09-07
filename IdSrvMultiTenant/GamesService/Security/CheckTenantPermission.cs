using System;
using Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GamesService.Security
{
    public class CheckTenantPermission: Attribute, IResourceFilter
    {
        public CheckTenantPermission(params string[] headers)
        {
            
        }
        
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var split = context.HttpContext.Request.Host.Host.Split(".");
            if(split.Length < 0 && String.IsNullOrWhiteSpace(split[0]))
                throw new ArgumentNullException("No tenant set in url");

            var urlTenant = split[0];
            var userTenant = context.HttpContext.User.FindFirst(CustomClaimTypes.TenantId)?.Value;
            if (urlTenant != userTenant)
                throw new UnauthorizedAccessException($"User has no access to the given tenant {urlTenant}");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}