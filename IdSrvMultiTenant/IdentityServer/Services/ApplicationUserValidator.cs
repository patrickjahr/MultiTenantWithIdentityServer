using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public class ApplicationUserValidator : IUserValidator<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITenantService _tenantService;

        public ApplicationUserValidator(IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantService = tenantService;
        }
        
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidUser",
                    Description = "Email or Username must be set"
                });
            }
            else if (!string.IsNullOrEmpty(manager.Options.User.AllowedUserNameCharacters) &&
                     user.UserName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains<char>(c)))
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidUser",
                    Description = "Email or Username contains not allowed characters"
                });
            } else
            {
                var url = _httpContextAccessor?.HttpContext?.Request?.GetEncodedUrl();
                var userTenant = await _tenantService.GetTenantNameAsync(user.TenantId);
                if ((url != null && !url.Contains(userTenant)) || user.TenantId == Guid.Empty)
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidUser",
                        Description = "User has no valid tenant"
                    });
                }
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}