using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace IdentityServerAspNetIdentity.SharedCore.Nginx
{
    public static class NginxServiceCollectionExtensions
    {
        public static IServiceCollection AddNginx(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            return services;
        }
        public static IApplicationBuilder UseNginx(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UseForwardedPrefix();
            return app;
        }
        public static IApplicationBuilder UseForwardedPrefix(this IApplicationBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                //https://github.com/dotnet/aspnetcore/issues/23263#issuecomment-767394044
                if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefixVals))
                {
                    if (!StringValues.IsNullOrEmpty(prefixVals))
                    {
                        string prefix = prefixVals.Last();
                        ctx.Request.PathBase = $"/{prefix}";
                        string host = ctx.Request.Host.Value;
                        ctx.Request.Host = new HostString($"{host}/{prefix}");
                    }
                }

                await next();
            });
            return app;
        }
    }
}
