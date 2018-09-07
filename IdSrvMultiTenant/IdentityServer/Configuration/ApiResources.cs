using System.Collections.Generic;
using Core;
using IdentityServer4.Models;

namespace IdentityServer.Configuration
{
    public static class ApiResources
    {
        public static IEnumerable<ApiResource> Get() => new List<ApiResource>
        {
            new ApiResource("games-api")
            {
                Scopes = new List<Scope>{new Scope("games-api")},
                UserClaims = new List<string>
                {
                    CustomClaimTypes.TenantId
                }
            },
            
        };
    }
}