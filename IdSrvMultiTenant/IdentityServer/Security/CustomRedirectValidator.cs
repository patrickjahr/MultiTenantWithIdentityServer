using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Services;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace IdentityServer.Security
{
    public class CustomRedirectValidator : IRedirectUriValidator
    {
        private readonly ITenantService _tenantService;

        public CustomRedirectValidator(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        
        public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(CheckRedirectUrl(requestedUri, client));
        }
        

        public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(CheckRedirectUrl(requestedUri, client));
        }
        
        private bool CheckRedirectUrl(string requestedUri, Client client)
        {
            if (requestedUri == null) 
                throw new ArgumentNullException(nameof(requestedUri));
            
            var tenantName = _tenantService.GetTenantName(requestedUri);
            if (String.IsNullOrEmpty(tenantName))
                return false;

            return client.RedirectUris.Any(u =>
            {
                var redirectUrl = String.Format(u, tenantName);
                return requestedUri.Equals(redirectUrl);
            });
        }
    }
}