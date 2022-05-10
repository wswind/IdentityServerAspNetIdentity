// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Options;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IdentityServerAspNetIdentity
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<IdentityDbContext>(options =>
               options.UseSqlServer(connectionString));

            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

           services.AddDbContext<ConfigurationDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName)));

            services.AddDbContext<PersistedGrantDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            var storeOptionsCfg = new ConfigurationStoreOptions();
            services.AddSingleton(storeOptionsCfg);

            var storeOptionsOpr = new OperationalStoreOptions();
            services.AddSingleton(storeOptionsOpr);

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<IdentityDbContext>();
                    var configContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    var grantContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

                    context.Database.Migrate();
                    configContext.Database.Migrate();
                    grantContext.Database.Migrate();


                    //create roles
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var adminRole = roleMgr.FindByNameAsync("Admin").Result;
                    if (adminRole == null)
                    {
                        adminRole = new IdentityRole
                        {
                            Name = "Admin"
                        };
                        var result = roleMgr.CreateAsync(adminRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Console.WriteLine("admin created");
                    }
                    else
                    {
                        Console.WriteLine("admin already exists");
                    }

                    // create users
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "AliceSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("alice created");

                        //add alice to admin
                        result = userMgr.AddToRoleAsync(alice, "Admin").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Console.WriteLine("add admin user to role");
                    }
                    else
                    {
                        Log.Debug("alice already exists");
                    }

                    var bob = userMgr.FindByNameAsync("bob").Result;
                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Email = "BobSmith@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("bob created");
                    }
                    else
                    {
                        Log.Debug("bob already exists");
                    }

                    //Create Clients IdentityResources ApiScopes
                    if (!configContext.Clients.Any())
                    {
                        foreach (var client in Config.Clients)
                        {
                            configContext.Clients.Add(client.ToEntity());
                        }
                        configContext.SaveChanges();
                        Log.Debug("clients saved");
                    }

                    if (!configContext.IdentityResources.Any())
                    {
                        foreach (var resource in Config.IdentityResources)
                        {
                            configContext.IdentityResources.Add(resource.ToEntity());
                        }
                        configContext.SaveChanges();
                        Log.Debug("identityresources saved");
                    }

                    if (!configContext.ApiResources.Any())
                    {
                        foreach (var api in Config.ApiResources)
                        {
                            configContext.ApiResources.Add(api.ToEntity());
                        }
                        configContext.SaveChanges();
                        Log.Debug("apiresources saved");
                    }


                    if (!configContext.ApiScopes.Any())
                    {
                        foreach (var resource in Config.ApiScopes)
                        {
                            configContext.ApiScopes.Add(resource.ToEntity());
                        }
                        configContext.SaveChanges();
                        Log.Debug("apiscopes saved");
                    }
                }
            }
        }
    }
}
