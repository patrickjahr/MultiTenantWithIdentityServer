using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.Configuration
{
    public static class IdentityResources
    {
        public static IEnumerable<IdentityResource> Get() => new List<IdentityResource>()
        {
            new IdentityServer4.Models.IdentityResources.OpenId(),
            new IdentityServer4.Models.IdentityResources.Email(),
            new IdentityServer4.Models.IdentityResources.Profile(),
            new IdentityServer4.Models.IdentityResources.Phone(),
            new IdentityServer4.Models.IdentityResources.Address(),
        };
    }
}