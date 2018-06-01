// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace src
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("sample_identity_scope",
                    new[]
                    {
                        "role",
                        "admin",
                        "user"
                    })
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("api1", "My API #1"),
                new ApiResource
                {
                    Name = "sample_api_resource",
                    DisplayName = "My Sample API",
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "sample_api_scope",
                            DisplayName = "The sample API scope.",
                            UserClaims =
                            {
                                "role",
                                "admin",
                                "user"
                            }
                        }
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = { "api1" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api1" }
                },

                // SPA client using implicit flow
                new Client
                {
                    ClientId = "mvc.implicit",
                    ClientName = "mvc.implicit",
                    ClientUri = "http://localhost:44077",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        "http://localhost:44077/"
                    },

                    PostLogoutRedirectUris = { "http://localhost:44077" },
                    AllowedCorsOrigins = { "http://localhost:44077" },

                    AllowedScopes = { "openid", "profile", "email", "api1" }
                },

                // swagger UI
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "swagger",
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =
                    {
                        "http://localhost:65389/swagger/o2c.html"
                    },
                    PostLogoutRedirectUris = {"http://localhost:65389/swagger"},
                    AllowedCorsOrigins = {"http://localhost:65389"},
                    AccessTokenLifetime = 300,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "role",
                        "user",
                        "admin",
                        "sample_identity_scope",
                        "sample_api_scope"
                    }
                },
            };
        }
    }
}