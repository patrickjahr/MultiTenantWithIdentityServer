using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configuration
{
    public static class Clients
    {
        public static IEnumerable<Client> Get() => new List<Client>
        {
            new Client()
            {
                AllowedGrantTypes = new List<string>(GrantTypes.Implicit),
                ClientName = "Games List",
                ClientId = "941b8aa0-0085-47ad-84da-73340390d946",
                AccessTokenType = AccessTokenType.Jwt,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    "games-api",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    IdentityServerConstants.StandardScopes.Address
                },
                RedirectUris = new List<string>()
                {
                    "http://{0}.gameslist.local:4200/",
                },
                PostLogoutRedirectUris = new List<string>()
                {
                    "http://{0}.gameslist.local:4200/",
                },
                ClientSecrets = new List<Secret>()
                {
                    new Secret("games-secret".Sha512())
                }
            }
        };
    }
}
