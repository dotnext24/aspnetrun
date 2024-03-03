
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1" }
            }
            };

        public static List<TestUser> Users =>
            new List<TestUser>
            {
            new TestUser
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password"
            }
            };
    }
}
