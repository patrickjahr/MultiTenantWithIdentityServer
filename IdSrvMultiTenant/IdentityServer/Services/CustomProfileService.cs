using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core;
using Core.Services;
using IdentityServer.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITenantService _tenantService;

        public CustomProfileService(UserManager<ApplicationUser> userManager, ITenantService tenantService)
        {
            _userManager = userManager;
            _tenantService = tenantService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;
            var tenant = await _tenantService.GetTenantNameAsync(user.TenantId);

            var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.TenantId, tenant),
            };

            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            context.IsActive = (user != null) && user.EmailConfirmed;

            //>Return
            return Task.FromResult(0);
        }
    }
}