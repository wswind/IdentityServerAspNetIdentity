// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServerAspNetIdentity.Customization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                //// local api
                //new ApiResource(IdentityServerConstants.LocalApi.,"IdentityServer Local API"){
                //    Scopes = { IdentityServerConstants.LocalApi.ScopeName} ,
                //},
                new ApiResource("ApiResource1"){
                    Scopes = { "Scope1" } ,
                    // include username and role in jwt
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Role,
                        "job" // add custom claim
                    },
                },
                new ApiResource("ApiResource2"){
                    Scopes = { "Scope2" }
                },
                new ApiResource("ApiResource3"){
                    Scopes = { "Scope1" , "Scope2"}
                },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
                new ApiScope("Scope1"),
                new ApiScope("Scope2")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "client1",
                    ClientName = "ResourceOwner Password Client",
                    AllowedGrantTypes = { GrantTypes.ResourceOwnerPassword.First() },
                    ClientSecrets = { new Secret("client1secret".Sha256()) },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "Scope1"
                    },
                    AccessTokenLifetime = 3600 * 24 * 2,//2 days
                    AllowOfflineAccess = true,
                },
                new Client
                {
                    ClientId = "client2",
                    ClientName = "ClientCredential  Client",
                    AllowedGrantTypes = { GrantTypes.ClientCredentials.First() },
                    ClientSecrets = { new Secret("client2secret".Sha256()) },
                    AllowedScopes = {
                        "Scope2"
                    },
                    AccessTokenLifetime = 3600 * 24 * 2,//2 days
                },
                new Client
                {
                    ClientId = "client3",
                    ClientName = "ClientCredential  Client",
                    AllowedGrantTypes = { GrantTypes.ClientCredentials.First() },
                    ClientSecrets = { new Secret("client3secret".Sha256()) },
                    AllowedScopes = {
                        "Scope1", "Scope2"
                    },
                    AccessTokenLifetime = 3600 * 24 * 2,//2 days
                },
                new Client
                {
                    ClientId = "IdsClient",
                    ClientName = "Ids Local Api Client",
                    AllowedGrantTypes = { GrantTypes.ClientCredentials.First() },
                    ClientSecrets = { new Secret("idsclientsecret".Sha256()) },
                    AllowedScopes = {
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                    AccessTokenLifetime = 3600 * 24 * 2,//2 days
                },
                new Client
                {
                    ClientId = "client4",
                    ClientName = "custom validate client",
                    AllowedGrantTypes = { new CustomValidator().GrantType },
                    ClientSecrets = { new Secret("client4secret".Sha256()) },
                    AllowedScopes = {
                        "Scope1", "Scope2"
                    },
                    AccessTokenLifetime = 3600 * 24 * 2,//2 days
                },
            };

        public static Action<IdentityOptions> AspNetIdentityOptions
        {
            get
            {
                return options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 1;
                    options.User.RequireUniqueEmail = false;
                };
            }
        }

      
        

     

    }
}