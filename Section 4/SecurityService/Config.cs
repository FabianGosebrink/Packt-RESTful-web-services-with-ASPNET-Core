using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace SecurityService
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { new ApiResource("resourcesScope", "My API") { UserClaims = { "role" } } };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client> {
                    new Client
                    {
                        ClientId = "postmanClient",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        AllowAccessTokensViaBrowser = true,
                        AllowOfflineAccess = true,
                        AlwaysSendClientClaims = true,
                        RequireClientSecret = false,
                        AllowedScopes = {
                            IdentityServerConstants.StandardScopes.Profile,
                            "roles",
                            "resourcesScope"
                        }
                    }
                };
        }
    }
}
