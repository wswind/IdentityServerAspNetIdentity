using IdentityServer4.Services;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity
{
    public class AllowAllCorsPolicyService : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            return Task.FromResult(true);
        }
    }
}
