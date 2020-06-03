using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerWeb
{
    public class Config
    {
        /// <summary>
        /// 配置资源服务器
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1","my api")
            };
        }

        public static IEnumerable<IdentityResource> Ids =>
           new List<IdentityResource>{
                         new IdentityResources.OpenId(),
                         new IdentityResources.Profile(),
                   };

        /// <summary>
        /// 配置客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets ={
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },
                 // interactive ASP.NET Core MVC client
                new Client
                 {
                     ClientId = "portal",
                     ClientSecrets = { new Secret("portal".Sha256()) },

                     AllowedGrantTypes = GrantTypes.Code,
                     RequireConsent = false,
                     RequirePkce = true,

                    // where to redirect to after login
                     RedirectUris = { "http://localhost:5002/signin-oidc" },

                        // where to redirect to after logout
                     PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                     AllowedScopes = new List<string>
                     {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                     }
                }
            };
        }
    }
}