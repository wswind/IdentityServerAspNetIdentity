﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using IdentityServerAspNetIdentity.Core.Cors;
using IdentityServerAspNetIdentity.Core.Nginx;
using IdentityServerAspNetIdentity.Customization;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServerAspNetIdentity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNginx();

            services.AddControllersWithViews();

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(Config.AspNetIdentityOptions)
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            //cors https://identityserver4.readthedocs.io/en/latest/topics/cors.html
            services.AddAllowAnyCors();

            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

                // discovery localapi
                options.Discovery.CustomEntries.Add("local_api", "~/localapi");
            })
            //.AddInMemoryIdentityResources(Config.IdentityResources)
            //.AddInMemoryApiScopes(Config.ApiScopes)
            //.AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName));
            })
            // cors https://identityserver4.readthedocs.io/en/latest/topics/cors.html
            .AddCorsPolicyService<AllowAllCorsPolicyService>()
            // cert
            .AddSigningCredential(new X509Certificate2(GetCert()));

            // custom validator
            builder.AddExtensionGrantValidator<CustomValidator>();
            builder.AddProfileService<DefaultProfileService>();

            services.AddAuthentication();
            // local api
            services.AddLocalApiAuthentication();
        }

        public void Configure(IApplicationBuilder app)
        {
            // nginx
            app.UseNginx();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseAllowAnyCors();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private X509Certificate2 GetCert()
        {
            string certPath = Path.Combine(Environment.ContentRootPath, Configuration["Cert:PfxPath"]);
            string cerPwd = Configuration["Cert:Password"];
            return new X509Certificate2(certPath, cerPwd);
        }
    }
}