using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebApi1.AuthorPolicy;

namespace WebApi1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddIdentityServerAuth(Configuration);

            //https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-3.1
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Policy1", policy =>
                    policy.Requirements.Add(new ScopeRequirement("Scope1")));
                options.AddPolicy("Policy2", policy =>
                    policy.Requirements.Add(new ScopeRequirement("Scope2")));
            });
            services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
            
            //https://github.com/IdentityServer/IdentityServer4.AccessTokenValidation
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Policy1", builder =>
            //    {
            //        builder.RequireScope("Scope1");
            //        //builder.RequireClaim("scope", "Scope1");
            //    });
            //    options.AddPolicy("Policy2", builder =>
            //    {
            //        builder.RequireScope("Scope2");
            //        //builder.RequireClaim("scope", "Scope1");
            //    });
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
